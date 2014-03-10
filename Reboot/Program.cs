using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reboot {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			var options = args.Where(a => !string.IsNullOrEmpty(a) && 2 < a.Length && (a.StartsWith("-") || a.StartsWith("/")));
			bool force = options.Count(a => a.Substring(1).ToLower() == "force") > 0;
			new Wmi.Win32OperatingSystem().Reboot(force);
		}
	}
}
