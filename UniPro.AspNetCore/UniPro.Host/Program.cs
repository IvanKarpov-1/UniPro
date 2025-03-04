using Carter;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UniPro.Features;
using UniPro.Host;
using UniPro.Infrastructure;
using UniPro.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Add infrastructure dependencies to the DI
builder.Services.AddSecurity(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddFeatures(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await using var scope = app.Services.CreateAsyncScope();
    await using var uniProDbContext = scope.ServiceProvider.GetRequiredService<UniProDbContext>();
    await uniProDbContext.Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.MapGet("api/test", () => "Hello World!");

app.Run();
