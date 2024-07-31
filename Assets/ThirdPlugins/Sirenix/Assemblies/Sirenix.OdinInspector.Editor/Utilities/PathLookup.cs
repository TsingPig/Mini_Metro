//-----------------------------------------------------------------------
// <copyright file="PathLookup.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Internal
{
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using System;
    using System.Collections.Generic;

    internal class PathLookup<T> where T : class
    {
        public int Version;
        public Node Root = new Node();
        public int Count;
        public T[] Values = new T[32];
        public int CleanUpAtMostEveryXVersions = 20;
        public int VersionsSinceLastCleanUp = 0;

        public bool IsRebuilding;
        public int NodesUpdatedToLatestVersion;
        public bool HasUnUpdatedNodesSinceLastCleanUp;

        public void BeginRebuild()
        {
            if (this.IsRebuilding)
            {
                throw new Exception("PathLookup is already rebuilding");
            }

            this.IsRebuilding = true;

            unchecked
            {
                this.Version++;
                this.Root.Version = this.Version;
                this.Root.ValueIndex = -1;
            }

            var values = this.Values;
            var count = this.Count;

            for (int i = 0; i < count; i++)
            {
                values[i] = null;
            }

            this.Count = 0;
            this.NodesUpdatedToLatestVersion = 0;
        }

        public void FinishRebuild()
        {
            if (!this.IsRebuilding)
            {
                throw new Exception("PathLookup is not rebuilding");
            }

            this.IsRebuilding = false;
            this.VersionsSinceLastCleanUp++;

            if (this.NodesUpdatedToLatestVersion != this.Count)
            {
                this.HasUnUpdatedNodesSinceLastCleanUp = true;
            }

            if (this.HasUnUpdatedNodesSinceLastCleanUp && this.VersionsSinceLastCleanUp >= this.CleanUpAtMostEveryXVersions)
            {
                this.HasUnUpdatedNodesSinceLastCleanUp = false;
                this.VersionsSinceLastCleanUp = 0;
                this.CleanUp();
            }
        }

        public void CleanUp()
        {
            // Maybe we need to shrink the values array if the number of mods has gone way down?
            {
                int newLength = this.Values.Length;

                while (this.Count * 3 < newLength)
                {
                    newLength /= 2;
                }

                if (newLength < this.Count)
                {
                    newLength = this.Count;
                }

                if (newLength != this.Values.Length)
                {
                    Array.Resize(ref this.Values, newLength);
                }
            }

            this.Root.Cleanup(this.Version);
        }

        public bool TryGetValue(StringSlice path, out StringSlice nearestPath, out bool childValuesExistForValue, out T value)
        {
            Node current = this.Root;
            StringSlice remainingPathSlice = path;

            nearestPath = default(StringSlice);
            value = null;
            childValuesExistForValue = false;

            while (true)
            {
                if (current.Version != this.Version || current.Children == null)
                {
                    nearestPath = remainingPathSlice.Index == path.Index ? string.Empty : path.Slice(0, (remainingPathSlice.Index - path.Index) - 1);
                    return false;
                }

                var nextSeparator = remainingPathSlice.FirstIndexOf('.');
                var step = nextSeparator == -1 ? remainingPathSlice : remainingPathSlice.Slice(0, nextSeparator);

                Node child;

                if (!current.Children.TryGetValue(step, out child))
                {
                    //childValuesExistForValue = current.ValuesExistForChildren;
                    nearestPath = step.Index == path.Index ? string.Empty : path.Slice(0, (step.Index - path.Index) - 1);
                    return false;
                }

                if (nextSeparator == -1)
                {
                    childValuesExistForValue = child.ValuesExistForChildren;

                    if (child.ValueIndex == -1)
                    { 
                        nearestPath = step.Index == path.Index ? string.Empty : path.Slice(0, (step.Index - path.Index) - 1);
                        return false;
                    }

                    value = this.Values[child.ValueIndex];
                    return true;
                }

                current = child;
                remainingPathSlice = remainingPathSlice.Slice(nextSeparator + 1);
            }
        }

        public void AddValue(StringSlice path, T value)
        {
            if (!this.IsRebuilding) throw new Exception("Cannot add values to a PathLookup while it is not rebuilding");

            var valueIndex = this.Count;

            while (valueIndex >= this.Values.Length)
            {
                var values = this.Values;
                var newValues = new T[Math.Max(values.Length, 4) * 2];
                for (int i = 0; i < values.Length; i++)
                {
                    newValues[i] = values[i];
                }
                this.Values = newValues;
            }

            this.Values[valueIndex] = value;
            this.Count++;

            Node current = this.Root;
            StringSlice remainingPathSlice = path;

            while (true)
            {
                if (current.Version != this.Version)
                {
                    current.Version = this.Version;
                    current.ValueIndex = -1;
                }

                current.ValuesExistForChildren = true;
                var nextSeparator = remainingPathSlice.FirstIndexOf('.');

                if (current.Children == null)
                {
                    current.Children = new Dictionary<StringSlice, Node>(StringSliceEqualityComparer.Instance);
                }

                var step = nextSeparator == -1 ? remainingPathSlice : remainingPathSlice.Slice(0, nextSeparator);

                Node child;

                if (!current.Children.TryGetValue(step, out child))
                {
                    child = new Node();
                    current.Children.Add(step, child);
                }

                if (child.Version != this.Version)
                {
                    child.Version = this.Version;
                    child.ValueIndex = -1;
                    child.ValuesExistForChildren = false;
                }

                if (nextSeparator == -1)
                {
                    child.ValueIndex = valueIndex;
                    break;
                }
                else
                {
                    current = child;
                    remainingPathSlice = remainingPathSlice.Slice(nextSeparator + 1);
                }
            }
        }

        public class Node
        {
            public int Version;
            public Dictionary<StringSlice, Node> Children;
            public int ValueIndex = -1;
            public bool ValuesExistForChildren;

            private static StringSlice[] childrenToRemove = new StringSlice[2];
            private static int childrenToRemoveCount;

            public void Cleanup(int version)
            {
                if (this.Children != null)
                {
                    childrenToRemoveCount = 0;

                    foreach (var pair in this.Children.GFIterator())
                    {
                        if (pair.Value.Version != version)
                        {
                            if (childrenToRemoveCount >= childrenToRemove.Length)
                            {
                                Array.Resize(ref childrenToRemove, childrenToRemove.Length * 2);
                            }

                            childrenToRemove[childrenToRemoveCount++] = pair.Key;
                        }
                    }

                    for (int i = 0; i < childrenToRemoveCount; i++)
                    {
                        this.Children.Remove(childrenToRemove[i]);
                    }

                    foreach (var child in this.Children.GFValueIterator())
                    {
                        child.Cleanup(version);
                    }
                }
            }
        }
    }
}
#endif