//-----------------------------------------------------------------------
// <copyright file="DisplayAsStringAttributeDrawer.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Drawers
{
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Draws properties marked with <see cref="DisplayAsStringAttribute"/>.
    /// Calls the properties ToString method to get the string to draw.
    /// </summary>
    /// <seealso cref="HideLabelAttribute"/>
    /// <seealso cref="LabelTextAttribute"/>
    /// <seealso cref="InfoBoxAttribute"/>
    /// <seealso cref="DetailedInfoBoxAttribute"/>
    /// <seealso cref="MultiLinePropertyAttribute"/>
    /// <seealso cref="MultilineAttribute"/>
    public sealed class DisplayAsStringAttributeDrawer<T> : OdinAttributeDrawer<DisplayAsStringAttribute, T>, IDefinesGenericMenuItems
    {
        public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            genericMenu.AddItem(new GUIContent("Copy to clipboard"), false, () =>
            {
                var str = this.ValueEntry.SmartValue == null ? "Null" : this.ValueEntry.SmartValue.ToString();
                GUIUtility.systemCopyBuffer = str;
            });
        }

        /// <summary>
        /// Draws the property.
        /// </summary>
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var entry = this.ValueEntry;
            var attribute = this.Attribute;

            if (entry.Property.ChildResolver is ICollectionResolver)
            {
                this.CallNextDrawer(label);
                return;
            }

            string str = entry.SmartValue == null ? "Null" : entry.SmartValue.ToString();

            if (label == null)
            {
                EditorGUILayout.LabelField(str, !attribute.Overflow ? SirenixGUIStyles.MultiLineLabel : EditorStyles.label, GUILayoutOptions.MinWidth(0));
            }
            else if (!attribute.Overflow)
            {
                var stringLabel = GUIHelper.TempContent(str);
                var position = EditorGUILayout.GetControlRect(false, SirenixGUIStyles.MultiLineLabel.CalcHeight(stringLabel, entry.Property.LastDrawnValueRect.width - GUIHelper.BetterLabelWidth), GUILayoutOptions.MinWidth(0));
                var rect = EditorGUI.PrefixLabel(position, label);
                GUI.Label(rect, stringLabel, SirenixGUIStyles.MultiLineLabel);
            }
            else
            {
                int id;
                bool keyboard;
                Rect rect;
                SirenixEditorGUI.GetFeatureRichControlRect(label, out id, out keyboard, out rect);
                GUI.Label(rect, str);
            }
        }
    }
}
#endif