using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Authentication;
using TokenOptions = UptimeHippoApi.Data.Models.Authentication.TokenOptions;

namespace UptimeHippoApi.Data.Services.Authentication
{
    public interface ITokenGeneratorService
    {
        Task<Token> CreateJwtToken(ApplicationUser user, UserManager<ApplicationUser> userManager, TokenOptions tokenOptions);
    }
}