#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using System;
    using UnityEditor;
    using UnityEngine;

#if !ODIN_TRIAL && !ODIN_EDUCATIONAL

#if !ODIN_TRIAL && !ODIN_ENTERPRISE && SIRENIX_INTERNAL
    internal static class INTERNAL_RemoveEULAConsent
    {
        [MenuItem("Sirenix/Utilities/Remove EULA Consent")]
        private static void RemoveEULAConsent()
        {
            EditorPrefs.SetBool(AcceptEULAWindow.HAS_ACCEPTED_EULA_PREFS_KEY, false);
        }
    }
#endif
#endif
}
#endif