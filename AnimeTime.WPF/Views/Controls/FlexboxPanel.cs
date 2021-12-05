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
        #region Dependency Properties



        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Spacing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register("Spacing", typeof(double), typeof(FlexboxPanel), new PropertyMetadata(0.0));



        public JustifyOptions Justify
        {
            get { return (JustifyOptions)GetValue(JustifyProperty); }
            set { SetValue(JustifyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Justify.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JustifyProperty =
            DependencyProperty.Register("Justify", typeof(JustifyOptions), typeof(FlexboxPanel), new PropertyMetadata(JustifyOptions.SpaceBetween));


        #endregion

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
            switch (Justify)
            {
                case JustifyOptions.SpaceBetween:
                    return ArrangeSpaceBetween(finalSize);
                case JustifyOptions.SpaceEven:
                    return ArrangeSpaceEven(finalSize);
            }
            return new Size();
        }

        private Size ArrangeSpaceEven(Size finalSize)
        {
            double x = 0.0;
            double y = 0.0;

            var itemsPerRow = GetMaxItemsPerRow(finalSize.Width);
            var fullItemWidth = InternalChildren[0].DesiredSize.Width + Spacing;            
            var childInRow = 0;
            var extraSpacing = (finalSize.Width - ((itemsPerRow - 1) * fullItemWidth + InternalChildren[0].DesiredSize.Width)) / (itemsPerRow - 1);
            foreach (var child in InternalChildren.Cast<UIElement>())
            {
                childInRow++;
                if(childInRow > itemsPerRow)
                {
                    childInRow = 1;
                    y += child.DesiredSize.Height;
                    x = 0;
                }

                child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
                x += child.DesiredSize.Width + Spacing + extraSpacing;
            }

            return finalSize;
        }
        private Size ArrangeSpaceBetween(Size finalSize)
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
        private int GetMaxItemsPerRow(double availableWidth)
        {
            if (InternalChildren.Count == 0) return 0;

            return Convert.ToInt32((availableWidth + Spacing) / (InternalChildren[0].DesiredSize.Width + Spacing));
        }
    }

    public enum JustifyOptions
    {
        SpaceBetween,
        SpaceEven
    }
}
