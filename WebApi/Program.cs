using Application.Behaviors;
using Application.Commands;
using Application.Validators;
using Domain.Repositories;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Dapper;
using MediatR;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Setup MySql
builder.Services.AddScoped<IDapperExecutor, DapperExecutor>();
var connectionString = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddScoped<IMySqlConnectionFactory>(_ => new MySqlConnectionFactory(connectionString));


// Register Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Blogger API", Version = "v1" });
});

// Inject MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreatePostCommand>());

// Inject Repositories
builder.Services.AddScoped<IPostRepository, PostRepository>();

// Inject FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreatePostCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();