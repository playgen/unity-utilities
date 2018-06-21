# Font Replace
## Function 
Allows for a single place to monitor which Fonts are used in your Unity project and replace unwanted Fonts with a new Font easily.
## Usage
Launch the tool in Unity at PlayGen Tools > Replace Fonts. This will launch the replace fonts UI, which will take a few seconds to evaluate all fonts used in the project. By selecting a font in the right most box and selecting "Replace With", all instances of that font will be replaced with the new font.
## Limitations
- Not thoroughly tested with the use of prefabs.
## Gotchas
- After applying sometimes need to select a text object in the hierarchy for the scene to update.
- Would be advised to use the FontManager model, a singleton that contains and enum for Text types (title, body, button etc.) and a mapping for each font. On each Text Component there should be a class to retrieve the font for their type at run time. This is more dynamic and gives more control and consistency