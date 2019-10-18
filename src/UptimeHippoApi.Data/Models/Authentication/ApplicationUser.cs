using Microsoft.AspNetCore.Identity;
using System;

namespace UptimeHippoApi.Data.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public bool Active { get; set; } = true;
        public bool NotificationsEnabled { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}