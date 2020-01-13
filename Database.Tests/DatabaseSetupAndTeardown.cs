using System;
using Microsoft.SqlServer.Dac;
using System.Data.SqlClient;
using System.IO;
using Database.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace Cbi.FinancialSharedService.Database.Tests
{
    [TestClass]
    public class DatabaseSetupAndTeardown
    {
        private static string MasterConnectionString = string.Empty;
        private static readonly string DatabaseName = $"SampleDatabase_Debug_{DatabaseConnectionString.UniqueDatabaseId}";

        [AssemblyInitialize()]
        public static void SetupDatabase(TestContext testContext)
        {
            var configuration = InitConfiguration();
            MasterConnectionString = configuration.GetConnectionString("MasterDatabase");
            var dacServices = new DacServices(MasterConnectionString);
            var dacpacPath = GetDacPacFilePath();

            var dacDeployOptions = new DacDeployOptions
            {
                CreateNewDatabase = true
            };

            dacDeployOptions.SqlCommandVariableValues.Add("Environment", "Debug");

            dacServices.Deploy(
                DacPackage.Load(dacpacPath),
                DatabaseName,
                true,
                options: dacDeployOptions
            );
        }

        [AssemblyCleanup]
        public static void TearDown()
        {
            var offlineCommand = $"ALTER DATABASE {DatabaseName} SET OFFLINE WITH ROLLBACK IMMEDIATE";
            var onlineCommand = $"ALTER DATABASE {DatabaseName} SET ONLINE;";
            var dropCommand = $"DROP DATABASE {DatabaseName}";

            using (var sqlConnection = new SqlConnection(MasterConnectionString))
            {
                sqlConnection.Open();
                var sqlCommand = new SqlCommand
                {
                    Connection = sqlConnection
                };

                sqlCommand.CommandText = offlineCommand;
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = onlineCommand;
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = dropCommand;
                sqlCommand.ExecuteNonQuery();
            }
        }

        private static string GetDacPacFilePath()
        {
            var currentPath = AppDomain.CurrentDomain.BaseDirectory.Replace("netcoreapp3.0", "");
            var databaseProjectName = "Database";
            var databaseTestProjectName = "Database.Tests";
            return Path.Combine(currentPath.Replace(databaseTestProjectName, databaseProjectName), "Database.dacpac");
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