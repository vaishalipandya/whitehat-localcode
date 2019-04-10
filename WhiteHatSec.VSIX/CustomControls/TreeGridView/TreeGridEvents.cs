using System.ComponentModel;

namespace WhiteHatSec.VSIX.TreeGridView
{

    /// <summary>
    /// Tree Grid Event Node
    /// </summary>
    public class TreeGridNodeEventBase
    {
        /// <summary>
        /// Tree Grid Node Event Base
        /// </summary>
        /// <param name="node"></param>
        public TreeGridNodeEventBase(TreeGridNode node)
        {
            Node = node;
        }

        /// <summary>
        /// Node 
        /// </summary>
        public TreeGridNode Node { get; private set; }
    }

    /// <summary>
    /// Collapsing Event Arguments
    /// </summary>
    public class CollapsingEventArgs : CancelEventArgs
    {
        private CollapsingEventArgs()
        {
        }

        /// <summary>
        /// Collapsing Event Arguments
        /// </summary>
        /// <param name="node"></param>
        public CollapsingEventArgs(TreeGridNode node)
        {
            Node = node;
        }
        /// <summary>
        /// Tree grid Node
        /// </summary>
        public TreeGridNode Node { get; private set; }
    }
    /// <summary>
    /// Collapsing Event Args
    /// </summary>
    public class CollapsedEventArgs : TreeGridNodeEventBase
    {
        /// <summary>
        /// Collapased Event Arguments
        /// </summary>
        /// <param name="node"></param>
        public CollapsedEventArgs(TreeGridNode node)
            : base(node)
        {
        }
    }

    /// <summary>
    /// Expanding Event Arguments
    /// </summary>
    public class ExpandingEventArgs : CancelEventArgs
    {
        private ExpandingEventArgs()
        {
        }

        /// <summary>
        /// Expanding Event Arguments
        /// </summary>
        /// <param name="node"></param>
        public ExpandingEventArgs(TreeGridNode node)
        {
            Node = node;
        }
        /// <summary>
        /// Node for tree grid
        /// </summary>
        public TreeGridNode Node { get; private set; }
    }

    /// <summary>
    /// Expanded Event Arguments
    /// </summary>
    public class ExpandedEventArgs : TreeGridNodeEventBase
    {
        /// <summary>
        /// Expanded Event Arguments
        /// </summary>
        /// <param name="node"></param>
        public ExpandedEventArgs(TreeGridNode node)
            : base(node)
        {
        }
    }
    /// <summary>
    /// Expanding Event Handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ExpandingEventHandler(object sender, ExpandingEventArgs e);

    /// <summary>
    /// Expanded Event Handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ExpandedEventHandler(object sender, ExpandedEventArgs e);
    /// <summary>
    /// Collapsing Event Handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CollapsingEventHandler(object sender, CollapsingEventArgs e);
    /// <summary>
    /// Collapsed Event Handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void CollapsedEventHandler(object sender, CollapsedEventArgs e);
}