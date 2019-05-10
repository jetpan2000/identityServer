using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Dapper;
using Newtonsoft.Json.Linq;
using Octacom.Odiss.Core.Contracts.Infrastructure;
using Octacom.Odiss.Core.Contracts.Settings;
using Octacom.Odiss.Core.Contracts.Settings.Entities;
using Octacom.Odiss.Core.Contracts.Validation;

namespace Octacom.Odiss.Core.Settings
{
    public class ApplicationService : IApplicationService
    {
        private const string CACHE_KEY = "ApplicationService";
        private readonly ICachingService cachingService;
        private readonly IDbContextFactory<DbContext> dbContextFactory;
        private readonly ApplicationTypeRegistry registry;
        private readonly IFieldValidationProvider validationProvider;

        public ApplicationService(ICachingService cachingService, IDbContextFactory<DbContext> dbContextFactory, ApplicationTypeRegistry registry, IFieldValidationProvider validationProvider)
        {
            this.cachingService = cachingService;
            this.dbContextFactory = dbContextFactory;
            this.registry = registry;
            this.validationProvider = validationProvider;
        }

        public Application Get(Type type)
        {
            if (!registry.HasTypeRegistered(type))
            {
                return null;
            }

            var identifier = registry.GetIdentifier(type);

            return Get(identifier);
        }

        public Application Get(string identifier)
        {
            return cachingService.GetOrSet($"{CACHE_KEY}_{identifier}", () =>
            {
                using (var ctx = dbContextFactory.Create())
                using (var db = new Database(ctx.Database.Connection.ConnectionString).Get)
                {
                    var results = db.QueryMultiple(@"
SELECT CONVERT(VARCHAR(50), ID) AS Identifier, Name AS DisplayName, CustomData, EnableCreate, EnableUpdate, EnableDelete FROM [dbo].[Applications]
WHERE ID = @identifier

SELECT * FROM [dbo].[Fields]
WHERE IDApplication = @identifier
", new { identifier });

                    var application = results.ReadFirstOrDefault<Application>();

                    if (application == null)
                    {
                        return null;
                    }

                    var fieldResult = results.Read<FieldResult>();

                    application.Identifier = application.Identifier.ToLower();

                    var searchVisibilities = new int[] { 0, 1 };
                    var gridVisibilities = new int[] { 0, 2, 6 };
                    var propertyVisibilities = new int[] { 0, 4 };

                    var searchFields = new List<SearchField>();
                    var gridFields = new List<GridField>();
                    var propertyFields = new List<PropertyField>();
                    var lookupPropertyFields = new List<LookupPropertyField>();
                    var hiddenFields = new List<Field>();

                    foreach (var field in fieldResult)
                    {
                        if (searchVisibilities.Contains(field.VisibilityType) && field.NotVisibleFilter != true)
                        {
                            searchFields.Add(field.ToSearchField());
                        }

                        if (gridVisibilities.Contains(field.VisibilityType) && field.NotVisibleList != true)
                        {
                            gridFields.Add(field.ToGridField());
                        }

                        if (propertyVisibilities.Contains(field.VisibilityType) && field.NotVisibleViewer != true)
                        {
                            if (field.Type == 10)
                            {
                                var lookupField = field.ToLookupPropertyField();
                                lookupPropertyFields.Add(lookupField);
                                propertyFields.Add(lookupField);
                            }
                            else
                            {
                                propertyFields.Add(field.ToPropertyField());
                            }
                        }

                        if (field.NotVisibleFilter == true && field.NotVisibleList == true && field.NotVisibleViewer == true)
                        {
                            hiddenFields.Add(field.ToField<Field>());
                        }
                    }

                    application.SearchFields = searchFields;
                    application.GridFields = gridFields;
                    application.PropertyFields = propertyFields;
                    application.LookupPropertyFields = lookupPropertyFields;
                    application.HiddenFields = hiddenFields;

                    var validationRules = validationProvider
                        .GetRules(identifier)
                        .GroupBy(x => x.FieldIdentifier);

                    foreach (var propertyField in application.PropertyFields)
                    {
                        var propertyRules = validationRules.FirstOrDefault(x => x.Key == propertyField.Identifier);

                        if (propertyRules == null)
                        {
                            continue;
                        }

                        propertyField.ValidationRules = propertyRules.ToList();
                    }

                    return application;
                }
            });
        }

        private SearchFieldConfiguration ParseSearchFieldConfiguration(string filterData)
        {
            if (string.IsNullOrEmpty(filterData))
            {
                return null;
            }

            var obj = JObject.Parse(filterData);

            var search = obj.ContainsKey("search") ? obj["search"].ToObject<SearchFieldConfiguration>() : new SearchFieldConfiguration();
            search.DisplayFormat = obj.ContainsKey("displayFormat") ? obj["displayFormat"].ToString() : null;

            if (obj.ContainsKey("value") && obj.ContainsKey("text"))
            {
                search.AutocompleteConfiguration = new SearchFieldAutocompleteConfiguration
                {
                    Text = obj["text"].ToString(),
                    Value = obj["value"].ToString()
                };
            }

            return search;
        }
    }
}