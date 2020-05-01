using FluentAssertions;
using Atlas.MatchingAlgorithm.Data.Persistent.Models;
using Atlas.MatchingAlgorithm.Data.Services;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders.TransientSqlDatabase;
using Atlas.MatchingAlgorithm.Services.ConfigurationProviders.TransientSqlDatabase.ConnectionStringProviders;
using Atlas.MatchingAlgorithm.ConfigSettings;
using NSubstitute;
using NUnit.Framework;

namespace Atlas.MatchingAlgorithm.Test.Services.ConfigurationProviders
{
    [TestFixture]
    public class ActiveTransientSqlConnectionStringProviderTests
    {
        private readonly ConnectionStrings connectionStrings = new ConnectionStrings
        {
            TransientA = "connection-A",
            TransientB = "connection-B"
        };

        private IActiveDatabaseProvider activeDatabaseProvider;

        private IConnectionStringProvider activeConnectionStringProvider;
        
        [SetUp]
        public void SetUp()
        {
            activeDatabaseProvider = Substitute.For<IActiveDatabaseProvider>();
            
            activeConnectionStringProvider = new ActiveTransientSqlConnectionStringProvider(connectionStrings, activeDatabaseProvider);
        }

        [Test]
        public void GetConnectionString_WhenDatabaseAActive_ReturnsDatabaseA()
        {
            activeDatabaseProvider.GetActiveDatabase().Returns(TransientDatabase.DatabaseA);
            
            var connectionString = activeConnectionStringProvider.GetConnectionString();

            connectionString.Should().Be(connectionStrings.TransientA);
        }

        [Test]
        public void GetConnectionString_WhenDatabaseBActive_ReturnsDatabaseB()
        {
            activeDatabaseProvider.GetActiveDatabase().Returns(TransientDatabase.DatabaseB);
            
            var connectionString = activeConnectionStringProvider.GetConnectionString();

            connectionString.Should().Be(connectionStrings.TransientB);
        }
    }
}