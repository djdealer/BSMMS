namespace BSMMS.Core.Extension
{
	using System;
	using System.Linq;
	using System.Security;
	using System.Security.AccessControl;
	using System.Security.Principal;
	using Microsoft.Win32;

	public class RegistryUtils : IRegistryUtils
	{
		private static IRegistryUtils instance;

		private const string RegPath = @"SOFTWARE\BRANOVA\InstaFollow";
		private readonly RegistryKey view64;
		private readonly RegistrySecurity registrySecurity;

		public static IRegistryUtils Instance
		{
			get { return instance ?? (instance = new RegistryUtils()); }
		}

		private RegistryUtils()
		{
			var user = string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName);
			this.registrySecurity = new RegistrySecurity();
			var rule = new RegistryAccessRule(
				user,
				RegistryRights.FullControl,
				InheritanceFlags.None,
				PropagationFlags.None,
				AccessControlType.Allow);

			this.registrySecurity.AddAccessRule(rule);

			this.view64 = RegistryKey.OpenBaseKey(
				RegistryHive.CurrentUser,
				Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
		}

		public object ReadValue(string valueName, object defaultValue)
		{
			var regValue = defaultValue;
			RegistryKey regKey;
			try
			{
				regKey = this.view64.OpenSubKey(RegPath, RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.ReadKey);
			}
			catch (Exception ex)
			{
				throw;
			}

			if (regKey == null)
			{
				return regValue;
			}

			regValue = regKey.GetValue(valueName) ?? defaultValue;

			return regValue;
		}

		public void WriteValue(string valueName, object value)
		{
			RegistryKey regKey;
			try
			{
				regKey = this.view64.OpenSubKey(
					@"SOFTWARE",
					RegistryKeyPermissionCheck.ReadWriteSubTree,
					RegistryRights.FullControl);
				if (regKey == null)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				throw;
			}

			var keyString = string.Format(@"{0}\{1}", this.view64.Name, RegPath);

			var pathToken = keyString.Split('\\').ToList();

			if (pathToken.Count < 3)
			{
				return;
			}

			RegistryKey subKey;

			for (var index = 2; index < pathToken.Count; index++)
			{
				var token = pathToken[index];
				if (regKey == null)
				{
					break;
				}

				try
				{
					subKey = regKey.OpenSubKey(token, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
				}
				catch (ArgumentException ex)
				{
					throw;
				}
				catch (ObjectDisposedException ex)
				{
					throw;
				}
				catch (SecurityException ex)
				{
					throw;
				}

				try
				{
					if (subKey == null)
					{
						subKey = regKey.CreateSubKey(
							token,
							RegistryKeyPermissionCheck.ReadWriteSubTree,
							RegistryOptions.None,
							this.registrySecurity);
					}
				}
				catch (Exception ex)
				{
					throw;
				}

				regKey = subKey;
			}

			if (regKey != null)
			{
				regKey.SetValue(valueName, value);
			}
			else
			{
				throw new Exception(string.Format(@"Key '{0}\{1}' not found", this.view64, RegPath));
			}
		}

		internal void DeleteValue(string keyPath, string valueName)
		{
			using (var regKey = this.view64.OpenSubKey(keyPath, true))
			{
				if (regKey != null)
				{
					regKey.DeleteValue(valueName, false);
				}
				else
				{
					throw new Exception("Schlüssel " + keyPath + @"\" + valueName + " nicht gefunden");
				}
			}
		}

		internal void DeleteKey(string keyPath)
		{
			var i = keyPath.LastIndexOf(@"\", StringComparison.Ordinal);
			var parentKeyPath = keyPath.Substring(0, i);
			var keyName = keyPath.Substring(i + 1, keyPath.Length - i - 1);

			using (var regKey = this.view64.OpenSubKey(parentKeyPath, true))
			{

				if (regKey != null)
				{
					regKey.DeleteSubKey(keyName);
				}
				else
				{
					throw new Exception("Schlüssel " + parentKeyPath + @"\" + keyName + " nicht gefunden");
				}
			}
		}
	}

	public interface IRegistryUtils
	{
		void WriteValue(string valueName, object value);
		object ReadValue(string valueName, object defaultValue);
	}
}