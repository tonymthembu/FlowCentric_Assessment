using System;

namespace FlowCentric_Assessment.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public double UnitPrice { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
