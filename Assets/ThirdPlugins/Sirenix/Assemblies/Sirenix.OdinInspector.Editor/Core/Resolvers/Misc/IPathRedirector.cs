//-----------------------------------------------------------------------
// <copyright file="IPathRedirector.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    public interface IPathRedirector
    {
        bool TryGetRedirectedProperty(string childName, out InspectorProperty property);
    }
}
#endif