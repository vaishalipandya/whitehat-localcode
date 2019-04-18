using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using WhiteHatSec.Entity;
using WhiteHatSec.VSIX.BaseControl;
using WhiteHatSec.VSIX.UserControls.Search;

namespace WhiteHatSec.UnitTest
{
    [TestFixture]
    public class SearchVulnTest
    {
        private readonly SearchVulnerability searchVuln = new SearchVulnerability();

        public List<ApplicationVulnerabilityInfo.AppsVulnsDataCollection> GetAppVulnData()
        {
            var vulnData = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream("WhiteHatSec.UnitTest.SearchVuln.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    vulnData = reader.ReadToEnd();
                }
            }

            var appVulnInfo =
                JsonConvert.DeserializeObject<ApplicationVulnerabilityInfo.AppsVulnsData>(vulnData);
            var appVulnCollection = appVulnInfo.Collection;
            return appVulnCollection;
        }

        [Test]
        public void FilterMatchVulnClassTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("Path Traversal", appsVulnData);
            Assert.AreEqual(2, vulnFilterData.Count);
        }

        [Test]
        public void FilterMatchVulnIdTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("240676", appsVulnData);
            Assert.AreEqual(1, vulnFilterData.Count);
        }

        [Test]
        public void FilterMatchVulnOpenDateTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("2015", appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FilterMatchVulnSeverityTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("medium", appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FilterMatchVulnStatusTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("Open", appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FilterNotMatchVulnClassTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("234568", appsVulnData);
            Assert.AreEqual(0, vulnFilterData.Count);
        }

        [Test]
        public void FilterNotMatchVulnIdTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("234568", appsVulnData);
            Assert.AreNotEqual(1, vulnFilterData.Count);
        }

        [Test]
        public void FilterNotMatchVulnOpenDateTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("OpenStatus", appsVulnData);
            Assert.AreEqual(0, vulnFilterData.Count);
        }

        [Test]
        public void FilterNotMatchVulnSeverityTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("AllData", appsVulnData);
            Assert.AreEqual(0, vulnFilterData.Count);
        }

        [Test]
        public void FilterNotMatchVulnStatusTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FilterVuln("OpenStatus", appsVulnData);
            Assert.AreEqual(0, vulnFilterData.Count);
        }

        [Test]
        public void FindAppVulnTest()
        {
            var appsVulnData = GetAppVulnData();
            var server = "sentinel.whitehatsec.com";
            var apikey = "34f2b9a3-d288-429e-a712-3a7d30a8853b";
            searchVuln.BindSeverity();
            searchVuln.severityComboBox.SelectedIndex = 0;
            searchVuln.BindState();
            searchVuln.stateComboBox.SelectedIndex = 2;
            var actual = searchVuln.GetAppVulnsByKey(server, "7807", apikey);
            Assert.AreEqual(appsVulnData.Count, actual.Collection.Count);
        }

        [Test]
        public void FindEmptyVulnDateTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnDate(string.Empty, string.Empty, appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FindEmptyVulnIdTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnId(string.Empty, appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FindEmptyVulnSeverityTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnSeverity(string.Empty, appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FindEmptyVulnStateTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnState(string.Empty, appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FindInvalidToVulnDateTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnDate("6/5/2015", "6/5/2015", appsVulnData);
            Assert.AreNotEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FindInvalidVulnIdTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnId("test", appsVulnData);
            Assert.AreEqual(0, vulnFilterData.Count);
        }

        [Test]
        public void FindInvalidVulnSeverityTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnSeverity("test", appsVulnData);
            Assert.AreEqual(0, vulnFilterData.Count);
        }

        [Test]
        public void FindInvalidVulnStateTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnState("test", appsVulnData);
            Assert.AreEqual(0, vulnFilterData.Count);
        }

        [Test]
        public void FindValidVulnDateTest()
        {
            var appsVulnData = GetAppVulnData();

            var vulnFilterData = searchVuln.FindVulnDate("6/5/2015", string.Empty, appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FindValidVulnIdTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnId("240689", appsVulnData);
            Assert.AreEqual(1, vulnFilterData.Count);
        }

        [Test]
        public void FindValidVulnSeverityTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnSeverity("medium", appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FindValidVulnStateTest()
        {
            var appsVulnData = GetAppVulnData();
            var vulnFilterData = searchVuln.FindVulnState("Open", appsVulnData);
            Assert.AreEqual(5, vulnFilterData.Count);
        }

        [Test]
        public void FromDateGreaterThanToDateTest()
        {
            bool actual;
            var fromDate = "6/6/2015";
            var toDate = "5/6/2015";
            actual = searchVuln.ValidateDate(fromDate, toDate);
            Assert.AreNotEqual(true, actual);
        }

        [Test]
        public void GetVulnDetailsByIdTest()
        {
            BaseControl.BaseInstance.SentinelServerName = "sentinel.whitehatsec.com";
            BaseControl.BaseInstance.ApiKey = "2e2f0e37-6fec-4a55-b193-888a19ced149";
            BaseControl.BaseInstance.IsAuthenticatedByApiKey = true;
            searchVuln.SearchWhiteHatWindow = new VSIX.UserControls.WhsMainWindow();
            searchVuln.SearchWhiteHatWindow.GetDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            searchVuln.SearchWhiteHatWindow.GetDetailsToolStripMenuItem.Checked = false;
            var vulnData = searchVuln.GetVulnDetailsInfo("240689");
            Assert.NotNull(vulnData);
        }

        [Test]
        public void ValidateEmptyFromDateTest()
        {
            var actual = false;
            var fromDate = string.Empty;
            var toDate = "5/6/2015";
            actual = searchVuln.ValidateDate(fromDate, toDate);
            Assert.IsFalse(actual);
        }

        [Test]
        public void ValidateEmptyToDateTest()
        {
            var actual = false;
            var fromDate = "5/6/2015";
            var toDate = string.Empty;
            actual = searchVuln.ValidateDate(fromDate, toDate);
            Assert.IsFalse(actual);
        }

        [Test]
        public void ValidDateTest()
        {
            var actual = false;
            var fromDate = "5/6/2015";
            var toDate = "5/7/2015";
            actual = searchVuln.ValidateDate(fromDate, toDate);
            Assert.IsTrue(actual);
        }

        [Test]
        public void BindStateTest()
        {
            searchVuln.BindState();
            Assert.IsNotNull(searchVuln.stateComboBox.DataSource);
        }

        [Test]
        public void BindSeverityTest()
        {
            searchVuln.BindSeverity();
            Assert.IsNotNull(searchVuln.severityComboBox.DataSource);
        }

        [Test]
        public void BindAppVulnTest()
        {
            List<ApplicationVulnerabilityInfo.AppsVulnsDataCollection> appsVulnCollection = GetAppVulnData();
            searchVuln.BindAppVuln(appsVulnCollection);
            //Assert.IsNotEmpty(searchVuln.ColumnApps.DataPropertyName);
        }
    }
}