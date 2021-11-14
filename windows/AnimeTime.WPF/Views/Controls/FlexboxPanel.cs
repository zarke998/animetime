using AnimeTime.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AnimeTime.WPF.Views.Controls
{
    public class FlexboxPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var totalSize = new Size(0, 0);
            foreach (var row in GetChildrenRows(availableSize.Width))
            {
                totalSize.Width = 0;
                foreach (var child in row)
                {
                    child.Measure(availableSize);
                    totalSize.Width += child.DesiredSize.Width;
                }
                var firstChild = row.FirstOrDefault();
                if (firstChild != null)
                    totalSize.Height += firstChild.DesiredSize.Height;
            }
            return totalSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            double x = 0.0;
            double y = 0.0;

            var rows = GetChildrenRows(finalSize.Width);
            foreach (var row in rows)
            {
                var extraWidth = finalSize.Width - rows.First().Sum(c => c.DesiredSize.Width);
                var spacing = extraWidth / (rows.First().Count() - 1);
                foreach (var child in row)
                {
                    child.Arrange(new Rect(new Point(x, y), child.DesiredSize));

                    x += child.DesiredSize.Width + spacing;
                }
                x = 0;
                y += row.MaxOrDefault(e => e.DesiredSize.Height);
            }

            return finalSize;
        }

        private double GetChildrenWidth() => InternalChildren.Cast<UIElement>().Sum(c => c.DesiredSize.Width);
        private IEnumerable<IEnumerable<UIElement>> GetChildrenRows(double availableWidth)
        {
            var rows = new List<IEnumerable<UIElement>>();
            var totalWidth = 0.0;

            var activeRow = new List<UIElement>();
            rows.Add(activeRow);
            foreach (var child in InternalChildren.Cast<UIElement>())
            {
                totalWidth += child.DesiredSize.Width;
                if (totalWidth > availableWidth)
                {
                    activeRow = new List<UIElement>();
                    activeRow.Add(child);

                    rows.Add(activeRow);
                    totalWidth = child.DesiredSize.Width;
                }
                else
                {
                    activeRow.Add(child);
                }
            }

            return rows;
        }
    }
}
