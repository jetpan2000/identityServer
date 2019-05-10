using System;
using System.Collections.Generic;
using Dapper;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories;

namespace Octacom.Odiss.Core.DataLayer.User
{
    public class UserGroupRepository : DbRepository<Entities.User.UserGroup, Guid, MainDatabase>, IUserGroupRepository
    {
        public UserGroupRepository() : base("dbo", "Applications", "ID", null)
        {
        }

        public IEnumerable<Entities.User.UserGroup> GetByUserId(Guid userId)
        {
            using (var db = new MainDatabase().Get)
            {
                string sql = @"
                    SELECT G.*
                    FROM [dbo].[Groups] AS G
                    LEFT JOIN [dbo].[UsersGroups] AS UG ON UG.IDGroup = G.ID
                    WHERE UG.IDUser = @userId
                    ";

                return db.Query<Entities.User.UserGroup>(sql, new { userId });
            }
        }
    }
}
