using System;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	[Serializable]
	public class SfxClip
	{
		/// <summary>
		/// AudioClip to use
		/// </summary>
		[Tooltip("AudioClip to play")]
		public AudioClip Clip;
		/// <summary>
		/// Clip identifier
		/// </summary>
		[Tooltip("Clip identifier")]
		public string Name;
		/// <summary>
		/// Volume multiplier
		/// </summary>
		[Tooltip("Volume multiplier")]
		[Range(0f, 1f)] public float Volume = 0.5f;
		/// <summary>
		/// AudioSource priority. Remember, lower is higher priority!
		/// </summary>
		[Tooltip("AudioSource priority. Remember, lower is higher priority!")]
		[Range(0, 256)] public int Priority = 128;
		/// <summary>
		/// Should this clip being automaticaly played from the start?
		/// </summary>
		[Tooltip("Should this clip being automaticaly played from the start?")]
		public bool AutoPlay;
		/// <summary>
		/// Should this clip be looped?
		/// </summary>
		[Tooltip("Should this clip be looped?")]
		public bool Loop;
		[HideInInspector]
		public AudioSource AudioSource;
	}
}
