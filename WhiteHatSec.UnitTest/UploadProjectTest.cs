using System;
using System.Collections.Specialized;
using System.IO;
using NUnit.Framework;
using WhiteHatSec.VSIX.UserControls;

namespace WhiteHatSec.UnitTest
{
    [TestFixture]
    public class UploadProjectTest
    {
        private readonly UploadProject uploadProject = new UploadProject();
        private string solutionPath = System.IO.Directory.GetCurrentDirectory().Replace("WhiteHatSec.UnitTest" + "\\bin\\Debug", "\\");

        [Test]
        public void DestinationDirNotExistTest()
        {
            TestDelegate copyDelegate = delegate () {
                var sourcePath = solutionPath + "WhiteHatSec.Entity";
                var destinationPath = solutionPath + "UploadEntity";
                var actual = uploadProject.CopyDirectory(sourcePath, destinationPath);
            };
            Assert.Throws<DirectoryNotFoundException>(copyDelegate);
        }

        [Test]
        public void ShowUploadProjectTest()
        {
            uploadProject.Show();
        }

        [Test]
        public void ZipDirNotExistTest()
        {
            var destinationPath = solutionPath + "UploadEntity";
            var zipPath = solutionPath + "EntityZip.zip";
            var actual = uploadProject.CreateZip(zipPath, destinationPath);
            Assert.That(actual,Is.Null.Or.Empty);
        }

        [Test]
        public void UploadZipWithInValidServerUrlTest()
        {
            TestDelegate test = delegate() {
                var collection = new NameValueCollection
                 {
                    {
                        "table_name", "uploadfile"
                    },
                    {
                        "commit", "uploadfile"
                    }
                };
                var fileParameters = "uploadfile";
                var contentType = "application/octet-stream";
                var zipPath = solutionPath + "EntityZip.zip";
                var serverUrl = string.Empty;
                uploadProject.UploadProjectFiles(serverUrl, zipPath, fileParameters, contentType, collection);
            };

            Assert.Throws<UriFormatException>(test);
        }
    }
}