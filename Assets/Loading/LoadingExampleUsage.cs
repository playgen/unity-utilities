using PlayGen.Unity.Utilities.Loading;

using UnityEngine;

public class LoadingExampleUsage : MonoBehaviour {

	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
		{
			Loading.Start("Loading with text!");
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Loading.Start();
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			Loading.Stop("It'll stop in two seconds", 2);
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			Loading.Stop();
		}
	}
}
