namespace PlayGen.Unity.Utilities.Loading
{

	public static class Loading
	{
		public static LoadingSpinner LoadingSpinner;

        public static bool IsActive => LoadingSpinner != null && LoadingSpinner.IsActive;

		public static void Set(int speed, bool clockwise)
		{
			if (LoadingSpinner)
			{
				LoadingSpinner.Set(clockwise, speed);
			}
		}

		public static void Start(string text = "")
		{
			if (LoadingSpinner)
			{
				LoadingSpinner.StartSpinner(text);
			}
		}

		public static void Stop(string text = "", float stopDelay = 0f)
		{
			if (LoadingSpinner)
			{
				LoadingSpinner.StopSpinner(text, stopDelay);
			}
		}
	}
}