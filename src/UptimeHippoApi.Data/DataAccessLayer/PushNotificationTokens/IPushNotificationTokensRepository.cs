using System.Threading.Tasks;
using UptimeHippoApi.Data.Models.Notification;

namespace UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens
{
    public interface IPushNotificationTokensRepository
    {
        Task SavePushNotificationToken(PushNotificationToken token);

        Task RemovePushNotificationToken(PushNotificationToken token);
    }
}