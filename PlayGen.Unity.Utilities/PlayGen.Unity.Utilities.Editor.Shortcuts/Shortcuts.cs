using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace PlayGen.Unity.Utilities.Editor.Shortcuts
{
	public static class Shortcuts
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
		private static void ResetRectTransformOffsets()
		{
			var objects = Selection.transforms.Select(o => o.GetComponent<RectTransform>()).ToList();
			if (objects.Any())
			{
				Undo.RecordObjects(objects.ToArray(), "Reset Offset");
			}
			foreach (var o in objects)
			{
				ResetOffset(o);
			}
		}

		// Validate the menu item defined by the function above.
		// The menu item will be disabled if this function returns false.
		[MenuItem("PlayGen Tools/UI/Reset Offsets %&r", true)]
		static bool ValidateResetRectTransformOffsets()
		{
			var objects = Selection.transforms;
			return objects.Any(o => o.GetComponent<RectTransform>());
		}

		/// <summary>
		/// Create a button that fills the panel it is created in
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Create Button %&q")]
		private static void CreateButtonAsChild()
		{
			var objects = Selection.gameObjects;
			foreach (var o in objects)
			{
				var go = new GameObject();
				go.transform.parent = o.transform;

				go.AddComponent<Button>();
				var rt = go.AddComponent<RectTransform>();
				go.AddComponent<Image>();

				go.name = "New Button";

				ResetAnchors(rt);
				ResetOffset(rt);
				ResetScale(rt);
				Undo.RegisterCreatedObjectUndo(go, "Create Button");
			}
		}

		[MenuItem("PlayGen Tools/UI/Create Button %&q", true)]
		static bool ValidateCreateButtonAsChild()
		{
			var objects = Selection.gameObjects;
			return objects.Any(o => o.GetComponent<RectTransform>());
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
		private static void MoveObjectUp()
		{
			var selected = Selection.gameObjects;
			foreach (var go in selected)
			{
				MoveObjectInHierarchy(go, Direction.up);
			}
		}

		[MenuItem("PlayGen Tools/UI/Move Selected Object Up %#UP", true)]
		static bool ValidateMoveObjectUp()
		{
			var objects = Selection.gameObjects;
			return objects.Any();
		}

		/// <summary>
		/// Move an object down in the hierarchy
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Move Selected Object Down %#DOWN")]
		private static void MoveObjectDown()
		{
			var selected = Selection.gameObjects;
			for (var i = selected.Length - 1; i >= 0; i--)
			{
				MoveObjectInHierarchy(selected[i], Direction.down);
			}
		}

		[MenuItem("PlayGen Tools/UI/Move Selected Object Down %#DOWN", true)]
		static bool ValidateMoveObjectDown()
		{
			var objects = Selection.gameObjects;
			return objects.Any();
		}

		/// <summary>
		/// Move an object Out of parent in the hierarchy
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Move Selected Object Sibling Of Parent %#LEFT")]
		private static void MoveObjectOut()
		{
			var selected = Selection.gameObjects;
			foreach (var obj in selected)
			{
				MoveObjectInHierarchy(obj, Direction.siblingOfParent);
			}
		}

		[MenuItem("PlayGen Tools/UI/Move Selected Object Sibling Of Parent %#LEFT", true)]
		static bool ValidateMoveObjectOut()
		{
			var objects = Selection.gameObjects;
			return objects.Any();
		}

		/// <summary>
		/// Move an object into object below in the hierarchy
		/// </summary>
		[MenuItem("PlayGen Tools/UI/Move Selected Object Child Of Sibling %#RIGHT")]
		private static void MoveObjectIn()
		{
			var selected = Selection.gameObjects;
			foreach (var obj in selected)
			{
				MoveObjectInHierarchy(obj, Direction.childOfSibling);
			}
		}

		[MenuItem("PlayGen Tools/UI/Move Selected Object Child Of Sibling %#RIGHT", true)]
		static bool ValidateMoveObjectIn()
		{
			var objects = Selection.gameObjects;
			return objects.Any();
		}

		#region MoveObjects

		private enum Direction { up, down, siblingOfParent, childOfSibling }

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

						Undo.SetTransformParent(go.transform, parent, "Move Selected Object Sibling Of Parent");
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

						Undo.SetTransformParent(go.transform, parent, "Move Selected Object Child Of Sibling");
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
								Undo.SetTransformParent(go.transform, root[i].transform, "Move Selected Object Child Of Sibling");
								go.transform.SetSiblingIndex(0);
							}
						}
					}
					break;
			}
		}

		private static void MoveToNewChildIndex(GameObject go, int newIndex)
		{
			if (newIndex >= 0 && (go.transform.parent == null || newIndex < go.transform.parent.childCount))
			{
				if (go.transform.parent != null)
				{
					Undo.RegisterFullObjectHierarchyUndo(go.transform.parent, "Move Selected Object");
				}
				go.transform.SetSiblingIndex(newIndex);
			}
		}
		#endregion
	}
}