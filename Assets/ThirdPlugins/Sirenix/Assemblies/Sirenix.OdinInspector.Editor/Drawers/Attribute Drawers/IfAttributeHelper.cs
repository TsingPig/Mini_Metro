//-----------------------------------------------------------------------
// <copyright file="IfAttributeHelper.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Sirenix.OdinInspector.Editor.ValueResolvers;
    using System;

    public class IfAttributeHelper
    {
        private readonly ValueResolver<object> valueResolver;

        private bool result;

        public bool DefaultResult;

        //使用enum flags contains 来确定结果
        public bool flagsRule;

        public string ErrorMessage { get; private set; }

        public IfAttributeHelper(InspectorProperty property, string memberName, bool defaultResult = false, bool flagsRule = false)
        {
            this.valueResolver = ValueResolver.Get<object>(property, memberName);
            this.ErrorMessage = this.valueResolver.ErrorMessage;
            this.DefaultResult = defaultResult;
            this.flagsRule = flagsRule;
        }

        public bool GetValue(object value)
        {
            if (this.ErrorMessage == null)
            {
                this.result = false;
                object resolvedValue = this.valueResolver.GetValue();

                if (resolvedValue is UnityEngine.Object)
                {
                    // Unity objects can be 'fake null', and to detect that we have to test the value as a Unity object.
                    this.result = ((UnityEngine.Object)resolvedValue) != null;
                }
                else if (resolvedValue is bool)
                {
                    this.result = (bool)resolvedValue;
                }
                else if (resolvedValue is string)
                {
                    this.result = string.IsNullOrEmpty((string)resolvedValue) == false;
                }
                else if (flagsRule && resolvedValue is Enum && value is Enum)
                {
                    this.result = ((byte)resolvedValue & (byte)value) > 0;
                }
                else if (value == null)
                {
                    if (resolvedValue != null)
                    {
                        this.result = true;
                    }
                }
                else if (Equals(resolvedValue, value))
                {
                    this.result = true;
                }

                return this.result;
            }

            return this.DefaultResult;
        }
    }
}
#endif