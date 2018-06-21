using PlayGen.Unity.Utilities.Settings;
using PlayGen.Unity.Utilities.Localization;
using UnityEngine;
using UnityEngine.UI;

public class SettingsExampleUsage : MonoBehaviour {
	private SettingCreation _creation;
	private Button _apply;
	private Button _save;
	private Button _cancel;
	private Dropdown _quality;
	private Dropdown _resolution;
	private Dropdown _language;
	private Toggle _fullScreen;
	private Slider _master;
	private Slider _music;
	private Slider _soundEffect;

	void Awake()
	{
		_creation = GetComponentInChildren<SettingCreation>(true);

		_creation.TryQualityForPlatform(out _quality, true, false);
		_creation.TryResolutionForPlatform(out _resolution, 800, 600, null, false, false);
		_creation.TryLanguageForPlatform(out _language, false, false);
		_creation.TryFullScreenForPlatform(out _fullScreen, false);
		_creation.TryVolumeForPlatform("Master Volume", out _master, 1, false, false);
		_creation.TryVolumeForPlatform("Music Volume", out _music, 1, false, false);
		_creation.TryVolumeForPlatform("SFX Volume", out _soundEffect, 1, false, false);

		var buttonLayout = _creation.HorizontalLayout("Buttons");

		if (_creation.TryForPlatform("Apply", true, out _apply))
		{
			_creation.AddToLayout(buttonLayout, _apply);
			_apply.onClick.AddListener(ApplyChanges);
		}

		if (_creation.TryForPlatform("Cancel", true, out _cancel))
		{
			_creation.AddToLayout(buttonLayout, _cancel);
		}
	}

	void OnEnable()
	{
		_creation.RebuildLayout();
	}

	void ApplyChanges()
	{
		var fullScreen = _fullScreen != null ? _fullScreen.isOn : Screen.fullScreen;
		
		if (_resolution != null)
		{
			var newResolutionSplit = _resolution.options[_resolution.value].text.Split('x');
			var newResolution = new Resolution { width = int.Parse(newResolutionSplit[0]), height = int.Parse(newResolutionSplit[1]) };
			
			Screen.SetResolution(newResolution.width, newResolution.height, fullScreen);
		}

		if (_quality != null)
		{
			var quality = _quality.value;
			QualitySettings.SetQualityLevel(quality, true);
		}

		if (_master != null)
		{
			PlayerPrefs.SetFloat(_master.name, _master.value);
		}

		if (_music != null)
		{
			PlayerPrefs.SetFloat(_music.name, _music.value);
		}

		if (_soundEffect != null)
		{
			PlayerPrefs.SetFloat(_soundEffect.name, _soundEffect.value);
		}

		if (_language != null)
		{
			Localization.UpdateLanguage(Localization.Languages[_language.value]);
		}
	}
}
