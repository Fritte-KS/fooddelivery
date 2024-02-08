using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions opts) : base(opts) { }

    public DbSet<FoodOrder> FoodOrders { get; set; }
    public DbSet<Dish> Dishes { get; set; }


}