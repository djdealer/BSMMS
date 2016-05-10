using System.Windows;
using System.Windows.Controls;

namespace BSMMS.Control
{
	public sealed class BindablePasswordBox : Decorator
	{
		public static readonly DependencyProperty PasswordProperty;

		private bool isPreventCallback;
		private readonly RoutedEventHandler savedCallback;

		static BindablePasswordBox()
		{
			PasswordProperty = DependencyProperty.Register(
				"Password",
				typeof(string),
				typeof(BindablePasswordBox),
				new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPasswordPropertyChanged)
			);
		}

		public BindablePasswordBox()
		{
			this.savedCallback = this.HandlePasswordChanged;

			var passwordBox = new PasswordBox();
			passwordBox.PasswordChanged += this.savedCallback;
			this.Child = passwordBox;
		}

		public string Password
		{
			get { return this.GetValue(PasswordProperty) as string; }
			set { this.SetValue(PasswordProperty, value); }
		}

		private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs eventArgs)
		{
			var bindablePasswordBox = (BindablePasswordBox)d;
			var passwordBox = (PasswordBox)bindablePasswordBox.Child;

			if (bindablePasswordBox.isPreventCallback)
			{
				return;
			}

			passwordBox.PasswordChanged -= bindablePasswordBox.savedCallback;
			passwordBox.Password = (eventArgs.NewValue != null) ? eventArgs.NewValue.ToString() : "";
			passwordBox.PasswordChanged += bindablePasswordBox.savedCallback;
		}

		private void HandlePasswordChanged(object sender, RoutedEventArgs eventArgs)
		{
			var passwordBox = (PasswordBox)sender;

			this.isPreventCallback = true;
			this.Password = passwordBox.Password;
			this.isPreventCallback = false;
		}
	}
}