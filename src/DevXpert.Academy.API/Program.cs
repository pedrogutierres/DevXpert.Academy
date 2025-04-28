using DevXpert.Academy.API.Configurations;
using DevXpert.Academy.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfig();
builder.Services.AddCorsConfig(builder.Configuration);
builder.Services.AddOpenApi(); 
builder.Services.AddDbContextConfig(builder.Configuration, builder.Environment);
builder.Services.AddApiSecurity(builder.Configuration);
builder.Services.AddSwaggerConfig();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddDIConfiguration();

var app = builder.Build();

app.ConfigureExceptionHandler(app.Environment, app.Services.GetRequiredService<ILoggerFactory>());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
    app.UseCors("Development");
    //DbMigrationHelper.SeedDataAsync(app).Wait();
}
else
    app.UseCors("Production");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.UseDbMigrationHelper();

app.Run();
