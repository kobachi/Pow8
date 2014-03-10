using System;
using System.Collections.Generic;
using System.Management;

namespace Wmi
{
	/// <summary>
	/// Win32OperatingSyste WMI
	/// </summary>
    public class Win32OperatingSystem
    {
		/// <summary>
		/// 接続オプション
		/// </summary>
		protected ConnectionOptions ConnectionOptions;

		/// <summary>
		/// 管理スコープ
		/// </summary>
		protected ManagementScope ManagementScope;

		/// <summary>
		/// WMI呼び出しクエリ
		/// </summary>
		protected static readonly ObjectQuery Query = new ObjectQuery("select * from Win32_OperatingSystem");

		/// <summary>
		/// メソッド名
		/// </summary>
		protected static readonly string Win32Shutdown = "Win32Shutdown";

		/// <summary>
		/// ログオフ用フラグ
		/// </summary>
		protected static readonly int Win32ShutdownFlagLogOff = 0;

		/// <summary>
		/// シャットダウン（電源をOFFにしない）用フラグ
		/// </summary>
		[Obsolete]
		protected static readonly int Win32ShutdownFlagShutdown = 1;

		/// <summary>
		/// 再起動用フラグ
		/// </summary>
		protected static readonly int Win32ShutdownFlagReboot = 2;

		/// <summary>
		/// シャットダウン（電源もOFFにする）用フラグ
		/// </summary>
		protected static readonly int Win32ShutdownFlagPowerOff = 8;

		/// <summary>
		/// 強制用フラグ
		/// </summary>
		protected static readonly int Win32ShutdownFlagForce = 4;

		/// <summary>
		/// このコンピューターのWin32OperatingSystem WMIオブジェクトを作成します．
		/// </summary>
		public Win32OperatingSystem()
			: this(@"\ROOT\CIMV2") {
		}

		/// <summary>
		/// 指定されたパスのWin32OperatingSystem WMIオブジェクトを作成します．
		/// </summary>
		/// <param name="path"></param>
		public Win32OperatingSystem(string path) {
			ConnectionOptions = new ConnectionOptions();
			ConnectionOptions.Impersonation = ImpersonationLevel.Impersonate;
			ConnectionOptions.EnablePrivileges = true;
			ManagementScope = new ManagementScope(path, ConnectionOptions);
			ManagementScope.Connect();
		}

		/// <summary>
		/// 指定されたメソッドを実行します
		/// </summary>
		/// <param name="method"></param>
		/// <param name="flag"></param>
		/// <returns></returns>
		protected ManagementBaseObject Invoke(string method, int flag) {
			ManagementBaseObject result = null;
			using (var searcher = new ManagementObjectSearcher(ManagementScope, Query)) {
				foreach (ManagementObject mo in searcher.Get()) {
					var param = mo.GetMethodParameters(method);
					param["Flags"] = flag;
					param["Reserved"] = 0;
					result = mo.InvokeMethod(method, param, null);
					mo.Dispose();
				}
			}
			return result;
		}

		/// <summary>
		/// ログオフします。ログオフ時に問題が発生した場合は、ユーザーに問い合わせます。
		/// </summary>
		public void LogOff() {
			LogOff(false);
		}

		/// <summary>
		/// ログオフします。
		/// </summary>
		/// <param name="force">ログオフ時に問題が発生した場合でもユーザーに問い合わせしない場合はtrue</param>
		public void LogOff(bool force) {
			Invoke(Win32Shutdown, Win32ShutdownFlagLogOff + (force ? Win32ShutdownFlagForce : 0));
		}

		/// <summary>
		/// シャットダウンします。PCの電源は切りません。シャットダウン時に問題が発生した場合は、ユーザーに問い合わせます。
		/// </summary>
		[Obsolete]
		public void Shutdown() {
			Shutdown(false);
		}

		/// <summary>
		/// シャットダウンします。PCの電源は切りません。
		/// </summary>
		/// <param name="force">シャットダウン時に問題が発生した場合でもユーザーに問い合わせしない場合はtrue</param>
		[Obsolete]
		public void Shutdown(bool force) {
			Invoke(Win32Shutdown, Win32ShutdownFlagShutdown + (force ? Win32ShutdownFlagForce : 0));
		}

		/// <summary>
		/// 再起動します。再起動中に問題が発生した場合は、ユーザーに問い合わせます。
		/// </summary>
		public void Reboot() {
			Reboot(false);
		}

		/// <summary>
		/// 再起動します。
		/// </summary>
		/// <param name="force">再起動時に問題が発生した場合でもユーザーに問い合わせしない場合はtrue</param>
		public void Reboot(bool force) {
			Invoke(Win32Shutdown, Win32ShutdownFlagReboot + (force ? Win32ShutdownFlagForce : 0));
		}

		/// <summary>
		/// シャットダウンします。PCの電源も切ります。シャットダウン時に問題が発生した場合は、ユーザーに問い合わせます。
		/// </summary>
		public void PowerOff() {
			PowerOff(false);
		}

		/// <summary>
		/// シャットダウンします。PCの電源も切ります。
		/// </summary>
		/// <param name="force">シャットダウン時に問題が発生板場合でもユーザーに問い合わせしない場合はtrue</param>
		public void PowerOff(bool force) {
			Invoke(Win32Shutdown, Win32ShutdownFlagPowerOff + (force ? Win32ShutdownFlagForce : 0));
		}
    }
}
