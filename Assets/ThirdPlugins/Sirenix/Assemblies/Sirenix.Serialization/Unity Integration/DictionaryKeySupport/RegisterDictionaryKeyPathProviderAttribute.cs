//-----------------------------------------------------------------------
// <copyright file="RegisterDictionaryKeyPathProviderAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Sirenix.Serialization
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RegisterDictionaryKeyPathProviderAttribute : Attribute
    {
        public readonly Type ProviderType;

        public RegisterDictionaryKeyPathProviderAttribute(Type providerType)
        {
            this.ProviderType = providerType;
        }
    }
}