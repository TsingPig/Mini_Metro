//-----------------------------------------------------------------------
// <copyright file="DrawerLocator.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using System;

    /// <summary>
    /// <para>Utility class for locating and sorting property drawers for the inspector.</para>
    /// <para>See Odin manual section 'Drawers in Depth' for details on how the DrawerLocator determines which drawers to use.</para>
    /// </summary>
    [Obsolete("Use DrawerUtilies and the new DrawerChain feature instead.", true)]
    public static class DrawerLocator
    {
    }
}
#endif