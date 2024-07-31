//-----------------------------------------------------------------------
// <copyright file="IMemberSelector.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMemberSelector
    {
        IList<MemberInfo> SelectMembers(Type type);
    }
}
#endif