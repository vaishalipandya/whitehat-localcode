using System.ComponentModel;
using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.VSIX.TreeGridView;
using System.Windows.Forms;
namespace WhiteHatSec.VSIX.UserControls
{
   public partial class QaFilter :BaseControl.BaseControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QaFilter));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnAskQuestion = new System.Windows.Forms.Button();
            this.lblInstruction = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeGridViewQAFilter = new WhiteHatSec.VSIX.TreeGridView.TreeGridView();
            this.ColumnDate = new WhiteHatSec.VSIX.TreeGridView.TreeGridColumn();
            this.ColumnCommentedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnQuestion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTipQA = new System.Windows.Forms.ToolTip(this.components);
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.cultureManager1 = new WhiteHatSec.Localization.Culture.Resource.CultureManager(this.components);
            this.pictureBoxClearFilter = new System.Windows.Forms.PictureBox();
            this.pictureBoxRefresh = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridViewQAFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClearFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRefresh)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel1.Controls.Add(this.pictureBoxClearFilter);
            this.panel1.Controls.Add(this.txtFilter);
            this.panel1.Controls.Add(this.lblFilter);
            this.panel1.Controls.Add(this.pictureBoxRefresh);
            this.panel1.Controls.Add(this.btnAskQuestion);
            this.panel1.Controls.Add(this.lblInstruction);
            this.panel1.Name = "panel1";
            // 
            // txtFilter
            // 
            resources.ApplyResources(this.txtFilter, "txtFilter");
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.TextChanged += new System.EventHandler(this.TxtFilter_TextChanged);
            // 
            // lblFilter
            // 
            resources.ApplyResources(this.lblFilter, "lblFilter");
            this.lblFilter.Name = "lblFilter";
            // 
            // btnAskQuestion
            // 
            this.btnAskQuestion.AutoEllipsis = true;
            resources.ApplyResources(this.btnAskQuestion, "btnAskQuestion");
            this.btnAskQuestion.Name = "btnAskQuestion";
            this.btnAskQuestion.UseVisualStyleBackColor = true;
            this.btnAskQuestion.Click += new System.EventHandler(this.BtnAskQuestion_Click);
            // 
            // lblInstruction
            // 
            resources.ApplyResources(this.lblInstruction, "lblInstruction");
            this.lblInstruction.Name = "lblInstruction";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.treeGridViewQAFilter);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // treeGridViewQAFilter
            // 
            this.treeGridViewQAFilter.AllowUserToAddRows = false;
            this.treeGridViewQAFilter.AllowUserToDeleteRows = false;
            this.treeGridViewQAFilter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.treeGridViewQAFilter.BackgroundColor = System.Drawing.Color.White;
            this.treeGridViewQAFilter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeGridViewQAFilter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDate,
            this.ColumnCommentedBy,
            this.ColumnQuestion});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeGridViewQAFilter.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.treeGridViewQAFilter, "treeGridViewQAFilter");
            this.treeGridViewQAFilter.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.treeGridViewQAFilter.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.treeGridViewQAFilter.ImageList = null;
            this.treeGridViewQAFilter.Name = "treeGridViewQAFilter";
            this.treeGridViewQAFilter.RowHeadersVisible = false;
            this.treeGridViewQAFilter.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.treeGridViewQAFilter.ShowLines = false;
            // 
            // ColumnDate
            // 
            this.ColumnDate.DefaultNodeImage = null;
            resources.ApplyResources(this.ColumnDate, "ColumnDate");
            this.ColumnDate.Name = "ColumnDate";
            // 
            // ColumnCommentedBy
            // 
            resources.ApplyResources(this.ColumnCommentedBy, "ColumnCommentedBy");
            this.ColumnCommentedBy.Name = "ColumnCommentedBy";
            // 
            // ColumnQuestion
            // 
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnQuestion.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.ColumnQuestion, "ColumnQuestion");
            this.ColumnQuestion.Name = "ColumnQuestion";
            // 
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageStrip, "imageStrip");
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // cultureManager1
            // 
            this.cultureManager1.ManagedControl = this;
            // 
            // pictureBoxClearFilter
            // 
            this.pictureBoxClearFilter.Image = global::WhiteHatSec.VSIX.Resources.Resources.ClearEdit;
            resources.ApplyResources(this.pictureBoxClearFilter, "pictureBoxClearFilter");
            this.pictureBoxClearFilter.Name = "pictureBoxClearFilter";
            this.pictureBoxClearFilter.TabStop = false;
            this.toolTipQA.SetToolTip(this.pictureBoxClearFilter, resources.GetString("pictureBoxClearFilter.ToolTip"));
            this.pictureBoxClearFilter.Click += new System.EventHandler(this.PictureBoxClearFilter_Click);
            // 
            // pictureBoxRefresh
            // 
            this.pictureBoxRefresh.Image = global::WhiteHatSec.VSIX.Resources.Resources.refresh;
            resources.ApplyResources(this.pictureBoxRefresh, "pictureBoxRefresh");
            this.pictureBoxRefresh.Name = "pictureBoxRefresh";
            this.pictureBoxRefresh.TabStop = false;
            this.toolTipQA.SetToolTip(this.pictureBoxRefresh, resources.GetString("pictureBoxRefresh.ToolTip"));
            this.pictureBoxRefresh.Click += new System.EventHandler(this.PictureBoxRefresh_Click);
            // 
            // QaFilter
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "QaFilter";
            this.Load += new System.EventHandler(this.QAFilter_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridViewQAFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClearFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRefresh)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private Label lblInstruction;
        private Button btnAskQuestion;
        private PictureBox pictureBoxRefresh;
        private Panel panel2;
        public TreeGridView.TreeGridView treeGridViewQAFilter;
        private ToolTip toolTipQA;
        private ImageList imageStrip;
        private TextBox txtFilter;
        private Label lblFilter;
        private PictureBox pictureBoxClearFilter;
        private CultureManager cultureManager1;
        private TreeGridColumn ColumnDate;
        private DataGridViewTextBoxColumn ColumnCommentedBy;
        private DataGridViewTextBoxColumn ColumnQuestion;
    }
}
