using Microsoft.VisualStudio.PlatformUI;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WhiteHatSec.VSIX.TreeGridView
{
    /// <summary>
    ///     Summary description for TreeGridView.
    /// </summary>
    [DesignerCategory("code"),
     Designer(typeof(ControlDesigner)),
     ComplexBindingProperties,
     Docking(DockingBehavior.Ask)]
    public class TreeGridView : DataGridView
    {
        private readonly TreeGridNode treeGridRoot;
        private TreeGridColumn expandableColumnData;
        internal ImageList ImageListData;
        private bool inExpandCollapseData = true;
        internal bool InExpandCollapseMouseCaptureData;
        private bool showLinesData;
        private bool showPlusMinusData;
        private Control hideScrollBarControl;


        #region Constructor

        /// <summary>
        /// Tree grid View
        /// </summary>
        public TreeGridView()
        {
            VirtualNodes = false;
            // Control when edit occurs because edit mode shouldn't start when expanding/collapsing
            EditMode = DataGridViewEditMode.EditProgrammatically;
            RowTemplate = new TreeGridNode();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            // This sample does not support adding or deleting rows by the user.
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            treeGridRoot = new TreeGridNode(this);
            treeGridRoot.IsRoot = true;
            // Ensures that all rows are added unshared by listening to the CollectionChanged event.
            base.Rows.CollectionChanged += delegate { };
        }

        #endregion
        #region Keyboard F2 to begin edit support
            /// <summary>
            /// On Key down 
            /// </summary>
            /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Cause edit mode to begin since edit mode is disabled to support 
            // expanding/collapsing 
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                if (e.KeyCode == Keys.F2 && CurrentCellAddress.X > -1 && CurrentCellAddress.Y > -1)
                {
                    if (!CurrentCell.Displayed)
                    {
                        FirstDisplayedScrollingRowIndex = CurrentCellAddress.Y;
                    }

                    SelectionMode = DataGridViewSelectionMode.CellSelect;
                    BeginEdit(true);
                }
                else if (e.KeyCode == Keys.Enter && !IsCurrentCellInEditMode)
                {
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    CurrentCell.OwningRow.Selected = true;
                }
            }
        }

        #endregion

        #region Shadow and hide DGV properties
        /// <summary>
        /// data source
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataSource
        {
            get { return null; }
            set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
        }
        /// <summary>
        /// Data Member
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Never)]
        public new object DataMember
        {
            get { return null; }
            set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
        }

        /// <summary>
        /// Row Collection
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRowCollection Rows
        {
            get { return base.Rows; }
        }

        /// <summary>
        /// Virtual Mode
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Never)]
        public new bool VirtualMode
        {
            get { return false; }
            set { throw new NotSupportedException("The TreeGridView does not support virtual mode"); }
        }

        // none of the rows/nodes created use the row template, so it is hidden.
        /// <summary>
        /// Row Template to add 
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
         EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate; }
            set { base.RowTemplate = value; }
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Get node for row
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(DataGridViewRow row)
        {
            return row as TreeGridNode;
        }
        /// <summary>
        /// Get node for row
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(int index)
        {
            return GetNodeForRow(base.Rows[index]);
        }

        #endregion

        #region Public properties
        /// <summary>
        /// Nodes
        /// </summary>
        [Category("Data"),
         Description("The collection of root nodes in the treelist."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public TreeGridNodeCollection Nodes
        {
            get { return treeGridRoot.Nodes; }
        }

        /// <summary>
        /// Current Row
        /// </summary>
        public new TreeGridNode CurrentRow
        {
            get { return base.CurrentRow as TreeGridNode; }
        }
        /// <summary>
        /// Virtual Nodes
        /// </summary>
        [DefaultValue(false),
         Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes.")]
        public bool VirtualNodes { get; set; }
        /// <summary>
        /// Current Node
        /// </summary>
        public TreeGridNode CurrentNode
        {
            get { return CurrentRow; }
        }

        /// <summary>
        /// Show Lines
        /// </summary>
        [DefaultValue(true)]
        public bool ShowLines
        {
            get { return showLinesData; }
            set
            {
                if (value != showLinesData)
                {
                    showLinesData = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Show Plus Minus for expand 
        /// </summary>
        [DefaultValue(false)]
        public bool ShowPlusMinus
        {
            get { return showPlusMinusData; }
            set
            {
                if (value != showPlusMinusData)
                {
                    showPlusMinusData = value;
                    Invalidate();
                }
            }
        }
        /// <summary>
        /// Image list
        /// </summary>
        public ImageList ImageList
        {
            get { return ImageListData; }
            set { ImageListData = value; }
        }

        /// <summary>
        /// Row Count
        /// </summary>
        public new int RowCount
        {
            get { return Nodes.Count; }
            set
            {
                for (int i = 0; i < value; i++)
                    Nodes.Add(new TreeGridNode());
            }
        }

        #endregion

        #region Site nodes and collapse/expand support
        /// <summary>
        /// On Row Added
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            // Notify the row when it is added to the base grid 
            int count = e.RowCount - 1;
            TreeGridNode row;
            while (count >= 0)
            {
                row = base.Rows[e.RowIndex + count] as TreeGridNode;
                if (row != null)
                {
                    row.Sited();
                }
                count--;
            }
        }

        /// <summary>
        /// UnSite All
        /// </summary>
        protected internal void UnSiteAll()
        {
            if (treeGridRoot != null)
                UnSiteNode(treeGridRoot);
        }

        /// <summary>
        /// UnSite Node
        /// </summary>
        /// <param name="node"></param>
        protected internal virtual void UnSiteNode(TreeGridNode node)
        {
            if (node != null)
            {
                if (node.IsSited || node.IsRoot)
                {
                    // remove child rows first
                    foreach (TreeGridNode childNode in node.Nodes)
                    {
                        UnSiteNode(childNode);
                    }

                    // now remove this row except for the root
                    if (!node.IsRoot)
                    {
                        base.Rows.Remove(node);
                        node.UnSited();
                    }
                }
            }
        }

        /// <summary>
        /// Collapse Node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected internal virtual bool CollapseNode(TreeGridNode node)
        {
            if (node.IsExpanded)
            {
                CollapsingEventArgs expandGridNode = new CollapsingEventArgs(node);
                OnNodeCollapsing(expandGridNode);

                if (!expandGridNode.Cancel)
                {
                    LockVerticalScrollBarUpdate(true);
                    SuspendLayout();
                    inExpandCollapseData = true;
                    node.IsExpanded = false;

                    foreach (TreeGridNode childNode in node.Nodes)
                    {

                        UnSiteNode(childNode);
                    }

                    CollapsedEventArgs collapsedNode = new CollapsedEventArgs(node);
                    OnNodeCollapsed(collapsedNode);

                    inExpandCollapseData = false;
                    LockVerticalScrollBarUpdate(false);
                    ResumeLayout(true);
                    InvalidateCell(node.Cells[0]);
                }

                return !expandGridNode.Cancel;
            }

            return false;
        }

        /// <summary>
        /// Site Node
        /// </summary>
        /// <param name="node"></param>
        protected internal virtual void SiteNode(TreeGridNode node)
        {
            int rowIndex = -1;
            TreeGridNode currentRow;
            node.GridData = this;

            if (node.Parent != null && node.Parent.IsRoot == false)
            {
                // row is a child
                if (node.Index > 0)
                {
                    currentRow = node.Parent.Nodes[node.Index - 1];
                }
                else
                {
                    currentRow = node.Parent;
                }
            }
            else
            {
                // row is being added to the root
                if (node.Index > 0)
                {
                    currentRow = node.Parent.Nodes[node.Index - 1];
                }
                else
                {
                    currentRow = null;
                }
            }

            if (currentRow != null)
            {
                while (currentRow.Level >= node.Level)
                {
                    if (currentRow.RowIndex < base.Rows.Count - 1)
                    {
                        currentRow = base.Rows[currentRow.RowIndex + 1] as TreeGridNode;
                    }
                    else
                        // no more rows, site this node at the end.
                        break;
                }
                if (currentRow == node.Parent)
                    rowIndex = currentRow.RowIndex + 1;
                else if (currentRow.Level < node.Level)
                    rowIndex = currentRow.RowIndex;
                else
                    rowIndex = currentRow.RowIndex + 1;
            }
            else
                rowIndex = 0;
            SiteNode(node, rowIndex);

            if (node.IsExpanded)
            {
                // add all child rows to display
                foreach (TreeGridNode childNode in node.Nodes)
                {
                    SiteNode(childNode);
                }
            }
        }

        /// <summary>
        /// Site Node
        /// </summary>
        /// <param name="node"></param>
        /// <param name="index"></param>
        protected internal virtual void SiteNode(TreeGridNode node, int index)
        {
            if (index < base.Rows.Count)
            {
                base.Rows.Insert(index, node);
            }
            else
            {
                // for the last item.
                base.Rows.Add(node);
            }
        }

        /// <summary>
        /// Expand Node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected internal virtual bool ExpandNode(TreeGridNode node)
        {
            if (!node.IsExpanded || VirtualNodes)
            {
                ExpandingEventArgs expandNode = new ExpandingEventArgs(node);
                OnNodeExpanding(expandNode);

                if (!expandNode.Cancel)
                {
                    LockVerticalScrollBarUpdate(true);
                    SuspendLayout();
                    inExpandCollapseData = true;
                    node.IsExpanded = true;


                    foreach (TreeGridNode childNode in node.Nodes)
                    {


                        SiteNode(childNode);
                    }

                    ExpandedEventArgs expandedNode = new ExpandedEventArgs(node);
                    OnNodeExpanded(expandedNode);

                    inExpandCollapseData = false;
                    LockVerticalScrollBarUpdate(false);
                    ResumeLayout(true);
                    InvalidateCell(node.Cells[0]);
                }

                return !expandNode.Cancel;
            }

            return false;
        }

        /// <summary>
        /// On Mouse up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // used to keep extra mouse moves from selecting more rows when collapsing
            base.OnMouseUp(e);
            InExpandCollapseMouseCaptureData = false;
        }

        /// <summary>
        /// On Mouse Move
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // while we are expanding and collapsing a node mouse moves are
            // supressed to keep selections from being messed up.
            if (!InExpandCollapseMouseCaptureData)
                base.OnMouseMove(e);
        }

        #endregion

        #region Collapse/Expand events
        /// <summary>
        /// Node Expanding
        /// </summary>
        public event ExpandingEventHandler NodeExpanding;
        /// <summary>
        /// Node Expanded
        /// </summary>
        public event ExpandedEventHandler NodeExpanded;
        /// <summary>
        /// Node Collapsing
        /// </summary>
        public event CollapsingEventHandler NodeCollapsing;
        /// <summary>
        /// Node collapsed
        /// </summary>
        public event CollapsedEventHandler NodeCollapsed;
        /// <summary>
        /// On Node Expanding
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (NodeExpanding != null)
            {
                NodeExpanding(this, e);
            }
        }

        /// <summary>
        /// On Node Expanded
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (NodeExpanded != null)
            {
                NodeExpanded(this, e);
            }
        }

        /// <summary>
        /// On Node Collapsing
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (NodeCollapsing != null)
            {
                NodeCollapsing(this, e);
            }
        }

        /// <summary>
        /// On Node collapsed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (NodeCollapsed != null)
            {
                NodeCollapsed(this, e);
            }
        }

        #endregion

        #region Helper methods
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            treeGridRoot.Dispose();
            base.Dispose(Disposing);
            UnSiteAll();
        }

        /// <summary>
        /// On handle created
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // this control is used to temporarly hide the vertical scroll bar
            hideScrollBarControl = new Control();
            hideScrollBarControl.Visible = false;
            hideScrollBarControl.Enabled = false;
            hideScrollBarControl.TabStop = false;
            // control is disposed automatically when the grid is disposed
            Controls.Add(hideScrollBarControl);
        }
        /// <summary>
        /// On ROw Enters
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            // ensure full row select
            base.OnRowEnter(e);
            if (SelectionMode == DataGridViewSelectionMode.CellSelect ||
                (SelectionMode == DataGridViewSelectionMode.FullRowSelect &&
                 base.Rows[e.RowIndex].Selected == false))
            {
                SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                base.Rows[e.RowIndex].Selected = true;
            }
        }

        private void LockVerticalScrollBarUpdate(bool lockUpdate)
        {
            // Temporarly hide/show the vertical scroll bar by changing its parent
            if (!inExpandCollapseData)
            {
                if (lockUpdate)
                {
                    VerticalScrollBar.Parent = hideScrollBarControl;
                }
                else
                {
                    VerticalScrollBar.Parent = this;
                }
            }
        }

        /// <summary>
        /// On Double click
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        /// <summary>
        /// On column added
        /// </summary>
        /// <param name="e"></param>
        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (typeof(TreeGridColumn).IsAssignableFrom(e.Column.GetType()))
            {
                if (expandableColumnData == null)
                {
                    // identify the expanding column.			
                    expandableColumnData = (TreeGridColumn)e.Column;
                }
            }

            base.OnColumnAdded(e);
        }

        #endregion
    }
}