using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.FeedbackPanel
{

	public class FeedbackWindow : MonoBehaviour
	{
		private Button _cancelButton;
		private Button _sendButton;

		private UnityAction<string> _sendAction;

		private InputField _feedbackText;

		public void Setup(UnityAction<string> action)
		{
			_cancelButton = transform.FindChild("Panel/FooterPanel/CancelButton").GetComponent<Button>();
			_sendButton = transform.FindChild("Panel/FooterPanel/SendButton").GetComponent<Button>();

			_feedbackText = transform.FindChild("Panel/BodyPanel/Feedback").GetComponent<InputField>();

			_feedbackText.text = "";

			_sendAction = action;

			_sendButton.onClick.AddListener(SendPressed);
			_cancelButton.onClick.AddListener(CancelPressed);
		}

		private void SendPressed()
		{
			_sendAction(_feedbackText.text);
		}

		private void CancelPressed()
		{
			_feedbackText.text = "";
		}
	}
}