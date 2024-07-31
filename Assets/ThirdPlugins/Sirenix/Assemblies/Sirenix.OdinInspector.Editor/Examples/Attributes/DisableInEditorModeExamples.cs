//-----------------------------------------------------------------------
// <copyright file="DisableInEditorModeExamples.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    using UnityEngine;

    [AttributeExample(typeof(DisableInEditorModeAttribute))]
    internal class DisableInEditorModeExamples
    {
        [Title("Disabled in edit mode")]
        [DisableInEditorMode]
        public GameObject A;

        [DisableInEditorMode]
        public Material B;
    }
}
#endif