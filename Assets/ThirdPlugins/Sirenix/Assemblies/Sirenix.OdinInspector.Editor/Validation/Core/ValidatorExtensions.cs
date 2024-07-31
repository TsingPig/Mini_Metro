//-----------------------------------------------------------------------
// <copyright file="ValidatorExtensions.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    using Sirenix.OdinInspector;

    public static class ValidatorExtensions
    {
        public static ValidationResultType ToValidationResultType(this InfoMessageType messageType)
        {
            if (messageType == InfoMessageType.Error)
                return ValidationResultType.Error;
            else if (messageType == InfoMessageType.Warning)
                return ValidationResultType.Warning;
            else return ValidationResultType.Valid;
        }
    }
}
#endif