using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace WhiteHatSec.VSIX.TreeGridView
{
    /// <summary>
    /// Tree Grid Node
    /// </summary>
    [
        ToolboxItem(false),
        DesignTimeVisible(false)
    ]
    public class TreeGridNode : DataGridViewRow
    {
        private readonly Random rndSeed = new Random();
        internal TreeGridView GridData;
        internal Image ImageData;
        internal int ImageIndexData;
        private int indexData;
        internal bool IsFirstSiblingData;
        internal bool IsLastSiblingData;
        internal bool IsSitedData;
        private int levelData;
        internal TreeGridNodeCollection OwnerData;
        internal TreeGridNode ParentData;
        private TreeGridCell treeCellData;
        private bool childCellsCreated;
        private TreeGridNodeCollection childrenNodes;
        private EventHandler disposed;
        internal bool IsExpanded;
        internal bool IsRoot;
        /// <summary>
        /// Unique value
        /// </summary>
        public int UniqueValue = -1;

        public int vulnID { get; set; }

        internal TreeGridNode(TreeGridView owner)
            : this()
        {
            GridData = owner;
            IsExpanded = true;
        }
        /// <summary>
        /// Tree Grid Node
        /// </summary>
        public TreeGridNode()
        {
            Site = null;
            indexData = -1;
            levelData = -1;
            IsExpanded = false;
            UniqueValue = rndSeed.Next();
            IsSitedData = false;
            IsFirstSiblingData = false;
            IsLastSiblingData = false;
            ImageIndexData = -1;
        }

        // Represents the index of this row in the Grid
        /// <summary>
        /// Row Index
        /// </summary>
        [Description("Represents the index of this row in the Grid. Advanced usage."),
         EditorBrowsable(EditorBrowsableState.Advanced),
         Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RowIndex
        {
            get { return base.Index; }
        }
        
        // Represents the index of this row based upon its position in the collection.
        /// <summary>
        /// Index
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new int Index
        {
            get
            {
                if (indexData == -1)
                {
                    // get the index from the collection if unknown
                    indexData = OwnerData.IndexOf(this);
                }

                return indexData;
            }
            internal set { indexData = value; }
        }

        /// <summary>
        /// Image List
        /// </summary>
        [Browsable(false),
         EditorBrowsable(EditorBrowsableState.Never),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImageList ImageList
        {
            get
            {
                if (GridData != null)
                    return GridData.ImageList;
                return null;
            }
        }
        /// <summary>
        /// Image Index
        /// </summary>
        [Category("Appearance"),
         Description("..."), DefaultValue(-1),
         TypeConverter(typeof (ImageIndexConverter)),
         Editor("System.Windows.Forms.Design.ImageIndexEditor", typeof (UITypeEditor))]
        public int ImageIndex
        {
            get { return ImageIndexData; }
            set
            {
                ImageIndexData = value;
                if (ImageIndexData != -1)
                {                   
                    ImageData = null;
                }
                if (IsSitedData)
                {
                    // when the image changes the cell's style must be updated
                    treeCellData.UpdateStyle();
                    if (Displayed)
                        GridData.InvalidateRow(RowIndex);
                }
            }
        }

        /// <summary>
        /// Image
        /// </summary>
        public Image Image
        {
            get
            {
                if (ImageData == null && ImageIndexData != -1)
                {
                    if (ImageList != null && ImageIndexData < ImageList.Images.Count)
                    {
                        // get image from image index
                        return ImageList.Images[ImageIndexData];
                    }
                    return null;
                }
                // image from image property
                return ImageData;
                ;
            }
            set
            {
                ImageData = value;
                if (ImageData != null)
                {
                    // when a image is provided we do not store the imageIndex.
                    ImageIndexData = -1;
                }
                if (IsSitedData)
                {
                    // when the image changes the cell's style must be updated
                    treeCellData.UpdateStyle();
                    if (Displayed)
                        GridData.InvalidateRow(RowIndex);
                }
            }
        }

        /// <summary>
        /// Tree Grid Node Collection
        /// </summary>
        [Category("Data"),
         Description("The collection of root nodes in the treelist."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         Editor(typeof (CollectionEditor), typeof (UITypeEditor))]
        public TreeGridNodeCollection Nodes
        {
            get
            {
                if (childrenNodes == null)
                {
                    childrenNodes = new TreeGridNodeCollection(this);
                }
                return childrenNodes;
            }
            set { ; }
        }

        // Create a new Cell property because by default a row is not in the grid and won't
        // have any cells. We have to fabricate the cell collection ourself.
        /// <summary>
        /// Cells
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewCellCollection Cells
        {
            get
            {
                if (!childCellsCreated && DataGridView == null)
                {
                    if (GridData == null) return null;

                    CreateCells(GridData);
                    childCellsCreated = true;
                }
                return base.Cells;
            }
        }

        /// <summary>
        /// Level
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Level
        {
            get
            {
                if (levelData == -1)
                {
                    // calculate level
                    int walk = 0;
                    TreeGridNode walkRow = Parent;
                    while (walkRow != null)
                    {
                        walk++;
                        walkRow = walkRow.Parent;
                    }
                    levelData = walk;
                }
                return levelData;
            }
        }
        /// <summary>
        /// Parent
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TreeGridNode Parent
        {
            get { return ParentData; }
        }
        /// <summary>
        /// Check for children
        /// </summary>
        [Browsable(false),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool HasChildren
        {
            get { return childrenNodes != null && Nodes.Count != 0; }
        }
        /// <summary>
        /// Is Sited
        /// </summary>
        [Browsable(false)]
        public bool IsSited
        {
            get { return IsSitedData; }
        }

        /// <summary>
        /// Check for fisrst sibling
        /// </summary>
        [Browsable(false)]
        public bool IsFirstSibling
        {
            get { return Index == 0; }
        }
        /// <summary>
        /// Check for last sibling
        /// </summary>
        [Browsable(false)]
        public bool IsLastSibling
        {
            get
            {
                TreeGridNode parent = Parent;
                if (parent != null && parent.HasChildren)
                {
                    return Index == parent.Nodes.Count - 1;
                }
                return true;
            }
        }
        /// <summary>
        /// Site
        /// </summary>
        [
            Browsable(false),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public ISite Site { get; set; }
        /// <summary>
        /// Clone
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            TreeGridNode treegridNode = (TreeGridNode)base.Clone();
            treegridNode.UniqueValue = -1;
            treegridNode.levelData = levelData;
            treegridNode.GridData = GridData;
            treegridNode.ParentData = Parent;

            treegridNode.ImageIndexData = ImageIndexData;
            if (treegridNode.ImageIndexData == -1)
                treegridNode.Image = Image;

            treegridNode.IsExpanded = IsExpanded;


            return treegridNode;
        }
        /// <summary>
        /// Unsited Node
        /// </summary>
        protected internal virtual void UnSited()
        {
            // This row is being removed from being displayed on the grid.
            TreeGridCell cell;
            foreach (DataGridViewCell datagridCellItem in Cells)
            {
                cell = datagridCellItem as TreeGridCell;
                if (cell != null)
                {
                    cell.UnSited();
                }
            }
            IsSitedData = false;
        }

        /// <summary>
        /// Sited
        /// </summary>
        protected internal virtual void Sited()
        {
            // This row is being added to the grid.
            IsSitedData = true;
            childCellsCreated = true;   
            TreeGridCell cell;
            foreach (DataGridViewCell datagridCellInfo in Cells)
            {
                cell = datagridCellInfo as TreeGridCell;
                if (cell != null)
                {
                    cell.Sited();
                }
            }
        }
        /// <summary>
        /// Serialize Image Index
        /// </summary>
        /// <returns></returns>
        private bool ShouldSerializeImageIndex()
        {
            return ImageIndexData != -1 && ImageData == null;
        }

        private bool ShouldSerializeImage()
        {
            return ImageIndexData == -1 && ImageData != null;
        }
        /// <summary>
        /// Override for datagridViewCollection
        /// </summary>
        /// <returns></returns>
        protected override DataGridViewCellCollection CreateCellsInstance()
        {
            DataGridViewCellCollection cells = base.CreateCellsInstance();
            cells.CollectionChanged += Cells_CollectionChanged;
            return cells;
        }
        /// <summary>
        /// Cell collection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cells_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            // Exit if there already is a tree cell for this row
            if (treeCellData != null) return;

            if (e.Action == CollectionChangeAction.Add || e.Action == CollectionChangeAction.Refresh)
            {
                TreeGridCell treeCell = null;

                if (e.Element == null)
                {
                    foreach (DataGridViewCell cell in base.Cells)
                    {
                        if (cell.GetType().IsAssignableFrom(typeof (TreeGridCell)))
                        {
                            treeCell = (TreeGridCell) cell;
                            break;
                        }
                    }
                }
                else
                {
                    treeCell = e.Element as TreeGridCell;
                }

                if (treeCell != null)
                    treeCellData = treeCell;
            }
        }

        /// <summary>
        /// Collapse
        /// </summary>
        /// <returns></returns>

        public virtual bool Collapse()
        {
            return GridData.CollapseNode(this);
        }
        /// <summary>
        /// Expand
        /// </summary>
        /// <returns></returns>

        public virtual bool Expand()
        {
            if (GridData != null)
                return GridData.ExpandNode(this);
            IsExpanded = true;
            return true;
        }
        /// <summary>
        /// Insert Child Node
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        protected internal virtual bool InsertChildNode(int index, TreeGridNode node)
        {
            node.ParentData = this;
            node.GridData = GridData;

            // ensure that all children of this node has their grid set
            if (GridData != null)
                UpdateChildNodes(node);


            if ((IsSitedData || IsRoot) && IsExpanded)
                GridData.SiteNode(node);
            return true;
        }
        /// <summary>
        /// Insert Child Node
        /// </summary>
        /// <param name="index"></param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected internal virtual bool InsertChildNodes(int index, params TreeGridNode[] nodes)
        {
            foreach (TreeGridNode node in nodes)
            {
                InsertChildNode(index, node);
            }
            return true;
        }
        /// <summary>
        /// Add Single Node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected internal virtual bool AddChildNode(TreeGridNode node)
        {
            node.ParentData = this;
            node.GridData = GridData;

            // ensure that all children of this node has their grid set
            if (GridData != null)
                UpdateChildNodes(node);

            if ((IsSitedData || IsRoot) && IsExpanded && !node.IsSitedData)
                GridData.SiteNode(node);

            return true;
        }
        /// <summary>
        /// Add Child Nodes
        /// </summary>
        /// <param name="nodes"></param>
        /// <returns></returns>
        protected internal virtual bool AddChildNodes(params TreeGridNode[] nodes)
        {
            foreach (TreeGridNode node in nodes)
            {
                AddChildNode(node);
            }
            return true;
        }
        /// <summary>
        /// Remove ChildNodes
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected internal virtual bool RemoveChildNode(TreeGridNode node)
        {
            if ((IsRoot || IsSitedData) && IsExpanded)
            {
                //We only unsite out child node if we are sited and expanded.
                GridData.UnSiteNode(node);
            }
            node.GridData = null;
            node.ParentData = null;
            return true;
        }
        /// <summary>
        /// Clear  Nodes
        /// </summary>
        /// <returns></returns>
        protected internal virtual bool ClearNodes()
        {
            if (HasChildren)
            {
                for (int i = Nodes.Count - 1; i >= 0; i--)
                {
                    Nodes.RemoveAt(i);
                }
            }
            return true;
        }

        /// <summary>
        /// Event Handler for Dispose
        /// </summary>
        [
            Browsable(false),
            EditorBrowsable(EditorBrowsableState.Advanced)
        ]
        public event EventHandler Disposed
        {
            add { disposed += value; }
            remove { disposed -= value; }
        }
        /// <summary>
        /// Update Child Nodes
        /// </summary>
        /// <param name="node"></param>
        private void UpdateChildNodes(TreeGridNode node)
        {
            if (node.HasChildren)
            {
                foreach (TreeGridNode childNode in node.Nodes)
                {
                    childNode.GridData = node.GridData;
                    UpdateChildNodes(childNode);
                }
            }
        }
        /// <summary>
        /// Override string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(36);
            sb.Append("TreeGridNode { Index=");
            sb.Append(RowIndex.ToString(CultureInfo.CurrentCulture));
            sb.Append(" }");
            return sb.ToString();
        }
    }
}