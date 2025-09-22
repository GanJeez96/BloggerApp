using Application;
using Infrastructure;
using WebApi;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

var connectionString = builder.Configuration.GetConnectionString("MySql");
builder.Services.AddInfrastructure(connectionString);

builder.Services.AddApiServices();

builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program;