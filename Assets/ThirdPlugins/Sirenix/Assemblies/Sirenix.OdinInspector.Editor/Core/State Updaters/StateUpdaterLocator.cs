//-----------------------------------------------------------------------
// <copyright file="StateUpdaterLocator.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    public abstract class StateUpdaterLocator
    {
        public abstract StateUpdater[] GetStateUpdaters(InspectorProperty property);
    }
}
#endif