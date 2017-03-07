namespace PlayGen.Unity.Utilities.Loading
{
	public static class Loading
	{
		internal static LoadingSpinner LoadingSpinner;

        /// <summary>
        /// Is the spinner currently being displayed?
        /// </summary>
        public static bool IsActive => LoadingSpinner != null && LoadingSpinner.IsActive;

        /// <summary>
        /// Set the speed and direction of the spinner
        /// </summary>
		public static void Set(int speed, bool clockwise)
		{
			if (LoadingSpinner)
			{
				LoadingSpinner.Set(clockwise, speed);
			}
		}

        /// <summary>
        /// Set the spinner to be shown and spinning
        /// </summary>
		public static void Start(string text = "")
		{
			if (LoadingSpinner)
			{
				LoadingSpinner.StartSpinner(text);
			}
		}

        /// <summary>
        /// Set the spinner to stop being shown in stopDelay seconds (default is 0)
        /// </summary>
		public static void Stop(string text = "", float stopDelay = 0f)
		{
			if (LoadingSpinner)
			{
				LoadingSpinner.StopSpinner(text, stopDelay);
			}
		}
	}
}