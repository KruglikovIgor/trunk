using System;
using System.Linq;
using KPK2.Modes;
using Sparc.KPK2.NFunctions;

namespace KPK2
{
	public class Mode
	{
		private readonly string[] _argsContains = new []{ "/mode:", "emulation" };

		private static Lazy<Mode> mode;

		public static void InitializeMode(string[] args)
		{
			if (mode == null)
			{
				mode = new Lazy<Mode>(() => new Mode(args));
			}
		}

		public static Mode GetMode()
		{
			if (mode == null)
			{
                InitializeMode(Environment.GetCommandLineArgs());
			}

			return mode.Value;
		}

		public ModeApplication ModeApplication { get; private set; }
		
		private Mode(string[] args)
		{
			SetModeArg(args);
			SetEmulationArg(args);
		}

		public ModeApplicationState GetModeApplication()
		{
			if (ModeApplication == ModeApplication.RepairCheck)
			{
				return new ModeWorkCheck();
			}

			return new ModeDefaultCheck();
		}
		
		private bool IsNotArgMode(string[] args)
		{
			return !args.Any(arg => arg.Contains(_argsContains[0]));
		}

		private void SetModeArg(string[] args)
		{
			if (IsNotArgMode(args))
			{
				ModeApplication = ModeApplication.DefaultCheck;
				return;
			}
			
			string mode = args.First(f => f.Contains("/mode:"));
			if (mode == "/mode:WorkCheck")
			{
				ModeApplication = ModeApplication.RepairCheck;
			}
			else if (mode == "/mode:DefaultCheck")
			{
				ModeApplication = ModeApplication.DefaultCheck;
			}
		}

		private void SetEmulationArg(string[] args)
		{
			if (!args.Contains(_argsContains[1]))
				return;

			Globals.Emulation = true;
		}
	}
}
