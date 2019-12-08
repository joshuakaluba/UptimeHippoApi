using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Domain.Entity;
using UptimeHippoApi.Data.Models.Notification;
using UptimeHippoApi.Data.Models.Static;

namespace UptimeHippoApi.Data.DataContext
{
    public sealed class UptimeHippoDataContext : IdentityDbContext<ApplicationUser>
    {
        internal DbSet<PushNotificationToken> PushNotificationTokens { get; set; }
        internal DbSet<Monitor> Monitors { get; set; }
        internal DbSet<MonitorLog> MonitorLogs { get; set; }

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