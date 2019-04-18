using System.Net;
using NUnit.Framework;
using WhiteHatSec.VSIX.BaseControl;
using WhiteHatSec.VSIX.UserControls;
using System;
using System.Windows.Forms;

namespace WhiteHatSec.UnitTest
{
    [TestFixture]
    public class LoginTest
    {
        public static CookieContainer cookieContainer = new CookieContainer();
        private readonly Login login = new Login();

        [Test]
        public void CheckWithEmptyServerNameTest()
        {
            TestDelegate test = delegate ()
            {
                var server = string.Empty;
                var userName = "vaishali.pandya@einfochips.com";
                var password = "Google1234";
                login.ValidUser(server, userName, password, cookieContainer);
            };
            
            Assert.Throws<UriFormatException>(test);
        }

        [Test]
        public void CheckWithEmptyUserNamePasswordTest()
        {
            var server = "sentinel.whitehatsec.com";
            var userName = string.Empty;
            var password = string.Empty;
            var result = login.ValidUser(server, userName, password, cookieContainer);
            StringAssert.AreNotEqualIgnoringCase("true", result);
        }

        [Test]
        public void CheckWithInvalidCredentialTest()
        {
            var server = "sentinel.whitehatsec.com";
            var userName = "vaishali1.pandya@einfochips.com";
            var password = "Google111234";
            var result = login.ValidUser(server, userName, password, cookieContainer);
            StringAssert.AreNotEqualIgnoringCase("true", result);
        }

        [Test]
        public void CheckWithValidCredentialTest()
        {
            var server = "sentinel.whitehatsec.com";
            var userName = "spandan.prajapati@einfochips.com";
            var password = "Spandan123";
            var result = login.ValidUser(server, userName, password, cookieContainer);
            StringAssert.AreEqualIgnoringCase("true", result);
        }

        [Test]
        public void GetApplicationByInvalidKeyTest()
        {
            var server = "sentinel.whitehatsec.com";
            var apikey = "-4a55-b193-888a19ced149";
            var appInfo = login.GetApplicationsByKey(server, apikey);
            StringAssert.AreNotEqualIgnoringCase("true", appInfo.ResponseMessage);
        }

        [Test]
        public void GetApplicationByValidKeyTest()
        {
            var server = "sentinel.whitehatsec.com";
            var apikey = "2e2f0e37-6fec-4a55-b193-888a19ced149";
            var appInfo = login.GetApplicationsByKey(server, apikey);
            StringAssert.AreEqualIgnoringCase("true", appInfo.ResponseMessage);
        }

        [Test]
        public void ValidateApiKeyTest()
        {
            login.serverTextBox = new TextBox();
            login.serverTextBox.Text = "sentinel.whitehatsec.com";
            var apikey = "34f2b9a3-d288-429e-a712-3a7d30a8853b";
            login.ValidateApiKey(apikey);
            Assert.IsTrue(BaseControl.BaseInstance.IsAuthenticatedByApiKey);
        }
    }
}