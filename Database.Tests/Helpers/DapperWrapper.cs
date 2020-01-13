using Dapper;
using System.Collections.Generic;
using System.Data;

namespace Database.Tests.Helpers
{
    public class DapperWrapper
    {
        public int Execute(IDbConnection dbConnection, string sql, object param = null)
        {
            var numberOfRowsAffected = dbConnection.Execute(sql, param);
            return numberOfRowsAffected;
        }

        public IEnumerable<T> Query<T>(IDbConnection dbConnection, string sql, object param = null)
        {
            return dbConnection.Query<T>(sql, param);
        }
    }
}
