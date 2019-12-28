using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Authentication;

namespace UptimeHippoApi.Web.Controllers
{
    public abstract class BaseController<T> : Controller
    {
        protected readonly UserManager<ApplicationUser> UserManager;
        protected ILogger<T> Logger;

        protected BaseController(UserManager<ApplicationUser> userManager, ILogger<T> logger)
        {
            UserManager = userManager;
            Logger = logger;
        }

        protected string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }

        protected async Task<ApplicationUser> GetCurrentAuthenticatedUser()
        {
            var user = await UserManager.FindByIdAsync(GetUserId());

            if (user == null)
            {
                throw new System.NullReferenceException("Unable to retrieve authenticated user");
            }

            return user;
        }
    }
}