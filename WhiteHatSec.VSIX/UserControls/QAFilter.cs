using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using WhiteHatSec.Entity;
using WhiteHatSec.Localization.Culture.Resource;
using WhiteHatSec.VSIX.TreeGridView;
using log4net;
using WhiteHatSec.Services;
using System.ComponentModel;
using Microsoft.VisualStudio.PlatformUI;

namespace WhiteHatSec.VSIX.UserControls
{
    /// <summary>
    ///     Question and Answer
    /// </summary>
    public partial class QaFilter : BaseControl.BaseControl
    {
        #region "Constructor"

        /// <summary>
        ///     Initializes a new instance of the <see cref="QaFilter" /> class.
        /// </summary>
        public QaFilter()
        {
            try
            {
                InitializeComponent();
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                SetDefaultColors();
                ColumnDate.HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                treeGridViewQAFilter.Sort(ColumnDate, ListSortDirection.Ascending);
                VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;

            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On InitializeComponent***** ", ex);
            }
        }

        #endregion
       
      
        #region "Properties and variable"
        /// <summary>
        /// Vulnerability trace
        /// </summary>
        public VulnerabilityTrace VulnerabilityTraceInfo { get; set; }

        /// <summary>
        ///     The _ question answer data.
        /// </summary>
        public List<QuestionAnswerInfo.QuestionAnswerCollection> QuestionAnswerData = new List<QuestionAnswerInfo.QuestionAnswerCollection>();
        /// <summary>
        /// Ask Question
        /// </summary>
        private AskQuestion askQuestion;

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>
        ///     The sort order.
        /// </value>
        public string SortOrder { get; set; }
        /// <summary>
        /// Logger Instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Ascending order
        /// </summary>
        public const string Ascending = "Ascending";
        /// <summary>
        /// Descending order
        /// </summary>
        public const string Descending = "Descending";
        /// <summary>
        /// Display date format
        /// </summary>
        public const string DisplayDateFormat = "MM-dd-yyyy";
        #endregion

        #region "Events"

        /// <summary>
        ///     Handles the Click event of the btnAskQuestion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void BtnAskQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                if (BaseInstance.IsAuthenticated || BaseInstance.IsAuthenticatedByApiKey)
                {
                    LoadAskQuestion();
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Ask Question button click***** ", ex);
                MessageBox.Show("Error Occured on on ask question click.", MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Click event of the btnCancel control to redirect to question list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                saveSortOrder();
                CancelQuestionSubmit();
                restoreSortOrder();
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On cancel button click of Ask Que. Form***** ", ex);
                MessageBox.Show("Error Occured while canceling for asking question.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Load event of the QAFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void QAFilter_Load(object sender, EventArgs e)
        {
            try
            {
                imageStrip.ImageSize = new Size(16, 16);
                imageStrip.TransparentColor = Color.Magenta;
                imageStrip.ImageSize = new Size(16, 16);
                imageStrip.Images.AddStrip(Resources.Resources.Arrow);
                treeGridViewQAFilter.ImageList = imageStrip;
                treeGridViewQAFilter.ShowPlusMinus = false;
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured On Load F&Q page***** ", ex);
            }
        }

        /// <summary>
        ///     Handles the Node Expanding event of the treeGridViewQAFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ExpandingEventArgs" /> instance containing the event data.</param>
        private void TreeGridViewQAFilter_NodeExpanding(object sender, ExpandingEventArgs e)
        {
            try
            {
                if (e.Node.Image != null)
                {
                    e.Node.Image = Resources.Resources.DownArrow;
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured Expand Question for Answers in List of F&Q Tab***** ", ex);
            }
        }

        /// <summary>
        ///     Handles the Node Collapsing event of the treeGridViewQAFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CollapsingEventArgs" /> instance containing the event data.</param>
        private void TreeGridViewQAFilter_NodeCollapsing(object sender, CollapsingEventArgs e)
        {
            try
            {
                if (e.Node.Image != null)
                {
                    e.Node.Image = Resources.Resources.Arrow;
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured Collapse Answers of Question in List of F&Q Tab***** ", ex);
            }
        }

        /// <summary>
        ///     Handles the Click event of the pictureBoxRefresh control to refresh data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PictureBoxRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                saveSortOrder();
                BindQaData(BaseInstance.ActiveVulnId, Ascending);
                restoreSortOrder();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error("****Error Occured on refreshing Questions and Answers of Vuln Id in F&Q Tab***** ", ex);
                MessageBox.Show("Error Occured while refreshing question and answer.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Handles the Text Changed event of the txtFilter control to filter data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void TxtFilter_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (QuestionAnswerData.Count <= 0)
                {
                    return;
                }
                string filterText = txtFilter.Text.Trim();
                Log.InfoFormat("****Going to filter QA List by Filter Key : '{0}'****", filterText);

                //Check for filter text empty or not , create data based on that
                if (string.IsNullOrEmpty(filterText))
                {
                    CreateQaData(QuestionAnswerData, Ascending);
                }
                else
                {
                    List<QuestionAnswerInfo.QuestionAnswerCollection> questionAnswerFilterCriteria = FilterQa
                          (filterText, QuestionAnswerData);
                    CreateQaData(questionAnswerFilterCriteria, SortOrder);
                }
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured on filter textbox text change event in F&Q Tab***** ", ex);
            }
        }

        private static SortOrder sortOrder;
        private static DataGridViewColumnSortMode sortDirection;
        private static string sortCol;
        private void saveSortOrder()
        {
            if (treeGridViewQAFilter.SortedColumn != null)
            {
                sortCol = treeGridViewQAFilter.SortedColumn.Name;
                sortOrder = treeGridViewQAFilter.SortedColumn.HeaderCell.SortGlyphDirection;
                sortDirection = treeGridViewQAFilter.SortedColumn.SortMode;
            }else
            {
                sortCol = null;
            }
        }
        private void restoreSortOrder()
        {
            if(sortCol == null)
            {
                return;
            }
            foreach(DataGridViewColumn col in treeGridViewQAFilter.Columns)
            {
                if (col.Name.Equals(sortCol))
                {
                   
                    col.HeaderCell.SortGlyphDirection = sortOrder;
                    col.SortMode = sortDirection;
                    treeGridViewQAFilter.Sort(col, sortOrder==System.Windows.Forms.SortOrder.Ascending?ListSortDirection.Ascending:ListSortDirection.Descending);
                    break;
                }
            }
        }

        /// <summary>
        ///     Handles the Click event of the pictureBoxClearFilter control to clear filter and rebind data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void PictureBoxClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                saveSortOrder();
                txtFilter.Text = string.Empty;
                if(!string.IsNullOrEmpty(BaseControl.BaseControl.BaseInstance.ActiveVulnId))
                {
                    CreateQaData(QuestionAnswerData, SortOrder);
                }
                else
                {
                    CreateQaData(null, SortOrder);
                }
                restoreSortOrder();
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured on filter textbox text change event in F&Q Tab***** ", ex);
                MessageBox.Show("Error occured on clear filter.", MessageLog.ErrorMessage, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        ///     Sort Vulnerability trace by Vuln Id
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DataGridViewCellMouseEventArgs" /> instance containing the event data.</param>
        private void TreeGridViewQAFilter_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        ///     Handles the Click event of the btnSubmit control to post question.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                string question = askQuestion.TxtQuestion.Text;
                if (!BaseInstance.IsAuthenticated && !BaseInstance.IsAuthenticatedByApiKey)
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                saveSortOrder();
                //Check if text entered for post question to server otherwise display message to enter question
                if (string.IsNullOrEmpty(question))
                {
                    //Display message tti enter question
                    MessageBox.Show(MessageLog.PleaseEnterQuestion, MessageLog.Message, MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                SubmitQuestion(question);
                restoreSortOrder();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error("****Error Occured on Submitting Question in F&Q Tab***** ", ex);
                MessageBox.Show("Error occured while submitting question.", MessageLog.ErrorMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            SetDefaultColors();
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// Cancel question submit
        /// </summary>
        public void CancelQuestionSubmit()
        {
            if (BaseInstance.IsAuthenticated || BaseInstance.IsAuthenticatedByApiKey)
            {
                LoadQuestionAnswer();
                CreateQaData(QuestionAnswerData, SortOrder);
            }
        }
        /// <summary>
        /// Filter Qustion answer data
        /// </summary>
        /// <param name="filterText"></param>
        /// <param name="questionAnswerData"></param>
        /// <returns></returns>
        public List<QuestionAnswerInfo.QuestionAnswerCollection> FilterQa(string filterText, List<QuestionAnswerInfo.QuestionAnswerCollection> questionAnswerData)
        {
            return
                          questionAnswerData.Where(t =>
                              t.Author.Contains(filterText) ||
                              t.Topic.ToLower().Contains(filterText.ToLower()) ||
                              t.Created.ToString().Contains(filterText)).ToList();
        }
        /// <summary>
        /// Submit Question 
        /// </summary>
        /// <param name="question"></param>
        public void SubmitQuestion(string question)
        {
            int postQuestionResponseCode = 0;

            if (BaseInstance.IsAuthenticatedByApiKey)
            {
                postQuestionResponseCode = PostQuestionByApiKey(question, BaseInstance.SentinelServerName,
                   BaseInstance.ActiveVulnId, BaseInstance.ApiKey);
            }
            else
            {
                postQuestionResponseCode = PostQuestion(question, BaseInstance.SentinelServerName,
                   BaseInstance.ActiveVulnId, BaseInstance.SentinelCookie);
            }
            //Display proper message based on response code
            if (postQuestionResponseCode == 201)
            {
                Cursor.Current = Cursors.Default;
                LoadQuestionAnswer();
                BindQaData(BaseInstance.ActiveVulnId, Ascending);
                return;
            }

            //Display message if other status code except "created" found from response
            Cursor.Current = Cursors.Default;
            MessageBox.Show(MessageLog.FailedToPostQuestion, MessageLog.Message, MessageBoxButtons.OK,
                MessageBoxIcon.Information);

        }
        /// <summary>
        /// Post Question to server 
        /// </summary>
        /// <param name="question"></param>
        /// <param name="server"></param>
        /// <param name="vulnId"></param>
        /// <param name="sentinelCookie"></param>
        /// <returns></returns>
        public int PostQuestion(string question, string server, string vulnId, CookieContainer sentinelCookie)
        {
            //Check by APi key authentication, as we have seperate api for different authentication
            return QaService.PostQuestion(server, sentinelCookie, vulnId, question.Trim(), string.Empty);
        }

        /// <summary>
        /// Post Question By Api key
        /// </summary>
        /// <param name="question"></param>
        /// <param name="server"></param>
        /// <param name="vulnId"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public int PostQuestionByApiKey(string question, string server, string vulnId, string apiKey)
        {
            //Check by APi key authentication, as we have seperate api for different authentication
            return QaService.PostQuestion(server, null, vulnId, question.Trim(), apiKey);
        }
        /// <summary>
        ///     Redirects to ask question.
        /// </summary>
        public void LoadAskQuestion()
        {
            try
            {

                if (Int32.Parse(BaseInstance.ActiveVulnId) <= 0)
                {
                    return;
                }

                Log.Info("****Going to redirect Ask Question Form*****");
                ClearControl();
                ShowAskQuestion(BaseInstance.ActiveVulnId);
                AddControl();
                Log.Info("****Successfully redirect Ask Question Form*****");
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured on redirecting Ask Question Form***** ", ex);
            }
        }
        /// <summary>
        /// Clear Control 
        /// </summary>
        public void ClearControl()
        {
            VulnerabilityTraceInfo.PanelQA.Controls.Clear();
        }
        /// <summary>
        /// Show Ask Question with data
        /// </summary>
        public void ShowAskQuestion(string vulnId)
        {
            askQuestion = new AskQuestion();
            askQuestion.BtnCancel.Click += BtnCancel_Click;
            askQuestion.BtnSubmit.Click += BtnSubmit_Click;
            askQuestion.Dock = DockStyle.Fill;
            askQuestion.VulnerabilityTraceInfo = VulnerabilityTraceInfo;
            askQuestion.LblVulnId.Text = vulnId;
        }
        /// <summary>
        /// Add control to Vulnerability 
        /// </summary>
        public void AddControl()
        {
            VulnerabilityTraceInfo.PanelQA.Controls.Add(askQuestion);
        }
        /// <summary>
        ///     Redirects to Question answer.
        /// </summary>
        public void LoadQuestionAnswer()
        {
            try
            {
                Log.Info("****Going to redirect QA listing form*****");
                ClearControl();
                VulnerabilityTraceInfo.PanelQA.Controls.Add(this);
                Log.Info("****Successfully QA listing form*****");
            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured on redirecting QA listing form***** ", ex);
            }
        }

        /// <summary>
        ///      Binds the question answer data by Vuln id.
        /// </summary>
        /// <param name="vulnId">The vuln identifier.</param>
        /// <param name="sortOrder">The sort order.</param>
        public void BindQaData(string vulnId, string sortOrder)
        {
            try
            {
                if (string.IsNullOrEmpty(vulnId))
                {
                    List<QuestionAnswerInfo.QuestionAnswerCollection> questionAnswers = new List<QuestionAnswerInfo.QuestionAnswerCollection>();

                    CreateQaData(questionAnswers, sortOrder);
                    return;
                }
                QuestionAnswerInfo.QuestionAnswerData questionAnswerData = new QuestionAnswerInfo.QuestionAnswerData();
                try
                {
                    //Check user authentication for api key or user id, password , as we have separate rest service 
                    if (BaseInstance.IsAuthenticatedByApiKey)
                    {
                        questionAnswerData = GetQaDataByApiKey(BaseInstance.SentinelServerName, vulnId, BaseInstance.ApiKey);
                    }
                    else
                    {
                        questionAnswerData = GetQaData(BaseInstance.SentinelServerName, vulnId, BaseInstance.SentinelCookie);
                    }

                    QuestionAnswerData = questionAnswerData.Collection;
                    CreateQaData(QuestionAnswerData, sortOrder);
                    Log.Info("****Successfully to bind QA by VulnId*****");
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.Message, MessageLog.ErrorMessage, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Log.Error(ex.Message, ex);
                }


            }
            catch (Exception ex)
            {
                Log.Error("****Error Occured on binding QA by VulnId" + vulnId + "***** ", ex);
            }
        }
        /// <summary>
        /// Get Question answer data by Api key
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="vulnId"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public QuestionAnswerInfo.QuestionAnswerData GetQaDataByApiKey(string serverName, string vulnId, string apiKey)
        {
            return QaService.GetQuestionAnswers(serverName, null, vulnId,
                   apiKey);
        }
        /// <summary>
        /// Get question answer data by Sentinel cookie
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="vulnId"></param>
        /// <param name="sentinelCookie"></param>
        /// <returns></returns>
        public QuestionAnswerInfo.QuestionAnswerData GetQaData(string serverName, string vulnId, CookieContainer sentinelCookie)
        {
            return QaService.GetQuestionAnswers(serverName,
                sentinelCookie, vulnId, string.Empty);
        }
        /// <summary>
        /// Sort Qustion answer data
        /// </summary>
        /// <param name="questionAnswerList"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public List<QuestionAnswerInfo.QuestionAnswerCollection> SortQaData(List<QuestionAnswerInfo.QuestionAnswerCollection> questionAnswerList, string sortOrder)
        {
            if (sortOrder == Ascending)
                questionAnswerList = questionAnswerList.OrderBy
                    (x => x.Created).ToList();
            else questionAnswerList = questionAnswerList.OrderByDescending(x => x.Created).ToList();
            return questionAnswerList;
        }

        /// <summary>
        ///    Create Tree structure for Question answer data 
        /// </summary>
        /// <param name="questionAnswers">The  question answer collection.</param>
        /// <param name="sortOrder">The sort order.</param>
        public void CreateQaData(List<QuestionAnswerInfo.QuestionAnswerCollection> questionAnswers, string sortOrder)
        {
            try
            {
                // litte bit complex method as we are using merge two control TreeView and grid and assigning data

                treeGridViewQAFilter.ShowPlusMinus = false;
                int questionAnswerCount = questionAnswers.Count;

                questionAnswers = SortQaData(questionAnswers, sortOrder);

                treeGridViewQAFilter.Nodes.Clear(); //clear data 
                for (int i = 0; i < questionAnswerCount; i++)
                {
                    bool isParent = true;
                    string[] questionAnswer = new string[4];
                    if (!string.IsNullOrEmpty(questionAnswers[i].Author)) //author 
                        questionAnswer = questionAnswers[i].Author.Split(new[] { "/" }, StringSplitOptions.None);
                    string qauthor = questionAnswer[3] != string.Empty ? questionAnswer[3] : string.Empty;
                    TreeGridNode node = treeGridViewQAFilter.Nodes.Add(
                        questionAnswers[i].Created, qauthor,
                        questionAnswers[i].Topic);

                    GenerateQaTreeStructure(i, node, questionAnswers, isParent);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        /// <summary>
        /// Generate Tree structure
        /// </summary>
        /// <param name="i"></param>
        /// <param name="node"></param>
        /// <param name="questionAnswerList"></param>
        /// <param name="isParent"></param>
        public void GenerateQaTreeStructure(int i, TreeGridNode node, List<QuestionAnswerInfo.QuestionAnswerCollection> questionAnswerList, bool isParent)
        {
            Color backColor = Color.FromArgb(185, 209, 234);
            if (i == 0)
                node.Selected = false;

            node.ImageIndex = 0;
            foreach (QuestionAnswerInfo.ResponseCollection questionAnswerItem in questionAnswerList[i].Responses.Collection)
            {
                string[] authorInfo = new string[4];
                if (!string.IsNullOrEmpty(questionAnswerItem.Author))
                {
                    authorInfo = questionAnswerItem.Author.Split(new[] { "/" }, StringSplitOptions.None);
                }

                string authorName = authorInfo[3] != string.Empty ? authorInfo[3] : string.Empty;
                if (!isParent)
                {
                    //generate child element for question
                    node = node.Parent.Nodes.Add(null, authorName, questionAnswerItem.Content);
                    node.DefaultCellStyle.BackColor = backColor;
                    node.DefaultCellStyle.ForeColor = Color.Blue;
                }
                else
                {
                    //Generate parent 
                    node = node.Nodes.Add(null, authorName, questionAnswerItem.Content);
                    node.DefaultCellStyle.BackColor = backColor;
                    node.DefaultCellStyle.ForeColor = Color.Blue;
                    isParent = false;
                }
            }
            if (questionAnswerList[i].Responses.Collection.Count == 0)
            {
                node.Image = null;
                node.ImageIndex = -1;
            }
        }
        void SetDefaultColors()
        {
            Color defaultBackground = CurrentThemeBackColor;
            Color defaultForeground = CurrentThemeForColor;
            panel1.BackColor = CurrentThemeBackColor;
            btnAskQuestion.BackColor = CurrentThemeBackColor;
            btnAskQuestion.ForeColor = CurrentThemeForColor;
            treeGridViewQAFilter.GridColor = defaultBackground;
            treeGridViewQAFilter.BackgroundColor = defaultBackground;
            treeGridViewQAFilter.ForeColor = defaultForeground;
            treeGridViewQAFilter.DefaultCellStyle.BackColor = defaultBackground;
            treeGridViewQAFilter.DefaultCellStyle.ForeColor = defaultForeground;
            treeGridViewQAFilter.ColumnHeadersDefaultCellStyle.BackColor = defaultBackground;
            treeGridViewQAFilter.ColumnHeadersDefaultCellStyle.ForeColor = defaultForeground;
            treeGridViewQAFilter.EnableHeadersVisualStyles = false;
            treeGridViewQAFilter.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            txtFilter.BackColor = CurrentThemeBackColor;
            txtFilter.ForeColor = CurrentThemeForColor;
            BackColor = CurrentThemeBackColor;
        }
        #endregion
    }
}