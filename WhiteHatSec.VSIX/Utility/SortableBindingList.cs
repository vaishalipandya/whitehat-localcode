using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WhiteHatSec.VSIX.Utility
{
    /// <summary>
    ///     Provides a generic collection that supports data binding and additionally supports sorting.
    ///     If the elements are IComparable it uses that; otherwise compares the ToString()
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SortableBindingList<T> : BindingList<T> where T : class
    {
        /// <summary>
        ///     The is sorted for sorting.
        /// </summary>
        private bool isSorted;

        /// <summary>
        ///     The sort direction.
        /// </summary>
        private ListSortDirection sortDirection = ListSortDirection.Ascending;

        /// <summary>
        ///     The sort property.
        /// </summary>
        private PropertyDescriptor sortProperty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SortableBindingList{T}" /> class.
        /// </summary>
        public SortableBindingList()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SortableBindingList{T}" /> class.
        /// </summary>
        /// <param name="list">
        ///     An <see cref="T:System.Collections.Generic.IList`1" /> of items to be contained in the
        ///     <see cref="T:System.ComponentModel.BindingList`1" />.
        /// </param>
        public SortableBindingList(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        ///     Gets a value indicating whether the list supports sorting.
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        ///     Gets a value indicating whether the list is sorted.
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        /// <summary>
        ///     Gets the direction the list is sorted.
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        /// <summary>
        ///     Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class;
        ///     otherwise, returns null.
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        /// <summary>
        ///     Removes any sort applied with ApplySortCore if sorting is implemented.
        /// </summary>
        protected override void RemoveSortCore()
        {
            sortDirection = ListSortDirection.Ascending;
            sortProperty = null;
            isSorted = false;
        }

        /// <summary>
        ///     Sorts the items if overridden in a derived class.
        /// </summary>
        /// <param name="propertyDescriptor"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor propertyDescriptor, ListSortDirection direction)
        {
            sortProperty = propertyDescriptor;
            sortDirection = direction;

            List<T> list = Items as List<T>;
            if (list == null) return;

            list.Sort(Compare);

            isSorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        ///     Compares the specified LHS.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns></returns>
        private int Compare(T lhs, T rhs)
        {
            int resultComparison = OnComparison(lhs, rhs);

            if (sortDirection == ListSortDirection.Descending)
                resultComparison = -resultComparison;
            return resultComparison;
        }

        /// <summary>
        ///     Called when [comparison].
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns></returns>
        private int OnComparison(T lhs, T rhs)
        {
            object lhsValue = lhs == null ? null : sortProperty.GetValue(lhs);
            object rhsValue = rhs == null ? null : sortProperty.GetValue(rhs);
            if (lhsValue == null)
            {
                return (rhsValue == null) ? 0 : -1;
            }

            if (rhsValue == null)
            {
                return 1;
            }

            if (lhsValue is IComparable)
            {
                return ((IComparable) lhsValue).CompareTo(rhsValue);
            }

            // both are the same
            if (lhsValue.Equals(rhsValue))
            {
                return 0;
            }

            // not comparable, compare ToString
            return lhsValue.ToString().CompareTo(rhsValue.ToString());
        }
    }
}