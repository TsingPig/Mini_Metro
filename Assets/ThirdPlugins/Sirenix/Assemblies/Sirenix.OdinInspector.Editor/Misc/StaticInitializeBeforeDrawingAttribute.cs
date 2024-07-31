//-----------------------------------------------------------------------
// <copyright file="StaticInitializeBeforeDrawingAttribute.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly)]
    public class StaticInitializeBeforeDrawingAttribute : Attribute
    {
        public StaticInitializeBeforeDrawingAttribute(params Type[] types)
        {
            this.Types = types;
        }

        public Type[] Types;
    }
}
#endif