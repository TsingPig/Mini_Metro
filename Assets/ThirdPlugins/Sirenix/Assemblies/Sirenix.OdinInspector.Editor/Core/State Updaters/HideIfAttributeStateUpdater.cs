//-----------------------------------------------------------------------
// <copyright file="HideIfAttributeStateUpdater.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.RegisterStateUpdater(typeof(Sirenix.OdinInspector.Editor.StateUpdaters.HideIfAttributeStateUpdater))]

namespace Sirenix.OdinInspector.Editor.StateUpdaters
{
    using Sirenix.OdinInspector.Editor.Drawers;

    public sealed class HideIfAttributeStateUpdater : AttributeStateUpdater<HideIfAttribute>
    {
        private IfAttributeHelper helper;

        protected override void Initialize()
        {
            this.helper = new IfAttributeHelper(this.Property, this.Attribute.Condition, false);
            this.ErrorMessage = this.helper.ErrorMessage;
            this.Property.AnimateVisibility = this.Attribute.Animate;
        }

        public override void OnStateUpdate()
        {
            this.Property.State.Visible = !this.helper.GetValue(this.Attribute.Value);
            this.ErrorMessage = this.helper.ErrorMessage;
        }
    }
}
#endif