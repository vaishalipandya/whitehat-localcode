using System.ComponentModel;
using WhiteHatSec.Localization.Culture.Resource;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.UserControls
{
  public partial class Login :BaseControl.BaseControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtAPIKey = new System.Windows.Forms.TextBox();
            this.lblAPIkey = new System.Windows.Forms.Label();
            this.lblOr = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.whsEUradioButton = new System.Windows.Forms.RadioButton();
            this.whsCOMradioButton = new System.Windows.Forms.RadioButton();
            this.cultureManager1 = new WhiteHatSec.Localization.Culture.Resource.CultureManager(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLogin
            // 
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // txtAPIKey
            // 
            resources.ApplyResources(this.txtAPIKey, "txtAPIKey");
            this.txtAPIKey.Name = "txtAPIKey";
            // 
            // lblAPIkey
            // 
            resources.ApplyResources(this.lblAPIkey, "lblAPIkey");
            this.lblAPIkey.Name = "lblAPIkey";
            // 
            // lblOr
            // 
            resources.ApplyResources(this.lblOr, "lblOr");
            this.lblOr.ForeColor = System.Drawing.Color.Blue;
            this.lblOr.Name = "lblOr";
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.Name = "lblPassword";
            // 
            // txtUserName
            // 
            resources.ApplyResources(this.txtUserName, "txtUserName");
            this.txtUserName.Name = "txtUserName";
            // 
            // lblUserName
            // 
            resources.ApplyResources(this.lblUserName, "lblUserName");
            this.lblUserName.Name = "lblUserName";
            // 
            // txtVersion
            // 
            this.txtVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txtVersion.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.txtVersion, "txtVersion");
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ReadOnly = true;
            this.txtVersion.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.whsEUradioButton);
            this.groupBox1.Controls.Add(this.whsCOMradioButton);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // whsEUradioButton
            // 
            resources.ApplyResources(this.whsEUradioButton, "whsEUradioButton");
            this.whsEUradioButton.Name = "whsEUradioButton";
            this.whsEUradioButton.TabStop = true;
            this.whsEUradioButton.UseVisualStyleBackColor = true;
            this.whsEUradioButton.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // whsCOMradioButton
            // 
            resources.ApplyResources(this.whsCOMradioButton, "whsCOMradioButton");
            this.whsCOMradioButton.Name = "whsCOMradioButton";
            this.whsCOMradioButton.TabStop = true;
            this.whsCOMradioButton.UseVisualStyleBackColor = true;
            this.whsCOMradioButton.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // cultureManager1
            // 
            this.cultureManager1.ManagedControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // serverTextBox
            // 
            resources.ApplyResources(this.serverTextBox, "serverTextBox");
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.TextChanged += new System.EventHandler(this.serverTextBox_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.serverTextBox);
            this.groupBox2.Controls.Add(this.lblUserName);
            this.groupBox2.Controls.Add(this.txtUserName);
            this.groupBox2.Controls.Add(this.lblPassword);
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.btnLogin);
            this.groupBox2.Controls.Add(this.lblOr);
            this.groupBox2.Controls.Add(this.txtAPIKey);
            this.groupBox2.Controls.Add(this.lblAPIkey);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // Login
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtVersion);
            this.Name = "Login";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Label lblUserName;
        private TextBox txtUserName;
        private Label lblPassword;
        private TextBox txtPassword;
        private Label lblOr;
        private Label lblAPIkey;
        private TextBox txtAPIKey;
        private Button btnLogin;
        private CultureManager cultureManager1;
        private TextBox txtVersion;
        private GroupBox groupBox1;
        public RadioButton whsEUradioButton;
        public RadioButton whsCOMradioButton;
        private Label label1;
        private GroupBox groupBox2;
        public TextBox serverTextBox;
    }
}
