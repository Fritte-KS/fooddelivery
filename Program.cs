/*
    Dag 1
    * ASP.Net
    * Minimal API
    * Routing
    * DI Container
    * Model Binding

    Dag 2
    * Entity Framework (Lägg till dbcontext till samtliga metoder i OrderDelivery)
    * Asynkront kod i aspnet (Gör samtliga endpoints asynkrona)
    * Använda vårt api till något (Consume an API) (Skriv ett admingränssnitt)
    * (Gör en hemsida som kan hantera beställningar)
    * Inlämningsuppgift WebAPI

    Dag 3
    * Controllers och Actions
*/

using Microsoft.EntityFrameworkCore;

//Skapa först objektet WebApplicationBuilder som har till uppgift
//att samla ihop lite inställingar och sedan 'bygga' vårt slutgiltiga
//webapplication-objekt som är det som är vår faktiska server-klass
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Innan Build() körs läggs olika services till DI-containern
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=database.db"));
builder.Services.AddScoped<FoodDelivery>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Här skapas WebApplikationobjektet med de services som lagts till ovan
WebApplication webApp = builder.Build();

webApp.UseSwagger();
webApp.UseSwaggerUI();

webApp.UseCors("AllowAll");

//Nu kan vi konfigurera våra olika 'routes' med MapGet, MapPost osv.

webApp.MapGet("/orders", async (FoodDelivery fd) => await fd.GetAllOrdersAsync());

webApp.MapGet("/orders/{id}", async (int id, FoodDelivery fd) => await fd.GetOrderByIdAsync(id));

webApp.MapPost("/orders", (FoodOrderRequest fo, FoodDelivery fd) =>
{
    int id = fd.OrderFood(fo);

    if (id == -1)
    {
        return Results.BadRequest("Nope!");
    }

    return Results.Created("/orders/" + id, fo);
});

webApp.MapGet("/dishes", async (AppDbContext db) => await db.Dishes.Where(d => d.Name != "Noname").ToListAsync()).RequireCors("AllowAll");

webApp.MapPost("/dishes", (Dish dish, AppDbContext db, HttpContext ctx) =>
{
    if (ctx.Request.Headers["secretpasskey"] != "pingvin") return Results.Unauthorized();

    if (dish.Name == "Noname" || dish.Price < 5) return Results.BadRequest("Äh skärp dig.");

    db.Add(dish);
    db.SaveChanges();

    return Results.Created("/", dish);
});
webApp.UseStaticFiles();

//Här startar vår server och börjar lyssna på inkommande HTTP-requests
webApp.Run();
