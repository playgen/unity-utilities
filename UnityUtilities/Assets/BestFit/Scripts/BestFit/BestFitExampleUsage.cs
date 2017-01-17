using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestFitExampleUsage : MonoBehaviour {

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
		gameObject.BestFit();
	}
}
