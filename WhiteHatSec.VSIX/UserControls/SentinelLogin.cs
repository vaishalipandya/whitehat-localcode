using System.Windows.Forms;

namespace WhiteHatSec.VSIX.UserControls
{
    /// <summary>
    /// </summary>
    public partial class SentinelLogin : Form
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SentinelLogin" /> class.
        /// </summary>
        public SentinelLogin()
        {
            InitializeComponent();
        }

        private void SentinelLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("After login, you can populate data into \"Sentinel\" tab.", "Message", MessageBoxButtons.OK,
                MessageBoxIcon.Question);
        }
    }
}