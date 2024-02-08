using Microsoft.EntityFrameworkCore;

public class FoodDelivery
{
    private readonly AppDbContext _dbContext;

    public FoodDelivery(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
    }

    public int OrderFood(FoodOrderRequest foodOrderRequest)
    {
        Dish? dish = _dbContext.Dishes.Find(foodOrderRequest.DishId);

        if (dish == null)
        {
            return -1;
        }

        FoodOrder newOrder = new() { Dish = dish, OrderDate = DateTime.Now, Price = dish.Price };

        if (foodOrderRequest.DiscountCode == "suvnet23")
        {
            newOrder.Price *= 0.75f;
        }

        _dbContext.Add(newOrder);
        _dbContext.SaveChanges();

        return newOrder.Id;
    }

    public async Task<List<FoodOrder>> GetAllOrdersAsync()
    {
        return await _dbContext.FoodOrders.ToListAsync();
    }

    public async Task<FoodOrder?> GetOrderByIdAsync(int id)
    {
        return await _dbContext.FoodOrders.FindAsync(id);
    }
}

public class FoodOrder
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Dish? Dish { get; set; }
    public float Price { get; set; }
    public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.Ordered;
}

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; } = "Noname";
    public float Price { get; set; }
}

public enum DeliveryStatus
{
    Ordered,
    BeingPrepared,
    OutForDelivery,
    Delivered,
    Cancelled
}

public class FoodOrderRequest
{
    public int DishId { get; set; }
    public string DiscountCode { get; set; }
}