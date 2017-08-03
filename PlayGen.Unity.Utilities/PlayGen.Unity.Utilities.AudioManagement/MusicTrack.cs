using System;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	[Serializable]
	public class MusicTrack
	{
		public AudioClip Clip;
		public string Artist;
		public string TrackName;
		[Range(0f, 1f)] public float MaxVolume = 0.5f;
		[Range(0, 256)] public int Priority = 128;
	}
}