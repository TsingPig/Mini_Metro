//-----------------------------------------------------------------------
// <copyright file="CustomEditorLoader.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using UnityEditor;

    [InitializeOnLoad]
    internal static class CustomEditorLoader
    {
        static CustomEditorLoader()
        {
            if (InspectorConfig.HasInstanceLoaded)
            {
                InspectorConfig.Instance.UpdateOdinEditors();
            }
            else
            {
                UnityEditorEventUtility.DelayAction(() => InspectorConfig.Instance.UpdateOdinEditors());
            }
        }
    }
}
#endif