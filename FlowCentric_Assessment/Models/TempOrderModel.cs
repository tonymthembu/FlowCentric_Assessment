namespace FlowCentric_Assessment.Models
{
    public class TempOrderModel
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }

        public required string CustomerName { get; set; }
        public double SalesValueExcludingVAT { get; set; }
        public double Discount { get; set; }
        public double SalesValueIncludingVAT { get; set; }
        //public User User { get; set; } = new User();
        public List<OrderDetails> OrderDetails { get; set; } 
    }
}
