//-----------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using System.Collections.Generic;

    public class SearchResult
    {
        public InspectorProperty MatchedProperty;
        public List<SearchResult> ChildResults = new List<SearchResult>();
    }
}
#endif