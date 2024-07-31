//-----------------------------------------------------------------------
// <copyright file="NamedValueGetter.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.ActionResolvers
{
    public delegate object NamedValueGetter(ref ActionResolverContext context, int selectionIndex);
}
#endif