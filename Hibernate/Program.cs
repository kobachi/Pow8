using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hibernate {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main(string[] args) {
			var options = args.Where(a => !string.IsNullOrEmpty(a) && 2 < a.Length && (a.StartsWith("-") || a.StartsWith("/")));
			bool force = options.Count(a => a.Substring(1).ToLower() == "force") > 0;
			bool noWakeup = options.Count(a => a.Substring(1).ToLower() == "nowakeup") > 0;
			Application.SetSuspendState(PowerState.Hibernate, force, noWakeup);
		}
	}
}
