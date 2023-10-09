using FlowCentric_Assessment.Models;

namespace FlowCentric_Assessment
{
    public class UserService : IUserService
    {
        public List<User> Users { get; } = new List<User>();
    }
}
