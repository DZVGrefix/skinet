
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*ezt én írom*/
builder.Services.AddDbContext<StoreContext>(opt => {
    //létrehozzok a kapcsolatott az adatbázissal amit a 
    //appsettings.Development.json beállítottunk connections részben
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//builder.Services.AddScoped<IProductRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
/*Eddig ...*/


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


/*SAJÁT adatbázis létrehozása ha nincs*/
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch(Exception ex)
{
    logger.LogError(ex, "AN ERROR OCCURED DURING MIGRATION");
}
/*SAJÁT Vége*/

app.Run();
