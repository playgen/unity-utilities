# iOS Requirements
## Function 
Automatically generates privacy permission information for iOS builds. As of iOS 10.0, all privacy system requirement usage must be explained to the user when choosing if they want to accept permissions. Unity provides minimal fields for this in player settings and Xcode will not generate the privacy properties.
## Usage
The tool is launched via PlayGen Tools > iOS Requirements, which displays a form which provides a list of privacy permissions available to request from the user. Simply select the permission needed in Tools/iOS Requirements and provide a short description of how the game uses it.
## Gotchas
- If there are troubles running this package, make sure that the filePath in iOSRequirements.cs is correct
- This asset heavily relies on keys being set to the exact name, as expected in the iOS requirements list, a full list of all requirements can be found [here](https://developer.apple.com/library/archive/documentation/General/Reference/InfoPlistKeyReference/Articles/CocoaKeys.html#//apple_ref/doc/uid/TP40009251-SW1)