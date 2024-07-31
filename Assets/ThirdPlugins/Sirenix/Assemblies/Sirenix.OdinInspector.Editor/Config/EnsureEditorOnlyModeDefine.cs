//-----------------------------------------------------------------------
// <copyright file="EnsureEditorOnlyModeDefine.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using Sirenix.Utilities;
    using System;
    using System.Linq;
    using UnityEditor;

    internal static class EnsureEditorOnlyModeDefine
    {
        [InitializeOnLoadMethod]
        private static void EnsureScriptingDefineSymbol()
        {
            var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;

            if (currentTarget == BuildTargetGroup.Unknown)
            {
                return;
            }

            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
            var defines = definesString.Split(';');

            bool changed = false;

            var define = "ODIN_INSPECTOR_EDITOR_ONLY";
            var isEditorOnlyMode = EditorOnlyModeConfig.Instance.IsEditorOnlyModeEnabled();

            if (isEditorOnlyMode)
            {
                if (!defines.Contains(define))
                {
                    if (definesString.EndsWith(";", StringComparison.InvariantCulture) == false)
                    {
                        definesString += ";";
                    }

                    definesString += define;
                    changed = true;
                }
            }
            else
            {
                for (int i = 0; i < defines.Length; i++)
                {
                    if (defines[i] == define)
                    {
                        var list = defines.ToList();
                        list.RemoveAt(i);
                        definesString = string.Join(";", list.ToArray());
                        changed = true;
                        break;
                    }
                }
            }

            if (changed)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
            }
        }
    }
}
#endif