Send Feeback Asset using Elastic Email

Updated 11-01-2017
By Felix Wentworth

To get the asset working, open the example scene "SendFeedbackExample.unity" and open the ElasticEmailClient class and sent the following values
- ApiKey		Find the api key at, ElasticEmail -> Account -> Settings -> Api configuration
- FromEmail		The email address that will be displayed as the sender
- FromName		The name of the sender
- ToEmail		The email address that you wish to send to

Once this data has been added, save the class and run the scene, you will be able to see a simple feedback screen that takes text input and has options to send and cancel.
By entering text into the input field and pressing send, the email should send to the address specified in the Client

To see how this works, see FeedbackExampleUsage.cs.