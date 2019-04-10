using System.ComponentModel;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.UserControls
{
    public partial class BrowseFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseFile));
            this.BtnBrowseFile = new System.Windows.Forms.Button();
            this.LblBrowseFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnBrowseFile
            // 
            resources.ApplyResources(this.BtnBrowseFile, "BtnBrowseFile");
            this.BtnBrowseFile.Name = "BtnBrowseFile";
            this.BtnBrowseFile.UseVisualStyleBackColor = true;
            // 
            // LblBrowseFile
            // 
            resources.ApplyResources(this.LblBrowseFile, "LblBrowseFile");
            this.LblBrowseFile.ForeColor = System.Drawing.Color.Navy;
            this.LblBrowseFile.Name = "LblBrowseFile";
            // 
            // BrowseFile
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LblBrowseFile);
            this.Controls.Add(this.BtnBrowseFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BrowseFile";
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The BTN browse file
        /// </summary>
        public Button BtnBrowseFile;
        /// <summary>
        /// The label browse file
        /// </summary>
        public Label LblBrowseFile;
    }
}