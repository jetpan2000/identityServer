using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories;
using Octacom.Odiss.Core.Contracts.Repositories.Searching;
using Octacom.Odiss.Core.Entities.User;

namespace Octacom.Odiss.Core.DataLayer.User
{
    public class UserRepository : DbRepository<Entities.User.User, Guid, MainDatabase>, IUserRepository
    {
        public UserRepository() : base("dbo", "Users", "ID", null)
        {
        }

        public Entities.User.User GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            using (var db = new MainDatabase().Get)
            {
                var user = db.Query<Entities.User.User>("SELECT ID, UserName, Type, FirstName, LastName, Email, Permissions FROM Users WHERE Email = @Email",
                    new
                    {
                        Email = email
                    }).FirstOrDefault();

                return user;
            }
        }

        public bool EmailAlreadyRegistered(string email, Guid? currentUID)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            using (var db = new MainDatabase().Get)
            {
                if (currentUID.HasValue)
                {
                    var userCount = db.Query<int>("SELECT COUNT(1) FROM Users WHERE Email = @Email AND ID <> @ID",
                        new
                        {
                            Email = email,
                            ID = currentUID.Value
                        }).FirstOrDefault();

                    return userCount > 0;
                }

                return GetByEmail(email) != null;
            }
        }

        public SearchResult<Entities.User.User> Search(UserSearchParameters parameters)
        {
            return DapperHelper.SearchDapper<Entities.User.User, SearchParameters, MainDatabase>(parameters, FullTableName, new MainDatabase());
        }

        public Entities.User.User GetByUsername(string username)
        {
            using (var db = new MainDatabase().Get)
            {
                return db.QueryFirstOrDefault<Entities.User.User>($"SELECT * FROM {FullTableName} WHERE UserName = @username", new { username });
            }
        }

        protected override void SetKey(Entities.User.User item, Guid key)
        {
            item.Id = key;
        }

        public override IEnumerable<string> ColumnNames => base.ColumnNames.Except(new string[] { "DocumentsChanged", "PasswordChanged" });
    }
}
