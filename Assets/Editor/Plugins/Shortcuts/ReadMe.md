# Shortcuts
## Function 
A selection of shortcuts for Unity to make UI creation and editing easier.
## Usage
The following shortcuts are used:
Function | Shortcut | Description
--- | --- | ---
Reset Offsets | Ctrl + Alt + R | Sets the offsets of the currently selected rect transform offsets to equal 0, useful for setting up anchors for elements.
Create Button | Ctrl + Alt + Q | Creates a new generic button (no text) object that will fill the currently selected transforms. The default Unity UI button is a set size and contains a text element.
Move Selected Object Up | Ctrl + Shift + UpArrow | Move the currently selected object up in the hierarchy.
Move Selected Object Down | Ctrl + Shift + DownArrow | Move the currently selected object down in the hierarchy.
Move Selected Object Sibling Of Parent | Ctrl + Shift + LeftArrow | Move the currently selected object to be a child of the object below it in the hierarchy.
Move Selected Object Child Of Sibling | Ctrl + Shift + RightArrow | Move the currently selected object to be a sibling of the object it is currently a child of.

## Limitations
- No warnings for breaking prefabs
- When moving object to be a child of an object, it does not maintain focus unless the new parent is already expanded.

## Gotchas
- Shortcuts still work when Editor text fields are focused.