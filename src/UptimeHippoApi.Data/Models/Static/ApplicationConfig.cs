using System;

namespace UptimeHippoApi.Data.Models.Static
{
    public static class ApplicationConfig
    {
        #region Database Configuration

        internal static string DatabaseHost
           = Environment.GetEnvironmentVariable
               ("UPTIME_HIPPO_API_DB_HOST", target: EnvironmentVariableTarget.Process);

        public static string Port
            = Environment.GetEnvironmentVariable
                ("UPTIME_HIPPO_API_APPLICATION_PORT", target: EnvironmentVariableTarget.Process);

        internal static string DatabaseName
            = Environment.GetEnvironmentVariable
                ("UPTIME_HIPPO_API_DB_NAME", target: EnvironmentVariableTarget.Process);

        internal static string DatabaseUser
            = Environment.GetEnvironmentVariable
                ("UPTIME_HIPPO_API_DB_USER", target: EnvironmentVariableTarget.Process);

        internal static string DatabasePassword
            = Environment.GetEnvironmentVariable
                ("UPTIME_HIPPO_API_DB_PASSWORD", target: EnvironmentVariableTarget.Process);

        public static string PushNotificationMicroUrl
            = Environment.GetEnvironmentVariable
                ("PUSH_NOTIFICATION_MICRO", target: EnvironmentVariableTarget.Process);

        #endregion Database Configuration

        #region Test User Credentials

        public static string TestUserEmail
            = Environment.GetEnvironmentVariable
                ("UPTIME_HIPPO_TEST_USER_EMAIL", target: EnvironmentVariableTarget.Process);

        public static string TestUserPassword
            = Environment.GetEnvironmentVariable
                ("UPTIME_HIPPO_TEST_USER_PASSWORD", target: EnvironmentVariableTarget.Process);

        #endregion Test User Credentials

        #region Twilio Config

        internal static bool SendTwillioMessage => Environment.GetEnvironmentVariable
                ("UPTIME_HIPPO_API_SEND_TWILIO_MESSAGE", target: EnvironmentVariableTarget.Process).ToLower().Contains("true");

        public static string TwilioAccountSId
            = Environment.GetEnvironmentVariable
                ("TWILIO_ACCOUNT_SID", target: EnvironmentVariableTarget.Process);

        public static string TwilioAuthenticationToken
            = Environment.GetEnvironmentVariable
                ("TWILIO_AUTHENTICATION_TOKEN", target: EnvironmentVariableTarget.Process);

        public static string TwilioSenderPhoneNumber
            = Environment.GetEnvironmentVariable
                ("TWILIO_SENDER_PHONENUMBER", target: EnvironmentVariableTarget.Process);

        public static string TwilioRecipientPhoneNumber
            = Environment.GetEnvironmentVariable
                ("TWILIO_RECIPIENT_PHONENUMBER", target: EnvironmentVariableTarget.Process);

        #endregion Twilio Config
    }
}