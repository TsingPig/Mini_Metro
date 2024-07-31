//-----------------------------------------------------------------------
// <copyright file="Int32Drawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Sirenix.Utilities.Editor;
    using UnityEngine;

    /// <summary>
    /// Int property drawer.
    /// </summary>
    public sealed class Int32Drawer : OdinValueDrawer<int>
    {
        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            this.ValueEntry.SmartValue = SirenixEditorFields.IntField(label, this.ValueEntry.SmartValue);
        }
    }
}
#endif