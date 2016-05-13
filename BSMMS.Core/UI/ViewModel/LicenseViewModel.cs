using System;
using System.Windows.Input;
using BSMMS.Core.Context;
using BSMMS.Core.Extension;
using BSMMS.Core.Strategy;
using BSMMS.Core.UI.Command;

namespace BSMMS.Core.UI.ViewModel
{
	public class LicenseViewModel : BaseViewModel, IVerifyContext
	{
		private string licenseKey;
		private bool licenseVerified = false;

		/// <summary>
		/// Prevents a default instance of the <see cref="InstagramViewModel"/> class from being created.
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

			new GetMachineKeyStrategy(this).GetMachineKey();
			this.RaisePropertyChanged("MachineKey");
		}

		public string MachineKey { get; set; }

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
}