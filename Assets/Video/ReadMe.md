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

  // Play the selected VideoPlayer with default loop and playbackSpeed values
  void PlayDefault();

  // Play the selected VideoPlayer with the current VideoPlayer loop and playbackSpeed values
  void PlayCurrent();

  // Play the selected VideoPlayer
  void Play(bool loop = false, float playbackSpeed = 1f);
  
  // Continue playing the selected VideoPlayer with the current VideoPlayer loop and playbackSpeed values
  void Continue();

  // Continue playing the selected VideoPlayer
  void Continue(bool loop, float playbackSpeed = 1f);
  
  // Pause the selected VideoPlayer
  void Pause();
  
  // Stop playing the selected VideoPlayer
  void Stop();
```

## Limitations
- Videos will refuse to play if the editor is paused whilst playing.
- Play via URL Does not work with YouTube/Vimeo links (Unity 2018.1).  
Because of this, it is recommended to bundle your videos within the unity project so that they can be transcoded to a format that will work on the target platform.

## Gotchas
- Playback speed is capped at 3, otherwise the player consistently skips back and does not play properly.
- Audio in effect does not work if playback speed is not 1.
- Bad audio quality, [seemingly a Unity issue](https://issuetracker.unity3d.com/issues/win-stuttering-sound-of-mp4-files-video-lags), fixed in 2018.2.
- Position slider skips back to previous point when jumping to another point.