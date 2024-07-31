//-----------------------------------------------------------------------
// <copyright file="DisabledInInlineEditorScriptableObject.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    using UnityEngine;

    public class DisabledInInlineEditorScriptableObject : ScriptableObject
    {
        public string AlwaysEnabled;

        [DisableInInlineEditors]
        public string DisabledInInlineEditor;
    }
}
#endif