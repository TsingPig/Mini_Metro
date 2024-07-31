//-----------------------------------------------------------------------
// <copyright file="EnumTypeUtilities.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.Utilities.Editor
{
    using Sirenix.OdinInspector;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    public static class EnumTypeUtilities<T>
    {
        private static readonly string[] enumNames;
        private static readonly string[] niceNames;
        private static readonly EnumMember[] allMembers;
        private static readonly EnumMember[] visibleMembers;
        private static readonly Dictionary<T, int> enumValueIndexLookup = new Dictionary<T, int>();
        private static readonly Type InspectorNameAttribute_Type;
        private static readonly FieldInfo InspectorNameAttribute_displayName;
        private static readonly bool isFlagEnum;

        static EnumTypeUtilities()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidCastException(typeof(T) + " Is not an enum type");
            }

            InspectorNameAttribute_Type = typeof(UnityEngine.Object).Assembly.GetType("UnityEngine.InspectorNameAttribute");
            if (InspectorNameAttribute_Type != null)
            {
                InspectorNameAttribute_displayName = InspectorNameAttribute_Type.GetField("displayName", Flags.InstanceAnyVisibility);
            }

            var fields = typeof(T).GetFields(Flags.StaticPublicDeclaredOnly);
            enumNames = new string[fields.Length];
            niceNames = new string[fields.Length];
            allMembers = new EnumMember[fields.Length];
            var visibleMembersList = new List<EnumMember>(fields.Length);

            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                var info = new EnumMember();

                try
                {
                    info.Value = (T)Enum.Parse(typeof(T), field.Name);
                    info.Name = field.Name;
                    info.NiceName = info.Name.SplitPascalCase();

                    var obs = field.GetAttribute<ObsoleteAttribute>(true);
                    var msg = field.GetAttribute<InfoBoxAttribute>(true);
                    var hide = field.GetAttribute<HideInInspector>();
                    var lblText = field.GetAttribute<LabelTextAttribute>(true);

                    info.IsObsolete = obs != null;
                    info.Message = obs == null ? "" : obs.Message;
                    info.Hide = hide != null;

                    if (lblText != null)
                    {
                        info.NiceName = lblText.Text ?? "";

                        if (lblText.NicifyText)
                        {
                            info.NiceName = ObjectNames.NicifyVariableName(info.NiceName);
                        }
                    }

                    if (InspectorNameAttribute_displayName != null)
                    {
                        object[] inspectorNames = field.GetCustomAttributes(InspectorNameAttribute_Type, false);

                        if (inspectorNames.Length > 0)
                        {
                            info.NiceName = ((string)InspectorNameAttribute_displayName.GetValue(inspectorNames[0])) ?? "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    info.Message = ex.Message;
                }

                info.Message = info.Message ?? "";
                allMembers[i] = info;
                enumNames[i] = info.Name;
                niceNames[i] = info.NiceName;
                enumValueIndexLookup[info.Value] = i;

                if (!info.Hide)
                {
                    visibleMembersList.Add(info);
                }
            }

            visibleMembers = visibleMembersList.ToArray();
            isFlagEnum = typeof(T).IsDefined<FlagsAttribute>();
        }

        public static bool IsFlagEnum { get { return isFlagEnum; } }
        public static string[] Names { get { return enumNames; } }
        public static string[] NiceNames { get { return niceNames; } }
        public static EnumMember[] AllEnumMemberInfos { get { return allMembers; } }
        public static EnumMember[] VisibleEnumMemberInfos { get { return visibleMembers; } }

        public static T[] DecomposeEnumFlagValues(T enumFlagValue)
        {
            if (!typeof(T).IsEnum) throw new InvalidCastException();

            var decomposedEnumValues = new List<T>();
            var values = Enum.GetValues(typeof(T));
            var enumFlagValueInt = Convert.ToInt64(enumFlagValue);
            for (int i = 0; i < values.Length; i++)
            {
                var column = (T)values.GetValue(i);
                if ((enumFlagValueInt & Convert.ToInt64(column)) != 0)
                {
                    decomposedEnumValues.Add(column);
                }
            }
            return decomposedEnumValues.ToArray();
        }
        public static int GetIndexOfEnumValue(T enumValue)
        {
            int index;
            if (enumValueIndexLookup.TryGetValue(enumValue, out index))
            {
                return index;
            }

            throw new Exception("No member with the value " + enumValue.ToString() + " was found on the Enum " + typeof(T).GetNiceFullName());
        }
        public static EnumMember GetEnumMemberInfo(T value)
        {
            var index = GetIndexOfEnumValue(value);
            return allMembers[index];
        }

        public struct EnumMember
        {
            public T Value;
            public string Name;
            public string NiceName;
            public bool IsObsolete;
            public string Message;
            public bool Hide;
        }
    }
}
#endif