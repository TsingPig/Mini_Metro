//-----------------------------------------------------------------------
// <copyright file="ITemporaryContext.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    /// <summary>
    /// Custom types used by the <see cref="TemporaryPropertyContext{T}"/> can choose to implement the ITemporaryContext
    /// interface in order to be notified when the context gets reset.
    /// </summary>
    public interface ITemporaryContext
    {
        /// <summary>
        /// Called by <see cref="TemporaryPropertyContext{T}"/> when the context gets reset.
        /// </summary>
        void Reset();
    }
}
#endif