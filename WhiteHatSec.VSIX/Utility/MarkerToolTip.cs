using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TextManager.Interop;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    ///     Text Marker To Highlight Code.
    /// </summary>
    internal class TextMarkerClient : IVsTextMarkerClient
    {
        /// <summary>
        ///     The tip text.
        /// </summary>
        private readonly string tipText;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TextMarkerClient" /> class.
        /// </summary>
        /// <param name="tipText">The tip text.</param>
        public TextMarkerClient(string tipText)
        {
            this.tipText = tipText;
        }

        /// <summary>
        ///     Called when the text associated with a marker is deleted by a user action.
        /// </summary>
        public virtual void MarkerInvalidated()
        {
        }

        /// <summary>
        ///     Called when [buffer save].
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public virtual void OnBufferSave(string fileName)
        {
        }

        /// <summary>
        ///     Sends notification that the text buffer is about to close.
        /// </summary>
        public virtual void OnBeforeBufferClose()
        {
        }

        /// <summary>
        ///     Signals that the text under the marker has been altered but the marker has not been deleted.
        /// </summary>
        public virtual void OnAfterSpanReload()
        {
        }

        /// <summary>
        ///     Called when [after marker change].
        /// </summary>
        /// <param name="marker">The marker.</param>
        /// <returns></returns>
        public virtual int OnAfterMarkerChange(IVsTextMarker marker)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Gets the tip text.
        /// </summary>
        /// <param name="textMarker">The text marker.</param>
        /// <param name="tipText">The tip text.</param>
        /// <returns></returns>
        public virtual int GetTipText(IVsTextMarker textMarker, string[] tipText)
        {
            if (tipText != null && tipText.Length > 0) tipText[0] = this.tipText;
            return VSConstants.S_OK;
        }

        /// <summary>
        ///     Gets the marker command information.
        /// </summary>
        /// <param name="textMarker">The text marker.</param>
        /// <param name="item">The item.</param>
        /// <param name="text">The text.</param>
        /// <param name="commandFlags">The command flags.</param>
        /// <returns></returns>
        public virtual int GetMarkerCommandInfo(IVsTextMarker textMarker, int item, string[] text, uint[] commandFlags)
        {
            // Returning S_OK results in error message appearing in editor's
            // context menu when you right click over the error message.
            if (commandFlags != null && commandFlags.Length > 0)
                commandFlags[0] = 0;
            if (text != null && text.Length > 0)
                text[0] = null;
            return VSConstants.E_NOTIMPL;
        }

        /// <summary>
        ///     Executes the marker command.
        /// </summary>
        /// <param name="textMarker">The marker.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public virtual int ExecMarkerCommand(IVsTextMarker textMarker, int item)
        {
            return VSConstants.S_OK;
        }
    }
}