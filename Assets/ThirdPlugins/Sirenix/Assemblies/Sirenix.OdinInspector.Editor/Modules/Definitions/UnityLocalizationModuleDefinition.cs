//-----------------------------------------------------------------------
// <copyright file="UnityLocalizationModuleDefinition.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using System;

namespace Sirenix.OdinInspector.Editor.Modules
{
    public class UnityLocalizationModuleDefinition : ModuleDefinition
    {
        public override string ID
        {
            get
            {
                return "Unity.Localization";
            }
        }

        public override string NiceName
        {
            get
            {
                return "Unity.Localization support";
            }
        }

        public override Version LatestVersion
        {
            get
            {
                return new Version(1, 0, 0, 0);
            }
        }

        public override string Description
        {
            get
            {
                return "This small module contains code to support proper serialization and drawing of LocalizedString from the Unity.Localization package.";
            }
        }

        public override string DependenciesDescription
        {
            get
            {
                return "com.unity.localization package (any version)";
            }
        }

        public override string BuildFromPath
        {
            get
            {
                return "../Sirenix Solution/Sirenix.OdinInspector.SmallModules/Packages/com.unity.localization/";
            }
        }

        public override bool CheckSupportsCurrentEnvironment()
        {
            return UnityPackageUtility.HasPackageInstalled("com.unity.localization", new Version(0, 0));
        }
    }
}
#endif