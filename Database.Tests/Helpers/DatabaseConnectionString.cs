using Microsoft.Extensions.Configuration;
using System;


namespace Database.Tests.Helpers
{
    public static class DatabaseConnectionString
    {
        private static readonly IConfiguration _configuration = InitConfiguration();

        public static string UniqueDatabaseId => Guid.NewGuid().ToString().Replace("-", "_");

        public static string GetDatabaseConnectionString()
        {
            var connectionString = _configuration.GetConnectionString("SampleDatabase");
            var buildConfiguration = _configuration.GetSection("BuildConfiguration").Value;
            var updatedConnectionString = connectionString.Replace(buildConfiguration, $"Debug_{UniqueDatabaseId}");
            return updatedConnectionString;
        }

        private static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return config;
        }
    }
}
