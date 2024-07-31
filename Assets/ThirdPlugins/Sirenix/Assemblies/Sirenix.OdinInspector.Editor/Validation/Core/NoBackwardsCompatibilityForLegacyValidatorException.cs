//-----------------------------------------------------------------------
// <copyright file="NoBackwardsCompatibilityForLegacyValidatorException.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    using System;

    internal class NoBackwardsCompatibilityForLegacyValidatorException : Exception
    {
    }
}
#endif