//-----------------------------------------------------------------------
// <copyright file="RangeValidator.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
[assembly: Sirenix.OdinInspector.Editor.Validation.RegisterValidator(typeof(Sirenix.OdinInspector.Editor.Validation.RangeValidator<>))]
[assembly: Sirenix.OdinInspector.Editor.Validation.RegisterValidator(typeof(Sirenix.OdinInspector.Editor.Validation.PropertyRangeValidator<>))]

namespace Sirenix.OdinInspector.Editor.Validation
{
    using Sirenix.OdinInspector.Editor.ValueResolvers;
    using System;
    using UnityEngine;

    public class RangeValidator<T> : AttributeValidator<RangeAttribute, T> where T : struct
    {
        private static readonly bool IsNumber = GenericNumberUtility.IsNumber(typeof(T));
        private static readonly bool IsVector = GenericNumberUtility.IsVector(typeof(T));

        public override bool CanValidateProperty(InspectorProperty property)
        {
            return IsNumber || IsVector;
        }

        protected override void Validate(ValidationResult result)
        {
            if (!GenericNumberUtility.NumberIsInRange(this.ValueEntry.SmartValue, Math.Min(this.Attribute.min, this.Attribute.max), Math.Max(this.Attribute.min, this.Attribute.max)))
            {
                result.Message = "Number is not in range.";
                result.ResultType = ValidationResultType.Error;
            }
        }
    }

    public class PropertyRangeValidator<T> : AttributeValidator<PropertyRangeAttribute, T>
        where T : struct
    {
        private static readonly bool IsNumber = GenericNumberUtility.IsNumber(typeof(T));
        private static readonly bool IsVector = GenericNumberUtility.IsVector(typeof(T));

        private ValueResolver<double> minValueGetter;
        private ValueResolver<double> maxValueGetter;

        public override bool CanValidateProperty(InspectorProperty property)
        {
            return IsNumber || IsVector;
        }

        protected override void Initialize()
        {
            this.minValueGetter = ValueResolver.Get<double>(this.Property, this.Attribute.MinGetter, this.Attribute.Min);
            this.maxValueGetter = ValueResolver.Get<double>(this.Property, this.Attribute.MaxGetter, this.Attribute.Max);
        }

        protected override void Validate(ValidationResult result)
        {
            if (this.minValueGetter.HasError || this.maxValueGetter.HasError)
            {
                result.Message = ValueResolver.GetCombinedErrors(this.minValueGetter, this.maxValueGetter);
                result.ResultType = ValidationResultType.Error;
                return;
            }

            var min = this.minValueGetter.GetValue();
            var max = this.maxValueGetter.GetValue();

            if (!GenericNumberUtility.NumberIsInRange(this.ValueEntry.SmartValue, Math.Min(min, max), Math.Max(min, max)))
            {
                result.Message = "Number is not in range.";
                result.ResultType = ValidationResultType.Error;
            }
        }
    }
}
#endif