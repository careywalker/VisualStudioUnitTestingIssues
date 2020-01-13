using Database.Tests.Helpers;
using Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Cbi.FinancialSharedService.Database.Tests.StoredProcedures
{
    [TestClass]
    public class InternalSelectMessageTests
    {
        private readonly string _sampleDatabaseConnection = DatabaseConnectionString.GetDatabaseConnectionString();
        private readonly DapperWrapper _dapperWrapper = new DapperWrapper();

        [TestMethod]
        public void SelectMessage_WhenCalledWithValidFilters_ReturnsMessagesThatMatchTheFilters()
        {
            //Arrange
            int firstRecordid;
            int secondRecordId;
            var uniqueEndpointIdentifier = Guid.NewGuid().ToString().Replace("-", "_");
            Message message;
            var baselineDateTime = DateTime.UtcNow;
            using (var connection = new SqlConnection(_sampleDatabaseConnection))
            {
                firstRecordid = _dapperWrapper.Query<int>(connection,
                    "exec internal.UpsertMessage @Id, @Identifier, @EndPoint", new
                    {
                        Id = 0,
                        Identifier = "IECBIBRS201911180000000000000001",
                        EndPoint = uniqueEndpointIdentifier
                    }).Single();

                secondRecordId = _dapperWrapper.Query<int>(connection,
                    "exec internal.UpsertMessage @Id, @Identifier, @EndPoint", new
                    {
                        Id = 0,
                        Identifier = "IECBIBRS201911180000000000000002",
                        EndPoint = uniqueEndpointIdentifier
                    }).Single();
            }

            //Act
            using (var connection = new SqlConnection(_sampleDatabaseConnection))
            {
                message = _dapperWrapper.Query<Message>(connection, "exec internal.SelectMessage", new
                {
                }).FirstOrDefault();
            }

            //Assert
            Assert.IsNotNull(message);
            Assert.AreEqual(firstRecordid, message.Id);
            Assert.AreEqual("IECBIBRS201911180000000000000001", message.Identifier);
            Assert.AreEqual(uniqueEndpointIdentifier, message.EndPoint);
        }
    }
}