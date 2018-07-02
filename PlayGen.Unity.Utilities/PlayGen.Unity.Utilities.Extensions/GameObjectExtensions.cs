using UnityEngine;

namespace PlayGen.Unity.Utilities.Extensions
{
	public static class GameObjectExtensions
	{
		public static GameObject Parent(this GameObject go)
		{
			return go.transform.parent?.gameObject;
		}

		public static GameObject Parent(this MonoBehaviour mono)
		{
			return mono.transform.parent?.gameObject;
		}
	}
}
