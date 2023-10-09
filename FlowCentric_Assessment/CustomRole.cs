using Microsoft.AspNetCore.Identity;

namespace FlowCentric_Assessment
{
    public class CustomRole : IdentityRole
    {
    
        public string Description { get; set; }
    }
}
