using System.Windows;
using System.Windows.Controls;

namespace BSMMS.Control
{
	public class GridLayout : Grid
	{
		public static readonly DependencyProperty ChildMarginProperty = DependencyProperty.Register(
			"ChildMargin",
			typeof(Thickness),
			typeof(GridLayout),
			new FrameworkPropertyMetadata(new Thickness(5))
			{
				AffectsArrange = true,
				AffectsMeasure = true
			});

		public Thickness ChildMargin
		{
			get { return (Thickness)this.GetValue(ChildMarginProperty); }
			set
			{
				this.SetValue(ChildMarginProperty, value);
				this.UpdateChildMargins();
			}
		}

		public void UpdateChildMargins()
		{
			var maxColumn = 0;
			var maxRow = 0;
			foreach (UIElement element in InternalChildren)
			{
				var row = GetRow(element);
				var rowSpan = GetRowSpan(element);
				var column = GetColumn(element);
				var columnSpan = GetColumnSpan(element);

				if (row + rowSpan > maxRow)
					maxRow = row + rowSpan;
				if (column + columnSpan > maxColumn)
					maxColumn = column + columnSpan;
			}

			foreach (UIElement element in InternalChildren)
			{
				var fe = element as FrameworkElement;
				if (null != fe)
				{
					var row = GetRow(fe);
					var rowSpan = GetRowSpan(fe);
					var column = GetColumn(fe);
					var columnSpan = GetColumnSpan(fe);
					var factorLeft = 0.5;
					var factorTop = 0.5;
					var factorRight = 0.5;
					var factorBottom = 0.5;

					if (row == 0)
						factorTop = 0;

					if (row + rowSpan >= maxRow)
						factorBottom = 0;

					if (column == 0)
						factorLeft = 0;

					if (column + columnSpan >= maxColumn)
						factorRight = 0;
					fe.Margin = new Thickness(this.ChildMargin.Left * factorLeft,
						this.ChildMargin.Top * factorTop,
						this.ChildMargin.Right * factorRight,
						this.ChildMargin.Bottom * factorBottom);
				}
			}
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			this.UpdateChildMargins();
			return base.MeasureOverride(availableSize);
		}
	}
}