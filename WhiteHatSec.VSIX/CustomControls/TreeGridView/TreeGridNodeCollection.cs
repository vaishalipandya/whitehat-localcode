using System;
using System.Collections;
using System.Collections.Generic;

namespace WhiteHatSec.VSIX.TreeGridView
{
    /// <summary>
    /// Tree Grid Node Collection
    /// </summary>
    public class TreeGridNodeCollection : IList<TreeGridNode>, IList
    {
        internal List<TreeGridNode> TreeGridList;
        internal TreeGridNode OwnerData;

        internal TreeGridNodeCollection(TreeGridNode owner)
        {
            OwnerData = owner;
            TreeGridList = new List<TreeGridNode>();
        }

        #region Public Members
        /// <summary>
        /// Add node
        /// </summary>
        /// <param name="treeGridNode"></param>
        public void Add(TreeGridNode treeGridNode)
        {
            // The row needs to exist in the child collection before the parent is notified.
            treeGridNode.GridData = OwnerData.GridData;

            bool hadChildren = OwnerData.HasChildren;
            treeGridNode.OwnerData = this;

            TreeGridList.Add(treeGridNode);

            OwnerData.AddChildNode(treeGridNode);

            // if the owner didn't have children but now does (asserted) and it is sited update it
            if (!hadChildren && OwnerData.IsSited)
            {
                OwnerData.GridData.InvalidateRow(OwnerData.RowIndex);
            }
        }
        /// <summary>
        /// Add Caption text to node
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>

        public TreeGridNode Add(string text)
        {
            TreeGridNode node = new TreeGridNode();
            Add(node);

            node.Cells[0].Value = text;
            return node;
        }
        /// <summary>
        /// Add Node
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public TreeGridNode Add(params object[] values)
        {
            TreeGridNode node = new TreeGridNode();
            Add(node);

            int cell = 0;

            if (values.Length > node.Cells.Count)
                throw new ArgumentOutOfRangeException("values");

            foreach (object value in values)
            {
                node.Cells[cell].Value = value;
                cell++;
            }
            return node;
        }

        /// <summary>
        /// Insert node item
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, TreeGridNode item)
        {
            // The row needs to exist in the child collection before the parent is notified.
            item.GridData = OwnerData.GridData;
            item.OwnerData = this;

            TreeGridList.Insert(index, item);

            OwnerData.InsertChildNode(index, item);
        }
        /// <summary>
        /// Remove node item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(TreeGridNode item)
        {
            // The parent is notified first then the row is removed from the child collection.
            OwnerData.RemoveChildNode(item);
            item.GridData = null;
            return TreeGridList.Remove(item);
        }

        /// <summary>
        /// Remove at index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            TreeGridNode row = TreeGridList[index];

            // The parent is notified first then the row is removed from the child collection.
            OwnerData.RemoveChildNode(row);
            row.GridData = null;
            TreeGridList.RemoveAt(index);
        }

        /// <summary>
        /// Clear node
        /// </summary>
        public void Clear()
        {
            // The parent is notified first then the row is removed from the child collection.
            OwnerData.ClearNodes();
            TreeGridList.Clear();
        }
        /// <summary>
        /// Index Of item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(TreeGridNode item)
        {
            return TreeGridList.IndexOf(item);
        }

        /// <summary>
        /// Tree Grid Node
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TreeGridNode this[int index]
        {
            get { return TreeGridList[index]; }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Check for item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(TreeGridNode item)
        {
            return TreeGridList.Contains(item);
        }

        /// <summary>
        /// Copy to node
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(TreeGridNode[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        /// <summary>
        /// Count 
        /// </summary>
        public int Count
        {
            get { return TreeGridList.Count; }
        }

        /// <summary>
        /// IS ReadOnly 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IList Interface

        void IList.Remove(object value)
        {
            Remove(value as TreeGridNode);
        }


        int IList.Add(object value)
        {
            TreeGridNode item = value as TreeGridNode;
            Add(item);
            return item.Index;
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }


        void IList.Clear()
        {
            Clear();
        }

        bool IList.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        int IList.IndexOf(object item)
        {
            return IndexOf(item as TreeGridNode);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, value as TreeGridNode);
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool IList.Contains(object value)
        {
            return Contains(value as TreeGridNode);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        #region IEnumerable<ExpandableRow> Members

        /// <summary>
        /// Get Enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TreeGridNode> GetEnumerator()
        {
            return TreeGridList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #endregion

        #region ICollection Members
        /// <summary>
        /// Is Synchronize 
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        /// <summary>
        /// Sync Root
        /// </summary>
        object ICollection.SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}