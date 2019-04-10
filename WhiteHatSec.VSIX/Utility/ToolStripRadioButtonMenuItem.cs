using System;
using System.Drawing;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    ///     Tool Strip For Radio Button Menu Item
    /// </summary>
    public class ToolStripRadioButtonMenuItem : ToolStripMenuItem
    {
      
        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolStripRadioButtonMenuItem" /> class.
        /// </summary>
        public ToolStripRadioButtonMenuItem()
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolStripRadioButtonMenuItem" /> class.
        /// </summary>
        /// <param name="text">The text to display on the menu item.</param>
        public ToolStripRadioButtonMenuItem(string text)
            : base(text, null, (EventHandler) null)
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolStripRadioButtonMenuItem" /> class.
        /// </summary>
        /// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
        public ToolStripRadioButtonMenuItem(Image image)
            : base(null, image, (EventHandler) null)
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolStripRadioButtonMenuItem" /> class.
        /// </summary>
        /// <param name="text">The text to display on the menu item.</param>
        /// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
        public ToolStripRadioButtonMenuItem(string text, Image image)
            : base(text, image, (EventHandler) null)
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolStripRadioButtonMenuItem" /> class.
        /// </summary>
        /// <param name="tooltipText">The text to display on the menu item.</param>
        /// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
        /// <param name="onClick">
        ///     An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when
        ///     the control is clicked.
        /// </param>
        public ToolStripRadioButtonMenuItem(
            string tooltipText,
            Image image,
            EventHandler onClick)
            : base(tooltipText, image, onClick)
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolStripRadioButtonMenuItem" /> class.
        /// </summary>
        /// <param name="tooltipText">The text to display on the menu item.</param>
        /// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
        /// <param name="onClick">
        ///     An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when
        ///     the control is clicked.
        /// </param>
        /// <param name="name">The name of the menu item.</param>
        public ToolStripRadioButtonMenuItem(
            string tooltipText,
            Image image,
            EventHandler onClick,
            string name)
            : base(tooltipText, image, onClick, name)
        {
            Initialize();
        }

        /// <summary>
        /// </summary>
        /// <param name="text"></param>
        /// <param name="image"></param>
        /// <param name="dropDownItems"></param>
        public ToolStripRadioButtonMenuItem(
            string text,
            Image image,
            params ToolStripItem[] dropDownItems)
            : base(text, image, dropDownItems)
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolStripRadioButtonMenuItem" /> class.
        /// </summary>
        /// <param name="tooltipText">The text to display on the menu item.</param>
        /// <param name="image">The <see cref="T:System.Drawing.Image" /> to display on the control.</param>
        /// <param name="onClick">
        ///     An event handler that raises the <see cref="E:System.Windows.Forms.Control.Click" /> event when
        ///     the control is clicked.
        /// </param>
        /// <param name="shortcutKeys">
        ///     One of the values of <see cref="T:System.Windows.Forms.Keys" /> that represents the shortcut
        ///     key for the <see cref="T:System.Windows.Forms.ToolStripMenuItem" />.
        /// </param>
        public ToolStripRadioButtonMenuItem(
            string tooltipText,
            Image image,
            EventHandler onClick,
            Keys shortcutKeys)
            : base(tooltipText, image, onClick)
        {
            Initialize();
            ShortcutKeys = shortcutKeys;
        }      

        /// <summary>
        ///     Gets or sets a value indicating whether the control is enabled.
        /// </summary>
        /// <returns>true if the control is enabled; otherwise, false. The default is true.</returns>
        public override bool Enabled
        {
            get
            {
                ToolStripMenuItem ownerMenuItem =
                    OwnerItem as ToolStripMenuItem;

                // Use the base value in design mode to prevent the designer 
                // from setting the base value to the calculated value. 
                if (!DesignMode &&
                    ownerMenuItem != null && ownerMenuItem.CheckOnClick)
                {
                    return base.Enabled && ownerMenuItem.Checked;
                }

                return base.Enabled;
            }

            set { base.Enabled = value; }
        }

        // Called by all constructors to initialize CheckOnClick. 
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            CheckOnClick = true;
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.ToolStripMenuItem.CheckedChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);

            // If this item is no longer in the checked state or if its  
            // parent has not yet been initialized, do nothing. 
            if (!Checked || Parent == null) return;

            // Clear the checked state for all siblings.  
            foreach (ToolStripItem item in Parent.Items)
            {
                ToolStripRadioButtonMenuItem radioItem =
                    item as ToolStripRadioButtonMenuItem;
                if (radioItem != null && radioItem != this && radioItem.Checked)
                {
                    radioItem.Checked = false;

                    // Only one item can be selected at a time,  
                    // so there is no need to continue. 
                    return;
                }
            }
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.ToolStripItem.Click" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnClick(EventArgs e)
        {
            // If the item is already in the checked state, do not call  
            // the base method, which would toggle the value.  
            if (Checked) return;

            base.OnClick(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(EventArgs e)
        {    
            // Force the item to repaint with the new RadioButton state.
            Invalidate();

            base.OnMouseEnter(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {           
            base.OnMouseLeave(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {          
            // Force the item to repaint with the new RadioButton state.
            Invalidate();

            base.OnMouseDown(e);
        }

        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.ToolStripItem.MouseUp" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }
     
        /// <summary>
        ///     Raises the <see cref="E:System.Windows.Forms.ToolStripItem.OwnerChanged" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnOwnerChanged(EventArgs e)
        {
            ToolStripMenuItem ownerMenuItem =
                OwnerItem as ToolStripMenuItem;
            if (ownerMenuItem != null && ownerMenuItem.CheckOnClick)
            {
                ownerMenuItem.CheckedChanged +=
                    OwnerMenuItem_CheckedChanged;
            }

            base.OnOwnerChanged(e);
        }

        // When the checked state of the parent item changes,  
        // repaint the item so that the new Enabled state is displayed.  
        /// <summary>
        ///     Handles the CheckedChanged event of the OwnerMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OwnerMenuItem_CheckedChanged(
            object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}