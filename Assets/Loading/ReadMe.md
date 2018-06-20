# Loading Screen
## Function 
Loading screen that can be started and stopped at command.
## Usage
A GameObject with the LoadingSpinner script on the root should exist and be active at the beginning of any Scene where it is needed. Following the layout set out in the LoadingSpinner class, the GameObject should have the following elements:
- 'Container' - Child of the root GameObject. Image which effectively blocks any buttons being clicked behind the spinner whilst it is active.
- 'Spinner' - Child of the Container GameObject. The image that will spin whilst the object is active.
- 'Text' - Child of the Container GameObject. The text that is displayed whilst the object is active.

The 'Loading' prefab provided follows the above structure.

Please note that all fields and methods in the LoadingSpinner class are at least protected and all methods are virtual, meaning this logic can be adjusted by creating a new class which inherits from LoadingSpinner. The only required element is that the following should be called in Awake():
``` c#
  Loading.LoadingSpinner = this;
```

Use the following functions to use the loading spinner:
``` c#
  // Set the speed and direction that the spinner will spin at
  void LoadingSpinner.Set(int speed, bool clockwise);
  
  // Call to start the loading spinner, which will continue to spin until stop is called
  void LoadingSpinner.Start(string text = "");
  
  // Call to stop the loading spinner after a stopDelay amount of seconds has passed
  void LoadingSpinner.Stop(string text = "", float stopDelay = 0f);
```