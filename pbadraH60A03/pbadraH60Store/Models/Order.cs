namespace pbadraH60A01.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateFulfilled { get; set; }
        public decimal Total { get; set; }
        public decimal Taxes { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
