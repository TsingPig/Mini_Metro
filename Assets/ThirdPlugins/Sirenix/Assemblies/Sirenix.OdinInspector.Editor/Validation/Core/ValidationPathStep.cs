//-----------------------------------------------------------------------
// <copyright file="ValidationPathStep.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Validation
{
    using System.Reflection;

    public struct ValidationPathStep
    {
        public string StepString;
        public object Value;
        public MemberInfo Member;
    }
}
#endif