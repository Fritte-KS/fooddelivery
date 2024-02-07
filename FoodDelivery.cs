public class FoodDelivery
{
    static List<FoodOrder> orders = new() { new FoodOrder { Food = "Soup", Price = 10 } };

    public void OrderFood(string food, int price)
    {
        FoodOrder newOrder = new() { Food = food, PlacedAt = DateTime.Now, Price = price };
        orders.Add(newOrder);
    }

    public List<FoodOrder> GetAllOrders()
    {
        return orders;
    }

    public int GetOrderCount(Func<FoodOrder, bool> predicate)
    {
        int result = 0;

        foreach (var item in orders)
        {
            if (predicate(item))
            {
                result++;
            }
        }

        return result;
    }
}

public class FoodOrder
{
    public DateTime PlacedAt { get; set; }
    public string Food { get; set; }
    public int Price { get; set; }
}