using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UptimeHippoApi.Data.Containers;
using UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens;
using UptimeHippoApi.Data.Models.Domain.Entity;
using UptimeHippoApi.Data.Models.Static;
using UptimeHippoApi.UptimeHandler.Models;

namespace UptimeHippoApi.UptimeHandler.Services.PushNotification
{
    public class PushNotificationService : IPushNotificationService
    {
        private string expoEndpoint = "/sendpushnotifications";

        private async Task<bool> Send(List<PushMessage> pushMessages)
        {
            try
            {
                using (var httpClient = new HttpClient { BaseAddress = new Uri(ApplicationConfig.PushNotificationMicroUrl) })
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.PostAsJsonAsync(expoEndpoint, pushMessages);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task SendPushNotifications(ApplicationUserMonitors applicationUserMonitors,
            IPushNotificationTokensRepository pushNotificationTokensRepository)
        {
            var pushMessages = new List<PushMessage>();

            foreach (var key in applicationUserMonitors)
            {
                var user = key.Key;

                if (user.PushNotificationsEnabled == false) continue;

                var monitors = key.Value;

                var monitorDescriptions
                    = monitors.Cast<Monitor>()
                        .Select(monitor => monitor.OutageDescription.Trim())
                            .ToArray();

                var downSites = string.Join(", ", monitorDescriptions);

                var pushNotificationTokens
                    = await pushNotificationTokensRepository.GetUserPushNotificationTokens(user);

                foreach (var token in pushNotificationTokens)
                {
                    var message = new PushMessage
                    {
                        To = token.Token,
                        Body = $"The following sites are down: {downSites}",
                        Title = "Site outage detected"
                    };

                    pushMessages.Add(message);
                }
            }

            await Send(pushMessages);
        }
    }
}