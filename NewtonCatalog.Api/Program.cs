using Microsoft.EntityFrameworkCore;
using NewtonCatalog.Api.Data;
using NewtonCatalog.Api.Data.DataMigrations;
using NewtonCatalog.Api.Repositories;
using NewtonCatalog.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<CatalogueDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()));
builder.Services.AddScoped<IVideoGameRepository, VideoGameRepository>();
builder.Services.AddScoped<IVideoGameService, VideoGameService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogueDbContext>();
    await db.Database.MigrateAsync();
    await DataMigrationRunner.RunAsync(db);
}

app.UseExceptionHandler();

app.MapGet("/openapi.yaml", () =>
    Results.File(Path.Combine(AppContext.BaseDirectory, "openapi.yaml"), "application/yaml"));
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi.yaml", "Newton Catalog API"));

app.MapControllers();

app.Run();
