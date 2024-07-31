//-----------------------------------------------------------------------
// <copyright file="AtomContainerAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.AtomContainer]

namespace Sirenix.OdinInspector.Editor
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly)]
    public class AtomContainerAttribute : Attribute
    {
    }
}
#endif