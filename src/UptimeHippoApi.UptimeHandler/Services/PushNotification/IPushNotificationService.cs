using System.Threading.Tasks;
using UptimeHippoApi.Data.Containers;
using UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens;

namespace UptimeHippoApi.UptimeHandler.Services.PushNotification
{
    public interface IPushNotificationService
    {
        Task SendPushNotifications(ApplicationUserMonitors applicationUserMonitors,
            IPushNotificationTokensRepository pushNotificationTokensRepository);
    }
}