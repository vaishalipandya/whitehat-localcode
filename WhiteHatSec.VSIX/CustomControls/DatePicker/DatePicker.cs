using Microsoft.VisualStudio.PlatformUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.DatePicker
{
    /// <summary>
    ///     Custom Date Picker
    /// </summary>
    public partial class CustomDateTimePicker : DateTimePicker, ISupportInitialize
    {
        #region Constructor

        /// <summary>
        ///     Basic Constructer + ReadOnly _textBox initialization
        /// </summary>
        public CustomDateTimePicker()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();

            InitTextBox();          
            base.Format = DateTimePickerFormat.Custom;
            formatDate = DateTimePickerFormat.Long;

            if (DesignMode)
            {
                SetFormat();
            }
            SetDefaultColors();
            VSColorTheme.ThemeChanged += VSColorTheme_ThemeChanged;
        }

        #endregion

        #region Member Variables Added To Allow Null Values

        private DateTimePickerFormat formatDate;

        /// Variable to store 'Format'
        private string customFormatDate;

        /// Variable to store 'CustomFormat'
        private string emptyTextDate = "";

        /// Variable to store empty Display Text

        #endregion

        #region Member Variables Added To Enable ReadOnly Mode
        private bool readOnlyDate; 

        private bool visibleDate = true; //Overridden to show the proper Display for Readonly Mode
        private bool tabStopWhenReadOnly; 
        private TextBox textBox; //TextBox Decorated when in ReadOnly Mode

        #endregion

        #region ISupportInitialize Members

        /// <summary>
        ///     The initializing
        /// </summary>
        private bool initializing = true;

        /// <summary>
        ///     Signals the object that initialization is starting.
        /// </summary>
        public void BeginInit()
        {
            initializing = true;
        }

        /// <summary>
        ///     Signals the object that initialization is complete.
        /// </summary>
        public void EndInit()
        {
            //Default the value to Today
            base.Value = DateTime.Today;

            initializing = false;

            if (DesignMode)
            {
                return;
            }

            if (Parent.GetType() == typeof (TableLayoutPanel))
            {
                TableLayoutPanelCellPosition locationPoint = ((TableLayoutPanel) Parent).GetPositionFromControl(this);

                ((TableLayoutPanel) Parent).Controls.Add(textBox, locationPoint.Column, locationPoint.Row);

                ((TableLayoutPanel) Parent).SetColumnSpan(textBox, ((TableLayoutPanel) Parent).GetColumnSpan(this));

                textBox.Anchor = Anchor;
            }


            else if (Parent.GetType() == typeof (FlowLayoutPanel))
            {
                ((FlowLayoutPanel) Parent).Controls.Add(textBox);

                ((FlowLayoutPanel) Parent).Controls.SetChildIndex(textBox,
                    ((FlowLayoutPanel) Parent).Controls.IndexOf(this));

                textBox.Anchor = Anchor;
            }

            else
            {
                textBox.Parent = Parent;

                textBox.Anchor = Anchor;
            }


            Control parent = this;

            bool foundLoadingParent = false;

            do
            {
                parent = parent.Parent;

                if (parent.GetType().IsSubclassOf(typeof (UserControl)))
                {
                    ((UserControl) parent).Load += CustomDateTimePicker_Load;

                    foundLoadingParent = true;
                }

                else if (parent.GetType().IsSubclassOf(typeof (Form)))
                {
                    ((Form) parent).Load += CustomDateTimePicker_Load;

                    foundLoadingParent = true;
                }
            } while (!foundLoadingParent);
        }

        private void CustomDateTimePicker_Load(object sender, EventArgs e)
        {
            SetVisibility();
        }

        #endregion

        #region Public Properties Modified/Added To Allow Null Values

        /// <summary>
        ///     The _is null
        /// </summary>
        private bool isNull;

       /// <summary>
       /// assign value
       /// </summary>
        public new object Value
        {
            get
            {
                if (isNull)
                {
                    return null;
                }

                return base.Value;
            }

            set
            {
                if (value == null || value == DBNull.Value)
                {
                    if (!isNull)
                    {
                        isNull = true;

                        OnValueChanged(EventArgs.Empty);
                    }
                }

                else
                {
                    //if null and value matches base.value take out of null and fire event    
                    if (isNull && base.Value == (DateTime) value)
                    {
                        isNull = false;

                        OnValueChanged(EventArgs.Empty);
                    }
                    else
                    {
                        //change to the new value(changed event fires from base class
                        isNull = false;

                        base.Value = (DateTime) value;
                    }
                }

                //refresh format
                SetFormat();
                textBox.Text = Text;
            }
        }

        #region DesignerModifiers

        /// <summary>
        ///     Gets or sets the null text.
        /// </summary>
        /// <value>
        ///     The null text.
        /// </value>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Text shown when DateTime is 'null'")]
        [DefaultValue("")]

        #endregion

            public string NullText
        {
            get { return emptyTextDate; }

            set { emptyTextDate = value; }
        }

        /// <summary>
        ///     Modified Format Property stores the assigned Format and the propagates the change to base.CustomFormat
        /// </summary>

        #region DesignerModifiers
        [Browsable(true)]
        [DefaultValue(DateTimePickerFormat.Long), TypeConverter(typeof (Enum))]

        #endregion

            public new DateTimePickerFormat Format
        {
            get { return formatDate; }

            set
            {
                formatDate = value;

                SetFormat();
            }
        }

        private void SetFormat()
        {          
            base.CustomFormat = null;
            //If null apply NullText
            if (isNull)
            {
                base.CustomFormat = string.Concat("'", NullText, "'");
            }

            else
            {
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;

                DateTimeFormatInfo dateFormatInfo = cultureInfo.DateTimeFormat;

                switch (formatDate)
                {
                    case DateTimePickerFormat.Long:

                        base.CustomFormat = dateFormatInfo.LongDatePattern;

                        break;

                    case DateTimePickerFormat.Short:

                        base.CustomFormat = "MM-dd-yyyy";

                        break;

                    case DateTimePickerFormat.Time:

                        base.CustomFormat = dateFormatInfo.ShortTimePattern;

                        break;

                    case DateTimePickerFormat.Custom:

                        base.CustomFormat = customFormatDate;

                        break;
                }
            }
        }

       /// <summary>
       /// Custom Format
       /// </summary>
        public new string CustomFormat
        {
            get { return customFormatDate; }

            set
            {
                customFormatDate = value;

                SetFormat();
            }
        }

        #endregion

        #region Public Properties Modified/Added To Enable ReadOnly Mode

        #region DesignerModifiers

        /// <summary>
        ///     Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [read only]; otherwise, <c>false</c>.
        /// </value>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Displays Control as ReadOnly(Black on Gray) if 'true'")]
        [DefaultValue(false)]

        #endregion

            public bool ReadOnly
        {
            get { return readOnlyDate; }

            set
            {
                readOnlyDate = value;

                SetVisibility();
            }
        }

        #region DesignerModifiers

        /// <summary>
        ///     Gets or sets a value indicating whether [tabstop when read only].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [tabstop when read only]; otherwise, <c>false</c>.
        /// </value>
        [Category("Behavior")]
        [DefaultValue(false)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]

        #endregion

            public bool TabstopWhenReadOnly
        {
            get { return tabStopWhenReadOnly; }

            set
            {
                tabStopWhenReadOnly = value;            
                textBox.TabStop = tabStopWhenReadOnly && TabStop;
            }
        }

       /// <summary>
       /// TabStop Property 
       /// </summary>
        public new bool TabStop
        {
            get { return base.TabStop; }

            set
            {
                base.TabStop = value;

                textBox.TabStop = tabStopWhenReadOnly && base.TabStop;
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the control and all its child controls are displayed.
        /// </summary>
        /// <returns>true if the control and all its child controls are displayed; otherwise, false. The default is true.</returns>      
        public new bool Visible
        {
            get { return visibleDate; }

            set
            {
                visibleDate = value;

                SetVisibility();
            }
        }

        #endregion

        #region OnXXXX() Modified To Allow Null Values

        /// <summary>
        ///     Overrides the <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" /> method.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x4e)
            {
                Nmhdr nm = (Nmhdr) m.GetLParam(typeof (Nmhdr));

                if (nm.Code == -746 || nm.Code == -722)
                {
                    //propagate change form base 
                    Value = base.Value;
                }
            }

            base.WndProc(ref m);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Nmhdr
        {
            /// <summary>
            ///     The HWND from
            /// </summary>
            private readonly IntPtr hwndFrom;

            /// <summary>
            ///     The identifier from
            /// </summary>
            private readonly int idFrom;

            /// <summary>
            ///     The code
            /// </summary>
            public readonly int Code;
        }

        /// <summary>
        ///     Sets UDTP Value to null when Delete or Backspace is pressed
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                Value = null;
            }

            base.OnKeyUp(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data.</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (isNull && char.IsDigit(e.KeyChar))
            {
                Value = base.Value;

                e.Handled = true;

                SendKeys.Send("{RIGHT}");

                SendKeys.Send(e.KeyChar.ToString());
            }

            else
                base.OnKeyPress(e);
        }

        #endregion

        #region OnXXXX() Modified To Enable ReadOnly Mode

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            if (DesignMode || initializing)
            {
                return;
            }

            //update the TextBox parent
            UpdateReadOnlyTextBoxParent();

            SetVisibility(); // Reset Visibilty for new parent
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.LocationChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            textBox.Location = Location;
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            textBox.Size = Size;
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            textBox.Size = Size;
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.DockChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnDockChanged(EventArgs e)
        {
            base.OnDockChanged(e);

            textBox.Dock = Dock;
        }


        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);

            textBox.RightToLeft = RightToLeft;
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.TabStopChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnTabStopChanged(EventArgs e)
        {
            base.OnTabStopChanged(e);

            textBox.TabStop = tabStopWhenReadOnly && TabStop;
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.TabIndexChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);

            textBox.TabIndex = TabIndex;
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.FontChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            textBox.Font = Font;
        }

        #endregion

        #region Private Methods Added To Enable ReadOnly Mode

        /// <summary>
        ///     Added to initialize the textBox to the default values to match the DTP
        /// </summary>
        private void InitTextBox()
        {
            if (DesignMode)
            {
                return;
            }

            textBox = new TextBox
            {
                ReadOnly = true,
                Location = Location,
                Size = Size,
                Dock = Dock,
                Anchor = Anchor,
                RightToLeft = RightToLeft,
                Font = Font,
                TabStop = TabStop,
                TabIndex = TabIndex,
                Visible = false,
                Parent = Parent
            };
        }

        private void SetVisibility()
        {
            if (DesignMode || initializing)
            {
                
                return;
            }

            if (visibleDate)
            {
                if (readOnlyDate)
                {
                
                    ShowTextBox();
                }
                else
                {
                  
                    ShowDtp();
                }
            }

            else
            {
              
                ShowNone();
            }
        }

        private void ShowTextBox()
        {
            base.Visible = false;

            textBox.Visible = true;

            textBox.TabStop = tabStopWhenReadOnly && TabStop;
        }

        private void ShowDtp()
        {
            textBox.Visible = false;

            base.Visible = true;
        }

        private void ShowNone()
        {
            textBox.Visible = false;

            base.Visible = false;
        }

        private void UpdateReadOnlyTextBoxParent()
        {
          

            if (Parent == null)
            {
                textBox.Parent = null;
                return;
            }

         
            if (textBox.Parent != Parent)
            {
                if (Parent.GetType() == typeof (TableLayoutPanel))
                {
                    TableLayoutPanelCellPosition cP = ((TableLayoutPanel) Parent).GetPositionFromControl(this);

                    ((TableLayoutPanel) Parent).Controls.Add(textBox, cP.Column, cP.Row);

                    ((TableLayoutPanel) Parent).SetColumnSpan(textBox, ((TableLayoutPanel) Parent).GetColumnSpan(this));

                    textBox.Anchor = Anchor;
                }


                else if (Parent.GetType() == typeof (FlowLayoutPanel))
                {
                    ((FlowLayoutPanel) Parent).Controls.Add(textBox);

                    ((FlowLayoutPanel) Parent).Controls.SetChildIndex(textBox,
                        ((FlowLayoutPanel) Parent).Controls.IndexOf(this));

                    textBox.Anchor = Anchor;
                }

                else
                {
                    textBox.Parent = Parent;

                    textBox.Anchor = Anchor;
                }
            }
        }

        #endregion

        #region Public Methods Overriden To Enable ReadOnly Mode

        /// <summary>
        ///     Displays the control to the user.
        /// </summary>       
        public new void Show()
        {
            Visible = true;
        }

        /// <summary>
        ///     Conceals the control from the user.
        /// </summary>       
        public new void Hide()
        {
            Visible = false;
        }

        #endregion
        #region Events
        protected override void OnPaint(PaintEventArgs e)
        {
            var currentThemeBackColor = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
            var currentThemeForColor = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey);      
            e.Graphics.FillRectangle(new SolidBrush(currentThemeBackColor), this.ClientRectangle);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(currentThemeForColor), 5, 3);
            e.Graphics.DrawImage(WhiteHatSec.VSIX.Resources.Resources.Calender, new Point(this.ClientRectangle.X + this.ClientRectangle.Width - 16, this.ClientRectangle.Y));

            Rectangle borderRectangle = this.ClientRectangle;
            ControlPaint.DrawBorder(e.Graphics, borderRectangle, VSColorTheme.GetThemedColor(EnvironmentColors.DropDownBorderColorKey), ButtonBorderStyle.Solid);
        }
        private void VSColorTheme_ThemeChanged(ThemeChangedEventArgs e)
        {
            SetDefaultColors();
        }
        #endregion Events
        #region Methods
        void SetDefaultColors()
        {
            this.Invalidate();
        }
        #endregion Methods
    }
}