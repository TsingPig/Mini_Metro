//-----------------------------------------------------------------------
// <copyright file="StringSlice.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.Utilities.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    public struct StringSlice : IEquatable<string>, IEquatable<StringSlice>
    {
        public string String;
        public int Index;
        public int Length;

        public StringSlice(string str)
        {
            this.String = str;
            this.Index = 0;
            this.Length = str.Length;
        }

        public StringSlice(string str, int index, int length)
        {
            int strLength = str.Length;
            if (index < 0 || length < 0 || index + length > strLength) throw new IndexOutOfRangeException();
            this.String = str;
            this.Index = index;
            this.Length = length;
        }

        public static implicit operator StringSlice(string str)
        {
            return new StringSlice()
            {
                String = str,
                Index = 0,
                Length = str.Length
            };
        }

        public static explicit operator string(StringSlice slice)
        {
            return slice.ToString();
        }

        public char this[int index]
        {
            get
            {
                return this.String[this.Index + index];
            }
        }

        public override string ToString()
        {
            if (this.String == null) return string.Empty;
            if (this.Index == 0 && this.Length == this.String.Length) return this.String;
            return this.String.Substring(this.Index, this.Length);
        }

        public unsafe override int GetHashCode()
        {
            if (this.String == null) return 0;
            int length = this.Length;
            //if (this.Index == 0 && length == this.String.Length) return this.String.GetHashCode();

            // Do the same thing strings do - though note, this does *not* guarantee 
            // that the hashcode for a slice is the same as the hashcode for an identical 
            // string as the slice!
            // 
            // System.String.GetHashCode() will in newer framework versions often return
            // a randomized value that changes each time the program executes.
            //
            // We don't care, though. We just want something deterministic such that 
            // different slices resulting in the same "string" sequence have the same
            // hash codes.

            fixed (char* basePtr = this.String)
            {
                char* ptr = basePtr + this.Index;
                int num = 352654597;
                int num2 = num;
                int* ptr2 = (int*)ptr;
                for (int index = length; index > 0; index -= 4)
                {
                    if (index <= 1)
                    {
                        num = (((num << 5) + num + (num >> 27)) ^ *((char*)ptr2));
                        break;
                    }

                    num = (((num << 5) + num + (num >> 27)) ^ *ptr2);

                    if (index <= 2)
                    {
                        break;
                    }

                    if (index <= 3)
                    {
                        num2 = (((num2 << 5) + num2 + (num2 >> 27)) ^ ((char*)ptr2)[2]);
                        break;
                    }

                    num2 = (((num2 << 5) + num2 + (num2 >> 27)) ^ ptr2[1]);
                    ptr2 += 2;
                }

                return num + num2 * 1566083941;
            }
        }

        public override bool Equals(object obj)
        {
            string str = obj as string;
            if (str != null) return this == str;
            if (obj is StringSlice) return this == (StringSlice)obj;
            return false;
        }

        public bool Equals(StringSlice other)
        {
            return this == other;
        }

        public bool Equals(string other)
        {
            return this == other;
        }

        public static bool operator ==(StringSlice a, StringSlice b)
        {
            var length = a.Length;

            if (length != b.Length) return false;
            if (a.Index == b.Index && a.String != null && object.ReferenceEquals(a.String, b.String)) return true;

            var end = a.Index + length;

            for (int a_i = a.Index, b_i = b.Index; a_i < end; a_i++, b_i++)
            {
                if (a.String[a_i] != b.String[b_i]) return false;
            }

            //for (int i = 0; i < length; i++)
            //{
            //    if (a[i] != b[i]) return false;
            //}

            return true;
        }

        public static bool operator ==(StringSlice slice, string str)
        {
            if (str == null)
            {
                return slice.String == null;
            }

            if (slice.String == null) return false;
            if (slice.Length != str.Length) return false;

            if (slice.Index == 0)
            {
                return slice.String == str;
            }

            for (int i = 0, j = slice.Index; i < str.Length; i++, j++)
            {
                if (str[i] != slice.String[j])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(StringSlice a, StringSlice b)
        {
            return !(a == b);
        }

        public static bool operator !=(StringSlice fs, string str)
        {
            return !(fs == str);
        }

        public StringSlice Slice(int index, int length)
        {
            if (index < 0 || length < 0 || index + length > this.Length) throw new IndexOutOfRangeException();
            return new StringSlice(this.String, this.Index + index, length);
        }

        public StringSlice Slice(int index)
        {
            if (index < 0  || index > this.Length) throw new IndexOutOfRangeException();
            return new StringSlice(this.String, this.Index + index, this.Length - index);
        }

        public bool StartsWith(string str)
        {
            if (this.String == null) return false;
            int length = str.Length;
            if (this.Length < length) return false;

            for (int i = 0; i < length; i++)
            {
                if (this[i] != str[i]) return false;
            }

            return true;
        }

        public bool StartsWith(ref StringSlice fs)
        {
            if (this.String == null) return false;
            int length = fs.Length;
            if (this.Length < length) return false;

            for (int i = 0; i < length; i++)
            {
                if (this[i] != fs[i]) return false;
            }

            return true;
        }

        public bool EndsWith(ref string str)
        {
            if (this.String == null) return false;
            int length = str.Length;
            if (this.Length < length) return false;

            for (int i = 0, j = this.Length - 1; i < length; i++, j--)
            {
                if (this[j] != str[i]) return false;
            }

            return true;
        }

        public bool EndsWith(ref StringSlice fs)
        {
            if (this.String == null) return false;
            int length = fs.Length;
            if (this.Length < length) return false;

            for (int i = 0, j = this.Length - 1; i < length; i++, j--)
            {
                if (this[j] != fs[i]) return false;
            }

            return true;
        }

        public bool Contains(string str)
        {
            if (this.String == null) return false;
            int strLength = str.Length;
            int length = this.Length;
            if (length < strLength) return false;

            for (int i = 0; i < length; i++)
            {
                if (this[i] == str[0])
                {
                    bool isMatch = true;

                    for (int fsIndex = 1, thisIndex = i + 1; fsIndex < strLength && thisIndex < length; fsIndex++, thisIndex++)
                    {
                        if (this[thisIndex] != str[fsIndex])
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch) return true;
                }
            }

            return false;
        }

        public bool Contains(ref StringSlice fs)
        {
            if (this.String == null) return false;
            int fsLength = fs.Length;
            int length = this.Length;
            if (length < fsLength) return false;

            for (int i = 0; i < length; i++)
            {
                if (this[i] == fs[0])
                {
                    bool isMatch = true;

                    for (int fsIndex = 1, thisIndex = i + 1; fsIndex < fsLength && thisIndex < length; fsIndex++, thisIndex++)
                    {
                        if (this[thisIndex] != fs[fsIndex])
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch) return true;
                }
            }

            return false;
        }

        public int FirstIndexOf(char c)
        {
            for (int i = this.Index, end = i + this.Length; i < end; i++)
            {
                if (this.String[i] == c) return i - this.Index;
            }

            return -1;
        }

        public int LastIndexOf(char c)
        {
            for (int i = this.Index + this.Length - 1; i >= this.Index; i--)
            {
                if (this.String[i] == c) return i - this.Index;
            }

            return -1;
        }

        public bool TryParseToInt(out int result)
        {
            var length = this.Length;
            
            bool isNegative = false;

            result = 0;

            for (int i = 0; i < length; i++)
            {
                char c = this[i];

                if (c == '-' && i == 0)
                {
                    isNegative = true;
                }
                else
                {
                    c -= '0';

                    if (c < 0 || c > 9)
                    {
                        result = 0;
                        return false;
                    }
                    else
                    {
                        result = (result * 10) + c;
                    }
                }
            }

            if (isNegative)
            {
                result = -result;
            }

            return true;
        }

        public struct PreHashed : IEquatable<PreHashed>
        {
            public StringSlice Slice;
            public int Hash;

            public PreHashed(StringSlice slice)
            {
                this.Slice = slice;
                this.Hash = this.Slice.GetHashCode();
            }

            public static implicit operator PreHashed(string str)
            {
                return new PreHashed(new StringSlice(str));
            }

            public static implicit operator PreHashed(StringSlice slice)
            {
                return new PreHashed(slice);
            }

            public bool Equals(PreHashed other)
            {
                return other.Slice == this.Slice;
            }

            public override int GetHashCode()
            {
                if (this.Hash == 0)
                {
                    this.Hash = this.Slice.GetHashCode();
                }
                return this.Hash;
            }
        }
    }

    public class StringSliceEqualityComparer : IEqualityComparer<StringSlice>, IEqualityComparer<StringSlice.PreHashed>
    {
        public static readonly StringSliceEqualityComparer Instance = new StringSliceEqualityComparer();

        public bool Equals(StringSlice.PreHashed x, StringSlice.PreHashed y)
        {
            return x.Slice == y.Slice;
        }

        public bool Equals(StringSlice x, StringSlice y)
        {
            return x == y;
        }

        public int GetHashCode(StringSlice.PreHashed obj)
        {
            return obj.GetHashCode();
        }

        public int GetHashCode(StringSlice obj)
        {
            return obj.GetHashCode();
        }
    }

    public static class StringSliceExtensions
    {
        public static void Append(this StringBuilder sb, ref StringSlice slice)
        {
            sb.Append(slice.String, slice.Index, slice.Length);
        }

        public static StringSlice Slice(this string str, int index, int length)
        {
            return new StringSlice(str, index, length);
        }

        public static StringSlice Slice(this string str, int index)
        {
            return new StringSlice(str, index, str.Length - index);
        }

        public static bool StartsWith(this string str, ref StringSlice slice)
        {
            int length = slice.Length;
            if (str.Length < length) return false;

            for (int i = 0; i < length; i++)
            {
                if (str[i] != slice[i]) return false;
            }

            return true;
        }

        public static bool EndsWith(this string str, ref StringSlice slice)
        {
            int length = slice.Length;
            if (str.Length < length) return false;

            for (int i = 0, j = length - 1; i < length; i++, j--)
            {
                if (str[j] != slice[i]) return false;
            }

            return true;
        }

        public static bool Contains(this string str, ref StringSlice slice)
        {
            int fsLength = slice.Length;
            int length = str.Length;
            if (length < fsLength) return false;

            for (int i = 0; i < length; i++)
            {
                if (str[i] == slice[0])
                {
                    bool isMatch = true;

                    for (int fsIndex = 1, thisIndex = i + 1; fsIndex < fsLength && thisIndex < length; fsIndex++, thisIndex++)
                    {
                        if (str[thisIndex] != slice[fsIndex])
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch) return true;
                }
            }

            return false;
        }
    }
}
#endif