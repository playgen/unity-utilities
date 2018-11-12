A collection of Unity based Utilities

# Licensing
See the [LICENCE](LICENCE.md) file included in this project.

# Key Project Structure
- **Assets**: *Unity project files for testing utilities*
  - **[UtitilityName]**: *Folder for example usage, used for testing purposes*

- **PlayGen.Unity.Utilities**: *source code for utilities*

# Build
**IMPORTANT** Solution must be built before opening the Project in Unity
- Open the solution file at PlayGen.Unity.Utilities\PlayGen.Unity.Utilities.sln and build your selected project
- .dll's are built to: *Assets/[Utility]/Plugins/..*

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
- ### Work from P:\unity-utilities.
- ### Commit .pdb files when committing .dlls.
- With the PlayGen.Unity.Utilities.sln open in Visual Studio, create a .NET FrameWork 4.6 Class Library project for your source code:
  1. Name it: *PlayGen.Utilities.[My Utility].csproj*  
  2. And reference the dlls in the lib/Unity folder should you need to access any of the UnityEngine API.  
  3. Set the build output to: *Assets/[My Utility]/Plugins*
- Unity Editor specific code needs to go in its own project:
  1. Name it: *PlayGen.Utilities.[My Utility].Editor.csproj*  
  2. And reference the dlls in the lib/Unity folder should you need to access any of the UnityEngine or UnityEditor API.
  3. Set the build output to: 
   *Assets/[My Utility]/Plugins/Editor*
- Each utility should have its own "ReadMe.md" file:
  - Placed in its *Assets/[My Utility]/* folder.
  - And a reference placed in the [Utilities](#Utilities) section with a brief overview.  
- Each Utility project must have its own folder within unity 
i.e:  
  - **Assets**
    - **[My Utility]**
      - **ReadMe.md**: Read me with usage etc instructions for the specific utility.
      - **Plugins**: *this is the folder you set the build output to in your visual studio project(s).*
        - **net46**: *visual studio will automatically create this folder so leave it as is.*
          - **[My Utility].dll**: *don't commit this.*
          - **[My Utility].dll.meta**: *commit this.*          
        - **Editor**
          - **net46**: *visual studio will automatically create this folder so leave it as is.*
            - **[My Utility].Editor.dll**: *don't commit this.*
            - **[My Utility].Editor.dll.meta**: *set this to Editor Only and commit this.*
      - **Prefabs**
      - **Scenes**
      - **Examples**: *put example scenes and scripts in here.*
      - **Resources**:
        - **[My Utility]**: *so resources of [My Utility] will be laoded with `Resources.Load([My Utility]/[resource 1])` etc.*
          - **[resource 1]**
          - **[resource 2]**

# Utilities

- [**Audio Management**](Assets/Audio/ReadMe.md) - Handle multiple audio sources in Unity.
- [**Extensions**](Assets/Extensions/ReadMe.md) - Shorthand extensions for commonly used functions in Unity.
- [**Feedback Panel**](Assets/SendFeedbackAsset/ReadMe.md) - A customisable panel for getting users to send feedback in game to a specified email using ElasticEmail.
- [**Font Replace**](Assets/Editor/Plugins/FontReplace/ReadMe.md) - Monitor which Fonts are used in your Unity project and replace unwanted Fonts with a new Font easily.
- [**Form Keyboard Controls**](Assets/FormKeyboardControls/ReadMe.md) - Tabbing between Selectable elements in a form-like layout.
- [**iOS Requirements**](Assets/iOSRequirements/ReadMe.md) - Automatically generates privacy permission information for iOS builds (10.0 and later).
- [**Loading**](Assets/Loading/ReadMe.md) - Loading screen that can be started and stopped at command.
- [**Localization**](Assets/Localization/ReadMe.md) - Storing and retrieving of text translated into multiple languages.
- [**Shortcuts**](Assets/Editor/Plugins/Shortcuts/ReadMe.md) - Shortcuts for Unity to make UI creation and editing easier.
- [**Text**](Assets/Text/ReadMe.md) - Better control of Text Elements in Unity using; best fit, text cut off, and forcing text to one line.
- [**UI**](Assets/UI/ReadMe.md) - Additional UI functionality.
- [**Video Player**](Assets/Video/ReadMe.md) - A basic video player that allows for playing, pausing, stopping and scrubbing through a video clip in Unity.

# Gotchas
- If building multiple utilities and they may reference the same .dll, Unity may complain that `The imported type [multi referenced type] is defined multiple times`. We suggest disabling all but one of the offending .dlls in the .meta files.