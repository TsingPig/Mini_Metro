//-----------------------------------------------------------------------
// <copyright file="OnInspectorInitAndDisposeMethodDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using UnityEngine;

    public class OnInspectorInitAndDisposeMethodDrawer : MethodDrawer
    {
        protected override bool CanDrawMethodProperty(InspectorProperty property)
        {
            var attrs = property.Attributes;

            return attrs.HasAttribute<OnInspectorDisposeAttribute>()
                || attrs.HasAttribute<OnInspectorInitAttribute>();
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Draw nothing
        }
    }
}
#endif