using PlayGen.Unity.Utilities.UI;
using System.Linq;
using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlayGen.Unity.Utilities.Editor.UI
{
    public class SetPlatformPositioning : MonoBehaviour
    {
        [MenuItem("PlayGen Tools/Set Positioning/Standalone")]
        public static void SetStandalone()
        {
            SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<PlatformPositioning>(true)).ToList().ForEach(p => p.SetPosition(true));
        }

        [MenuItem("PlayGen Tools/Set Positioning/Mobile")]
        public static void SetMobile()
        {
            SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(r => r.GetComponentsInChildren<PlatformPositioning>(true)).ToList().ForEach(p => p.SetPosition(true, true));
        }
    }
}