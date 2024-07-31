//-----------------------------------------------------------------------
// <copyright file="TypeMatcher.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.TypeSearch
{
    using System;

    public abstract class TypeMatcher
    {
        public abstract string Name { get; }
        public abstract Type Match(Type[] targets, ref bool stopMatching);
    }
}
#endif