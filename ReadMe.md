A collection of Unity based Utilities

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

# Known Issues
todo

# Room for Improvement
todo

# Utilities Included

## Audio Management
### Function
A utility to handle multiple audio sources in unity

### Usage
The audio management controls both Music Tracks and Sound Effects (SFX). Audio files are added to the list of audioClips through the Inspector in Unity.

The music manager can be used as follows:
```c#
  // Go To Next Track
  void MusicManager.NextTrack();

  // Play the current track
  void MusicManager.PlayTrack();

  // Update Volume (value between 0 and 1)
  void MusicManager.UpdateVolume(float newValue)
```

The SFX manager can be used as follows
```c#
  // Play SFX by name
  void SfxManager.Play(string name)

  // Stop SFX by name
  void SfxManager.Stop(string name)

  // Pause SFX by name
  void SfxManager.Pause(string name)

  // Resume SFX by name
  void SfxManager.Resume(string name)

  // Check if an SFX is playing
  bool SfxManager.IsPlaying(string name)
  
  // Update volume based on master volume
  void SfxManager.UpdateVolume()
```

### Gotchas
- Needs Reviewing for if its good enough as is.

## Best Fit
### Function
Allows for better control of the Best Fit functionality in unity, allowing to group text components together to all be the same size. eg. all options in a settings menu set to the same maximum font size.
### Usage
Can be used in the following ways:

**To set child objects of an object use**
```c#
  GameObject.BestFit();
  Transform.BestFit();
```

**To set a specified group of Text Components of an object use**
```c#
  List<Selectable>.BestFit();
  IEnumerable<Selectable>.BestFit();
  Selectable[].BestFit();
  List<Text>.BestFit();
  IEnumerable<Text>.BestFit();
  Text[].BestFit();
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
Launch the tool in Unity at PlayGen Tools > Replace Fonts. This will launch the replace fonts UI, which will take a few seconds to evaluate all fonts used in the project. By selecting a font in the right most box and selecting "Replace With", all intsances of that font will be replaced with the new font
### Limitations
- There is no undo button available. 
- Not thoroughly tested with the use of prefabs
#### Gotchas
- Would be advised to use the FontManager model, a singleton that contains and enum for Text types (title, body, button etc.) and a mapping for each font. On each Text Component there should be a class to retrieve the font for their type at run time. This is more dynamic and gives more control and consistency

## Localization
### Function 
A package to handle the storing a retrieving of different languages
### Usage
Get the localized string for the provided key, toUpper will return an upper case variant of the returned string, overrideLanguage allows for a different language to be returned from the currently selected language
``` c#
  string Get(string key, bool toUpper = false, string overrideLanguage = null)
```

### Limitations
todo

## Shortcuts
### Function 
A selection of shortcuts for unity to make UI creation and editing easier
### Usage
The following shortcuts are used
1. Reset Offsets: Ctrl + Alt + R - Sets the offsets of the currently selected rect transform offsets to equal 0, useful for setting up anchors for elements
2. Create Button: Ctrl + Alt + Q - Creates a new generic button (no text) object that will fill the currently selected transform. The default Unity UI button is a set size and contains a text element.
3. Move Selected Object Up: Ctrl + Shift + UpArrow - Move the currently selected object up in the hierarchy
4. Move Selected Object Down: Ctrl + Shift + DownArrow - Move the currently selected object down in the hierarchy
5. Move Selected Object Sibling Of Parent: Ctrl + Shift + LeftArrow - Move the currently selected object to be a child of the object below it in the hierarchy 
6. Move Selected Object Child Of Sibling: Ctrl + Shift + RightArrow - Move the currently selected object to be a sibling of the object it is currently a child of
### Limitations
- No warnings for breaking prefabs
- When moving object to be a child of an object, it does not maintain focus unless the new parent is already expanded

## iOS Requirements
### Function 
Automatically generates privacy permission information for iOS builds. As of IOS 10.0, all privacy system requirement usage must be explained to the user when choosing if they want to accept permissions, Unity provides minimal fields for this in player settings and Xcode generate will not generate the privacy properties.
### Usage
The tool is launched PlayGen Tools > iOS Requirements. Which displays a form which provides a list of privacy permissions available to request from the user, simply select the permission needed in Tools/iOS Requirements and provide a short description of how the game uses it.
### Gotchas
- If there are troubles running this package, make sure that the filePath in iOSRequirements.cs is correct
- This asset heavily relies on keys being set to the exact name, as expected in the iOS requirements list, a full list of all requirements can be found [here](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Articles/CocoaKeys.html#//apple_ref/doc/uid/TP40009251-SW1)

## Feedback Panel
### Function 
A customisable panel for getting users to send feedback in game to a specified email using ElasticEmail.
### Usage
Setup for this asset is within the PlayGen.Unity.Utilities.FeedbackPanel Solution. By default it uses the PlayGen elastic email account, this is set in ElasticEmailClient.cs. Once rebuilt, the prefab can be added to the project and visuals edited as required.
### Gotchas
- If making this available publicly, remove the PlayGen api keys in elastic email

## Extensions
### Function 
Shorthand extensions for commonly used functions in unity
### Usage
There are 3 types of components that have been extended in this utility

**GameObject**

``` c#
  Checks if the GameObject active status is different to the new status then changes the GameObject active status
    void GameObject.Active(bool active)
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
  
  // Finds a child of Type T by name and returns it, includeInactive determines in inactive objects should also be included in the search
  T Transform.FindComponentInChildren<T>(string find, bool includeInactive);
  
```
**Transform**

``` c#
  // Finds a child Transform and includes inactive Objects
  Transform Transform.FindInactive(string name)
  
  // Finds all children Transforms by name and returns them 
  List<Transform> Transform.FindAll(string name, bool includeInactive = false)
```

### Gotchas
- This utility does not add extra functionality, just makes it more efficient to do repetitive tasks

## Form Keyboard Controls
### Function 
Allows for tabbing between unity forms
### Gotchas

## Loading Screen
### Function 
Loading screen that can be started and stopped at command
### Usage
Use the following functions to use the loading spinner
``` c#
  // Set the speed and direction that the spinner will spin at
  void LoadingSpinner.Set(int speed, bool clockwise);
  
  // Call to start the loading spinner, it will continue to spin until stop is called.
  void LoadingSpinner.Start(string text = "");
  
  // Call to stop the loading spinner, a stop Delay will provide time for the text to be displayed to the player 
  void LoadingSpinner.Stop(string text = "", float stopDelay = 0f);
```

## Video Player
### Function 
A basic video player that allows for play, pause, stop and scrubbing through a video clip in unity.

### Usage
The video player requires 2 components to work
1. VideoPlayer (UnityEngine.Video.VideoPlayer)
2. RawImage (UnityEngine.UI.RawImage)

The utitlity allows the following interactions
``` c#
  // Play the selected VideoPlayer
  IEnumerator PlayVideo();
  
  // Continue playing the selected VideoPlayer
  void Continue();
  
  // Pause the selected VideoPlayer
  void Pause();
  
  // Stop playing the selected VideoPlayer
  void Stop();
```

### Limitations
todo