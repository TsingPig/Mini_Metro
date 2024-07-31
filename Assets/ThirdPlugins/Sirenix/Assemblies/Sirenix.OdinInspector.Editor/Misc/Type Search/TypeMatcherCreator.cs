//-----------------------------------------------------------------------
// <copyright file="TypeMatcherCreator.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.TypeSearch
{
    public abstract class TypeMatcherCreator
    {
        public abstract bool TryCreateMatcher(TypeSearchInfo info, out TypeMatcher matcher);
    }
}
#endif