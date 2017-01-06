
Use the iOS System Requirements tool to automatically generate privacy permission information for iOS builds.

As of IOS 10.0, all privacy system requirement usage must be explained to the user when choosing if they want to accept permissions, neither Unity or Xcode generate the privacy properties.

The tool provides a list of privacy permissions available to request from the user, simply select the permission needed in Tools/iOS Requirements and provide a short description of how the game uses it.

eg.
The game needs to use the calendar to create events on the users device
- Open Tools/iOS Requirements
- Select "NSCalendarsUsageDescription"
- In the text box, enter "Calendar required to create events"
- Click save

When building an iOS app, the ModifyIOSPlist class will be notified as a post process action, which will then locate the info.plist file and add all the requirement data that has been added.