using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Octacom.Odiss.Core.Identity.Contracts;
using Octacom.Odiss.Core.Identity.Entities;

namespace Octacom.Odiss.Core.Identity.Dapper
{
    public class OdissUserSessionStore : IUserSessionStore<Guid, Guid>
    {
        public async Task<IEnumerable<UserSession<Guid, Guid>>> ClearSessionsAsync()
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QueryAsync<UserSession<Guid, Guid>>(@"
DELETE FROM [dbo].[Sessions]
OUTPUT DELETED.*
WHERE Expire < GETDATE()");
            }
        }

        public async Task<Guid> CreateAsync(Guid userId, DateTime expiryDate)
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QuerySingleAsync<Guid>(@"
INSERT INTO [dbo].[Sessions] 
(ID, IDUser, Expire, Data) 
OUTPUT INSERTED.ID 
VALUES (NEWID(), @userId, @expiryDate, @data)", new { userId, expiryDate, data = new byte[0] });
            }
        }

        public async Task<UserSession<Guid, Guid>> GetAsync(Guid sessionId)
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QueryFirstOrDefaultAsync<UserSession<Guid, Guid>>(@"
SELECT TOP 1 ID AS SessionId, IDUser AS UserId, Expire AS ExpiryDate, Data, Created AS CreatedAt, LastAction AS LastActionAt 
FROM [dbo].[Sessions] 
WHERE ID = @sessionId 
ORDER BY LastAction DESC", new { sessionId });
            }
        }

        public async Task<UserSession<Guid, Guid>> GetByUserIdAsync(Guid userId)
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QueryFirstOrDefaultAsync<UserSession<Guid, Guid>>(@"
SELECT TOP 1 ID AS SessionId, IDUser AS UserId, Expire AS ExpiryDate, Data, Created AS CreatedAt, LastAction AS LastActionAt 
FROM [dbo].[Sessions] 
WHERE IDUser = @userId 
ORDER BY LastAction DESC", new { userId });
            }
        }

        public async Task<bool> RemoveAsync(Guid sessionId)
        {
            using (var db = new MainDatabase().Get)
            {
                var deletedId = await db.QueryFirstOrDefaultAsync<Guid>(@"
DELETE FROM [dbo].[Sessions]
OUTPUT DELETED.ID
WHERE ID = @sessionId", new { sessionId });

                return deletedId == sessionId;
            }
        }

        public async Task<bool> RemoveByUserIdAsync(Guid userId)
        {
            using (var db = new MainDatabase().Get)
            {
                var deletedId = await db.QueryFirstOrDefaultAsync<Guid>(@"
DELETE FROM [dbo].[Sessions] 
OUTPUT DELETED.IDUser
WHERE IDUser = @userId", new { userId });

                return deletedId == userId;
            }
        }

        public async Task<bool> UpdateAsync(UserSession<Guid, Guid> session)
        {
            using (var db = new MainDatabase().Get)
            {
                var updatedId = await db.QueryFirstOrDefaultAsync<Guid>(@"
UPDATE [dbo].[Sessions] 
SET Expire = @ExpiryDate, 
Data = @Data, 
LastAction = @LastActionAt
OUTPUT INSERTED.ID
WHERE ID = @SessionId", session);

                return updatedId == session.SessionId;
            }
        }
    }
}
