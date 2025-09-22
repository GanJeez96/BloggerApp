using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BloggerApp.Tests.Integration;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public string ConnectionString { get; set; } = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Integration");
        
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(IMySqlConnectionFactory));
            services.AddScoped<IMySqlConnectionFactory>(_ => new MySqlConnectionFactory(ConnectionString));
        });
    }
}