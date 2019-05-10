using System;
using System.Collections.Generic;
using Dapper;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories;

namespace Octacom.Odiss.Core.DataLayer.Application
{
    public class ApplicationRepository : DbRepository<Entities.Application.Application, Guid, MainDatabase>, IApplicationRepository
    {
        public ApplicationRepository() : base("dbo", "Applications", "ID", null)
        {
        }

        public IEnumerable<Entities.Application.Application> GetByUserId(Guid userId)
        {
            using (var db = new MainDatabase().Get)
            {
                string sql = @"
                    SELECT A.*
                    FROM [dbo].[Applications] AS A
                    LEFT JOIN [dbo].[UsersApplications] AS UA ON UA.IDApplication = A.ID
                    WHERE UA.IDUser = @userId
                    ";

                return db.Query<Entities.Application.Application>(sql, new { userId });
            }
        }
    }
}
