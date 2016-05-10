using System;

namespace BSMMS.Core.UI.ViewModel
{
	public class InfoViewModel: BaseViewModel
	{
		public string ImageSource
		{
			get { return @"..\Images\branova_logo.png"; }
		}
		public string InfoText
		{
			get
			{
				return @"This application was developed by" +
						Environment.NewLine +
						"BRANOVA Multimedia- & IT-Consulting" +
						Environment.NewLine +
						"Sebastian Lierka" +
						Environment.NewLine +
						Environment.NewLine +
						"For further information please contact" +
						Environment.NewLine +
						"slierka@branova.de"; 
			}
		}
	}
}