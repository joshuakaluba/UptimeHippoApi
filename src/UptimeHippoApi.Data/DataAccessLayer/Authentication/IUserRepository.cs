using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Authentication;

namespace UptimeHippoApi.Data.DataAccessLayer.Authentication
{
    public interface IUserRepository
    {
        Task<IdentityResult> RegisterNewUser(UserManager<ApplicationUser> userManager, ApplicationUser user, string password);
    }
}