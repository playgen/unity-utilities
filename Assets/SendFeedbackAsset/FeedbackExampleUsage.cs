using PlayGen.Unity.Utilities.FeedbackPanel;

using UnityEngine;

public class FeedbackExampleUsage : MonoBehaviour
{
	private FeedbackWindow _feedbackWindow;

	// Use this for initialization
	void Start ()
	{
		// Locate the class in scene
		_feedbackWindow = GameObject.Find("FeedbackPanel").GetComponent<FeedbackWindow>();

		// setup the window
		_feedbackWindow.Setup(SendFeedbackPressed);
		ElasticEmailClient.Details = new ElasticEmailDetails(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
	}

	private void SendFeedbackPressed(string feedback)
	{
		ElasticEmail.Instance.SendEmail(feedback, "Subject: Feedback Example Usage");
	}
}
