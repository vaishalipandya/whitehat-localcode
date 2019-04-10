using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.VSIX.Utility;
using log4net;

namespace WhiteHatSec.VSIX.UserControls
{
    /// <summary>
    /// Upload Zip Project
    /// </summary>
    public partial class UploadProject : Form
    {
        #region "Properties"

        private static readonly ILog Log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// WhiteHat  parent window control
        /// </summary>
        public WhsMainWindow ParentWhsWindow { get; set; }
        /// <summary>
        /// collection for language
        /// </summary>
        private NameValueCollection collection = new NameValueCollection
            {
                {
                    "Accepts-Language", "en-us,en;q=0.5"
                }
            };
        /// <summary>
        /// Boundry for ticks
        /// </summary>
        private string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        #endregion

        #region "Constructor"

        /// <summary>
        ///     Initializes a new instance of the <see cref="UploadProject" /> class.
        /// </summary>
        public UploadProject()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On InitializeComponent*****", ex);
            }
        }

        #endregion

        #region "Events"

        /// <summary>
        ///     Handles the Click event of the Upload Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    Log.Info("****Project is going to be Uploaded*****");
                    string serverUrl = txtServer.Text.Trim();

                    if (lstBoxProject.Items.Count == 0)
                    {
                        //If not any solution open, then not any project to upload so message display not project exist
                        MessageBox.Show(MessageLog.ThereIsNoProjectToUpload, MessageLog.Message, MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
                        return;
                    }

                    if (string.IsNullOrEmpty(serverUrl))
                    {
                        //Display message to enter server name 
                        MessageBox.Show(MessageLog.PleaseEnterServerToUploadProject, MessageLog.Message,
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    string selectedProjectPath = lstBoxProject.SelectedValue.ToString();
                    string selectedProjectName = Path.GetFileNameWithoutExtension(selectedProjectPath);
                    string projectSourceDirName = Path.GetDirectoryName(selectedProjectPath);

                    DoUpload(selectedProjectName, projectSourceDirName, serverUrl);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(MessageLog.FailedToUploadProject, MessageLog.ErrorMessage,
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Error("****Failed to Upload Project*****", ex);
                    return;

                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error("****Error Occured On Upload Click*****", ex);
                MessageBox.Show("Error occured on Upload click.", MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        /// <summary>
        ///     Handles the Load event of the ZipProject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void UploadProject_Load(object sender, EventArgs e)
        {
            try
            {
                BindProjects();
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Load*****", ex);
            }
        }

        #endregion

        #region "Methods"
        /// <summary>
        /// Upload
        /// </summary>
        /// <param name="selectedProjectName"></param>
        /// <param name="projectDirName"></param>
        /// <param name="serverUrl"></param>
        public void DoUpload(string selectedProjectName, string projectDirName, string serverUrl)
        {
            string appendDateTime = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            string solutionDirName =
                Path.GetDirectoryName(ParentWhsWindow.VisualStudioCurrentInstance.Solution.FullName);
            string projectDir = Path.Combine(solutionDirName, selectedProjectName + "ZipTemp_" + appendDateTime);
            string zipSolutionPath = Path.Combine(solutionDirName, selectedProjectName + "ZipDir_" + appendDateTime);

            //Create directory for Copy and zip project
            if (!Directory.Exists(projectDir))
            {
                Directory.CreateDirectory(projectDir);
            }
            if (!Directory.Exists(zipSolutionPath))
            {
                Directory.CreateDirectory(zipSolutionPath);
            }
            selectedProjectName = string.Concat(selectedProjectName, appendDateTime);
            //Copy source project to temp directory, so if file in use it will not give error 
            projectDir = CopyDirectory(projectDirName, projectDir);

            string destinationZipPath = Path.Combine(@zipSolutionPath, selectedProjectName + ".zip");
            //create zip for source project
            destinationZipPath = CreateZip(destinationZipPath, projectDir);

            //Upload project zip to remote server
            UploadProjectZip(serverUrl, destinationZipPath);

            if (Directory.Exists(projectDir))
            {
                Directory.Delete(projectDir, true);
            }
            if (Directory.Exists(zipSolutionPath))
            {
                Directory.Delete(zipSolutionPath, true);
            }
        }

        /// <summary>
        ///     Binds the projects.
        /// </summary>
        private void BindProjects()
        {
            try
            {
                Log.Info("****Going to bind projects in listbox*****");
                lstBoxProject.DataSource = SolutionInfo.GetProjects(ParentWhsWindow.VisualStudioCurrentInstance);
                lstBoxProject.ValueMember = "id";
                lstBoxProject.DisplayMember = "Name";
                Log.Info("****Sucessfully binded projects in listbox*****");
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On binding projects in listbox*****", ex);
            }
        }
        /// <summary>
        /// Upload Zip TO Remote Server
        /// </summary>
        public void UploadProjectZip(string uploadUrl, string zipPath)
        {
            collection = new NameValueCollection
                        {
                            {"table_name", "uploadfile"},
                            {"commit", "uploadfile"}
                        };
            try
            {
                UploadProjectFiles(uploadUrl, zipPath, "uploadfile", "application/octet-stream", collection);
                this.Dispose();

                MessageBox.Show(MessageLog.ProjectIsUploadedSucessfully, MessageLog.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.Info("****Project is Uploaded Sucessfully*****");
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageLog.FailedToUploadProject, MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("****Failed to Upload Project*****", ex);
            }
        }
        /// <summary>
        ///     Uploads the zip prject to remote URL.
        /// </summary>
        /// <param name="uploadUrl">The URL.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileRequestCollection">The file request collection.</param>
        public bool UploadProjectFiles(string uploadUrl, string fileName, string parameterName, string contentType,
            NameValueCollection fileRequestCollection)
        {

            HttpWebRequest uploadRequest = CreateRequest(uploadUrl);
            uploadRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            Stream requestStream = uploadRequest.GetRequestStream();
            WriteFiles(requestStream, parameterName, contentType, fileName);
           
                var uploadResponse = uploadRequest.GetResponse();
                Stream webResponseStream = uploadResponse.GetResponseStream();
                new StreamReader(webResponseStream);
                return true;
        }
        /// <summary>
        /// Create Request
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <returns></returns>
        public HttpWebRequest CreateRequest(string uploadUrl)
        {
            HttpWebRequest uploadRequest = (HttpWebRequest)WebRequest.Create(uploadUrl);
            uploadRequest.Method = "POST";
            uploadRequest.KeepAlive = true;
            uploadRequest.Credentials = CredentialCache.DefaultCredentials;
            uploadRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8,application/octet-stream";
            collection = new NameValueCollection
            {
                {
                    "Accepts-Language", "en-us,en;q=0.5"
                }
            };
            uploadRequest.Headers.Add(collection);

            return uploadRequest;
        }
        /// <summary>
        /// Write To file 
        /// </summary>
        /// <param name="requestStream"></param>
        /// <param name="parameterName"></param>
        /// <param name="contentType"></param>
        /// <param name="fileName"></param>
        public void WriteFiles(Stream requestStream, string parameterName, string contentType, string fileName)
        {
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] boundarybytesFile = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            bool isValid = true;
            string formDataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in collection.Keys)
            {
                if (!isValid)
                {
                    requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                }
                else
                {
                    requestStream.Write(boundarybytesFile, 0, boundarybytesFile.Length);
                    isValid = false;
                }

                string formItem = string.Format(formDataTemplate, key, collection[key]);
                byte[] formItemBytes = Encoding.UTF8.GetBytes(formItem);
                requestStream.Write(formItemBytes, 0, formItemBytes.Length);
            }

            requestStream.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, parameterName, new FileInfo(fileName).Name, contentType);
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            requestStream.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            byte[] fileBuffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = fileStream.Read(fileBuffer, 0, fileBuffer.Length)) != 0)
            {
                requestStream.Write(fileBuffer, 0, bytesRead);
            }

            fileStream.Close();
            byte[] fileToWrite = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            requestStream.Write(fileToWrite, 0, fileToWrite.Length);
            requestStream.Close();
        }
        /// <summary>
        ///     Creates the zip of source directory.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public string CreateZip(string destinationZipPath, string sourceZipDirName)
        {
            try
            {
                ZipFile.CreateFromDirectory(sourceZipDirName, destinationZipPath);
                return destinationZipPath;
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Creating Zip*****", ex);
                return string.Empty;
            }
        }
        /// <summary>
        ///     Copies the directory.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path</param>
        public string CopyDirectory(string sourcePath, string destinationPath)
        {
            if (destinationPath[destinationPath.Length - 1] != Path.DirectorySeparatorChar)
                destinationPath += Path.DirectorySeparatorChar;

            if (!Directory.Exists(destinationPath)) Directory.CreateDirectory(destinationPath);
            string[] files = Directory.GetFileSystemEntries(sourcePath);
            foreach (string file in files)
            {
                // Sub directories
                if (Directory.Exists(file))
                    CopyDirectory(file, destinationPath + Path.GetFileName(file));
                else
                {
                    File.Copy(file, destinationPath + Path.GetFileName(file), true);
                }
            }
            return destinationPath;
        }

        #endregion
    }
}