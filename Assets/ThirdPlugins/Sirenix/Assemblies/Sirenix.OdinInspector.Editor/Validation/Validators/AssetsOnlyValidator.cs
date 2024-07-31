//-----------------------------------------------------------------------
// <copyright file="AssetsOnlyValidator.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.Validation.RegisterValidator(typeof(Sirenix.OdinInspector.Editor.Validation.AssetsOnlyValidator<>))]

namespace Sirenix.OdinInspector.Editor.Validation
{
    using Sirenix.OdinInspector;
    using Sirenix.Utilities;
    using UnityEditor;
    using UnityEngine;

    public class AssetsOnlyValidator<T> : AttributeValidator<AssetsOnlyAttribute, T>
        where T : UnityEngine.Object
    {
        protected override void Validate(ValidationResult result)
        {
            var value = this.ValueEntry.SmartValue;

            if (value != null && !AssetDatabase.Contains(value))
            {
                string name = value.name;
                var component = value as Component;
                if (component != null)
                {
                    name = "from " + component.gameObject.name;
                }

                result.ResultType = ValidationResultType.Error;
                result.Message = (value as object).GetType().GetNiceName() + " " + name + " is not an asset.";
            }
        }
    }
    
}
#endif