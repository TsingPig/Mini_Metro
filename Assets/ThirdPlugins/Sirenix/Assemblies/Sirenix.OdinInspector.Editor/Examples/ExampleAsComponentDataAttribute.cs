//-----------------------------------------------------------------------
// <copyright file="ExampleAsComponentDataAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples.Internal
{
    using System;

    public class ExampleAsComponentDataAttribute : Attribute
    {
        public string[] AttributeDeclarations;
        public string[] Namespaces;
    }
}
#endif