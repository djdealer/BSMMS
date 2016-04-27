using System.Windows;
using System.Windows.Controls;

namespace InstaFollow.Control
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
			savedCallback = HandlePasswordChanged;

			var passwordBox = new PasswordBox();
			passwordBox.PasswordChanged += savedCallback;
			Child = passwordBox;
		}

		public string Password
		{
			get { return GetValue(PasswordProperty) as string; }
			set { SetValue(PasswordProperty, value); }
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

			isPreventCallback = true;
			Password = passwordBox.Password;
			isPreventCallback = false;
		}
	}
}