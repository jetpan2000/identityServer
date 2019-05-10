using System;
using System.Collections.Generic;
using Dapper;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Entities.User;

namespace Octacom.Odiss.Core.DataLayer.User
{
    public class UserDocumentRepository : DbRepository<UserDocument, Guid, MainDatabase>, IUserDocumentRepository
    {
        public UserDocumentRepository() : base("dbo", "UsersDocuments", "ID", null)
        {

        }

        public IEnumerable<UserDocument> GetByUserId(Guid userId)
        {
            using (var db = new MainDatabase().Get)
            {
                return db.Query<UserDocument>("SELECT * FROM [dbo].[vw_GetUserDocuments] WHERE IDUser = @userId", new { userId });
            }
        }
    }
}
