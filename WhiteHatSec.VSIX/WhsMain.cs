//------------------------------------------------------------------------------
// <copyright file="WhsMain.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace WhiteHatSec.VSIX
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using WhiteHatSec.VSIX.UserControls;
    using System.Windows.Forms;
    using EnvDTE80;
    using Microsoft.VisualStudio.PlatformUI;
    using System.Drawing;
    using static System.Windows.Forms.Control;


    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("9cfa46d9-b8cd-4f8b-bc9c-9a74866622d5")]
    public class WhsMain : ToolWindowPane
    {
        WhsMainWindow control = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhsMain"/> class.
        /// </summary>
        public WhsMain() : base(null)
        {
            this.Caption = "WhiteHat Sentinel IDE Plugin";
            this.control = new WhsMainWindow();

            this.control.VisualStudioCurrentInstance = WhsMainCommand.Instance.dte;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = null;
            SetDefaultColors();
            VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
        }
        /// <summary>
        /// according to visual studio current theme apply  to window
        /// </summary>
        /// <param name="e"></param>
        void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            SetDefaultColors();
        }
        /// <summary>
        /// according to visual studio current  theme apply to window
        /// </summary>
        void SetDefaultColors()
        {
            Color defaultBackground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
            Color defaultForeground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey);
            
            UpdateWindowColors(defaultBackground, defaultForeground);
        }
        /// <summary>
        /// update window background color
        /// </summary>
        /// <param name="clrBackground"></param>
        /// <param name="clrForeground"></param>
        void UpdateWindowColors(Color clrBackground, Color clrForeground)
        {
            // Update the window background
            control.BackColor = clrBackground;
            control.ForeColor = clrForeground;
            
            // Also update the label
            UpdateRecursively(clrBackground, clrForeground, control);
        }
        /// <summary>
        /// update window lable color
        /// </summary>
        /// <param name="clrBackground"></param>
        /// <param name="clrForeground"></param>
        /// <param name="control"></param>
        void UpdateRecursively(Color clrBackground, Color clrForeground, Control control)
        {
            foreach (Control child in control.Controls)
            {
                child.BackColor = clrBackground;
                child.ForeColor = clrForeground;
                UpdateRecursively(clrBackground, clrForeground, child);
            }
        }
        override public IWin32Window Window
        {
            get
            {
                return (IWin32Window)control;
            }
        }
    }
}
