# Feedback Panel
## Function 
A customisable panel for getting users to send feedback in game to a specified email using ElasticEmail.
## Usage
**Note** This utility currently requires an elastic email account, other email sending APIs will likely work as well

An example scene has been provided with this asset that outlines setup and changes required to get the asset ready for your game. Simply edit ElasticEmailClient.cs in the PlayGen.Unity.Utilities.FeedbackPanel Solution and replace the following with your own details.

Variable | Description
--- | ---
ApiKey | You can find your api key at ElasticEmail -> Account -> Settings -> API Configuration 
FromEmail | The email address that will be desplayed as the sender
FromName | The name of the sender
ToEmail | The email address that you wish to send to

There is an example of how to use the tool [Here](FeedbackExampleUsage.cs)

Setup for this asset is within the PlayGen.Unity.Utilities.FeedbackPanel Solution. By default it uses generic details which needs to be replaced by setting ElasticEmailClient.Details. Once rebuilt, the prefab can be added to the project and visuals edited as required.