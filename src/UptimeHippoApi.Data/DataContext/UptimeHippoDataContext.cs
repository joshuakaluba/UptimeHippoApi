using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Notification;
using UptimeHippoApi.Data.Models.Static;

namespace UptimeHippoApi.Data.DataContext
{
    public class UptimeHippoDataContext : IdentityDbContext<ApplicationUser>
    {
        internal DbSet<PushNotificationToken> PushNotificationTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString
                = $"Server={ApplicationConfig.DatabaseHost};" +
                    $"database={ApplicationConfig.DatabaseName};" +
                        $"uid={ApplicationConfig.DatabaseUser};" +
                            $"pwd={ApplicationConfig.DatabasePassword};" +
                                "pooling=true;";

            optionsBuilder.UseMySql(connectionString);
        }
    }
}