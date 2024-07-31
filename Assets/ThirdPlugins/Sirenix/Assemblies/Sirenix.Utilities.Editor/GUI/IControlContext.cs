//-----------------------------------------------------------------------
// <copyright file="IControlContext.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.Utilities.Editor
{
    internal interface IControlContext
    {
        int LastRenderedFrameId { get; set; }
    }
}
#endif