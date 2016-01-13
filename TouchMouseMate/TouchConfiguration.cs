using System.Configuration;
using TouchMouseMate.Configuration;

namespace TouchMouseMate
{
	public class TouchConfiguration
	{
		public TouchMouseMateSection Section { get; }

		public System.Configuration.Configuration Config { get; }

		public TouchConfiguration()
		{
			Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
			Section = (TouchMouseMateSection)Config.GetSection("touchMouseMate");
		}
	}
}