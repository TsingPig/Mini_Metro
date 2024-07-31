//-----------------------------------------------------------------------
// <copyright file="ActivationModes.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Licensing
{
    public enum ActivationModes
    {
        Activate,
        [LabelText("I don't use Odin")]
        IDontUseOdin,
    }
}
#endif