using UnityEngine;

namespace PlayGen.Unity.Utilities.Platforms
{

	public class MobilePlatform : MonoBehaviour
	{

		public static bool IsMobile()
		{
			return Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WSAPlayerARM;
		}
	}
}