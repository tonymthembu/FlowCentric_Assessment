using Microsoft.AspNetCore.Identity;

namespace FlowCentric_Assessment
{
    public class CustomRoleManager : RoleManager<CustomRole>
    {
        public CustomRoleManager(IRoleStore<CustomRole> store, IEnumerable<IRoleValidator<CustomRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<CustomRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
