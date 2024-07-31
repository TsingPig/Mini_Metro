//-----------------------------------------------------------------------
// <copyright file="EnableGUIAttributeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Utilities.Editor;
    using UnityEngine;

    /// <summary>
    /// Draws members marked with <see cref="EnableGUIAttribute"/>.
    /// </summary>
    [DrawerPriority(2, 0, 0)]
    public sealed class EnableGUIAttributeDrawer : OdinAttributeDrawer<EnableGUIAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUIHelper.PushGUIEnabled(true);
            this.CallNextDrawer(label);
            GUIHelper.PopGUIEnabled();
        }
    }
}
#endif