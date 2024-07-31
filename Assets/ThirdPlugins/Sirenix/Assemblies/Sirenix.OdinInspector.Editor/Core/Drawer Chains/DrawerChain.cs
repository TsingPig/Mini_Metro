//-----------------------------------------------------------------------
// <copyright file="DrawerChain.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public abstract class DrawerChain : IEnumerator<OdinDrawer>, IEnumerable<OdinDrawer>
    {
        public DrawerChain(InspectorProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            this.Property = property;
        }

        public InspectorProperty Property { get; private set; }

        public abstract OdinDrawer Current { get; }

        object IEnumerator.Current { get { return this.Current; } }

        public abstract bool MoveNext();

        public abstract void Reset();

        void IDisposable.Dispose()
        {
            this.Reset();
        }

        public IEnumerator<OdinDrawer> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
#endif