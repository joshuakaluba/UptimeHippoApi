using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Initialization;
using UptimeHippoApi.Data.Models.Authentication;

namespace UptimeHippoApi.Data.DataAccessLayer.Authentication
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public async Task<IdentityResult> RegisterNewUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, DataContextInitializer.DefaultUserClaim);
                await userManager.AddToRoleAsync(user, "User");
            }

            return result;
        }
    }
}