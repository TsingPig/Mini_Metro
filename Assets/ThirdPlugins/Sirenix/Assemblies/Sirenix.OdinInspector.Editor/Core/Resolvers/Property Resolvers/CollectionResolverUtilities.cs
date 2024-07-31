//-----------------------------------------------------------------------
// <copyright file="CollectionResolverUtilities.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;

namespace Sirenix.OdinInspector.Editor
{
    public static class CollectionResolverUtilities
    {
        public static string DefaultIndexToChildName(int index)
        {
            return "$" + index;
        }

        public static int DefaultChildNameToIndex(ref StringSlice name)
        {
            if (name.Length <= 1) return -1;

            int index;

            StringSlice numberSlice;
            if (name[0] == '$')
            {
                numberSlice = name.Slice(1);
            }
            else if (name.Length > 2 && name[0] == '[' && name[name.Length - 1] == ']')
            {
                numberSlice = name.Slice(1, name.Length - 2);
            }
            else return -1;

            if (numberSlice.TryParseToInt(out index) && index >= 0)
            {
                return index;
            }

            return -1;
        }

        public static int DefaultChildNameToIndex(string name)
        {
            StringSlice slice = name;
            return DefaultChildNameToIndex(ref slice);
        }
    }
}
#endif