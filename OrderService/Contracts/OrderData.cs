namespace OrderService.Contracts;

public class OrderData
{
    public int Id { get; set; }
    public OrderItem[] Items { get; set; }

    public class OrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}