using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Xml;
using UnityEngine;

public class ModifyIOSPlist
{
    /// <summary>
    /// As with iOS 10, builds require a key for each requirement needed and a string describing why added to the info.plist file
    /// This class will add the data to the info.plist as a post process build to add required data
    /// 
    /// see https://developer.apple.com/library/content/documentation/General/Reference/InfoPlistKeyReference/Articles/CocoaKeys.html
    /// for further details on key values
    /// 
    /// </summary>
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            // get our plist
            var plistPath = pathToBuiltProject + "/Info.plist";
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Get plist Root
            var plistRoot = plist.root;

            var requirements = iOSRequirements.ReadXml();

            string message;
            if (!iOSRequirements.IsXmlValid(requirements, out message))
            {
                Debug.LogWarning(message);
                return;
            }
            if (!iOSRequirements.isRequirementDataValid(requirements, out message))
            {
                Debug.LogWarning(message);
            }

            // Add all the requirements that have been specified in Tools/iOS Requirements
            foreach (XmlNode requirementNode in requirements.FirstChild.ChildNodes)
            {
                var requiredNode = requirementNode["Required"];
                if (requiredNode.InnerText == "True")
                {
                    var keyNode = requirementNode["Key"];
                    var valueNode = requirementNode["Value"];
                    plistRoot.SetString(keyNode.InnerText, valueNode.InnerText);
                }
            }
            
            // Write to file
            plist.WriteToFile(plistPath);
        }
    }
}
