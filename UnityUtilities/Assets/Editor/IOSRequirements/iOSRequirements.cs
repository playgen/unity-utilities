using System;
using UnityEngine;
using System.Xml;

public class iOSRequirements : MonoBehaviour
{
    private static string FilePath
    {
        get { return Application.dataPath + "/Editor/IOSRequirements/Requirements.xml"; }
    }

    public static XmlDocument ReadXml()
    {
        var text = System.IO.File.ReadAllText(FilePath);

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(text);

        return xmlDoc;
    }

    public static void WriteToXml(XmlDocument requirements)
    {
        requirements.Save(FilePath);
        Debug.Log(string.Format("Saved changes to {0}", FilePath));
    }
    /// <summary>
    /// Check that the xml data is valid
    /// Each node contains a Key, Value and Required element
    /// </summary>
    /// <param name="requirementsXml"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool IsXmlValid(XmlDocument requirementsXml, out string message)
    {
        message = null;

        foreach (XmlNode childNode in requirementsXml.FirstChild.ChildNodes)
        {
            if (childNode["Key"] == null || childNode["Value"] == null || childNode["Required"] == null)
            {
                message = "Xml structure is invalid.";
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// Check if requirements that are enabled have a description
    /// </summary>
    /// <param name="requirementsXml"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool isRequirementDataValid(XmlDocument requirementsXml, out string message)
    {
        message = null;

        foreach (XmlNode childNode in requirementsXml.FirstChild.ChildNodes)
        {
            if (childNode["Required"].InnerText == "True" && string.IsNullOrEmpty(childNode["Value"].InnerText))
            {
                message = "All enabled requiremnents must have a description!";
                return false;
            }
        }
        return true;
    }
}
