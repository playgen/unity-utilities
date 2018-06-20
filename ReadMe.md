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

- [Audio Management](Assets/Audio/ReadMe.md)
- [Best Fit](Assets/BestFit/ReadMe.md)
- [Extensions](Assets/Extensions/ReadMe.md)
- [Feedback Panel](Assets/SendFeedbackAsset/ReadMe.md)
- [Font Replace](Assets/Editor/Plugins/FontReplace/ReadMe.md)
- [Form Keyboard Controls](Assets/FormKeyboardControls/ReadMe.md)
- [iOS Requirements](Assets/iOSRequirements/ReadMe.md)
- [Loading](Assets/Loading/ReadMe.md)
- [Localization](Assets/Localization/ReadMe.md)
- [Shortcuts](Assets/Editor/Plugins/Shortcuts/ReadMe.md)
- [UI](Assets/UI/ReadMe.md)
- [Video Player](Assets/Video/ReadMe.md)