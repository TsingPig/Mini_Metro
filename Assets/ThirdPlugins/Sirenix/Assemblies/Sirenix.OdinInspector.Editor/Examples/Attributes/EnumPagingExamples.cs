//-----------------------------------------------------------------------
// <copyright file="EnumPagingExamples.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    [AttributeExample(typeof(EnumPagingAttribute))]
    internal class EnumPagingExamples
    {
        [EnumPaging]
        public SomeEnum SomeEnumField;
        
        public enum SomeEnum
        {
            A, B, C
        }
    }
}
#endif