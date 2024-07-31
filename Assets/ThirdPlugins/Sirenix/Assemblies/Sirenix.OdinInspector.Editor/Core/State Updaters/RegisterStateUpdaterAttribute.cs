//-----------------------------------------------------------------------
// <copyright file="RegisterStateUpdaterAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class RegisterStateUpdaterAttribute : Attribute
    {
        public readonly Type Type;
        public readonly double Priority;

        public RegisterStateUpdaterAttribute(Type type, double priority = 0)
        {
            this.Type = type;
            this.Priority = priority;
        }
    }
}
#endif