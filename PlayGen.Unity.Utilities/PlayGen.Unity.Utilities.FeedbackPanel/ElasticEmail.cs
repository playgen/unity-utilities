using System.Collections;
using UnityEngine;

namespace PlayGen.Unity.Utilities.FeedbackPanel
{
	public class ElasticEmail : MonoBehaviour
	{
		public static ElasticEmail Instance;

		protected virtual void Awake()
		{
			Instance = this;
		}

		public virtual void SendEmail(string body, string subject)
		{
			var address = ElasticEmailClient.GetAddress();
			var form = ElasticEmailClient.GetForm(subject, body);

			StartCoroutine(Send(address, form));
		}

		protected virtual IEnumerator Send(string address, WWWForm form)
		{
			var www = new WWW(address, form);
			yield return www;

			Debug.Log(!string.IsNullOrEmpty(www.error) ? www.error : "Success sending feedback");
		}
	}
}