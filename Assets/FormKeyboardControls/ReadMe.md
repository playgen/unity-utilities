# Form Keyboard Controls
## Function 
Allows for tabbing between Selectable elements in a form-like layout.
## Usage
Add PlayGen.Unity.Utilities.FormKeyboardControls.dll on the root of the panel and set the buttons that should be activated when Enter or Escape is pressed.

Navigation between the various UI elements has to be set in order for this utility to work properly. 

**Note**: It is **strongly** recommended that navigation is set up using the 'Explicit' option in order to get accurate navigation between elements of the form.

## Gotchas
- Currently does not account for selecting GameObjects that are currently inactive, which will produce unusual looking behaviour.