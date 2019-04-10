using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using WhiteHatSec.Entity;
using WhiteHatSec.VSIX.BaseControl;
using WhiteHatSec.VSIX.UserControls;

namespace WhiteHatSec.UnitTest
{
    [TestFixture]
    public class QuestionAnswerTest
    {
        private readonly QaFilter qa = new QaFilter();

        public List<QuestionAnswerInfo.QuestionAnswerCollection> GetQa()
        {
            string content;
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream("WhiteHatSec.UnitTest.QuestionAnswer.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    content = reader.ReadToEnd();
                }
            }

            var questionAnswerInfo = JsonConvert.DeserializeObject<QuestionAnswerInfo.QuestionAnswerData>(content);
            return questionAnswerInfo.Collection;
        }

        [Test]
        public void AscendingSortQaDataTest()
        {
            var sortOrder = "Ascending";
            var expected = GetQa();
            expected = expected.OrderBy(x => x.Created).ToList();
            var actual = qa.SortQaData(expected, sortOrder);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DescendingSortQaDataTest()
        {
            var sortOrder = "Descending";
            var expected = GetQa();
            expected = expected.OrderByDescending(x => x.Created).ToList();
            var actual = qa.SortQaData(expected, sortOrder);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FilterEmptyListTest()
        {
            var actual = qa.FilterQa("test", new List<QuestionAnswerInfo.QuestionAnswerCollection>());
            Assert.IsEmpty(actual);
        }

        [Test]
        public void FilterEmptyStringTest()
        {
            var qaList = GetQa();
            var qaFilterData = qa.FilterQa(string.Empty, qaList);
            Assert.AreEqual(32, qaFilterData.Count);
        }

        [Test]
        public void FilterMatchStringTest()
        {
            var questionAnswerData = GetQa();
            var qaFilterData = qa.FilterQa("vaishali", questionAnswerData);
            Assert.AreEqual(16, qaFilterData.Count);
        }

        [Test]
        public void FilterNotMatchStringTest()
        {
            var qaList = GetQa();
            var qaFilterData = qa.FilterQa("Worked", qaList);
            Assert.AreEqual(0, qaFilterData.Count);
        }

        [Test]
        public void GetQaDataTest()
        {
            var server = "sentinel.whitehatsec.com";
            var apiKey = "2e2f0e37-6fec-4a55-b193-888a19ced149";
            var vulnId = "234568";
            var actual = qa.GetQaDataByApiKey(server, vulnId, apiKey);
            Assert.LessOrEqual(36, actual.Collection.Count);
        }

        [Test]
        public void PostValidQuestionTest()
        {
            var question = "This is test Question";
            var server = "sentinel.whitehatsec.com";
            var apiKey = "2e2f0e37-6fec-4a55-b193-888a19ced149";
            var vulnId = "234568";
            var actual = qa.PostQuestionByApiKey(question, server, vulnId, apiKey);
            Assert.AreEqual(201, actual);
        }

        [Test]
        public void BindQaDataTest()
        {
            BaseControl.BaseInstance.SentinelServerName = "sentinel.whitehatsec.com";
            BaseControl.BaseInstance.ApiKey = "2e2f0e37-6fec-4a55-b193-888a19ced149";
            BaseControl.BaseInstance.IsAuthenticatedByApiKey = true;
            string vulnId = "234565";
            qa.BindQaData(vulnId, "Ascending");
            var count = qa.QuestionAnswerData.Count;
            Assert.AreEqual(34, count);
        }

        [Test]
        public void CancelQuestionTest()
        {
            BaseControl.BaseInstance.IsAuthenticatedByApiKey = true;
            qa.QuestionAnswerData = GetQa();
            qa.SortOrder = "Ascending";
            qa.CancelQuestionSubmit();
            var count = qa.treeGridViewQAFilter.Nodes.Count;
            Assert.AreEqual(32, count);
        }

        [Test]
        public void CreateQaDataTest()
        {
            BaseControl.BaseInstance.IsAuthenticatedByApiKey = true;
            qa.CreateQaData(GetQa(), "Ascending");
            var count = qa.treeGridViewQAFilter.Nodes.Count;
            Assert.AreEqual(32, count);
        }
    }
}