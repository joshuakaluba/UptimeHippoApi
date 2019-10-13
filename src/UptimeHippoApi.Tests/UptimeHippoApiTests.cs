using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.NetworkInformation;
using UptimeHippoApi.Common.Exception;
using UptimeHippoApi.Common.Messaging;
using UptimeHippoApi.Common.Utilities;
using UptimeHippoApi.Data.Models.Static;

namespace UptimeHippoApi.Tests
{
    [TestClass]
    public class UptimeHippoApiTests
    {
        [TestMethod]
        [ExpectedException(typeof(KeyWordNotFoundException))]
        public void TestGetKeywordFailing()
        {
            var html = @" <!doctype html>
                        <html lang='en'>
                          <head>
                            <!-- Required meta tags -->
                            <meta charset='utf-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
                            <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css'>
                            <title>Hello World</title>
                          </head>
                          <body>
                            <div class='container'>
                                <p> Hello World </p>
                            </div>
                            <script src='https://code.jquery.com/jquery-3.3.1.slim.min.js'></script>
                            <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js'></script>
                            <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js'></script>
                          </body>
                        </html>";

            var keyWord = "Microsoft";

            MonitoringHelper.EnsureKeyWordExists(html, keyWord);
        }

        [TestMethod]
        public void TestGetKeyword()
        {
            var html = @" <!doctype html>
                        <html lang='en'>
                          <head>
                            <!-- Required meta tags -->
                            <meta charset='utf-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>
                            <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css'>
                            <title>Hello World</title>
                          </head>
                          <body>
                            <div class='container'>
                                <p> Hello World </p>
                            </div>
                            <script src='https://code.jquery.com/jquery-3.3.1.slim.min.js'></script>
                            <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js'></script>
                            <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js'></script>
                          </body>
                        </html>";

            var keyWord = "Hello World";

            MonitoringHelper.EnsureKeyWordExists(html, keyWord);
        }

        [TestMethod]
        [ExpectedException(typeof(PingException))]
        public void TestFailingSitePing()
        {
            var fakeUrl = "joshuakaluba.kaluba";

            MonitoringHelper.PingHost(fakeUrl);
        }

        [TestMethod]
        public void TestPing()
        {
            var urlToTest = "www.google.com";

            MonitoringHelper.PingHost(urlToTest);
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