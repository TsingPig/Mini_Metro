//-----------------------------------------------------------------------
// <copyright file="ValidationKind.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    using System;

    [Obsolete("There is no longer any strict distinction between value and member validation.", false)]
    public enum ValidationKind
    {
        Value,
        Member
    }
}
#endif