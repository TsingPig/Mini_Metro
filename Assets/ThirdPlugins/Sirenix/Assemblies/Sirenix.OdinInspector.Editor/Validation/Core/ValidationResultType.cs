//-----------------------------------------------------------------------
// <copyright file="ValidationResultType.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    public enum ValidationResultType
    {
        Valid,
        Error,
        Warning,
        IgnoreResult
    }
}
#endif