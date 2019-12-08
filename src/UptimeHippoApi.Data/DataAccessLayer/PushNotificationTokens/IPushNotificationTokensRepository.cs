using System.Collections.Generic;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Notification;

namespace UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens
{
    public interface IPushNotificationTokensRepository
    {
        Task SavePushNotificationToken(PushNotificationToken token);

        Task RemovePushNotificationToken(PushNotificationToken token);

        Task<List<PushNotificationToken>> GetUserPushNotificationTokens(ApplicationUser user);
    }
}