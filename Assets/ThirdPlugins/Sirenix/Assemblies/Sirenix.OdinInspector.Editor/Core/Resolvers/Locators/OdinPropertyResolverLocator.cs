//-----------------------------------------------------------------------
// <copyright file="OdinPropertyResolverLocator.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    /// <summary>
    /// Base class for locator of <see cref="OdinPropertyResolver"/>. Use <see cref="DefaultOdinPropertyResolverLocator"/> for default implementation.
    /// </summary>
    public abstract class OdinPropertyResolverLocator
    {
        /// <summary>
        /// Gets an <see cref="OdinPropertyResolver"/> instance for the specified property.
        /// </summary>
        /// <param name="property">The property to get an <see cref="OdinPropertyResolver"/> instance for.</param>
        /// <returns>An instance of <see cref="OdinPropertyResolver"/> to resolver the specified property.</returns>
        public abstract OdinPropertyResolver GetResolver(InspectorProperty property);
    }
}
#endif