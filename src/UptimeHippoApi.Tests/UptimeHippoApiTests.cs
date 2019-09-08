using Microsoft.VisualStudio.TestTools.UnitTesting;
using UptimeHippoApi.Common.Messaging;
using UptimeHippoApi.Data.Models.Static;

namespace UptimeHippoApi.Tests
{
    //TODO write tests
    [TestClass]
    public class UptimeHippoApiTests
    {
        [TestMethod]
        [ExpectedException(typeof(System.AggregateException))]
        public void TestMethod()
        {
            throw new System.AggregateException();
        }

        [TestMethod]
        public void TestMethod2()
        {
        }

        [TestMethod]
        public void SendTwilioMessage()
        {
            var twilioMessage
                = new TwilioMessage(ApplicationConfig.TwilioAccountSId,
                ApplicationConfig.TwilioAuthenticationToken, ApplicationConfig.TwilioSenderPhoneNumber);

            twilioMessage.Send(ApplicationConfig.TwilioRecipientPhoneNumber, "Test Message from tests").Wait();
        }
    }
}