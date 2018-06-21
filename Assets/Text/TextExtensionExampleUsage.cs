using PlayGen.Unity.Utilities.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextExtensionExampleUsage : MonoBehaviour {

	private void OnEnable()
	{
		BestFit.ResolutionChange += OnResolutionChange;
		OnResolutionChange();
	}

	private void OnDisable()
	{
		BestFit.ResolutionChange -= OnResolutionChange;
	}

	private void OnResolutionChange()
	{
		gameObject.ForceOneLine().CutOff(20, null, "...").BestFit();
	}
}
