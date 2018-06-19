using UnityEngine;

namespace PlayGen.Unity.Utilities.FeedbackPanel
{
	public class ElasticEmailClient : MonoBehaviour
	{
		private const string ApiKey = "";
		private const string Address = "https://api.elasticemail.com/v2/email/send";
		private const string FromEmail = "sender@domain.com";
		private const string FromName = "Name";
		private const string ToEmail = "email@domain.com";

		public static WWWForm GetForm(string subject, string bodyText)
		{
			var form = new WWWForm();
			form.AddField("apiKey", ApiKey);
			form.AddField("from", FromEmail);
			form.AddField("fromName", FromName);
			form.AddField("to", ToEmail);
			form.AddField("subject", subject);
			form.AddField("bodyText", bodyText);
			form.AddField("isTransactional", "true");

			return form;
		}

		public static string GetAddress()
		{
			return Address;
		}
	}
}