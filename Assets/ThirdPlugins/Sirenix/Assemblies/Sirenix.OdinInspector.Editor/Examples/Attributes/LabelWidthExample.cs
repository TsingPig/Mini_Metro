//-----------------------------------------------------------------------
// <copyright file="LabelWidthExample.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    [AttributeExample(typeof(LabelWidthAttribute), "Change the width of the label for your property.")]
    internal class LabelWidthExample
    {
        public int DefaultWidth;

        [LabelWidth(50)]
        public int Thin;

        [LabelWidth(250)]
        public int Wide;
    }
}
#endif