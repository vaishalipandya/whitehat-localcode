using System.ComponentModel;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.UserControls
{
   public partial class UploadProject
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadProject));
            this.lblProject = new System.Windows.Forms.Label();
            this.lstBoxProject = new System.Windows.Forms.ListBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnUpload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblProject
            // 
            resources.ApplyResources(this.lblProject, "lblProject");
            this.lblProject.Name = "lblProject";
            // 
            // lstBoxProject
            // 
            this.lstBoxProject.FormattingEnabled = true;
            resources.ApplyResources(this.lstBoxProject, "lstBoxProject");
            this.lstBoxProject.Name = "lstBoxProject";
            // 
            // lblURL
            // 
            resources.ApplyResources(this.lblURL, "lblURL");
            this.lblURL.Name = "lblURL";
            // 
            // txtServer
            // 
            resources.ApplyResources(this.txtServer, "txtServer");
            this.txtServer.Name = "txtServer";
            // 
            // btnUpload
            // 
            resources.ApplyResources(this.btnUpload, "btnUpload");
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // UploadProject
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.lblURL);
            this.Controls.Add(this.lstBoxProject);
            this.Controls.Add(this.lblProject);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadProject";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.UploadProject_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblProject;
        private ListBox lstBoxProject;
        private Label lblURL;
        private TextBox txtServer;
        private Button btnUpload;
    }
}