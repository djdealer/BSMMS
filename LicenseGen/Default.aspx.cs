using System;
using System.Web.UI;
using LicenseGen;

namespace InstaFollow.LicenseGen
{
	public partial class Default : Page
	{
		/// <summary>
		/// Handles the Load event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Page_Load(object sender, EventArgs e)
		{
			var machineKey = Request.QueryString["machinekey"];

			if (machineKey == null)
			{
				return;
			}
			
			var licenseServer = new LicenceServer();
			
			// TODO: this will happen after buying a license with paypal!
			var licenseCode = licenseServer.GenerateLicenceKey(machineKey.GetHashCode());

			this.tbLicense.Text = licenseCode;
		}
	}
}