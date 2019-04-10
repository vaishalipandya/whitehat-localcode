using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WhiteHatSec.VSIX.TreeGridView
{   
        /// <summary>
        ///     Summary description for TreeGridCell.
        /// </summary>
        public class TreeGridCell : DataGridViewTextBoxCell
        {
            private const int INDENTWIDTH = 20;
            private const int INDENTMARGIN = 5;
            private int imageWidthData, imageHeightData, imageHeightOffsetData;
            private Padding previousPaddingData;
            private int calculatedLeftPadding;
            private int glyphWidth;
            internal bool IsSited;
            /// <summary>
            /// Tree Grid Cell
            /// </summary>
            public TreeGridCell()
            {
                glyphWidth = 15;
                calculatedLeftPadding = 0;
                IsSited = false;
            }

            /// <summary>
            /// Tree grid node level
            /// </summary>
            public int Level
            {
                get
                {
                    TreeGridNode row = OwningNode;
                    if (row != null)
                    {
                        return row.Level;
                    }
                    return -1;
                }
            }

            /// <summary>
            /// Glyph Margin
            /// </summary>
            protected virtual int GlyphMargin
            {
                get { return ((Level - 1) * INDENTWIDTH) + INDENTMARGIN; }
            }

            /// <summary>
            /// Glyoh Offset
            /// </summary>
            protected virtual int GlyphOffset
            {
                get { return (Level - 1) * INDENTWIDTH; }
            }

            /// <summary>
            /// Owning Node
            /// </summary>
            public TreeGridNode OwningNode
            {
                get { return OwningRow as TreeGridNode; }
            }
            /// <summary>
            /// Clone 
            /// </summary>
            /// <returns></returns>
            public override object Clone()
            {
                TreeGridCell clone = (TreeGridCell)base.Clone();

                clone.glyphWidth = glyphWidth;
                clone.calculatedLeftPadding = calculatedLeftPadding;

                return clone;
            }

            /// <summary>
            /// UnSited Node
            /// </summary>
            protected internal virtual void UnSited()
            {
                // The row this cell is in is being removed from the grid.
                IsSited = false;
                Style.Padding = previousPaddingData;
            }

            /// <summary>
            /// sited node
            /// </summary>
            protected internal virtual void Sited()
            {
                // when we are added to the DGV we can realize our style
                IsSited = true;


                //  previous padding size is so it can be restored when unsiting
                previousPaddingData = Style.Padding;

                UpdateStyle();
            }

            /// <summary>
            /// Update style
            /// </summary>
            protected internal virtual void UpdateStyle()
            {
                // styles shouldn't be modified when we are not sited.
                if (IsSited == false) return;

                int level = Level;

                Padding prevPadding = previousPaddingData;
                Size preferredSize;

                using (Graphics gridGraphics = OwningNode.GridData.CreateGraphics())
                {
                    preferredSize = GetPreferredSize(gridGraphics, InheritedStyle, RowIndex, new Size(0, 0));
                }

                Image image = OwningNode.Image;

                if (image != null)
                {
                    // calculate image size
                    imageWidthData = image.Width + 2;
                    imageHeightData = image.Height + 2;
                }
                else
                {
                    imageWidthData = glyphWidth;
                    imageHeightData = 0;
                }


                if (preferredSize.Height < imageHeightData)
                {
                    Style.Padding = new Padding(prevPadding.Left + (level * INDENTWIDTH) + imageWidthData + INDENTMARGIN,
                        prevPadding.Top + (imageHeightData / 2), prevPadding.Right, prevPadding.Bottom + (imageHeightData / 2));
                    imageHeightOffsetData = 2;
                }
                else
                {
                    Style.Padding = new Padding(prevPadding.Left + (level * INDENTWIDTH) + imageWidthData + INDENTMARGIN,
                        prevPadding.Top, prevPadding.Right, prevPadding.Bottom);
                }

                calculatedLeftPadding = ((level - 1) * glyphWidth) + imageWidthData + INDENTMARGIN;
            }

            /// <summary>
            /// Paint to draw object
            /// </summary>
            /// <param name="graphics"></param>
            /// <param name="clipBounds"></param>
            /// <param name="cellBounds"></param>
            /// <param name="rowIndex"></param>
            /// <param name="cellState"></param>
            /// <param name="value"></param>
            /// <param name="formattedValue"></param>
            /// <param name="errorText"></param>
            /// <param name="cellStyle"></param>
            /// <param name="advancedBorderStyle"></param>
            /// <param name="paintParts"></param>
            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
                DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
                DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
                DataGridViewPaintParts paintParts)
            {
                TreeGridNode node = OwningNode;
                if (node == null) return;

                Image image = node.Image;

                if (imageHeightData == 0 && image != null) UpdateStyle();

                // paint the cell normally
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText,
                    cellStyle, advancedBorderStyle, paintParts);


                Rectangle glyphRect = new Rectangle(cellBounds.X + GlyphMargin, cellBounds.Y, INDENTWIDTH, cellBounds.Height - 1);
                int glyphHalf = glyphRect.Width / 2;

                int level = Level;
                // for Images of the same size (ImageLayout.None)
                if (image != null)
                {
                    Point point;
                    if (imageHeightData > cellBounds.Height)
                        point = new Point(glyphRect.X + glyphWidth, cellBounds.Y + imageHeightOffsetData);
                    else
                        point = new Point(glyphRect.X + glyphWidth, (cellBounds.Height / 2) - (imageHeightData / 2) + cellBounds.Y);

                    // Graphics container to push/pop changes. This enables us to set clipping when painting
                    // the cell's image -- keeps it from bleeding outsize of cells.
                    GraphicsContainer graphicsContainer = graphics.BeginContainer();
                    {
                        graphics.SetClip(cellBounds);
                        graphics.DrawImageUnscaled(image, point);
                    }
                    graphics.EndContainer(graphicsContainer);
                }

                // Paint tree lines			
                if (node.GridData.ShowLines)
                {
                    using (Pen linePen = new Pen(SystemBrushes.ControlDark, 1.0f))
                    {
                        linePen.DashStyle = DashStyle.Dot;
                        bool isLastSibling = node.IsLastSibling;
                        bool isFirstSibling = node.IsFirstSibling;
                        if (node.Level == 1)
                        {
                            // the Root nodes display their lines differently
                            if (isFirstSibling && isLastSibling)
                            {
                                // only node, both first and last. Just draw horizontal line
                                graphics.DrawLine(linePen, (glyphRect.X + 4), (cellBounds.Top) + (cellBounds.Height)/2,
                                    glyphRect.Right, (cellBounds.Top) + (cellBounds.Height) / 2);
                            }
                            else if (isLastSibling)
                            {
                                // last sibling doesn't draw the line extended below. Paint horizontal then vertical
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2,
                                    glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4,
                                    cellBounds.Top + cellBounds.Height / 2);
                            }
                            else if (isFirstSibling)
                            {
                                // first sibling doesn't draw the line extended above. Paint horizontal then vertical
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2,
                                    glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2,
                                    glyphRect.X + 4, cellBounds.Bottom);
                            }
                            else
                            {
                                // normal drawing draws extended from top to bottom. Paint horizontal then vertical
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2,
                                    glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4,
                                    cellBounds.Bottom);
                            }
                        }
                        else
                        {
                            if (isLastSibling)
                            {
                                // last sibling doesn't draw the line extended below. Paint horizontal then vertical
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2,
                                    glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4,
                                    cellBounds.Top + cellBounds.Height / 2);
                            }
                            else
                            {
                                // normal drawing draws extended from top to bottom. Paint horizontal then vertical
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top + cellBounds.Height / 2,
                                    glyphRect.Right, cellBounds.Top + cellBounds.Height / 2);
                                graphics.DrawLine(linePen, glyphRect.X + 4, cellBounds.Top, glyphRect.X + 4,
                                    cellBounds.Bottom);
                            }

                            // paint lines of previous levels to the root
                            TreeGridNode previousNode = node.Parent;
                            int horizontalStop = (glyphRect.X + 4) - INDENTWIDTH;

                            while (!previousNode.IsRoot)
                            {
                                if (previousNode.HasChildren && !previousNode.IsLastSibling)
                                {
                                    // paint vertical line
                                    graphics.DrawLine(linePen, horizontalStop, cellBounds.Top, horizontalStop,
                                        cellBounds.Bottom);
                                }
                                previousNode = previousNode.Parent;
                                horizontalStop = horizontalStop - INDENTWIDTH;
                            }
                        }
                    }
                }

                if (node.HasChildren || node.GridData.VirtualNodes)
                {
                    // Paint node glyphs				
                    if (Application.RenderWithVisualStyles)
                    {
                        VisualStyleRenderer rOpen = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Opened);
                        VisualStyleRenderer rClosed = new VisualStyleRenderer(VisualStyleElement.TreeView.Glyph.Closed);
                      
                    }
                    else
                    {
                       
                        int x = glyphRect.X;
                        int y = glyphRect.Y + (glyphRect.Height / 2) - 4;
                       
                    }
                }
                node.GridData.ShowPlusMinus = false;
            }

            /// <summary>
            /// On Mouse up event
            /// </summary>
            /// <param name="e"></param>
            protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
            {
                base.OnMouseUp(e);

                TreeGridNode node = OwningNode;
                if (node != null)
                    node.GridData.InExpandCollapseMouseCaptureData = false;
            }

            /// <summary>
            /// on mouse down event
            /// </summary>
            /// <param name="e"></param>
            protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
            {
                if (e.Location.X > InheritedStyle.Padding.Left)
                {
                    base.OnMouseDown(e);
                }
                else
                {
                    // Expand the node

                    TreeGridNode node = OwningNode;
                    if (node != null)
                    {
                        node.GridData.InExpandCollapseMouseCaptureData = true;
                        if (node.IsExpanded)
                            node.Collapse();
                        else
                            node.Expand();
                    }
                }
            }
        }

    /// <summary>
    /// Tree Grid Column
    /// </summary>
        public class TreeGridColumn : DataGridViewTextBoxColumn
        {
            internal Image DefaultNodeImageData;
            /// <summary>
            /// Tree Grid Column
            /// </summary>
            public TreeGridColumn()
            {
                CellTemplate = new TreeGridCell();
            }
            /// <summary>
            /// Default Node Image
            /// </summary>
            public Image DefaultNodeImage
            {
                get { return DefaultNodeImageData; }
                set { DefaultNodeImageData = value; }
            }

            // Need to override Clone for design-time support.
            /// <summary>
            /// Clone
            /// </summary>
            /// <returns></returns>
            public override object Clone()
            {
                TreeGridColumn c = (TreeGridColumn)base.Clone();
                c.DefaultNodeImageData = DefaultNodeImageData;
                return c;
            }
        }
    
}