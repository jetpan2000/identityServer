using System;
using System.Collections.Generic;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Contracts.Services;
using Octacom.Odiss.Core.Contracts.Infrastructure;
using Octacom.Odiss.Core.Entities.Settings;
using Octacom.Odiss.Core.Common;
using Octacom.Odiss.Core.Entities.Application;
using System.Linq;
using Octacom.Odiss.Core.Entities.Application.Custom;

namespace Octacom.Odiss.Core.Business
{
    public class ConfigService : IConfigService
    {
        private readonly ISettingsRepository settingsRepository;
        private readonly IApplicationRepository applicationRepository;
        private readonly IFieldRepository fieldRepository;
        private readonly IDatabaseRepository databaseRepository;
        private readonly ICachingService cachingService;

        public ConfigService(ISettingsRepository settingsRepository, IApplicationRepository applicationRepository, IFieldRepository fieldRepository, IDatabaseRepository databaseRepository, ICachingService cachingService)
        {
            this.settingsRepository = settingsRepository;
            this.applicationRepository = applicationRepository;
            this.fieldRepository = fieldRepository;
            this.databaseRepository = databaseRepository;
            this.cachingService = cachingService;
        }

        public ApplicationSettings GetApplicationSettings()
        {
            return cachingService.GetOrSet("AppSettings", () =>
            {
                var dict = settingsRepository.GetDictionary();

                var settings = dict.ToEntity<ApplicationSettings>(new Dictionary<string, Func<object, object>>()
                {
                    { "EnabledLanguages", (input) => ((string)input).Split(',') }
                });

                return settings;
            });
        }

        public IEnumerable<Application> GetApplications()
        {
            return cachingService.GetOrSet("Applications", () =>
            {
                var applications = applicationRepository.GetAll().ToList();
                var fields = fieldRepository.GetAll().ToList();
                var databases = databaseRepository.GetAll().ToList();

                foreach (var app in applications)
                {
                    app.DBSchema = databases.FirstOrDefault(a => a.ID == app.IDDatabase)?.DBSchema;
                    app.Fields = fields.Where(a => a.IDApplication == app.ID && string.IsNullOrEmpty(a.ShowInView)).ToArray();
                    app.FieldsItems = fields.Where(a => a.IDApplication == app.ID && a.ShowInView == "Items").ToArray();
                    app.FieldsSummary = fields.Where(a => a.IDApplication == app.ID && a.ShowInView == "Summary").ToArray();
                    app.FieldsComment = fields.Where(a => a.IDApplication == app.ID && a.ShowInView == "Comment").ToArray();

                    if (!string.IsNullOrEmpty(app.CustomData))
                    {
                        try
                        {
                            app.Custom = app.CustomData.DeserializeJSON<AppCustomData>();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                return applications;
            });
        }

        public string GetDefaultEmail()
        {
            return "odiss.admin@octacom.ca";
        }
    }
}
