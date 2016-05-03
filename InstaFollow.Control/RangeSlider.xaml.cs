using System;
using System.Windows;
using System.Windows.Controls;

namespace InstaFollow.Control
{
    /// <summary>
    /// Interaction logic for RangeSlider.xaml
    /// </summary>
    public partial class RangeSlider : UserControl
    {
        public RangeSlider()
        {
            InitializeComponent();
            
            this.Loaded += this.Slider_Loaded;
        }

	    private void Slider_Loaded(object sender, RoutedEventArgs e)
        {
			this.LowerSlider.ValueChanged += this.LowerSlider_ValueChanged;
			this.UpperSlider.ValueChanged += this.UpperSlider_ValueChanged;
        }

        private void LowerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
			this.UpperSlider.Value = Math.Max(this.UpperSlider.Value, this.LowerSlider.Value);
        }

        private void UpperSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
			this.LowerSlider.Value = Math.Min(this.UpperSlider.Value, this.LowerSlider.Value);
        }

        public double Minimum
        {
			get { return (double)this.GetValue(MinimumProperty); }
			set { this.SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));

        public double LowerValue
        {
			get { return (double)this.GetValue(LowerValueProperty); }
			set { this.SetValue(LowerValueProperty, value); }
        }

        public static readonly DependencyProperty LowerValueProperty =
            DependencyProperty.Register("LowerValue", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));

        public double UpperValue
        {
			get { return (double)this.GetValue(UpperValueProperty); }
			set { this.SetValue(UpperValueProperty, value); }
        }

        public static readonly DependencyProperty UpperValueProperty =
            DependencyProperty.Register("UpperValue", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(0d));

        public double Maximum
        {
			get { return (double)this.GetValue(MaximumProperty); }
			set { this.SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(RangeSlider), new UIPropertyMetadata(1d));
    }
}
