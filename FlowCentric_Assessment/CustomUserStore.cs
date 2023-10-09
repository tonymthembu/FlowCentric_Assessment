using Azure;
using FlowCentric_Assessment.Models;
using Microsoft.AspNetCore.Identity;

namespace FlowCentric_Assessment
{
    public class CustomUserStore : IUserStore<User>
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        public CustomUserStore(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> FindById(int userId)
        {
            string apiKey = _configuration["ApiConnection:ApiKey"] ?? throw new InvalidOperationException("API Key not found.");
            var client = _clientFactory.CreateClient("ApiClient");

            // Set the authentication header
            client.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            var response = await client.GetAsync("/api/users");
            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<User>>();
                var user = users.FirstOrDefault(u => u.UserId == userId);

                return user;
            }

            return null;              
        }

        public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
