//-----------------------------------------------------------------------
// <copyright file="DrawerChainResolver.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    public abstract class DrawerChainResolver
    {
        public abstract DrawerChain GetDrawerChain(InspectorProperty property);
    }
}
#endif