//-----------------------------------------------------------------------
// <copyright file="HideInTablesExample.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Examples
{
    using Sirenix.OdinInspector.Editor.Examples.Internal;
    using System;
    using System.Collections.Generic;

    [AttributeExample(typeof(HideInTablesAttribute))]
	[ExampleAsComponentData(Namespaces = new string[] { "System", "System.Collections.Generic" })]
    internal class HideInTablesExample
    {
        public MyItem Item = new MyItem();

        [TableList]
        public List<MyItem> Table = new List<MyItem>()
        {
            new MyItem(),
            new MyItem(),
            new MyItem(),
        };

        [Serializable]
        public class MyItem
        {
            public string A;

            public int B;

            [HideInTables]
            public int Hidden;
        }
    }
}
#endif