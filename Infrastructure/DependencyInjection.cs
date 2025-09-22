using Domain.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDapperExecutor, DapperExecutor>();

        services.AddScoped<IMySqlConnectionFactory>(_ => new MySqlConnectionFactory(connectionString));

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }
}