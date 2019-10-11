using UptimeHippoApi.Data.Models.Authentication;

namespace UptimeHippoApi.Data.Services.Authentication
{
    public interface IUserValidatorService
    {
        bool ValidateUser(ApplicationUser user);
    }
}