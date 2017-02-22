using UnityEngine;

namespace PlayGen.Unity.Utilities.Platforms
{

	public class MobilePlatform : MonoBehaviour
	{

		public static bool IsMobile()
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WSAPlayerARM)
			{
				return true;
			}
			return false;
		}
	}
}