//-----------------------------------------------------------------------
// <copyright file="RegisterValidatorAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    using System;

    /// <summary>
    /// Apply this to an assembly to register validators for the validation system.
    /// This enables locating of all relevant validator types very quickly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class RegisterValidatorAttribute : Attribute
    {
        public readonly Type ValidatorType;
        public int Priority;

        public RegisterValidatorAttribute(Type validatorType)
        {
            this.ValidatorType = validatorType;
        }
    }
}
#endif