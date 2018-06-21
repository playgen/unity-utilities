namespace PlayGen.Unity.Utilities.FeedbackPanel
{
	public class ElasticEmailDetails
	{
		public string ApiKey { get; }
		public string Address { get; }
		public string FromEmail { get; }
		public string FromName { get; }
		public string ToEmail { get; }

		public ElasticEmailDetails(string api, string address, string fromEmail, string fromName, string toEmail)
		{
			ApiKey = api;
			Address = address;
			FromEmail = fromEmail;
			FromName = fromName;
			ToEmail = toEmail;
		}
	}
}
