//-----------------------------------------------------------------------
// <copyright file="EnableIfAttributeStateUpdater.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.RegisterStateUpdater(typeof(Sirenix.OdinInspector.Editor.StateUpdaters.EnableIfAttributeStateUpdater))]

namespace Sirenix.OdinInspector.Editor.StateUpdaters
{
    using Sirenix.OdinInspector.Editor.Drawers;

    public sealed class EnableIfAttributeStateUpdater : AttributeStateUpdater<EnableIfAttribute>
    {
        private IfAttributeHelper helper;

        protected override void Initialize()
        {
            this.helper = new IfAttributeHelper(this.Property, this.Attribute.Condition, true);
            this.ErrorMessage = this.helper.ErrorMessage;
        }

        public override void OnStateUpdate()
        {
            this.Property.State.Enabled = this.helper.GetValue(this.Attribute.Value);
            this.ErrorMessage = this.helper.ErrorMessage;
        }
    }
}
#endif