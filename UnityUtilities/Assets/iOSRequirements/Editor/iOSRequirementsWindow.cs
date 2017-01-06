using UnityEngine;
using UnityEditor;
using System.Xml;

public class iOSRequirementsWindow : EditorWindow
{
    // The full list of privacy requirments that need to be included in the plist
    // from https://developer.apple.com/library/content/documentation/General/Reference/InfoPlistKeyReference/Articles/CocoaKeys.html

    private XmlDocument _requirementsXml;
    private Vector2 _scrollPosition;

    [MenuItem("Tools/iOS Requirements")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(iOSRequirementsWindow), true, "iOS Requirements Manager", true);
    }

    void OnEnable()
    {
        _requirementsXml = iOSRequirements.ReadXml();
    }

    void OnGUI()
    {
        string message;
        if(!iOSRequirements.IsXmlValid(_requirementsXml, out message))
        {
            GUILayout.Label(message);
            return;
        }

        GUILayout.Label("Select the requirements that are being used, and for each one provide a short description of where it is being used", EditorStyles.wordWrappedLabel);
        GUILayout.Label("As of iOS 10.0, any system requirement request for private information must have a key reference added to the info.plist or apps will fail validation", EditorStyles.wordWrappedLabel);
        GUILayout.Space(10f);

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
        foreach (XmlNode requirementNode in _requirementsXml.FirstChild.ChildNodes)
        {
            GUILayout.BeginHorizontal();

            var keyNode = requirementNode["Key"];
            var valueNode = requirementNode["Value"];
            var requiredNode = requirementNode["Required"];


            requiredNode.InnerText = GUILayout.Toggle(requiredNode.InnerText == "True", keyNode.InnerText/*, EditorStyles.boldLabel*/) 
                ? "True" 
                : "False";

            GUILayout.EndHorizontal();

            if (requiredNode.InnerText == "True")
            {
               GUILayout.BeginVertical();

                // If the requirement is needed we must add a description
                GUILayout.Label("Description:");
                valueNode.InnerText = GUILayout.TextField(valueNode.InnerText ?? "");

                GUILayout.EndVertical();
            }
            GUILayout.Space(10);
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("Save"))
        {
            iOSRequirements.WriteToXml(_requirementsXml);
            if (!iOSRequirements.isRequirementDataValid(_requirementsXml, out message))
            {
                EditorUtility.DisplayDialog("Warning", message, "Ok");
                Debug.Log(message);
            }

        }   
    }
}
