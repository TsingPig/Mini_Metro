//-----------------------------------------------------------------------
// <copyright file="RevalidationCriteria.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    public enum RevalidationCriteria
    {
        Always,
        OnValueChange,
        OnValueChangeOrChildValueChange
    }
}
#endif