using Microsoft.EntityFrameworkCore;
using NewtonCatalog.Api.Data;
using NewtonCatalog.Api.Data.DataMigrations;
using NewtonCatalog.Api.Repositories;
using NewtonCatalog.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()));
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    await db.Database.MigrateAsync();
    await DM1_SeedInitialCatalog.MigrateAsync(db);
}

app.UseExceptionHandler();

app.MapGet("/openapi.yaml", () =>
    Results.File(Path.Combine(AppContext.BaseDirectory, "openapi.yaml"), "application/yaml"));
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi.yaml", "Newton Catalog API"));

app.MapControllers();

app.Run();
