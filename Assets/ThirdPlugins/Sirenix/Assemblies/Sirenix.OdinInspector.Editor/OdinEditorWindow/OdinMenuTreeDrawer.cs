//-----------------------------------------------------------------------
// <copyright file="OdinMenuTreeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using UnityEngine;

    internal class OdinMenuTreeDrawer : OdinValueDrawer<OdinMenuTree>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var entry = this.ValueEntry;
            var tree = entry.SmartValue;
            if (tree != null)
            {
                tree.DrawMenuTree();
                tree.HandleKeyboardMenuNavigation();
            }
        }
    }
}
#endif