//-----------------------------------------------------------------------
// <copyright file="OdinInspectorVersion.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor
{
    using Sirenix.Utilities;

    /// <summary>
    /// Installed Odin Inspector Version Info.
    /// </summary>
    public static class OdinInspectorVersion
    {
        private static string version;
        private static string buildName;
        private static string licensee;

        /// <summary>
        /// Gets the name of the current running version of Odin Inspector.
        /// </summary>
        public static string BuildName
        {
            get
            {
                if (buildName == null)
                {
                    var attribute = typeof(InspectorConfig).Assembly.GetAttribute<SirenixBuildNameAttribute>(true);
                    buildName = attribute != null ? attribute.BuildName : "Source Code";
                }

                return buildName;
            }
        }

        public static bool HasLicensee { get { return !string.IsNullOrEmpty(Licensee); } }

        public static string Licensee
        {
            get
            {
#if SIRENIX_INTERNAL
                return "Some Licensee Whatever";
#endif

                if (licensee == null)
                {
                    if (!BakedValues.TryGetBakedValue<string>("Licensee", out licensee))
                    {
                        licensee = "";
                    }
                }

                return licensee;
            }
        }

        /// <summary>
        /// Gets the current running version of Odin Inspector.
        /// </summary>
        public static string Version
        {
            get
            {
                if (version == null)
                {
                    var attribute = typeof(InspectorConfig).Assembly.GetAttribute<SirenixBuildVersionAttribute>(true);
                    version = attribute != null ? attribute.Version : "Source Code Mode";
                }

                return version;
            }
        }

        /// <summary>
        /// Whether the current version of Odin is an enterprise version.
        /// </summary>
        public static bool IsEnterprise
        {
            get
            {
                return true;
            }
        }
    }
}
#endif