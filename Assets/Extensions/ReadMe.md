# Extensions
## Function 
Shorthand extensions for commonly used functions in Unity.
## Usage
There are 3 types of components that have been extended in this utility:

### GameObject

``` c#
    // Returns the GameObject of the parent or null if a root object.
    GameObject GameObject.Parent();

    // Returns the GameObject of the parent or null if a root object.
    GameObject MonoBehaviour.Parent();
```
### RectTransform

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
### Transform

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
  
  // Find and return a child of Type T, includeInactive determines in inactive objects should also be included in the search
  T Transform.FindComponentInChildren<T>(string find, bool includeInactive);

  // Finds a child Transform and includes inactive Objects
  Transform Transform.FindInactive(string name)
  
  // Finds all children Transforms by name and returns them 
  List<Transform> Transform.FindAll(string name, bool includeInactive = false)
```