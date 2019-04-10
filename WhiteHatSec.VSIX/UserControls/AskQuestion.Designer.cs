using System.ComponentModel;
using WhiteHatSec.Localization.Culture.Resource;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.UserControls
{
    public partial class AskQuestion :BaseControl.BaseControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AskQuestion));
            this.PanelQA = new System.Windows.Forms.Panel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSubmit = new System.Windows.Forms.Button();
            this.TxtQuestion = new System.Windows.Forms.TextBox();
            this.LblQuestion = new System.Windows.Forms.Label();
            this.LblVulnId = new System.Windows.Forms.Label();
            this.LblVulnerabiltiesId = new System.Windows.Forms.Label();
            this.cultureManager1 = new WhiteHatSec.Localization.Culture.Resource.CultureManager(this.components);
            this.PanelQA.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelQA
            // 
            this.PanelQA.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelQA.Controls.Add(this.BtnCancel);
            this.PanelQA.Controls.Add(this.BtnSubmit);
            this.PanelQA.Controls.Add(this.TxtQuestion);
            this.PanelQA.Controls.Add(this.LblQuestion);
            this.PanelQA.Controls.Add(this.LblVulnId);
            this.PanelQA.Controls.Add(this.LblVulnerabiltiesId);
            resources.ApplyResources(this.PanelQA, "PanelQA");
            this.PanelQA.Name = "PanelQA";
            // 
            // BtnCancel
            // 
            this.BtnCancel.AutoEllipsis = true;
            resources.ApplyResources(this.BtnCancel, "BtnCancel");
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.AutoEllipsis = true;
            resources.ApplyResources(this.BtnSubmit, "BtnSubmit");
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.UseVisualStyleBackColor = true;
            // 
            // TxtQuestion
            // 
            resources.ApplyResources(this.TxtQuestion, "TxtQuestion");
            this.TxtQuestion.Name = "TxtQuestion";
            // 
            // LblQuestion
            // 
            resources.ApplyResources(this.LblQuestion, "LblQuestion");
            this.LblQuestion.Name = "LblQuestion";
            // 
            // LblVulnId
            // 
            resources.ApplyResources(this.LblVulnId, "LblVulnId");
            this.LblVulnId.Name = "LblVulnId";
            // 
            // LblVulnerabiltiesId
            // 
            resources.ApplyResources(this.LblVulnerabiltiesId, "LblVulnerabiltiesId");
            this.LblVulnerabiltiesId.Name = "LblVulnerabiltiesId";
            // 
            // cultureManager1
            // 
            this.cultureManager1.ManagedControl = this;
            // 
            // AskQuestion
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelQA);
            this.Name = "AskQuestion";
            this.PanelQA.ResumeLayout(false);
            this.PanelQA.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// The panel1
        /// </summary>
        public Panel PanelQA;
        
        /// <summary>
        /// The label vulnerabilties identifier
        /// </summary>
        public Label LblVulnerabiltiesId;

        /// <summary>
        /// The label vuln identifier
        /// </summary>
        public Label LblVulnId;

        /// <summary>
        /// The label question
        /// </summary>
        public Label LblQuestion;

        /// <summary>
        /// The text question
        /// </summary>
        public TextBox TxtQuestion;

        /// <summary>
        /// The BTN submit
        /// </summary>
        public Button BtnSubmit;

        /// <summary>
        /// The BTN cancel
        /// </summary>
        public Button BtnCancel;

        /// <summary>
        /// The culture manager1
        /// </summary>
        private CultureManager cultureManager1;
    }
}
