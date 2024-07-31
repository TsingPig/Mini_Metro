//-----------------------------------------------------------------------
// <copyright file="HideInPlayModeAttributeStateUpdater.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.RegisterStateUpdater(typeof(Sirenix.OdinInspector.Editor.StateUpdaters.HideInPlayModeAttributeStateUpdater))]

namespace Sirenix.OdinInspector.Editor.StateUpdaters
{
    using UnityEngine;

    public sealed class HideInPlayModeAttributeStateUpdater : AttributeStateUpdater<HideInPlayModeAttribute>
    {
        public override void OnStateUpdate()
        {
            this.Property.State.Visible = !Application.isPlaying;
        }
    }
}
#endif