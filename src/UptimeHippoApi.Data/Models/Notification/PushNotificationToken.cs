using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Domain.Entity;

namespace UptimeHippoApi.Data.Models.Notification
{
    public class PushNotificationToken : Auditable
    {
        public string Token { get; set; }
        public bool Active { get; set; } = true;
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public override bool Equals(Auditable other)
        {
            var token = (PushNotificationToken)other;
            return Token == token.Token;
        }
    }
}