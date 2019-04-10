using System.Globalization;
using WhiteHatSec.VSIX.UserControls;
using NUnit.Framework;
using WhiteHatSec.Localization.Culture.Resource;

namespace WhiteHatSec.UnitTest
{
    [TestFixture]
    public class CultureTest
    {
        private WhsMainWindow whsMainWindow = new WhsMainWindow();

        [Test]
        public void CheckApplicationCultureTest()
        {
            whsMainWindow.ChangeAppCulture("de");
            CultureInfo cultureInfo = CultureManager.ApplicationUiCulture;
            string cultureName = cultureInfo.Name;
            Assert.AreEqual("de", cultureName);
        }
    }
}
