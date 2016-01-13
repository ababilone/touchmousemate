using System;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using TouchMouseMate.Menu;

namespace TouchMouseMate
{
	public sealed class SingleInstanceManager : WindowsFormsApplicationBase
	{
		[STAThread]
		public static void Main(string[] args)
		{
			new SingleInstanceManager().Run(args);
		}

		public SingleInstanceManager()
		{
			IsSingleInstance = true;

			_touchConfiguration = new TouchConfiguration();
			var stateMachine = new StateMachine(_touchConfiguration);
			_touchMouseEventManager = new TouchMouseEventManager(stateMachine, _touchConfiguration);
		}

		private NotifyIcon _icon;
		private readonly TouchMouseEventManager _touchMouseEventManager;
		private readonly TouchConfiguration _touchConfiguration;

		protected override bool OnStartup(StartupEventArgs e)
		{
			//XmlConfigurator.Configure();
			_touchMouseEventManager.Init();

			_icon = new NotifyIcon
			{
				Icon = Properties.Resources.mouse,
				Visible = true
			};

			var contextMenuStrip = new ContextMenuStrip();
			contextMenuStrip.Items.Add(new MiddleClickStripMenuItem(_touchConfiguration));
			contextMenuStrip.Items.Add(new TouchOverStripMenuItem(_touchConfiguration));
			contextMenuStrip.Items.Add(new LeftHandedStripMenuItem(_touchConfiguration));
			contextMenuStrip.Items.Add(new HelpToolStripMenuItem());
			contextMenuStrip.Items.Add(new ToolStripSeparator());
			contextMenuStrip.Items.Add(new MouseMapStripMenuItem(_touchMouseEventManager));
			contextMenuStrip.Items.Add(new ToolStripSeparator());
			contextMenuStrip.Items.Add(new ExitStripMenuItem(_icon));

			//Published on November 23rd 2008 by Aston.
			//Released under the Free Art (copyleft) license.
			// Icon for Windows XP, Vista and 7.

			_icon.ContextMenuStrip = contextMenuStrip;
			_icon.ShowBalloonTip(5000, "Info", "Touch Mouse Mate has started", ToolTipIcon.Info);
			_icon.Text = string.Format("Touch Mouse Mate {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
			Application.Run();
			return false;
		}

		protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
		{
			base.OnStartupNextInstance(eventArgs);
			_icon.ShowBalloonTip(5000, "Warning", "Touch Mouse Mate is already running", ToolTipIcon.Warning);
		}

		protected override void OnShutdown()
		{
			_touchMouseEventManager?.Dispose();
		}
	}
}
