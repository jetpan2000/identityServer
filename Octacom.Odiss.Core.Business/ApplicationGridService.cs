using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octacom.Odiss.Core.Contracts;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.DataLayer;

namespace Octacom.Odiss.Core.Business
{
    public class ApplicationGridService : IApplicationGridService
    {
        private readonly IConfigService configService;
        private readonly IApplicationService applicationService;

        public ApplicationGridService(IConfigService configService, IApplicationService applicationService)
        {
            this.configService = configService;
            this.applicationService = applicationService;
        }

        public TEntity MapDataToEntity<TEntity>(Guid appId, Dictionary<Guid, object> obj)
            where TEntity : new()
        {
            var app = configService.GetApplications().FirstOrDefault(x => x.ID == appId);

            if (app == null)
            {
                throw new ArgumentException($"Application {appId} does not exist");
            }

            var result = new TEntity();
            var type = typeof(TEntity);

            foreach (var key in obj.Keys)
            {
                var field = app.Fields.FirstOrDefault(x => x.ID == key);

                if (field == null)
                {
                    continue;
                }

                var propInfo = type.GetProperty(field.DBColumnName);
                if (propInfo == null)
                {
                    continue;
                }

                var value = obj[key];

                if (propInfo.PropertyType == typeof(Guid) && value.GetType() == typeof(string))
                {
                    Guid.TryParse((string)value, out var guid);
                    value = guid;
                }
                else if (propInfo.PropertyType == typeof(bool) && value.GetType() == typeof(string))
                {
                    bool.TryParse((string)value, out var boolValue);
                    value = boolValue;
                }

                propInfo.SetValue(result, value);
            }

            return result;
        }

        public Dictionary<Guid, object> MapEntityToData<TEntity>(Guid appId, TEntity entity)
        {
            var app = configService.GetApplications().FirstOrDefault(x => x.ID == appId);

            if (app == null)
            {
                throw new ArgumentException($"Application {appId} does not exist");
            }

            var result = new Dictionary<Guid, object>();
            var type = typeof(TEntity);

            foreach (var propInfo in type.GetProperties())
            {
                var field = app.Fields.FirstOrDefault(x => x.DBColumnName == propInfo.Name);

                if (field == null)
                {
                    continue;
                }

                result.Add(field.ID, propInfo.GetValue(entity));
            }

            return result;
        }

        public IEnumerable<FilterResult> ResolveFieldFilter(Guid fieldId, string value)
        {
            var apps = this.configService.GetApplications();
            var field = apps.SelectMany(m => m.Fields).SingleOrDefault(x => x.ID == fieldId);

            if (field == null)
            {
                throw new Exception($"Field with ID ${fieldId} not found in any application");
            }

            switch (field.FilterType)
            {
                default:
                case Entities.Application.FieldFilterType.None:
                    throw new Exception($"Field with ID ${fieldId} doesn't have a FilterType specified");
                case Entities.Application.FieldFilterType.View:
                    {
                        using (var db = new MainDatabase().Get)
                        {
                            return db.Query<FilterResult>($"SELECT * FROM {field.FilterCommand} ORDER BY DisplayOrder, FullName, Name");
                        }
                    }
                case Entities.Application.FieldFilterType.StoredProcedure:
                    {
                        using (var db = new MainDatabase().Get)
                        {
                            return db.Query<FilterResult>(field.FilterCommand, new { p1 = value }, commandType: System.Data.CommandType.StoredProcedure);
                        }
                    }
                case Entities.Application.FieldFilterType.Json:
                    {
                        return JsonConvert.DeserializeObject<IEnumerable<FilterResult>>(field.FilterData);
                    }
                case Entities.Application.FieldFilterType.Rest:
                    {
                        var baseUrl = this.applicationService.GetBaseUrl();
                        var apiUrl = baseUrl + field.FilterCommand;

                        using (var client = new HttpClient())
                        {
                            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var data = response.Content.ReadAsStringAsync().Result;

                                var mapper = JObject.Parse(field.FilterData);
                                var jArray = JArray.Parse(data);

                                int index = 0;

                                string valueField = mapper["value"].ToString();
                                string textField = mapper["text"].ToString();

                                return jArray.Select(jObject => new FilterResult
                                {
                                    DisplayOrder = index++,
                                    Code = jObject[valueField].ToString(),
                                    FullName = jObject[textField].ToString(),
                                    Name = jObject[textField].ToString()
                                }).ToList();
                            }
                            else
                            {
                                throw new Exception($"Got a error status code ${response.StatusCode} when requesting ${apiUrl} for field ${fieldId}");
                            }
                        }
                    }
            }
        }
    }
}
