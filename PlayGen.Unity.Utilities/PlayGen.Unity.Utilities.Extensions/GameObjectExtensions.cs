using UnityEngine;

namespace PlayGen.Unity.Utilities.Extensions
{
	public static class GameObjectExtensions
	{
		public static GameObject Parent(this GameObject go)
		{
			return go.transform.parent == null ? null : go.transform.parent.gameObject;
		}
	}
}
