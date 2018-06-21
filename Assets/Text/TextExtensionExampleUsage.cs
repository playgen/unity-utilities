using PlayGen.Unity.Utilities.Text;
using UnityEngine;

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
