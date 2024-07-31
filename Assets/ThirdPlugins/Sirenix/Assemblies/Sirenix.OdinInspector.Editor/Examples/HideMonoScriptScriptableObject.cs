//-----------------------------------------------------------------------
// <copyright file="HideMonoScriptScriptableObject.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    using UnityEngine;

    [HideMonoScript]
    public class HideMonoScriptScriptableObject : ScriptableObject
    {
        public string Value;
    }
}
#endif