# Best Fit
## Function
Allows for better control of the Best Fit functionality in Unity, allowing for a group of text components to all be the same size. eg. all options in a settings menu set to the same maximum font size.
## Usage
Can be used in the following ways:

**To set child objects of an object use:**
```c#
  GameObject.BestFit(bool includeInactive = true, List<string> newStrings = null);
  Component.BestFit(bool includeInactive = true, List<string> newStrings = null);
```

**To set a specified group of Text Components of an object use:**
```c#
  List<Component>.BestFit(bool includeInactive = true, List<string> newStrings = null);
  IEnumerable<Component>.BestFit(bool includeInactive = true, List<string> newStrings = null);
  Component[].BestFit(bool includeInactive = true, List<string> newStrings = null);
  List<GameObject>.BestFit(bool includeInactive = true, List<string> newStrings = null);
  IEnumerable<GameObject>.BestFit(bool includeInactive = true, List<string> newStrings = null);
  GameObject[].BestFit(bool includeInactive = true, List<string> newStrings = null);
```
- Setting includeInactive to false will mean that inactive GameObjects and text components will not be resized or used in resizing calculations.
- Providing a list of newStrings will result in a BestFit that takes into account all of the strings provided fitting into all text components.

### BestFit
This script can be placed on any UI GameObject and sets the min and max font size for all text objects when this script is the nearest parent BestFit component.

### BestFitAutomatic
Expanding upon the BestFit script, this script can also be placed on any UI GameObject and will set all child text to become the same size upon the GameObject becoming active, the resolution changing or the following being called in code:
```c#
  void OnChange(bool includeInactive = true, List<string> newStrings = null);
```

## Gotchas
- When using Layout Elements (Layout Groups, Aspect Ratio Fitters etc), inaccurate results will be returned if the Layout Element game object is made active after calling Best Fit.
- Depending on when BestFit is called, the scaling up/down of text may be visible to players for a couple frames.

# Cut Off
## Function
Allows for setting text and strings to be cut off after a certain length or when a character from those provided occurs, with an option to append characters to a string cut due to length.
## Usage
Can be used in the following ways:

**To set child objects of an object use:**
```c#
  GameObject.CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
  Component.CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
```

**To set a specified group of Text Components of an object use:**
```c#
  List<Component>.CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
  IEnumerable<Component>.CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
  Component[].CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
  List<GameObject>.CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
  IEnumerable<GameObject>.CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
  GameObject[].CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
```

**To set on a string use:**
```c#
  string.CutOff(uint maxLength = 0, char[] cutOffAfter = null, string cutOffAppendment = "");
```

- Default maxLength of 0 is taken as there being no max length.
- The cutOffAppendment is only added to the end of the string if it was cut off due to length.

# Force One Line
## Function
Replaces all spaces with characters that will force the string to be displayed on one line.
## Usage
Can be used in the following ways:

**To set child objects of an object use:**
```c#
  GameObject.ForceOneLine();
  Component.ForceOneLine();
```

**To set a specified group of Text Components of an object use:**
```c#
  List<Component>.ForceOneLine();
  IEnumerable<Component>.ForceOneLine();
  Component[].ForceOneLine();
  List<GameObject>.ForceOneLine();
  IEnumerable<GameObject>.ForceOneLine();
  GameObject[].ForceOneLine();
```

**To set on a string use:**
```c#
  string.ForceOneLine();
```