//-----------------------------------------------------------------------
// <copyright file="HideInEditorModeAttributeStateUpdater.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.RegisterStateUpdater(typeof(Sirenix.OdinInspector.Editor.StateUpdaters.HideInEditorModeAttributeStateUpdater))]

namespace Sirenix.OdinInspector.Editor.StateUpdaters
{
    using UnityEngine;

    public sealed class HideInEditorModeAttributeStateUpdater : AttributeStateUpdater<HideInEditorModeAttribute>
    {
        public override void OnStateUpdate()
        {
            this.Property.State.Visible = Application.isPlaying;
        }
    }
}
#endif