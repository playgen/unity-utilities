using System;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	[Serializable]
	public class MusicTrack
	{
		/// <summary>
		/// AudioClip to play
		/// </summary>
		[Tooltip("AudioClip to play")]
		public AudioClip Clip;
		/// <summary>
		/// Artist name
		/// </summary>
		[Tooltip("Artist name")]
		public string Artist;
		/// <summary>
		/// Track/Song name
		/// </summary>
		[Tooltip("Track/Song name")]
		public string TrackName;
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
	}
}