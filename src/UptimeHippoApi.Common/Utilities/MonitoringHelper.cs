using System.Net.NetworkInformation;
using UptimeHippoApi.Common.Exception;

namespace UptimeHippoApi.Common.Utilities
{
    public class MonitoringHelper
    {
        public static bool EnsureKeyWordExists(string searchString, string keyword)
        {
            if (searchString.ToLower().Contains(keyword.ToLower()))
            {
                return true;
            }
            else
            {
                throw new KeyWordNotFoundException();
            }
        }

        public static bool PingHost(string nameOrAddress)
        {
            using (var pinger = new Ping())
            {
                PingReply reply = pinger.Send(nameOrAddress);
                return reply.Status == IPStatus.Success;
            }
        }
    }
}