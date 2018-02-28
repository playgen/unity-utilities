using PlayGen.Unity.Utilities.Video;
using UnityEngine;

public class VideoExampleUsage : MonoBehaviour {

    [SerializeField]
    private VideoPlayerUI _video;

    private void Start()
    {
        _video.Play();
    }
}
