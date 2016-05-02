using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using InstaFollow.Core.Context;
using InstaFollow.Core.Extension;
using InstaFollow.Core.UI.Command;
using MvvmFoundation.Wpf;

namespace InstaFollow.Core.UI.ViewModel
{
	public class LicenseViewModel : BaseViewModel, IVerifyContext
	{
		private string licenseKey;
		private bool licenseVerified = false;

		/// <summary>
		/// Prevents a default instance of the <see cref="MainViewModel"/> class from being created.
		/// </summary>
		private LicenseViewModel() { }

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public override void Init()
		{
			ThreadDispatcher.Initialize();
			this.VerifyCommand = this.CoreFactory.CreateContextCommand<VerifyLicenseCommand, IVerifyContext>(this);
			this.GetKeyCommand = this.CoreFactory.CreateContextCommand<GetKeyFromWebPageCommand, IVerifyContext>(this);
			this.AbortCommand = this.CoreFactory.CreateContextCommand<LicenseAbortCommand, IVerifyContext>(this);
		}

		public string MachineKey
		{
			get { return "pdfsuighdfsp3204hgw34poerh"; }// TODO
		}

		public string LicenseKey
		{
			get { return licenseKey; }
			set
			{
				licenseKey = value;
				this.RaisePropertyChanged("LicenseKey");
			}
		}

		public bool LicenseVerified
		{
			get { return licenseVerified; }
			set
			{
				licenseVerified = value;
				this.RaisePropertyChanged("LicenseVerified");
			}
		}

		public ICommand VerifyCommand { get; set; }
		public ICommand GetKeyCommand { get; set; }
		public ICommand AbortCommand { get; set; }
	}

	public class LicenseAbortCommand : BaseContextCommand<IVerifyContext>
	{
		public override void Execute(object parameter)
		{
			this.CurrentContext.LicenseVerified = false;
			this.CurrentContext.CloseAction();
		}

		protected internal override bool EvaluateCanExecute()
		{
			return true;
		}
	}

	public class GetKeyFromWebPageCommand : BaseContextCommand<IVerifyContext>
	{
		private string licenseGetFromWebLink = @"http://activation.branova.de?machinekey={0}";

		public override void Execute(object obj)
		{
			var link = string.Format(this.licenseGetFromWebLink, this.CurrentContext.MachineKey);
			Process.Start(new ProcessStartInfo(link));
		}

		protected internal override bool EvaluateCanExecute()
		{
			return !string.IsNullOrEmpty(this.CurrentContext.MachineKey);
		}
	}

	public class VerifyLicenseCommand : BaseContextCommand<IVerifyContext>
	{
		private VerifyLicenseCommand() { }
		public override void Execute(object obj)
		{
			// TODO verify license

			this.CurrentContext.LicenseVerified = true;
			this.CurrentContext.CloseAction();
		}

		protected internal override bool EvaluateCanExecute()
		{
			return !string.IsNullOrEmpty(this.CurrentContext.LicenseKey) && !string.IsNullOrEmpty(this.CurrentContext.MachineKey);
		}
	}
}