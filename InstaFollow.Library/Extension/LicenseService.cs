using AppSoftware.LicenceEngine.Common;
using AppSoftware.LicenceEngine.KeyVerification;

namespace BSMMS.Core.Extension
{
	public class LicenseService: ILicenseService
	{
		private static ILicenseService instance;

		private LicenseService() { }

		public static ILicenseService Instance
		{
			get { return instance ?? (instance = new LicenseService()); }
		}

		public bool IsLicenseCodeValid(string licenseKey)
		{
			var pkvKeyCheck = new PkvKeyCheck();

			var keyBytes = new[] {

                    new KeyByteSet(5, 165, 15, 132), 
                    new KeyByteSet(6, 128, 175, 213)
                };

			var result = pkvKeyCheck.CheckKey(licenseKey, keyBytes, 8, null);

			return result == PkvLicenceKeyResult.KeyGood;
		}

		public bool IsValidRegistryLicenseCode()
		{
			var code = RegistryUtils.Instance.ReadValue("maincode", "xx").ToString();
			if (code == "xx")
			{
				return false;
			}

			return this.IsLicenseCodeValid(code);
		}

		public void WriteLicenseCodeToRegistry(string code)
		{
			RegistryUtils.Instance.WriteValue("maincode", code);
		}
	}
}