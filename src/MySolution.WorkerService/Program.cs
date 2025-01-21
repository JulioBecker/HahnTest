using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MySolution.Infrastructure.Data;
using MySolution.Domain.Interfaces;
using MySolution.Infrastructure.Repositories;
using MySolution.Application.Services;
using MySolution.BackgroundJobs;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ProductService>();

        services.AddHttpClient();

        services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(
                      configuration.GetConnectionString("DefaultConnection"),
                      new SqlServerStorageOptions
                      {
                          SchemaName = "Hangfire",
                          PrepareSchemaIfNecessary = true
                      });
        });

        services.AddHangfireServer();

        services.AddTransient<UpsertProductsJob>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var upsertJob = serviceProvider.GetRequiredService<UpsertProductsJob>();

    RecurringJob.AddOrUpdate<UpsertProductsJob>(
        "upsert-products-job",
        job => job.ExecuteAsync(),
        "0 * * * *" // Cron expression - run every start of the hour
    );
}

await host.RunAsync();
