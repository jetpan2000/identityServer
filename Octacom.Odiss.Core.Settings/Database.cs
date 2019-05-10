using System;
using System.Data;
using System.Data.SqlClient;

namespace Octacom.Odiss.Core.Settings
{
    internal class Database : IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _db;

        /// <summary>
        /// Use a specific connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Get
        {
            get
            {
                if (_db != null)
                    return _db;

                _db = new SqlConnection(_connectionString);

                if (_db.State == ConnectionState.Closed) _db.Open();

                return _db;
            }
        }

        public IDbConnection GetClosed
        {
            get
            {
                if (_db != null)
                    return _db;

                _db = new SqlConnection(_connectionString);

                return _db;
            }
        }

        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
