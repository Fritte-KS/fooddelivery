var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<FoodDelivery>();

WebApplication webApp = builder.Build();

webApp.MapGet("/", () => "Welcome to our restaurant!");

webApp.MapGet("/orders", (FoodDelivery fd) => fd.GetAllOrders());

// webApp.MapGet("/orders/{id}", (int id) => books[id]);

// webApp.MapPost("/orders", (Book book) =>
// {
//     books.Add(book);
//     return "Tack!";
// });

webApp.Run();
