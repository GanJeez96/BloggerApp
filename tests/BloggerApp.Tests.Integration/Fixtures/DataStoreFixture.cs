using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Testcontainers.MySql;
using Xunit;

namespace BloggerApp.Tests.Integration.Fixtures;

public class DataStoreFixture : IAsyncLifetime
{
    private MySqlContainer MySqlContainer { get; set; } 
    private IContainer FlywayContainer { get; set; } 
    private INetwork ContainerNetwork; 
    
    private readonly string ContainerNetworkAlias = "integration-tests-network"; 
    private readonly string _username = "testuser"; 
    private readonly string _password = "testpass"; 
    private readonly string _database = "bloggerappdbtest";
    private int _mySqlPort;
    public string ConnectionString { get; private set; } = string.Empty;
    
    public async Task InitializeAsync()
    {
        ContainerNetwork = new NetworkBuilder() 
            .WithName(Guid.NewGuid().ToString("D")) 
            .Build(); 
        
        await SetupMySqlContainer(); 
        await SetupFlywayContainer(); 
    }

    public async Task DisposeAsync()
    {
        await MySqlContainer.DisposeAsync(); 
        await FlywayContainer.DisposeAsync();
    } 
    
    #region Private Methods

    private async Task SetupMySqlContainer()
    {
        MySqlContainer = new MySqlBuilder() 
            .WithName("integration-tests-mysql") 
            .WithDatabase(_database) 
            .WithUsername(_username) 
            .WithPassword(_password) 
            .WithImage("mysql:8.0") 
            .WithNetwork(ContainerNetwork) 
            .WithNetworkAliases(ContainerNetworkAlias) 
            .WithCommand( 
                "--character-set-server=utf8mb4", 
                "--collation-server=utf8mb4_0900_ai_ci", 
                "--log-bin-trust-function-creators=1") 
            .WithWaitStrategy(Wait.ForUnixContainer() 
                .UntilCommandIsCompleted("mysqladmin ping -h localhost -u root -p" + _password)) 
            .Build(); 
        
        await MySqlContainer.StartAsync(); 
        
        _mySqlPort = MySqlContainer.GetMappedPublicPort(3306);
        
        ConnectionString = $"Server=localhost;Port={_mySqlPort};Database={_database};User={_username};Password={_password};";
    }

    private async Task SetupFlywayContainer()
    {
        FlywayContainer = new ContainerBuilder() 
            .WithName("integration-tests-flyway") 
            .WithImage("flyway/flyway:latest") 
            .WithResourceMapping("../../../../../migrations", "/flyway/migrations") 
            .WithResourceMapping("../../../../../flyway.conf", "/flyway/conf") 
            .WithNetwork(ContainerNetwork) 
            .WithCommand( $"-url=jdbc:mysql://{ContainerNetworkAlias}:3306/{_database}?allowPublicKeyRetrieval=true", 
                $"-user={_username}", 
                $"-password={_password}", 
                "migrate") 
            .WithWaitStrategy(Wait.ForUnixContainer() 
                .UntilMessageIsLogged("Successfully applied")) 
            .Build(); 
        
        await FlywayContainer.StartAsync();
    }

    #endregion
}