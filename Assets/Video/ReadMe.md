# Video Player
## Function 
A basic video player that allows for playing, pausing, stopping and scrubbing through a video clip in Unity.

## Usage
The video player requires 2 components to work:
1. VideoPlayer (UnityEngine.Video.VideoPlayer)
2. RawImage (UnityEngine.UI.RawImage)

All other components related to pausing/playing etc are optional.

The utility allows the following interactions:
``` c#
  // Set the video clip to play
  void SetClip(VideoClip clip);

  // Set the video url to play
  void SetURL(string url);

  // Play the selected VideoPlayer
  IEnumerator PlayVideo(bool loop = false, float playbackSpeed = 1f);
  
  // Continue playing the selected VideoPlayer
  void Continue(bool loop = false, float playbackSpeed = 1f);
  
  // Pause the selected VideoPlayer
  void Pause();
  
  // Stop playing the selected VideoPlayer
  void Stop();
```

## Limitations
- Videos will refuse to play if the editor is paused whilst playing.
- Bad audio quality, [seemingly a Unity issue](https://issuetracker.unity3d.com/issues/win-stuttering-sound-of-mp4-files-video-lags), fixed in 2018.2.
- Audio in effect does not work if playback speed is not 1.
- Does not work with URL as a source.
- Playback speed is capped at 3, otherwise the player consistently skipped back and does not play properly.
- Position slider skips back to previous point when jumping to another point.
- Play via URL Does not work with YouTube links currently.