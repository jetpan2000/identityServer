using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Dapper;
using Octacom.Odiss.Core.Contracts.Infrastructure;
using Octacom.Odiss.Core.Contracts.Settings;

namespace Octacom.Odiss.Core.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly ICachingService cachingService;
        private readonly IDbContextFactory<DbContext> dbContextFactory;
        private const string CACHE_KEY = "AppSettings";

        public SettingsService(ICachingService cachingService, IDbContextFactory<DbContext> dbContextFactory)
        {
            this.cachingService = cachingService;
            this.dbContextFactory = dbContextFactory;
        }

        public Contracts.Settings.Entities.Settings Get()
        {
            return cachingService.GetOrSet(CACHE_KEY, () =>
            {
                using (var ctx = dbContextFactory.Create())
                using (var db = new Database(ctx.Database.Connection.ConnectionString).Get)
                {
                    var result = db.Query("SELECT * FROM [dbo].[Settings]");
                    var dictionary = result.ToDictionary(x => (string)x.Name, x => x.Value);

                    return ToEntity<Contracts.Settings.Entities.Settings>(dictionary);
                }
            });
        }

        public void Save(Contracts.Settings.Entities.Settings settings)
        {
            throw new NotImplementedException();

            cachingService.Expire(CACHE_KEY);
        }

        internal static TEntity ToEntity<TEntity>(Dictionary<string, object> dictionary, Dictionary<string, Func<object, object>> customResolvers = null)
             where TEntity : class, new()
        {
            TEntity entity = new TEntity();
            var type = typeof(TEntity);

            foreach (var kvp in dictionary)
            {
                var property = type.GetProperty(kvp.Key);

                if (property == null || !property.CanWrite)
                {
                    continue;
                }

                object value = kvp.Value;

                if (customResolvers != null && customResolvers.ContainsKey(kvp.Key))
                {
                    value = customResolvers[kvp.Key](value);
                }
                else if (property.PropertyType != typeof(string))
                {
                    value = Convert.ChangeType(value, property.PropertyType);
                }

                property.SetValue(entity, value);
            }

            return entity;
        }
    }
}