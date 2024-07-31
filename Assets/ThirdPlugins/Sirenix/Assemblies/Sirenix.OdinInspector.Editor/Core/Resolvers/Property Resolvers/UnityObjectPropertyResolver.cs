//-----------------------------------------------------------------------
// <copyright file="UnityObjectPropertyResolver.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using Sirenix.Utilities.Editor;
    using System;

    public class UnityObjectPropertyResolver<T> : OdinPropertyResolver<T>, IMaySupportPrefabModifications
        where T : UnityEngine.Object
    {
        public bool MaySupportPrefabModifications { get { return true; } }

        public override int ChildNameToIndex(string name)
        {
            return -1;
        }

        public override int ChildNameToIndex(ref StringSlice name)
        {
            return -1;
        }

        public override InspectorPropertyInfo GetChildInfo(int childIndex)
        {
            throw new NotSupportedException();
        }

        protected override int GetChildCount(T value)
        {
            return 0;
        }

        public override bool CanResolveForPropertyFilter(InspectorProperty property)
        {
            return property != property.Tree.RootProperty;
        }
    }
}
#endif