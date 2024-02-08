/*

    * Entity Framework
    * Asynkront kod i aspnet
    * Använda vårt api till något (Consume an API)
    * Inlämningsuppgift WebAPI

*/

using Microsoft.EntityFrameworkCore;

//Skapa först objektet WebApplicationBuilder som har till uppgift
//att samla ihop lite inställingar och sedan 'bygga' vårt slutgiltiga
//webapplication-objekt som är det som är vår faktiska server-klass
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Innan Build() körs läggs olika services till DI-containern
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=database.db"));
builder.Services.AddScoped<FoodDelivery>();

//Här skapas WebApplikationobjektet med de services som lagts till ovan
WebApplication webApp = builder.Build();

//Nu kan vi konfigurera våra olika 'routes' med MapGet, MapPost osv.
webApp.MapGet("/", () => "Welcome to our restaurant!");
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

webApp.MapGet("/dishes", async (AppDbContext db) => await db.Dishes.ToListAsync());

webApp.MapPost("/dishes", (Dish dish, AppDbContext db) =>
{
    db.Add(dish);
    db.SaveChanges();
});

//Här startar vår server och börjar lyssna på inkommande HTTP-requests
webApp.Run();
