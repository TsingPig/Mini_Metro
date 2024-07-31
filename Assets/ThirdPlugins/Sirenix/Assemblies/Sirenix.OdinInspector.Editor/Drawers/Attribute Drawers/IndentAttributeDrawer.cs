//-----------------------------------------------------------------------
// <copyright file="IndentAttributeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Utilities.Editor;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Draws properties marked with <see cref="IndentAttribute"/>.
    /// </summary>
    /// <seealso cref="IndentAttribute"/>
    [DrawerPriority(0.5, 0, 0)]
    public sealed class IndentAttributeDrawer : OdinAttributeDrawer<IndentAttribute>
    {
        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUIHelper.PushIndentLevel(EditorGUI.indentLevel + this.Attribute.IndentLevel);
            this.CallNextDrawer(label);
            GUIHelper.PopIndentLevel();
        }
    }
}
#endif