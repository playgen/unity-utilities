using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace PlayGen.Unity.Utilities.Editor.Shortcuts
{

	public class Shortcuts : MonoBehaviour
	{
		/// <summary>
		/// This class handles custom shortcuts to use in the unity editor
		/// 
		/// % – CTRL on Windows / CMD on OSX
		/// # – Shift
		/// & – Alt
		/// LEFT/RIGHT/UP/DOWN – Arrow keys
		/// F1…F2 – F keys
		/// HOME, END, PGUP, PGDN
		/// 
		/// https://unity3d.com/learn/tutorials/topics/interface-essentials/unity-editor-extensions-menu-items
		/// 
		/// </summary>

		/// <summary>
		/// Reset the offsets for the selected Rect Transform to 0
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Reset Offsets %&r")]
		static void ResetRectTransformOffsets()
		{
			var objects = Selection.gameObjects;
			foreach (var o in objects)
			{
				var rect = o.GetComponent<RectTransform>();

				ResetOffset(rect);
			}
		}

		/// <summary>
		/// Create a button that fills the panel it is created in
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Create Button %&q")]
		static void CreateButtonAsChild()
		{
			var go = new GameObject();
			go.transform.parent = Selection.activeGameObject.transform;

			go.AddComponent<Button>();
			var rt = go.AddComponent<RectTransform>();
			go.AddComponent<Image>();

			go.name = "New Button";

			ResetAnchors(rt);
			ResetOffset(rt);
			ResetScale(rt);
		}

		#region Reset Rects

		private static void ResetOffset(RectTransform rect)
		{
			rect.offsetMax = Vector2.zero;
			rect.offsetMin = Vector2.zero;
		}
		private static void ResetAnchors(RectTransform rect)
		{
			rect.anchorMin = Vector2.zero;
			rect.anchorMax = Vector2.one;
		}
		private static void ResetScale(RectTransform rect)
		{
			rect.transform.localScale = Vector3.one;
		}

		#endregion

		/// <summary>
		/// Move an object up in the hierarchy
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Move Selected Object Up %#UP")]
		static void MoveObjectUp()
		{
			var selected = Selection.gameObjects;
			foreach (GameObject go in selected)
			{
				MoveObjectInHierarchy(go, Direction.up);
			}
		}

		/// <summary>
		/// Move an object down in the hierarchy
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Move Selected Object Down %#DOWN")]
		static void MoveObjectDown()
		{
			var selected = Selection.gameObjects;
			for (var i = selected.Length - 1; i >= 0; i--)
			{
				MoveObjectInHierarchy(selected[i], Direction.down);
			}
		}

		/// <summary>
		/// Move an object Out of parent in the hierarchy
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Move Selected Object Sibling Of Parent %#LEFT")]
		static void MoveObjectOut()
		{
			var selected = Selection.gameObjects;
			foreach (var obj in selected)
			{
				MoveObjectInHierarchy(obj, Direction.siblingOfParent);
			}
		}

		/// <summary>
		/// Move an object into object below in the hierarchy
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Move Selected Object Child Of Sibling %#RIGHT")]
		static void MoveObjectIn()
		{
			var selected = Selection.gameObjects;
			foreach (var obj in selected)
			{
				MoveObjectInHierarchy(obj, Direction.childOfSibling);
			}
		}

		#region MoveObjects

		enum Direction { up, down, siblingOfParent, childOfSibling }

		private static void MoveObjectInHierarchy(GameObject go, Direction direction)
		{
			var index = go.transform.GetSiblingIndex();
			var parent = go.transform.parent;

			switch (direction)
			{
				case Direction.up:

					MoveToNewChildIndex(go, index - 1);
					break;
				case Direction.down:

					MoveToNewChildIndex(go, index + 1);
					break;
				case Direction.siblingOfParent:

					if (parent)
					{
						var newIndex = parent.transform.GetSiblingIndex();

						parent = parent.parent;

						go.transform.SetParent(parent);
						go.transform.SetSiblingIndex(newIndex);
					}
					break;
				case Direction.childOfSibling:
					if (parent)
					{
						if (index + 1 >= parent.childCount)
						{
							break;
						}
						parent = parent.GetChild(index + 1);

						go.transform.SetParent(parent);
						go.transform.SetSiblingIndex(0);
					}
					else
					{
						// Root level
						var root = SceneManager.GetActiveScene().GetRootGameObjects();
						root = root.OrderBy(t => t.transform.GetSiblingIndex()).ToArray();

						for (var i = 0; i < root.Length; i++)
						{
							if (i == index + 1)
							{
								go.transform.SetParent(root[i].transform);
								go.transform.SetSiblingIndex(0);
							}
						}
					}
					break;
			}
		}

		private static void MoveToNewChildIndex(GameObject go, int newIndex)
		{
			if (newIndex >= 0)
			{
				go.transform.SetSiblingIndex(newIndex);
			}
		}
		#endregion
	}
}