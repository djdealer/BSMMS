using System;

namespace InstaFollow.Core.UI.ViewModel
{
	public class AboutViewModel : BaseViewModel
	{
		public string AboutText
		{
			get
			{
				return @"Application developed by" +
				       Environment.NewLine + 
					   "BRANOVA Multimedia- & IT-Consulting" +
					   Environment.NewLine +
					   "Sebastian Lierka"+
					   Environment.NewLine +
					   "For further information," +
					   Environment.NewLine +
					   "please contact: slierka@branova.de";
			}
		}
	}
}