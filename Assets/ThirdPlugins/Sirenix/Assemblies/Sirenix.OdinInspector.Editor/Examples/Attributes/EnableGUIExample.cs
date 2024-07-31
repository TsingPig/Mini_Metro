//-----------------------------------------------------------------------
// <copyright file="EnableGUIExample.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    [AttributeExample(typeof(EnableGUIAttribute))]
    internal class EnableGUIExample
    {
        [ShowInInspector]
        public int GUIDisabledProperty { get { return 10; } }

        [ShowInInspector, EnableGUI]
        public int GUIEnabledProperty { get { return 10; } }
    }
}
#endif