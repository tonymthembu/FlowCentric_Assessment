using FlowCentric_Assessment.Models;

namespace FlowCentric_Assessment
{
    public class ProductService: IProductService
    {
        public List<Product> Products { get; } = new List<Product>();
    }
}
