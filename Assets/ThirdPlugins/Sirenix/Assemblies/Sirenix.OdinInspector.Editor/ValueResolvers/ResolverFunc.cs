//-----------------------------------------------------------------------
// <copyright file="ResolverFunc.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.ValueResolvers
{
    using System;

    public delegate TResult ValueResolverFunc<TResult>(ref ValueResolverContext context, int selectionIndex);
}
#endif