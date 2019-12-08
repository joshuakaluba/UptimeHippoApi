using Microsoft.AspNetCore.Identity;
using System;

namespace UptimeHippoApi.Data.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public bool Active { get; set; } = true;

        public bool PushNotificationsEnabled { get; set; } = true;

        public bool NotificationTextMessagesEnabled { get; set; } = true;

        public bool NotificationEmailsEnabled { get; set; } = true;

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public void UpdateFromUserViewModel(UserSettingsViewModel userSettingsViewModel)
        {
            PhoneNumber = userSettingsViewModel.PhoneNumber;
            NotificationTextMessagesEnabled = userSettingsViewModel.NotificationTextMessagesEnabled;
            NotificationEmailsEnabled = userSettingsViewModel.NotificationEmailsEnabled;
            PushNotificationsEnabled = userSettingsViewModel.PushNotificationsEnabled;
        }
    }
}