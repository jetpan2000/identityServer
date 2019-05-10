using System;
using System.Collections.Generic;
using Dapper;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Entities.Application;

namespace Octacom.Odiss.Core.DataLayer.Application
{
    public class FieldRepository : DbRepository<Field, Guid, MainDatabase>, IFieldRepository
    {
        public FieldRepository() : base("dbo", "Fields", "ID", null)
        {
        }

        public IEnumerable<Field> GetFieldsByApplication(Guid applicationId)
        {
            using (var db = new MainDatabase().Get)
            {
                return db.Query<Field>($"SELECT * FROM {FullTableName} WHERE IDApplication = @applicationId", new { applicationId });
            }
        }
    }
}
