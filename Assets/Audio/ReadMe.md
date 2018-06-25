# Audio Management
## Function
A utility to handle multiple audio sources in Unity.

## Usage
The audio management controls both Music Tracks and Sound Effects (SFX). Audio files are added to the list of audioClips through the Inspector in Unity.

All public static methods in both managers call protected virtual methods which contain the actual logic, allowing for the logic to be changed or added to if needed.

Both managers are based off of a generic version so that alternative MusicTrack/SfxClip can be used if additional information needs to be provided.

The music manager can be used as follows:
```c#
  // Go To Previous Track
  void MusicManager.PreviousTrack();

  // Go To Next Track
  void MusicManager.NextTrack();

  // Play the track with this name in the list of MusicTracks
  void MusicManager.PlayTrack(string trackName);

  // Play the track with this AudioClip in the list of MusicTracks
  void MusicManager.PlayTrack(AudioClip clip);

  // Play the track at this position in the list of MusicTracks
  void MusicManager.PlayTrack(int trackNumber);

  // Update Volume (value between 0 and 1)
  void MusicManager.UpdateVolume(float newValue)
```

The SFX manager can be used as follows
```c#
  // Play SFX by name
  void SfxManager.Play(string sourceName)

  // Stop SFX by name
  void SfxManager.Stop(string sourceName)

  // Pause SFX by name
  void SfxManager.Pause(string sourceName)

  // Resume SFX by name
  void SfxManager.Resume(string sourceName)

  // Check if an SFX is playing
  bool SfxManager.IsPlaying(string sourceName)

  // Update volume based on master volume
  void SfxManager.UpdateVolume(float newValue)
```

## Gotchas
- Latest version needs to be tested.