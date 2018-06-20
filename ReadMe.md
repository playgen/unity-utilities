A collection of Unity based Utilities

# Licensing
See the [LICENCE](LICENCE.md) file included in this project.

# Key Project Structure
- **Assets**: *Unity project files for testing utilities*
  - **[UtitilityName]**: *Folder for example usage, used for testing purposes*

- **PlayGen.Unity.Utilities**: *precompiled source code for utilities*

# Development
## Environment Setup
For .pdb debugging it is expected that you work from `P:\unity-utilities` so that the source files can be resolved.

If you are going to be commiting DLLs or want to debug code inside the DLLs, you will need to do the following:

1. Open CMD.
2. Map the parent folder of your unity-utilities repository folder to the drive letter P:  
`SUBST P: [`Parent dir of unity-utilities project`]`  
If the path of the unity-utilities project is C:\Users\Bob\Projects\unity-utilities, the command would be:  
`SUBST P: C:\Users\Bob\Projects`
3. Navigate to P:\unity-utilities to make sure the mapping has worked.
4. Remember to always use this path when working on unity-utilities!

## Conventions
- Work from P:\unity-utilities.
- Commit .pdb files when committing .dlls.
- Each Utility project must have its own folder within unity i.e:  
  For [My Utility].csproj and Editor Only [My Utility].Editor.csproj
  - **Assets**
    - **[My Utility]**
      - **Plugins**
        - **net46**: *visual studio will automatically create this folder so leave it as is*
          - **[My Utility].dll**: *don't commit this*
          - **[My Utility].dll.meta**: *commit this*
          - **[My Utility].Editor.dll**: *don't commit this*
          - **[My Utility].Editor.dll.meta**: *set this to Editor Only and commit this*
      - **Prefabs**
      - **Scenes**
      - **Resources**:
        - **[My Utility]**: *so resources of [My Utility] will be laoded with `Resources.Load([My Utility]/[resource 1])`*
          - **[resource 1]**
          - **[resource 2]**

# Build
- Open the solution file at PlayGen.Unity.Utilities\PlayGen.Unity.Utilities.sln and build your selected project
- .dll's are outputted to ./obj/Release/net46

# Utilities Included

## Audio Management
### Function
A utility to handle multiple audio sources in Unity

### Usage
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

### Gotchas
- Needs further testing within production use.

## Best Fit
### Function
Allows for better control of the Best Fit functionality in Unity, allowing for a group of text components to all be the same size. eg. all options in a settings menu set to the same maximum font size.
### Usage
Can be used in the following ways:

**To set child objects of an object use**
```c#
  GameObject.BestFit();
  Component.BestFit();
```

**To set a specified group of Text Components of an object use**
```c#
  List<Component>.BestFit();
  IEnumerable<Component>.BestFit();
  Component[].BestFit();
  List<GameObject>.BestFit();
  IEnumerable<GameObject>.BestFit();
  GameObject[].BestFit();
```

### Gotchas
- The utility checks which is the biggest font size available and is still visible, this can cause text to scale up/down and be visible to players for a couple frames whilst it figures this out.

## Font Replace
### Function 
Allows for a single place to monitor which Fonts are used in your unity project and replace unwanted Fonts with a new Font easily.
### Usage
Launch the tool in Unity at PlayGen Tools > Replace Fonts. This will launch the replace fonts UI, which will take a few seconds to evaluate all fonts used in the project. By selecting a font in the right most box and selecting "Replace With", all instances of that font will be replaced with the new font
### Limitations
- There is no undo button available. 
- Not thoroughly tested with the use of prefabs.
- Doesn't seem to work.
### Gotchas
- Would be advised to use the FontManager model, a singleton that contains and enum for Text types (title, body, button etc.) and a mapping for each font. On each Text Component there should be a class to retrieve the font for their type at run time. This is more dynamic and gives more control and consistency

## Localization
### Function 
A package to handle the storing and retrieving of text translated into multiple languages.
### Usage
The following process is taken to set CurrentLanguage upon application start-up:
1. If localization has been set in the past, it is loaded using PlayerPrefs with the key "Last_Saved_Language".
2. If no language was saved or was not included as an option in the application, CultureInfo.CurrentUICulture is used.
3. If CultureInfo.CurrentUICulture was null or was not included as an option in the application, CultureInfo.CurrentCulture is used.
4. If CultureInfo.CurrentCulture was null or was not included as an option in the application, the language returned by GetFromSystemLanguage(), which uses Unity's Application.systemLanguage, is used.
5. If the language returned by GetFromSystemLanguage() is null or was not included as an option in the application, the DefaultLanguage is used.

The following process is used to decide which string is returned when getting localized text:
0. If in editor, not playing and LangaugeOverride has been set, LanguageOverride is efeectively CurrentLanguage in that situation.
1. If a value exists for the CurrentLanguage for the Key provided and it doesn't match the EmptyStringText, that value will be returned.
2. Otherwise, if the parent of the CurrentLanguage does not match the InvariantCulture, is included in the Language list and has a value that doesn't match the EmptyStringText, that value will be returned.
3. Otherwise, if any children of the CurrentLanguage's parent (or CurrentLnaguage if its parent matches the InvariantCulture) has a value that doesn't match the EmptyStringText, the first value found will be returned.
4. Otherwise, if a value exists for the DefaultLanguage for the Key provided and it doesn't match the EmptyStringText, that value will be returned.
5. Otherwise, the key itself will be returned.
#### UILocalization
Base class for UI, such as Text and Dropdown, to localize upon being enabled. Features an abstract 'Set' method which is called within the 'OnEnable' method.

LanguageOverride and 'Localize Text' buttons can be used to test localization on that UI element whilst not in Play Mode in-editor.

#### TextLocalization
Sets text to match the localized string for the provided key upon OnEnable or whenever the 'Set' method is called in code. Toggling ToUpper on results in a completely upper case variant being used.

#### DropdownLocalization
Set the options for the Dropdown to be localized upon OnEnable or whenever the 'Set' method is called in code. Dropdown options are set by passing all localization keys via the 'SetOptions' method in code or by setting them in-editor.
``` c#
  string SetOptions(List<string> options)
```

#### Code calls
In order to set text programmatically, the following methods can be used:
``` c#
  string Get(string key, bool toUpper = false, string overrideLanguage = null)
  string GetAndFormat(string key, bool toUpper, params object[] args)
  string GetAndFormat(string key, bool toUpper, params string[] args)
```
In order to update the CurrentLanguage, the following methods can be used:
``` c#
  string UpdateLanguage(string language)
  string UpdateLanguage(CultureInfo cultureLang)
```
Alongside being loaded from Resources at start-up, JSON can also be passed to Localization using the following:
``` c#
  string AddLocalization(TextAsset textAsset)
  string AddLocalization(string text)
```
### Limitations
- EmptyStringText const is set to 'XXXX' in code and currently cannot be changed.
- FilePath in Resources is set to 'Localization' and currently cannot be changed.
- PlayerPrefs key is set in code and currently cannot be changed.

### Gotchas


## Shortcuts
### Function 
A selection of shortcuts for Unity to make UI creation and editing easier.
### Usage
The following shortcuts are used:
Function | Shortcut | Description
--- | --- | ---
Reset Offsets | Ctrl + Alt + R | Sets the offsets of the currently selected rect transform offsets to equal 0, useful for setting up anchors for elements.
Create Button | Ctrl + Alt + Q | Creates a new generic button (no text) object that will fill the currently selected transforms. The default Unity UI button is a set size and contains a text element.
Move Selected Object Up | Ctrl + Shift + UpArrow | Move the currently selected object up in the hierarchy.
Move Selected Object Down | Ctrl + Shift + DownArrow | Move the currently selected object down in the hierarchy.
Move Selected Object Sibling Of Parent | Ctrl + Shift + LeftArrow | Move the currently selected object to be a child of the object below it in the hierarchy.
Move Selected Object Child Of Sibling | Ctrl + Shift + RightArrow | Move the currently selected object to be a sibling of the object it is currently a child of.

### Limitations
- No warnings for breaking prefabs
- When moving object to be a child of an object, it does not maintain focus unless the new parent is already expanded

## Gotchas
- Shortcuts still work when Editor text fields are focused.

## iOS Requirements
### Function 
Automatically generates privacy permission information for iOS builds. As of iOS 10.0, all privacy system requirement usage must be explained to the user when choosing if they want to accept permissions. Unity provides minimal fields for this in player settings and Xcode will not generate the privacy properties.
### Usage
The tool is launched via PlayGen Tools > iOS Requirements, which displays a form which provides a list of privacy permissions available to request from the user. Simply select the permission needed in Tools/iOS Requirements and provide a short description of how the game uses it.
### Gotchas
- If there are troubles running this package, make sure that the filePath in iOSRequirements.cs is correct
- This asset heavily relies on keys being set to the exact name, as expected in the iOS requirements list, a full list of all requirements can be found [here](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Articles/CocoaKeys.html#//apple_ref/doc/uid/TP40009251-SW1)

## Feedback Panel
### Function 
A customisable panel for getting users to send feedback in game to a specified email using ElasticEmail.
### Usage
Setup for this asset is within the PlayGen.Unity.Utilities.FeedbackPanel Solution. By default it uses generic details, this is set in ElasticEmailClient.cs. Once rebuilt, the prefab can be added to the project and visuals edited as required.
### Gotchas
- If making this available publicly, make generic details available to developers in-editor/code, unusable otherwise.

## Extensions
### Function 
Shorthand extensions for commonly used functions in Unity.
### Usage
There are 3 types of components that have been extended in this utility:

**GameObject**

``` c#
    // Returns the GameObject of the parent or null if a root object.
    GameObject GameObject.Parent();
```
**RectTransform**

``` c#
  // Returns the RectTransform from a Transform
  RectTransform Transform.RectTransform();
  
  // Returns the RectTransform from a GameObject
  RectTransform GaneObject.RectTransform();
  
  // Returns the RectTransform from a MonoBehaviour
  RectTransform MonoBehaviour.RectTransform();
  
  // Finds a child RectTransform by name and returns it
  RectTransform Transform.FindRect(string find);
```
**Transform**

``` c#
// Finds a component of type T that is a child of the current Transform and returns it
  T Transform.FindComponent<T>(string find);
  
  // Finds a child Image by name and returns it
  Image Transform.FindImage(string find);
  
  // Finds a child text by name and returns it
  Text Transform.FindText(string find);
  
  // Finds a child button by name and returns it
  Button Transform.FindButton(string find);
  
  // Finds a child GameObject by name and returns it
  GameObject Transform.FindObject(string find);

   // Returns the GameObject of the parent or null if a root object.
  GameObject Transform.Parent();
  
  // Finds a child of Type T by name and returns it, includeInactive determines in inactive objects should also be included in the search
  T Transform.FindComponentInChildren<T>(string find, bool includeInactive);

  // Finds a child Transform and includes inactive Objects
  Transform Transform.FindInactive(string name)
  
  // Finds all children Transforms by name and returns them 
  List<Transform> Transform.FindAll(string name, bool includeInactive = false)
```

### Gotchas
- This utility does not add extra functionality, just makes it more efficient to do repetitive tasks

## Form Keyboard Controls
### Function 
Allows for tabbing between unity forms.
### Usage

### Gotchas


## Loading Screen
### Function 
Loading screen that can be started and stopped at command.
### Usage
A GameObject with the LoadingSpinner script on the root should exist and be active at the beginning of any Scene where it is needed. Following the layout set out in the LoadingSpinner class, the GameObject should have the following elements:
- 'Container' - Child of the root GameObject. Image which effectively blocks any buttons being clicked behind the spinner whilst it is active.
- 'Spinner' - Child of the Container GameObject. The image that will spin whilst the object is active.
- 'Text' - Child of the Container GameObject. The text that is displayed whilst the object is active.

The 'Loading' prefab provided follows the above structure.

Please note that all fields and methods in the LoadingSpinner class are at least protected and all methods are virtual, meaning this logic can be adjusted by creating a new class which inherits from LoadingSpinner. The only required element is that the following should be called in Awake():
``` c#
  Loading.LoadingSpinner = this;
```

Use the following functions to use the loading spinner:
``` c#
  // Set the speed and direction that the spinner will spin at
  void LoadingSpinner.Set(int speed, bool clockwise);
  
  // Call to start the loading spinner, which will continue to spin until stop is called
  void LoadingSpinner.Start(string text = "");
  
  // Call to stop the loading spinner after a stopDelay amount of seconds has passed
  void LoadingSpinner.Stop(string text = "", float stopDelay = 0f);
```

## Video Player
### Function 
A basic video player that allows for playing, pausing, stopping and scrubbing through a video clip in Unity.

### Usage
The video player requires 2 components to work:
1. VideoPlayer (UnityEngine.Video.VideoPlayer)
2. RawImage (UnityEngine.UI.RawImage)

Al other components related to pausing/playing etc are optional.

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

### Limitations
- Videos will refuse to play if the editor is paused whilst playing.
- Bad audio quality, [seemingly a Unity issue](https://issuetracker.unity3d.com/issues/win-stuttering-sound-of-mp4-files-video-lags), fixed in 2018.2.
- Audio in effect does not work if playback speed is not 1.
- Does not work with URL as a source.
- Playback speed is capped at 3, otherwise the player consistently skipped back and does not play properly.
- Position slider skips back to previous point when jumping to another point.
- Play via URL Does not work with YouTube links currently.

## UI
### Function 
A set of classes utility to add additional UI functionality.

### Usage
#### ScrollRectFollowFocused
Place on a object that has a ScrollRect component. Primary for use with the 'Template' object on Dropdowns. Ensures that the focused UI element is visible within the ScrollRect.

#### SliderHandleSizeSetter
Place on the 'Handle' within the 'Handle Slide Area' of a Slider object. Sets the width of the Handle to be the height of the Handle Slide Area multipled by the scale provided. Hieght is automatically set to match the height of the Handle Slide Area by the Slider itself.

#### PlatformPositioning
Place on any RectTransform. Will set RectTransform anchors to match the anchors provided for the platform.

Script has Context Menu "Set Values to Current" which will set the Mobile and Standalone Positioning values to match the current anchors being used.

Editor has menu items for setting all objects with this script to match their Mobile or Standalone Positioning values.

### Limitations
- Cursor interacting with the ScrollRect can result in odd behaviour. Only intended for projects that are keyboard/controller only.
- For PlatformPositioning to work UI has to be set up using anchors rather than offsets. Will not fully work on transforms where the anchors are set by parent objects.