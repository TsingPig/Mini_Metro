//-----------------------------------------------------------------------
// <copyright file="TypeSearchInfo.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.TypeSearch
{
    using System;

    public struct TypeSearchInfo
    {
        public Type MatchType;
        public Type[] Targets;
        public double Priority;
    }
}
#endif