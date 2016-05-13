namespace BSMMS.Core.Extension
{
	public interface ILicenseService
	{
		bool IsLicenseCodeValid(string licenseKey);
		bool IsValidRegistryLicenseCode();
		void WriteLicenseCodeToRegistry(string code);
	}
}