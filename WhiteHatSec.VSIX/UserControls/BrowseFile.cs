using System;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace WhiteHatSec.VSIX.UserControls
{
    /// <summary>
    ///     Browse File control
    /// </summary>
    public partial class BrowseFile : Form
    {
        #region "Properties"
        /// <summary>
        /// Log instance
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger
            (MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region "Constructor"

        /// <summary>
        ///     Initializes a new instance of the <see cref="BrowseFile" /> class.
        /// </summary>
        public BrowseFile()
        {
            try
            {
                InitializeComponent();
                LblBrowseFile.AutoSize = false;
                
            }
            catch (Exception ex)
            {
                Log.Error("**** Error Occured On InitializeComponent ***** ", ex);
            }
        }

        #endregion
    }
}