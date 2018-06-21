using System;
using System.Collections;
using PlayGen.Unity.Utilities.Text;
using PlayGen.Unity.Utilities.Localization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.Settings
{
	public class SettingCreation : MonoBehaviour
	{
		[Tooltip("Horizontal Layout Prefab")]
		[SerializeField]
		protected HorizontalLayoutGroup _horizontalLayout;
		[Tooltip("Vertical Layout Prefab")]
		[SerializeField]
		protected VerticalLayoutGroup _verticalLayout;
		[Tooltip("Label Layout Prefab, used for UI labels and headers")]
		[SerializeField]
		protected UnityEngine.UI.Text _label;
		[Tooltip("Dropdown Prefab")]
		[SerializeField]
		protected Dropdown _dropdown;
		[Tooltip("Toggle Prefab")]
		[SerializeField]
		protected Toggle _toggle;
		[Tooltip("Slider Prefab")]
		[SerializeField]
		protected Slider _slider;
		[Tooltip("Button Prefab")]
		[SerializeField]
		protected Button _button;
		[Tooltip("Label Alignment")]
		[SerializeField]
		protected TextAnchor _labelAnchor = TextAnchor.MiddleRight;

		/// <summary>
		/// Create event listeners for language and resolution changes
		/// </summary>
		protected virtual void OnEnable()
		{
			Localization.Localization.LanguageChange += RebuildLayout;
			BestFit.ResolutionChange += RebuildLayout;
		}

		/// <summary>
		/// Remove event listeners
		/// </summary>
		protected virtual void OnDisable()
		{
			Localization.Localization.LanguageChange -= RebuildLayout;
			BestFit.ResolutionChange -= RebuildLayout;
		}

		/// <summary>
		/// Creates a dropdown for selecting screen resolution.
		/// Only resolutions with width and height above minimum and the current resolution are displayed.
		/// By default, not displayed on mobile platforms.
		/// Returns the created dropdown.
		/// </summary>
		public virtual bool TryResolutionForPlatform(out Dropdown settingForPlatform, int minWidth = 0, int minHeight = 0, Resolution[] newResolutions = null, bool localizeDropdown = false, bool layoutHorizontal = true, string title = "Resolution", bool localizeLabel = true, bool showOnMobile = false)
		{
			if (TryForPlatform(title, showOnMobile, out settingForPlatform, localizeLabel, layoutHorizontal, localizeDropdown))
			{
				settingForPlatform.ClearOptions();
				var resolutions = Screen.resolutions.Distinct().ToList();
				if (newResolutions != null)
				{
					resolutions = resolutions.Concat(newResolutions).Distinct().ToList();
				}

				resolutions = resolutions.Where(res => res.width >= minWidth && res.height >= minHeight).ToList();
				var current = new Resolution { height = Screen.height, width = Screen.width };
				if (!resolutions.Any(res => res.width == current.width && res.height == current.height))
				{
					resolutions.Add(current);
				}

				var resolutionStrings = resolutions.OrderByDescending(res => res.width).ThenByDescending(res => res.height).Select(res => res.width + " x " + res.height).Distinct().ToList();
				if (localizeDropdown)
				{
					settingForPlatform.GetComponent<DropdownLocalization>().SetOptions(resolutionStrings);
				}
				else
				{
					settingForPlatform.AddOptions(resolutionStrings);
				}

				settingForPlatform.value = resolutionStrings.IndexOf(Screen.width + " x " + Screen.height);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a dropdown for selecting the application's quality settings.
		/// Options used are those set in the 'Quality' project setting.
		/// By default, not displayed on mobile platforms.
		/// Returns the created dropdown.
		/// </summary>
		public virtual bool TryQualityForPlatform(out Dropdown settingForPlatform, bool localizeDropdown = true, bool layoutHorizontal = true, string title = "Quality", bool localizeLabel = true, bool showOnMobile = false)
		{
			if (TryForPlatform(title, showOnMobile, out settingForPlatform, localizeLabel, layoutHorizontal, localizeDropdown))
			{
				settingForPlatform.ClearOptions();
				var qualities = QualitySettings.names.ToList();
				if (localizeDropdown)
				{
					settingForPlatform.GetComponent<DropdownLocalization>().SetOptions(qualities);
				}
				else
				{
					settingForPlatform.AddOptions(qualities);
				}

				settingForPlatform.value = QualitySettings.GetQualityLevel();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a dropdown for selecting which langauge the Localization asset should use.
		/// Only languages with text provided in localization json are displayed.
		/// By default, displayed on mobile platforms.
		/// Returns the created dropdown.
		/// </summary>
		public virtual bool TryLanguageForPlatform(out Dropdown settingForPlatform, bool localizeDropdown = true, bool layoutHorizontal = true, string title = "Language", bool localizeLabel = true, bool showOnMobile = true)
		{
			if (TryForPlatform(title, showOnMobile, out settingForPlatform, localizeLabel, layoutHorizontal, localizeDropdown))
			{
				settingForPlatform.ClearOptions();
				var languages = Localization.Localization.Languages.Select(l => l.DisplayName).ToList();
				if (localizeDropdown)
				{
					settingForPlatform.GetComponent<DropdownLocalization>().SetOptions(languages);
				}
				else
				{
					settingForPlatform.AddOptions(languages);
				}

				var selectedIndex = Localization.Localization.Languages.IndexOf(Localization.Localization.SelectedLanguage);
				if (selectedIndex == -1)
				{
					var nullList = new List<string> { string.Empty };
					settingForPlatform.AddOptions(nullList);
					settingForPlatform.value = languages.Count;
					settingForPlatform.options.RemoveAt(languages.Count);
				}
				else
				{
					settingForPlatform.value = selectedIndex;
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a toggle for setting if the application should be displayed in full screen.
		/// By default, not displayed on mobile platforms.
		/// Returns the created toggle.
		/// </summary>
		public virtual bool TryFullScreenForPlatform(out Toggle settingForPlatform, bool layoutHorizontal = true, string title = "FullScreen", bool localizeLabel = true, bool showOnMobile = false)
		{
			if (TryForPlatform(title, showOnMobile, out settingForPlatform, localizeLabel, layoutHorizontal))
			{
				settingForPlatform.isOn = Screen.fullScreen;
				return true;
			}

			return false;
		}

		/// <summary>
		/// Creates a slider intended for use as a volume slider.
		/// By default, displayed on mobile platforms.
		/// Returns the created slider.
		/// </summary>
		public virtual bool TryVolumeForPlatform(string title, out Slider settingForPlatform, float defaultVolume = 1f, bool localizeLabel = true, bool layoutHorizontal = true, bool showOnMobile = true)
		{
			if (TryForPlatform(title, showOnMobile, out settingForPlatform, localizeLabel, layoutHorizontal))
			{
				if (!PlayerPrefs.HasKey(title))
				{
					PlayerPrefs.SetFloat(title, defaultVolume);
				}

				settingForPlatform.value = PlayerPrefs.GetFloat(title, defaultVolume);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Create a UI object of type T
		/// Returns created object of type T
		/// </summary>
		public virtual bool TryForPlatform<T>(string label, bool showOnMobile, out T settingForPlatform, bool localizeLabel = true, bool layoutHorizontal = true, bool localizeUI = true) where T : UIBehaviour
		{
			if (TryForPlatform(label, typeof(T), showOnMobile, layoutHorizontal, localizeLabel, localizeUI, out var settingContainerForPlatform))
			{
				settingForPlatform = settingContainerForPlatform.GetComponent<T>() ?? settingContainerForPlatform.GetComponentInChildren<T>();
			}
			else
			{
				settingForPlatform = null;
			}

			return settingForPlatform != null;
		}

		/// <summary>
		/// Create an object that automatically lays out child objects horizontally
		/// Returns the created gameobject
		/// </summary>
		public virtual GameObject HorizontalLayout(string title, float aspectRatio = 0)
		{
			var layout = Instantiate(_horizontalLayout, transform, false);
			layout.name = title;
			if (aspectRatio > 0)
			{
				layout.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
			}
			return layout.gameObject;
		}

		/// <summary>
		/// Create an object that automatically lays out child objects vertically
		/// Returns the created gameobject
		/// </summary>
		public virtual GameObject VerticalLayout(string title, float aspectRatio = 0)
		{
			var layout = Instantiate(_verticalLayout, transform, false);
			layout.name = title;
			if (aspectRatio > 0)
			{
				layout.GetComponent<AspectRatioFitter>().aspectRatio = aspectRatio;
			}
			return layout.gameObject;
		}

		/// <summary>
		/// Adds a Selectable object to a GameObject with a LayoutGroup
		/// </summary>
		public virtual void AddToLayout(GameObject layoutObject, Selectable addition)
		{
			var layout = layoutObject.GetComponent<LayoutGroup>();
			if (layout)
			{
				AddToLayout(layout, addition.transform);
			}
		}

		/// <summary>
		/// Adds a Graphic object to a GameObject with a LayoutGroup
		/// </summary>
		public virtual void AddToLayout(GameObject layoutObject, Graphic addition)
		{
			var layout = layoutObject.GetComponent<LayoutGroup>();
			if (layout)
			{
				AddToLayout(layout, addition.transform);
			}
		}

		protected virtual void AddToLayout(LayoutGroup layout, Transform addition)
		{
			var addObj = addition;
			while (addObj.transform.parent != null && addObj.transform.parent != transform)
			{
				addObj = addObj.parent;
			}
			addObj.SetParent(layout.transform, false);
		}

		/// <summary>
		/// Set the alignment of text labels
		/// </summary>
		public virtual void SetLabelAlignment(TextAnchor align)
		{
			_labelAnchor = align;
		}

		protected virtual bool TryForPlatform(string title, Type objectType, bool showOnMobile, bool layoutHorizontal, bool localizeLabel, bool localizeUI, out GameObject settingContainerForPlatform)
		{
			if (Application.isMobilePlatform && !showOnMobile)
			{
				settingContainerForPlatform = null;
			}
			else
			{
				var objectLayoutGroup = GetComponent<LayoutGroup>();
				var objectContentSize = GetComponent<ContentSizeFitter>();
				if (objectLayoutGroup)
				{
					objectLayoutGroup.enabled = false;
				}

				if (objectContentSize)
				{
					objectContentSize.enabled = false;
				}

				((RectTransform)transform).sizeDelta = Vector2.zero;
				((RectTransform)transform).anchoredPosition = Vector2.zero;
				if (objectType == typeof(Button))
				{
					var button = Instantiate(_button, transform, false);
					button.name = title;
					if (button.GetComponentInChildren<UnityEngine.UI.Text>())
					{
						if (localizeUI)
						{
							button.GetComponentInChildren<UnityEngine.UI.Text>().gameObject.AddComponent<TextLocalization>().Key = title;
							button.GetComponentInChildren<UnityEngine.UI.Text>().text = Localization.Localization.Get(title);
						}
						else
						{
							button.GetComponentInChildren<UnityEngine.UI.Text>().text = title;
						}
					}

					settingContainerForPlatform = button.gameObject;
				}
				else
				{
					var layout = layoutHorizontal ? HorizontalLayout(title) : VerticalLayout(title);
					var label = Instantiate(_label);
					AddToLayout(layout, label);
					label.name = _label.name;
					if (localizeLabel)
					{
						label.gameObject.AddComponent<TextLocalization>().Key = title;
						label.text = Localization.Localization.Get(title);
					}
					else
					{
						label.text = title;
					}

					label.alignment = _labelAnchor;

					switch (objectType.Name)
					{
						case "Dropdown":
							var dropdown = Instantiate(_dropdown);
							var dropdownComp = dropdown.GetComponent<Dropdown>() ?? dropdown.GetComponentInChildren<Dropdown>();
							AddToLayout(layout, dropdownComp);
							var templateCanvas = dropdownComp.template.gameObject.AddComponent<Canvas>();
							templateCanvas.overrideSorting = false;
							var parentObject = transform.parent;
							var parentCanvas = parentObject.GetComponent<Canvas>();
							while (parentCanvas == null && parentObject != transform.root)
							{
								parentObject = parentObject.parent;
								parentCanvas = parentObject.GetComponent<Canvas>();
							}

							if (parentCanvas != null)
							{
								templateCanvas.sortingOrder = parentCanvas.sortingOrder;
								templateCanvas.sortingLayerName = parentCanvas.sortingLayerName;
							}

							dropdown.name = title;
							if (localizeUI)
							{
								dropdown.gameObject.AddComponent<DropdownLocalization>();
							}

							break;
						case "Slider":
							var slider = Instantiate(_slider);
							AddToLayout(layout, slider.GetComponent<Slider>() ?? slider.GetComponentInChildren<Slider>());
							slider.name = title;
							break;
						case "Toggle":
							var toggle = Instantiate(_toggle);
							AddToLayout(layout, toggle.GetComponent<Toggle>() ?? toggle.GetComponentInChildren<Toggle>());
							toggle.name = title;
							break;
						case "Text":
							layout.GetComponent<LayoutElement>().preferredHeight *= 1.5f;
							layout.GetComponent<AspectRatioFitter>().aspectRatio /= 1.5f;
							break;

					}

					settingContainerForPlatform = layout.gameObject;
				}
			}

			return settingContainerForPlatform != null;
		}

		/// <summary>
		/// Trigger a rebuild of the panel to ensure all objects are positioned and sized correctly
		/// Note: LayoutGroups with one child with text will be set to 1.5x normal size
		/// </summary>
		public virtual void RebuildLayout()
		{
			//Coroutine cannot be called if gameobject is inactive, so go straight to loop instead
			if (gameObject.activeInHierarchy)
			{
				StartCoroutine(RebuildLayoutDelay());
			}
			else
			{
				RebuildLayoutLoop();
			}
		}

		protected virtual IEnumerator RebuildLayoutDelay()
		{
			//Log logic copied from https://bitbucket.org/Unity-Technologies/ui/src/a3f89d5f7d145e4b6fa11cf9f2de768fea2c500f/UnityEngine.UI/UI/Core/Layout/CanvasScaler.cs?at=2017.3&fileviewer=file-view-default
			//Allows us to check if canvas size is inaccurate, which it is on at least the first frame, and wait if it isn't
			var canvasScaler = GetComponentInParent<CanvasScaler>();
			var logWidth = Mathf.Log(Screen.width / canvasScaler.referenceResolution.x, 2);
			var logHeight = Mathf.Log(Screen.height / canvasScaler.referenceResolution.y, 2);
			var logWeightedAverage = Mathf.Lerp(logWidth, logHeight, canvasScaler.matchWidthOrHeight);
			var logScaleFactor = Mathf.Pow(2, logWeightedAverage);
			if (!Mathf.Approximately(canvasScaler.transform.localScale.x, logScaleFactor))
			{
				yield return new WaitForEndOfFrame();
			}
			RebuildLayoutLoop();
		}

		protected virtual void RebuildLayoutLoop()
		{
			//Loop through up to ten times or until sizing remains consistent
			var previousSmallSize = 0;
			var smallestFontSize = 0;
			var checkCount = 0;
			while ((previousSmallSize == 0 || previousSmallSize != smallestFontSize) && checkCount < 10)
			{
				previousSmallSize = smallestFontSize;
				smallestFontSize = RebuildLayoutGroups();
				checkCount++;
			}
		}

		protected virtual int RebuildLayoutGroups()
		{
			var objectLayoutGroup = GetComponent<LayoutGroup>();
			var objectContentSize = GetComponent<ContentSizeFitter>();
			if (objectLayoutGroup)
			{
				objectLayoutGroup.enabled = false;
			}
			if (objectContentSize)
			{
				objectContentSize.enabled = false;
			}
			((RectTransform)transform).sizeDelta = Vector2.zero;
			((RectTransform)transform).anchoredPosition = Vector2.zero;

			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

			foreach (var layout in GetComponentsInChildren<LayoutGroup>(true))
			{
				if (layout.gameObject != gameObject)
				{
					foreach (Transform trans in layout.transform)
					{
						var aspect = trans.GetComponent<AspectRatioFitter>() ?? trans.gameObject.AddComponent<AspectRatioFitter>();
						var layoutElement = trans.GetComponent<LayoutElement>() ?? trans.gameObject.AddComponent<LayoutElement>();
						aspect.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
						var padding = GetComponent<LayoutGroup>().padding;
						Vector3[] objCorners = new Vector3[4];
						((RectTransform)gameObject.transform).GetWorldCorners(objCorners);
						var canvasSize = new Vector2(objCorners.Max(c => c.x) - objCorners.Min(c => c.x), objCorners.Max(c => c.y) - objCorners.Min(c => c.y));
						//Log logic copied from https://bitbucket.org/Unity-Technologies/ui/src/a3f89d5f7d145e4b6fa11cf9f2de768fea2c500f/UnityEngine.UI/UI/Core/Layout/CanvasScaler.cs?at=2017.3&fileviewer=file-view-default
						//Helps sizing calculation to be more accurate
						var canvasScaler = GetComponentInParent<CanvasScaler>();
						var canvas = GetComponentInParent<Canvas>();
						var logWidth = Mathf.Log(Screen.width / canvasScaler.referenceResolution.x, 2);
						var logHeight = Mathf.Log(Screen.height / canvasScaler.referenceResolution.y, 2);
						var logWeightedAverage = Mathf.Lerp(logWidth, logHeight, canvasScaler.matchWidthOrHeight);
						var logScaleFactor = Mathf.Pow(2, logWeightedAverage);
						if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera)
						{
							logScaleFactor *= canvas.worldCamera.orthographicSize / (Screen.height * 0.5f);
						}
						if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && !canvas.worldCamera) || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera && canvas.worldCamera.orthographic))
						{
							canvasSize /= logScaleFactor;
						}
						if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera && !canvas.worldCamera.orthographic)
						{
							canvasSize /= canvas.transform.localScale.x;
						}
						canvasSize -= new Vector2(padding.horizontal, padding.vertical);
						layout.GetComponent<LayoutElement>().preferredHeight = canvasSize.x / layout.GetComponent<AspectRatioFitter>().aspectRatio;
						if (layout.GetType() == typeof(HorizontalLayoutGroup))
						{
							layoutElement.preferredHeight = layout.GetComponent<LayoutElement>().preferredHeight;
							aspect.aspectRatio = (layout.GetComponent<AspectRatioFitter>().aspectRatio - (layout.GetComponent<HorizontalLayoutGroup>().spacing / layoutElement.preferredHeight)) / layout.transform.childCount;
						}
						else
						{
							layoutElement.preferredHeight = layout.GetComponent<LayoutElement>().preferredHeight / layout.transform.childCount;
							aspect.aspectRatio = layout.GetComponent<AspectRatioFitter>().aspectRatio * layout.transform.childCount;
						}
					}
				}
			}

			if (objectLayoutGroup)
			{
				objectLayoutGroup.enabled = true;
			}
			if (objectContentSize)
			{
				objectContentSize.enabled = true;
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

			//BestFit is done like this as otherwise an inactive object would result in a min font size
			var bestFitSize = gameObject.BestFit(false);
			foreach (var layout in GetComponentsInChildren<LayoutGroup>(true))
			{
				if (layout.gameObject != gameObject)
				{
					if (layout.transform.childCount == 1 && layout.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>())
					{
						layout.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = (int)(bestFitSize * 1.5f);
					}
				}
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

			return bestFitSize;
		}

		/// <summary>
		/// Clear the panel of objects
		/// </summary>
		public virtual void Wipe()
		{
			foreach (Transform child in transform)
			{
				Destroy(child.gameObject);
			}
		}
	}
}
