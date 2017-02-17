using UnityEngine;

public class MobilePlatform : MonoBehaviour {

	public static bool IsMobile()
	{
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP_8_1 || UNITY_WSA_8_1 || UNITY_WSA_10_0
		return true;
#else
		return false;
#endif
	}
}
