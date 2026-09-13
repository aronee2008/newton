using Microsoft.EntityFrameworkCore;
using NewtonCatalog.Api.Data;
using NewtonCatalog.Api.Data.DataMigrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogueDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()));

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogueDbContext>();
    await db.Database.MigrateAsync();
    await DataMigrationRunner.RunAsync(db);
}

app.MapGet("/", () => "Hello World from NewtonCatalog.Api");

app.Run();
