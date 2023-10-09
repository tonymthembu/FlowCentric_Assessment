using FlowCentric_Assessment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Net.Http;
using System.Text;

namespace FlowCentric_Assessment.Controllers
{
    public class OrderController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public OrderController(IHttpClientFactory clientFactory, IConfiguration configuration, IUserService userService)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            string apiKey = _configuration["ApiConnection:ApiKey"] ?? throw new InvalidOperationException("API Key not found.");
            var client = _clientFactory.CreateClient("ApiClient");

            // Set the authentication header
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            var response = await client.GetAsync("/api/products");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                return View(products);

            }

            return View();
        }

        //[Authorize(Roles = "Sales Person")]  --- I struggled to configure Identity using custom user information from the API
        public async Task<ActionResult> Create()
        {

            string apiKey = _configuration["ApiConnection:ApiKey"] ?? throw new InvalidOperationException("API Key not found.");
            var client = _clientFactory.CreateClient("ApiClient");

            // Set the authentication header
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            var response = await client.GetAsync("/api/products");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<Product>>() ?? new List<Product>();                
                
                return View(new Order
                {
                    AvailableProducts = products,
                    SelectedProductQuantities = new Dictionary<int, int>(),
                    CustomerName = ""
                }); ;

            }


            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Order order, IFormCollection collection)
        {
            var loggedInUser = _userService.Users.FirstOrDefault();

            if (loggedInUser != null && loggedInUser.role == "Sales Person")
            {
                string apiKey = _configuration["ApiConnection:ApiKey"] ?? throw new InvalidOperationException("API Key not found.");
                var client = _clientFactory.CreateClient("ApiClient");


                double TotalPrice = 0;
                // Set the authentication header
                client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);

                if (order != null)
                {
                    order.orderDetails = new List<OrderDetails>();
                    foreach (int productId in order.SelectedProductIds)
                    {
                        var response = await client.GetAsync("/api/products/" + productId);

                        if (response.IsSuccessStatusCode)
                        {
                            Product product = await response.Content.ReadFromJsonAsync<Product>() ?? new Product();

                            order.orderDetails.Add(new OrderDetails()
                            {
                                OrderDetailsId = 0,
                                OrderId = 0,
                                ProductId = productId,
                                Quantity = order.SelectedProductQuantities[productId]
                            });

                            TotalPrice += (product.UnitPrice * order.SelectedProductQuantities[productId]);

                        }
                    }

                    if (TotalPrice > 200 && TotalPrice < 500)
                    {
                        order.AppliedDiscount = (3 / TotalPrice) * 100;
                    }
                    else if (TotalPrice > 500)
                    {
                        order.AppliedDiscount = (10 / TotalPrice) * 100;
                    }
                    else
                    {
                        order.AppliedDiscount = 0;
                    }

                    order.OrderDate = DateTime.Now;
                    order.SalesValueExcl = TotalPrice - order.AppliedDiscount;
                    order.SalesValueIncl = TotalPrice;


                    order.User = loggedInUser;
                    order.UserId = loggedInUser.UserId;


                    try
                    {
                        var newOrder = new TempOrderModel()
                        {
                            OrderId = order.OrderId,
                            UserId = order.UserId,
                            OrderDate = order.OrderDate,
                            CustomerName = order.CustomerName,
                            SalesValueExcludingVAT = order.SalesValueExcl,
                            Discount = order.AppliedDiscount,
                            SalesValueIncludingVAT = order.SalesValueIncl,
                            OrderDetails = order.orderDetails

                        };



                        // Serialize the object to JSON
                        var jsonContent = new StringContent(
                            Newtonsoft.Json.JsonConvert.SerializeObject(newOrder),
                            Encoding.UTF8,
                            "application/json"
                        );

                        // Send the POST request to the API
                        var response = await client.PostAsync("/api/Orders", jsonContent);

                        // Check if the request was successful
                        if (response.IsSuccessStatusCode)                        {
                            
                            var responseBody = await response.Content.ReadAsStringAsync();
                            return RedirectToAction("SuccessAction");
                            //return Content(responseBody);
                        }
                        else
                        {
                            // Handle the error (e.g., log or return an error message)
                            return BadRequest("API request failed.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., log or return an error message)
                        return StatusCode(500, "Internal Server Error");
                    }


                }
            }
            return RedirectToAction("FailureAction");

        }

        public IActionResult SuccessAction()
        {
            ViewBag.SuccessMessage = "The operation was successful!";
            return View("Success"); 
        }

        public IActionResult FailureAction()
        {
            ViewBag.ErrorMessage = "An error occurred during the operation.";
            return View("Failure"); 
        }

    }

}

