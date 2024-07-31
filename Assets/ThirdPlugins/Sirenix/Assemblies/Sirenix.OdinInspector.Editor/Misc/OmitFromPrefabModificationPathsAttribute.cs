//-----------------------------------------------------------------------
// <copyright file="OmitFromPrefabModificationPathsAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    internal sealed class OmitFromPrefabModificationPathsAttribute : Attribute
    {
    }
}
#endif