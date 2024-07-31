//-----------------------------------------------------------------------
// <copyright file="GuidDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Sirenix.Utilities.Editor;
    using System;
    using UnityEngine;

    /// <summary>
    /// Int property drawer.
    /// </summary>
    public sealed class GuidDrawer : OdinValueDrawer<Guid>
    {
        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var entry = this.ValueEntry;
            entry.SmartValue = SirenixEditorFields.GuidField(label, entry.SmartValue);
        }
    }
}
#endif