using UnityEngine;

namespace PlayGen.Unity.Utilities.FeedbackPanel
{
	public static class ElasticEmailClient
	{
		public static ElasticEmailDetails Details = new ElasticEmailDetails(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

		public static WWWForm GetForm(string subject, string bodyText)
		{
			var form = new WWWForm();
			form.AddField("apiKey", Details.ApiKey);
			form.AddField("from", Details.FromEmail);
			form.AddField("fromName", Details.FromName);
			form.AddField("to", Details.ToEmail);
			form.AddField("subject", subject);
			form.AddField("bodyText", bodyText);
			form.AddField("isTransactional", "true");

			return form;
		}

		public static string GetAddress()
		{
			return Details.Address;
		}
	}
}