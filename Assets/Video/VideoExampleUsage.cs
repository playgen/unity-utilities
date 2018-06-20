using PlayGen.Unity.Utilities.Video;
using UnityEngine;
using UnityEngine.Video;

public class VideoExampleUsage : MonoBehaviour {

	[SerializeField]
	private VideoClip _clip;
	[SerializeField]
	private VideoPlayerUI _video;

	private void Start()
	{
		//_video.SetURL("");
		_video.SetClip(_clip);
		_video.Play();
	}
}
