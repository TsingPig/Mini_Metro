//-----------------------------------------------------------------------
// <copyright file="MyScriptyScriptableObject.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    using UnityEngine;

    [TypeInfoBox("The TypeInfoBox attribute can also be used to display a text at the top of, for example, MonoBehaviours or ScriptableObjects.")]
    public class MyScriptyScriptableObject : ScriptableObject
    {
        public string MyText = ExampleHelper.GetString();
        [TextArea(10, 15)]
        public string Box;
    }
}
#endif