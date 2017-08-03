using System;
using UnityEngine;

namespace PlayGen.Unity.Utilities.AudioManagement
{
	[Serializable]
	public class SfxClip
	{
		public string Name;
		public AudioClip Clip;
		[Range(0f, 1f)] public float Volume = 0.5f;
		[Range(0, 256)] public int Priority = 128;
		public bool AutoPlay;
		public bool Loop;
		[HideInInspector]
		public AudioSource AudioSource;
	}
}
