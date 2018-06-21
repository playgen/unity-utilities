using UnityEngine;

namespace PlayGen.Unity.Utilities.UI
{
	/// <summary>
	/// Sets the slider handle size to be consistant across multiple resolutions
	/// </summary>
	public class SliderHandleSizeSetter : MonoBehaviour
	{
		/// <summary>
		/// The scale of the handle compared to its parent
		/// </summary>
		[Tooltip("The scale of the handle compared to its parent")]
		[SerializeField]
		protected float _scale = 2.4f;

		protected virtual void Update()
		{
			if (transform.hasChanged)
			{
				((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform.parent).rect.height * _scale, ((RectTransform)transform).sizeDelta.y);
			}
		}
	}
}