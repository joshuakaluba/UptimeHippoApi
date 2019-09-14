using UptimeHippoApi.Data.Models.Authentication;

namespace UptimeHippoApi.Services.Authentication
{
    public static class UserValidator
    {
        public static bool Validate(ApplicationUser user)
        {
            return user.Active;
        }
    }
}