using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UptimeHippoApi.Data.DataContext;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Notification;

namespace UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens
{
    public sealed class PushNotificationTokensRepository : BaseRepository, IPushNotificationTokensRepository
    {
        public async Task SavePushNotificationToken(PushNotificationToken token)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                DataContext.PushNotificationTokens
                    .RemoveRange(DataContext.PushNotificationTokens
                        .Where(t => t.Token == token.Token));

                DataContext.PushNotificationTokens.Add(token);

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task RemovePushNotificationToken(PushNotificationToken token)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                DataContext.PushNotificationTokens
                    .RemoveRange(DataContext.PushNotificationTokens
                        .Where(t => t.Token == token.Token));

                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<List<PushNotificationToken>> GetUserPushNotificationTokens(ApplicationUser user)
        {
            using (DataContext = new UptimeHippoDataContext())
            {
                var pushNotificationTokens
                    = await DataContext.PushNotificationTokens
                        .Where(t => t.ApplicationUserId == user.Id)
                            .ToListAsync();

                return pushNotificationTokens;
            }
        }
    }
}