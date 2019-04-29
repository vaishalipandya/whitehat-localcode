using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.VisualStudio.PlatformUI;

namespace WhiteHatSec.VSIX.UserControls
{
    /// <summary>
    /// Ask Question
    /// </summary>
    public partial class AskQuestion :BaseControl.BaseControl
    {
        #region "Constructor"

        /// <summary>
        ///     Initializes a new instance of the <see cref="AskQuestion" /> class.
        /// </summary>
        public AskQuestion()
        {
            try
            {
                InitializeComponent();
                SetStyle(ControlStyles.SupportsTransparentBackColor, true);
                SetDefaultColors();
                VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
            }
            catch (Exception ex)
            {
                Log.Error("**** Error Occured On InitializeComponent ***** ", ex);
            }
        }

        #endregion
        void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            SetDefaultColors();
        }
        void SetDefaultColors()
        {
            TxtQuestion.BackColor = CurrentThemeBackColor;
            TxtQuestion.ForeColor = CurrentThemeForColor;
            BtnSubmit.BackColor = CurrentThemeBackColor;
            BtnSubmit.ForeColor = CurrentThemeForColor;
            BtnCancel.BackColor = CurrentThemeBackColor;
            BtnCancel.ForeColor = CurrentThemeForColor;
            PanelQA.BackColor = CurrentThemeBackColor;
            BackColor = CurrentThemeBackColor;
            ForeColor = CurrentThemeForColor;
        }
        #region "Properties"

        /// <summary>
        ///     Gets the form that the container control is assigned to.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:System.Windows.Forms.Form" /> that the container control is assigned to. This property will
        ///     return null if the control is hosted inside of Internet Explorer or in another hosting context where there is no
        ///     parent form.
        /// </returns>
        public VulnerabilityTrace VulnerabilityTraceInfo { get; set; }

        /// <summary>
        ///     The log instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
    }
}