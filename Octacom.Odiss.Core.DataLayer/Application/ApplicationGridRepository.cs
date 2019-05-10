using System.Collections.Generic;
using System;
using Dapper;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Entities.Application.Custom;
using System.Linq;
using Octacom.Odiss.Core.Entities.Application;
using System.Data;
using Octacom.Odiss.Core.Contracts.Repositories.Searching;

namespace Octacom.Odiss.Core.DataLayer.Application
{
    public class ApplicationGridRepository : IApplicationGridRepository
    {
        private readonly IApplicationRepository applicationRepository;
        private readonly IDatabaseRepository databaseRepository;
        private readonly IFieldRepository fieldRepository;
        private readonly IServiceProvider serviceProvider;
        private Entities.Application.Application application;
        private Entities.Application.Database database;
        private IEnumerable<Field> fields;

        private IDictionary<Guid, Type> applicationTypeMappings;

        public ApplicationGridRepository(IApplicationRepository applicationRepository, IDatabaseRepository databaseRepository, IFieldRepository fieldRepository, IServiceProvider serviceProvider)
        {
            this.applicationRepository = applicationRepository;
            this.databaseRepository = databaseRepository;
            this.fieldRepository = fieldRepository;
            this.serviceProvider = serviceProvider;
        }

        public IEnumerable<dynamic> GetAll(Guid appId)
        {
            SetData(appId);
            var columns = fields.Select(x => x.DBColumnName);

            using (var db = new MainDatabase().Get)
            {
                return db.Query($"SELECT {string.Join(", ", columns)} FROM {database.DBSchema}.{application.TableName}");
            }
        }

        public dynamic Insert(Guid appId, Dictionary<Guid, object> obj)
        {
            SetData(appId);
            var columns = fields.Select(x => x.DBColumnName);
            var fullTableName = $"{database.DBSchema}.{application.TableName}";

            using (var db = new MainDatabase().Get)
            {
                var (primaryKey, primaryKeyField) = TryGetPrimaryKeyData(db);

                var inserts = fields.Where(x => x.DBColumnName != primaryKey)
                    .Select(x => $"@{x.DBColumnName}");

                var param = new DynamicParameters();
                foreach (var field in fields)
                {
                    if (obj.ContainsKey(field.ID))
                    {
                        param.Add($"@{field.DBColumnName}", obj[field.ID]);
                    }
                }

                return db.QuerySingle($"INSERT INTO {fullTableName} ({string.Join(", ", fields.Where(x => x.DBColumnName != primaryKey).Select(x => x.DBColumnName))}) OUTPUT INSERTED.* VALUES ({string.Join(", ", inserts)})", param);
            }
        }

        public void Update(Guid appId, Dictionary<Guid, object> obj)
        {
            SetData(appId);
            var columns = fields.Select(x => x.DBColumnName);
            var fullTableName = $"{database.DBSchema}.{application.TableName}";

            using (var db = new MainDatabase().Get)
            {
                var (primaryKey, primaryKeyField) = TryGetPrimaryKeyData(db);

                var updates = fields.Where(x => x.DBColumnName != primaryKey)
                    .Where(x => x.Editable)
                    .Select(x => $"{x.DBColumnName} = @{x.DBColumnName}");

                var param = new DynamicParameters();
                foreach (var field in fields)
                {
                    if (obj.ContainsKey(field.ID) && field.Editable || field.DBColumnName == primaryKey)
                    {
                        param.Add($"@{field.DBColumnName}", obj[field.ID]);
                    }
                }

                // TODO - Update this query to have update concurrency protection
                db.Execute($"UPDATE {fullTableName} SET {string.Join(", ", updates)} WHERE {primaryKeyField.DBColumnName} = @{primaryKeyField.DBColumnName}", param);
            }
        }

        public void Delete(Guid appId, object recordKey)
        {
            SetData(appId);
            var columns = fields.Select(x => x.DBColumnName);
            var fullTableName = $"{database.DBSchema}.{application.TableName}";

            using (var db = new MainDatabase().Get)
            {
                var (primaryKey, primaryKeyField) = TryGetPrimaryKeyData(db);

                db.Execute($"DELETE FROM {fullTableName} WHERE {primaryKey} = @recordKey", new { recordKey });
            }
        }

        private (string primaryKey, Field primaryKeyField) TryGetPrimaryKeyData(IDbConnection dbConnection)
        {
            var tableNameSplit = application.TableName.Split('.'); // 0 is schema, 1 is table name
            var fullTableName = $"{database.DBSchema}.{application.TableName}";

            // Figure out which is the primary key
            var primaryKey = dbConnection.QueryFirstOrDefault<string>("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE TABLE_NAME = @tableName AND TABLE_SCHEMA = @schemaName", new { tableName = tableNameSplit[1], schemaName = tableNameSplit[0] });

            if (string.IsNullOrEmpty(primaryKey))
            {
                throw new Exception($"Table {fullTableName} doesn't have primary key");
            }

            var primaryField = fields.SingleOrDefault(x => x.DBColumnName == primaryKey);

            if (primaryField == null)
            {
                throw new Exception($"Primary key {primaryKey} of table {fullTableName} isn't specified as a field in the application {application.Name}.");
            }

            return (primaryKey, primaryField);
        }

        private void SetData(Guid appId)
        {
            this.application = applicationRepository.Get(appId);

            if (this.application == null)
            {
                throw new Exception("No application found with id " + appId);
            }
            else if (this.application.Type != ApplicationType.DataGrid)
            {
                throw new Exception($"Application {appId} is expected to be of type {ApplicationType.DataGrid.ToString()} but is instead of type {this.application.Type.ToString()}");
            }

            this.fields = fieldRepository.GetFieldsByApplication(appId);

            this.database = databaseRepository.Get(this.application.IDDatabase);

            if (this.database == null)
            {
                throw new Exception("No database found with id " + this.application.IDDatabase);
            }
        }

        public dynamic Search(Guid appId, SearchOptions searchOptions)
        {
            if (!applicationTypeMappings.ContainsKey(appId))
            {
                throw new Exception($"Application {appId} does not have a entity type registered. It's currently required in order to be able to perform search with it (until we have a ISearchEngine which works without Entity Framework). To resolve this issue register in the DI container a initializer which calls SetMappings and has the application Id mapped to a entity.");
            }

            var type = applicationTypeMappings[appId];
            var searchEngineType = typeof(ISearchEngine<>).MakeGenericType(type);
            var searchEngine = serviceProvider.GetService(searchEngineType);

            var method = searchEngineType.GetMethod("Search", new Type[] { typeof(SearchOptions) });
            return method.Invoke(searchEngine, new object[] { searchOptions });
        }

        public void SetMappings(IDictionary<Guid, Type> mappings)
        {
            this.applicationTypeMappings = mappings;
        }
    }
}
