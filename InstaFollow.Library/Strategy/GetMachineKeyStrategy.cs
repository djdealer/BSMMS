using System.Management;
using InstaFollow.Core.Context;
using InstaFollow.Core.Extension;

namespace InstaFollow.Core.Strategy
{
	public class GetMachineKeyStrategy : BaseContextStrategy<IVerifyContext>
	{
		public GetMachineKeyStrategy(IVerifyContext context) : base(context)
		{
		}

		public void GetMachineKey()
		{
			var cpuInfo = string.Empty;
			var mc = new ManagementClass("win32_processor");
			foreach (var o in mc.GetInstances())
			{
				var mo = (ManagementObject) o;
				cpuInfo = mo.Properties["processorID"].Value.ToString();
				break;
			}

			var drive = "C";
			var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
			dsk.Get();
			var volumeSerial = dsk["VolumeSerialNumber"].ToString();

			this.CurrentContext.MachineKey = Hashing.CalculateMd5Hash(cpuInfo + volumeSerial);
		}
	}
}