# UI
## Function 
A set of classes which add additional UI functionality.

## Usage
### ScrollRectFollowFocused
Place on an object that has a ScrollRect component. Primary for use with the 'Template' object on Dropdowns. Ensures that the focused UI element is visible within the ScrollRect.

### SliderHandleSizeSetter
Place on the 'Handle' within the 'Handle Slide Area' of a Slider object. Sets the width of the Handle to be the height of the Handle Slide Area multipled by the scale provided. Height is automatically set to match the height of the Handle Slide Area by the Slider itself.

### PlatformPositioning
Place on any RectTransform. Will set RectTransform anchors to match the anchors provided for the platform.

Script has Context Menu "Set Values to Current" which will set the Mobile and Standalone Positioning values to match the current anchors being used.

Editor has menu items for setting all objects with this script to match their Mobile or Standalone Positioning values.

## Limitations
- Cursor interacting with the ScrollRect can result in odd behaviour. Only intended for projects that are keyboard/controller only.
- PlatformPositioning will not fully work on transforms where the anchors are set by parent objects.

## Gotchas
- For PlatformPositioning to work UI has to be set up using anchors rather than offsets. 