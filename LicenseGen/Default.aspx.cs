using System;
using System.Web;
using System.Web.UI;
using log4net;

namespace InstaFollow.LicenseGen
{
	public partial class Default : Page
	{
		private readonly ILog log = LogManager.GetLogger(typeof(Default));

		/// <summary>
		/// Handles the Load event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, EventArgs e)
		{
		}

	    protected void OnClickBtnGetLicense(object sender, EventArgs e)
	    {
	        if (!this.SaveUserData())
	        {
	            return;
	        }

	        if (!this.PerformPaypalPayment())
	        {
	            return;
	        }

	        var licenseCode = this.GetLicenseKey();
	        if (licenseCode == string.Empty)
	        {
	            return;
	        }

            this.tbLicense.Text = licenseCode;
	    }

	    private bool PerformPaypalPayment()
	    {
	        return true;
	    }

	    private string GetLicenseKey()
	    {
	        var machineKey = this.Request.QueryString.Get("machinekey");

	        if (machineKey == null)
	        {
	            return string.Empty;
	        }

	        var licenseServer = new LicenceServer();
	        return licenseServer.GenerateLicenceKey(machineKey.GetHashCode());
	    }

	    private bool SaveUserData()
	    {
			this.log.Info("Name: " + this.tbFirstName.Text + " " + this.tbLastName.Text + " Email: " + this.tbEmail.Text);
            return true;
	    }

		protected void OnClickBtnDonate(object sender, ImageClickEventArgs e)
		{
			this.log.Info("Donation was clicked!");
		}
	}
}