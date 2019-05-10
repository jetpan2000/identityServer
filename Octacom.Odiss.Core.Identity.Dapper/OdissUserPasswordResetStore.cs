using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Octacom.Odiss.Core.Identity.Contracts;

namespace Octacom.Odiss.Core.Identity.Dapper
{
    public class OdissUserPasswordResetStore : IUserPasswordResetStore
    {
        private TimeSpan expiryLength = TimeSpan.FromHours(2); // default value

        private const string INIT_DB_SQL = @"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'UserPasswordReset')
BEGIN
    CREATE TABLE [dbo].[UserPasswordReset]
	(
		Id UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_UserPasswordReset_UserId DEFAULT NEWSEQUENTIALID(),
		UserId UNIQUEIDENTIFIER NOT NULL,
		ExpiryDate DATETIME NOT NULL,
		ResetPasswordToken VARCHAR(200) NOT NULL,
		CONSTRAINT PK_UserPasswordReset PRIMARY KEY NONCLUSTERED (Id ASC),
		CONSTRAINT FK_UserPasswordReset_UserId FOREIGN KEY (UserId) REFERENCES [dbo].[Users]([ID]) ON UPDATE CASCADE ON DELETE CASCADE
	)
END
";

        public OdissUserPasswordResetStore()
        {
            InitializeDatabase();
        }

        public void Initialize(TimeSpan expiryLength)
        {
            this.expiryLength = expiryLength;
        }

        public async Task<UserPasswordReset> GetUserPasswordResetAsync(string passwordResetKey)
        {
            using (var db = new MainDatabase().Get)
            {
                return await db.QueryFirstAsync<UserPasswordReset>("SELECT TOP 1 CONVERT(NVARCHAR(50), Id) AS PasswordResetKey, UserId, ExpiryDate, ResetPasswordToken AS PasswordResetToken FROM [dbo].[UserPasswordReset] WHERE Id = @passwordResetKey ORDER BY ExpiryDate DESC", new { passwordResetKey });
            }
        }

        public async Task<string> RegisterPasswordResetTokenAsync(Guid userId, string passwordResetToken)
        {
            using (var db = new MainDatabase().Get)
            using (var transaction = db.BeginTransaction())
            {
                try
                {
                    var expiryDate = DateTime.Now.Add(expiryLength);

                    await db.ExecuteAsync("DELETE FROM [dbo].[UserPasswordReset] WHERE UserId = @userId", new { userId }, transaction: transaction);
                    var generatedId = await db.QuerySingleAsync<Guid>("INSERT INTO [dbo].[UserPasswordReset] (UserId, ExpiryDate, ResetPasswordToken) OUTPUT INSERTED.Id VALUES (@userId, @expiryDate, @passwordResetToken)", new { userId, expiryDate, passwordResetToken }, transaction: transaction);

                    transaction.Commit();

                    return generatedId.ToString();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task ClearPasswordResetTokensAsync(Guid userId)
        {
            using (var db = new MainDatabase().Get)
            {
                await db.ExecuteAsync("DELETE FROM [dbo].[UserPasswordReset] WHERE UserId = @userId", new { userId });
            }
        }

        private void InitializeDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["main"].ConnectionString;
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = connectionStringBuilder.InitialCatalog;

            using (var connection = new SqlConnection(connectionStringBuilder.ToString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = INIT_DB_SQL;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
