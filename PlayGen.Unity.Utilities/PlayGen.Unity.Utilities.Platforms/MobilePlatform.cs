using UnityEngine;

namespace PlayGen.Unity.Utilities.Platforms
{
	public class MobilePlatform
	{
		/// <summary>
		/// Is the current platform a 'mobile' (iOS, Android, Windows Store ARM) platform?
		/// </summary>
		public static bool IsMobile()
		{
			return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WSAPlayerARM;
		}
	}
}