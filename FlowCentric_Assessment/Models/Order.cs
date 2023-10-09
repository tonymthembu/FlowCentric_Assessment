using System.ComponentModel;

namespace FlowCentric_Assessment.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }

        [DisplayName("Customer Name")]
        public required string CustomerName { get; set; }
        public double SalesValueExcl { get; set; }
        public double AppliedDiscount { get; set; }
        public double SalesValueIncl { get; set; }
        public User User { get; set; }

        public List<OrderDetails> orderDetails { get; set; }
        public List<Product> AvailableProducts { get; set; }
        public List<int> SelectedProductIds { get; set; }
        public Dictionary<int, int> SelectedProductQuantities { get; set; }
        
    }
}
