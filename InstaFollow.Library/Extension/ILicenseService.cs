namespace InstaFollow.Core.Extension
{
	public interface ILicenseService
	{
		bool IsLicenseCodeValid(string licenseKey);
	}
}