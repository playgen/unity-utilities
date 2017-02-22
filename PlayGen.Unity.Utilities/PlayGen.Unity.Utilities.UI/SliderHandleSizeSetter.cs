using UnityEngine;

namespace PlayGen.Unity.Utilities.UI
{

	public class SliderHandleSizeSetter : MonoBehaviour
	{
		void Update()
		{
			if (transform.hasChanged)
			{
				((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform.parent).rect.height * 0.6f, ((RectTransform)transform).sizeDelta.y);
			}
		}
	}
}