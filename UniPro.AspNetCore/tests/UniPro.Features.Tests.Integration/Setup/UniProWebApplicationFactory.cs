using System.Data.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;
using UniPro.Infrastructure.Database;
using Task = System.Threading.Tasks.Task;

namespace UniPro.Features.Tests.Integration.Setup;

public class UniProWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default!;

    public UniProDbContext DbContext { get; private set; } = default!;
    
    private readonly PostgreSqlContainer _dbContainer
        = new PostgreSqlBuilder()
            .WithImage(new PostgreSqlConfiguration().Image)
            .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "TestScheme";
                    options.DefaultChallengeScheme = "TestScheme";
                    options.DefaultScheme = "TestScheme";
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

            services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder("TestScheme")
                    .RequireAuthenticatedUser()
                    .Build());

            services.RemoveAll<UniProDbContext>();

            services.AddDbContext<UniProDbContext>(dbContextOptionsBuilder => dbContextOptionsBuilder
                .EnableSensitiveDataLogging()
                .UseNpgsql(_dbContainer.GetConnectionString())
                .UseSnakeCaseNamingConvention());
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        var script = await File.ReadAllTextAsync("Setup/super_tokens_tables.sql");
        await _dbContainer.ExecScriptAsync(script);
        
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await InitializeRespawner();
        
        var optionsBuilder = new DbContextOptionsBuilder<UniProDbContext>()
            .EnableSensitiveDataLogging()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .UseSnakeCaseNamingConvention();
        DbContext = new UniProDbContext(optionsBuilder.Options);
        
        HttpClient = CreateClient();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}