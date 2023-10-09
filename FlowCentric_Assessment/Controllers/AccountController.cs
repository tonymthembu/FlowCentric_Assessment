using FlowCentric_Assessment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlowCentric_Assessment.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;


        //private readonly SignInManager<User> _signInManager;
        //private readonly CustomUserManager<User> _userManager;
        
        public AccountController(IHttpClientFactory clientFactory, IConfiguration configuration, IUserService userService)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            //_signInManager = signInManager;
            _userService = userService;
            //_userManager = userManager;
        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                string apiKey = _configuration["ApiConnection:ApiKey"] ?? throw new InvalidOperationException("API Key not found.");
                var client = _clientFactory.CreateClient("ApiClient");

                // Set the authentication header
                client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
                var response = await client.GetAsync("/api/users");
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<User>>();

                    if (users != null && users.Any())
                    {                        
                        var firstUser = users.FirstOrDefault(u => u.UserName == model.Username && u.password == model.Password);

                        if (firstUser != null)
                        {
                            _userService.Users.Add(firstUser);
                            return RedirectToAction("Create", "Order");
                            //var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);

                            //if (result.Succeeded)
                            //{                                
                            //    return RedirectToAction("Index", "Home");
                            //}                            
                        }
                    }


                }
            }




            // Authentication failed, show an error message
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);

        }
    }

    
}
