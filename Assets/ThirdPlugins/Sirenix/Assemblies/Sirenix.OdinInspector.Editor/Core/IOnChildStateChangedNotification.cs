//-----------------------------------------------------------------------
// <copyright file="IOnChildStateChangedNotification.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    public interface IOnChildStateChangedNotification
    {
        void OnChildStateChanged(int childIndex, string state);
    }
}
#endif