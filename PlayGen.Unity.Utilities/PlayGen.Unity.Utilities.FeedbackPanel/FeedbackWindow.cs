using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayGen.Unity.Utilities.FeedbackPanel
{
	public class FeedbackWindow : MonoBehaviour
	{
		[SerializeField]
		protected Button _cancelButton;
		[SerializeField]
		protected Button _sendButton;
		[SerializeField]
		protected InputField _feedbackText;
		protected UnityAction<string> _sendAction;

		protected virtual void Awake()
		{
			Setup(_sendAction);
		}

		public virtual void Setup(UnityAction<string> action)
		{
			_feedbackText.text = string.Empty;
			_sendAction = action;
			_sendButton.onClick.AddListener(SendPressed);
			_cancelButton.onClick.AddListener(CancelPressed);
		}

		protected virtual void SendPressed()
		{
			_sendAction(_feedbackText.text);
		}

		protected virtual void CancelPressed()
		{
			_feedbackText.text = string.Empty;
		}
	}
}