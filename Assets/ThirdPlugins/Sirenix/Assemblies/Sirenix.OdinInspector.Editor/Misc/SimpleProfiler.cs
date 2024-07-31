//-----------------------------------------------------------------------
// <copyright file="SimpleProfiler.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using System;
    using UnityEngine;

    internal struct SimpleProfiler : IDisposable
    {
        public string Name;
        public System.Diagnostics.Stopwatch Watch;

        public static SimpleProfiler Section(string name)
        {
            return new SimpleProfiler()
            {
                //Name = name,
                //Watch = System.Diagnostics.Stopwatch.StartNew()
            };
        }

        public void Dispose()
        {
            //this.Watch.Stop();
            //Debug.Log(this.Name + " took " + this.Watch.Elapsed.TotalMilliseconds + "ms");
        }
    }
}
#endif