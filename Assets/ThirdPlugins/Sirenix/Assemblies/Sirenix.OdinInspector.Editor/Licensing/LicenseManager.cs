//-----------------------------------------------------------------------
// <copyright file="LicenseManager.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Licensing
{
    using Sirenix.Utilities;
    using System.IO;
    using System.Threading;
    using UnityEditor;
    using UnityEngine;

    public static class LicenseManager
    {
        public static bool LicensingIsEnabled { get { return GetBakedLicensingIsEnabled(); } }
        private static bool IsHeadlessOrBatchMode { get { return SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null || UnityEditorInternal.InternalEditorUtility.inBatchMode; } }

        [InitializeOnLoadMethod]
        private static void InitLicenseManager()
        {
            if (!LicensingIsEnabled || IsHeadlessOrBatchMode) return;

            var license = LicenseInfo;

            switch (license.ClaimType)
            {
                case ClaimedLicenseType.Unclaimed:
                    var thr = new Thread(() =>
                    {
                        while (WebsiteAPI.Reachability == WebsiteAPI.ReachableStatus.Waiting) { Thread.Sleep(10); }

                        // No need to do the popup if we can't connect to the license API
                        if (WebsiteAPI.Reachability == WebsiteAPI.ReachableStatus.Reachable)
                        {
                            UnityEditorEventUtility.DelayActionThreadSafe(() =>
                            {
                                // Do claim popup
                                OdinActivationWindow.OpenWindow("Odin has not been registered on this machine");
                            });
                        }
                    });

                    thr.IsBackground = true;
                    thr.Priority = System.Threading.ThreadPriority.BelowNormal;
                    thr.Name = "Check if key claim popup should be popped up";

                    thr.Start();

                    break;
                case ClaimedLicenseType.Disabled:
                    // Do nothing
                    break;
                case ClaimedLicenseType.ClaimViaOrgToken:
                case ClaimedLicenseType.ClaimViaLicenseKeyEntered:
                case ClaimedLicenseType.ClaimViaUserLogin:
                    {
                        // Verify claim via website API
                        WebsiteAPI.GetLicenseKeyStatusAsync(license, response =>
                        {
                            if (response.LicenseState == WebsiteAPI.KeyState.Disabled || response.LicenseState == WebsiteAPI.KeyState.Expired)
                            {
                                license.ClaimType = ClaimedLicenseType.Unclaimed;
                                license.ClaimToken = null;

                                SetLicenseInfo(license);
                                OdinActivationWindow.OpenWindow(response);
                            }
                        });
                    }
                    break;
                default:
                    break;
            }
        }

        private static string GetBakedOrgClaimToken()
        {
            string value;
            if (BakedValues.TryGetBakedValue<string>("OrgClaimToken", out value))
            {
                return value;
            }
            return "";
        }

        private static unsafe int GetBakedOrgID()
        {
            int value;
            if (BakedValues.TryGetBakedValue<int>("OrgID", out value))
            {
                return value;
            }
            return 0;
        }

        private static unsafe string GetBakedOrgName()
        {
            string value;
            if (BakedValues.TryGetBakedValue<string>("OrgName", out value))
            {
                return value;
            }
            return "";
        }

        public static bool GetBakedOrgClaimIsEnabled()
        {
#if SIRENIX_INTERNAL
            return true;
#endif

            bool value;
            if (BakedValues.TryGetBakedValue("OrgClaimIsEnabled", out value))
            {
                return value;
            }
            return false;
        }

        public static bool GetBakedLicensingIsEnabled()
        {
#if SIRENIX_INTERNAL
            return true;
            //return false;
#endif

            bool value;
            if (BakedValues.TryGetBakedValue("LicensingIsEnabled", out value))
            {
                return value;
            }
            return false;
        }

        private static bool orgInfoInitialized;

        private static OrganizationInfo orgInfo;

        public static OrganizationInfo OrgInfo
        {
            get
            {
                if (!orgInfoInitialized)
                {
                    var id = GetBakedOrgID();
                    var namePrefsString = GetPrefs_OrganizationName(id);
                    var name = EditorPrefs.GetString(namePrefsString, GetBakedOrgName());

                    orgInfo = new OrganizationInfo()
                    {
                        OrgId = id,
                        OrgName = name,
                        OrgClaimToken = GetBakedOrgClaimToken(),
                    };
                }

                return orgInfo;
            }
        }

        public static void SetOrgName(string name)
        {
            orgInfo.OrgName = name;
            var prefsString = GetPrefs_OrganizationName(orgInfo.OrgId);
            EditorPrefs.SetString(prefsString, name);
        }

        private static bool licenseInfoFetched;
        private static LicenseInfo licenseInfo;

        public static LicenseInfo LicenseInfo
        {
            get
            {
                if (!licenseInfoFetched)
                {
                    int orgId = OrgInfo.OrgId;

                    string prefs_ClaimType = GetPrefs_ClaimType(orgId);
                    string prefs_ClaimToken = GetPrefs_ClaimToken(orgId);
                    string prefs_MachineName = GetPrefs_MachineName(orgId);
                    string prefs_UserName = GetPrefs_UserName(orgId);
                    string prefs_EnteredLicenseKey = GetPrefs_EnteredLicenseKey();

                    if (EditorPrefs.HasKey(prefs_ClaimType))
                    {
                        licenseInfo.ClaimType = (ClaimedLicenseType)EditorPrefs.GetInt(prefs_ClaimType, 0);
                    }

                    if (EditorPrefs.HasKey(prefs_ClaimToken))
                    {
                        licenseInfo.ClaimToken = EditorPrefs.GetString(prefs_ClaimToken, "");
                    }

                    if (EditorPrefs.HasKey(prefs_MachineName))
                    {
                        licenseInfo.MachineName = EditorPrefs.GetString(prefs_MachineName, "");
                    }
                    else
                    {
                        licenseInfo.MachineName = SystemInfo.deviceName;
                    }

                    if (EditorPrefs.HasKey(prefs_UserName))
                    {
                        licenseInfo.UserName = EditorPrefs.GetString(prefs_UserName, "");
                    }

                    if (EditorPrefs.HasKey(prefs_EnteredLicenseKey))
                    {
                        licenseInfo.EnteredLicenseKey = EditorPrefs.GetString(prefs_EnteredLicenseKey, "");
                    }

                    licenseInfoFetched = true;
                }

                return licenseInfo;
            }
        }

        public static void SetLicenseInfo(LicenseInfo info)
        {
            int orgId = OrgInfo.OrgId;

            EditorPrefs.SetInt(GetPrefs_ClaimType(orgId), (int)info.ClaimType);
            EditorPrefs.SetString(GetPrefs_ClaimToken(orgId), (info.ClaimToken ?? "").Trim());
            EditorPrefs.SetString(GetPrefs_MachineName(orgId), (info.MachineName ?? "").Trim());
            EditorPrefs.SetString(GetPrefs_UserName(orgId), (info.UserName ?? "").Trim());
            EditorPrefs.SetBool(GetPrefs_IsKeylessClaim(orgId), info.IsKeylessClaim);
            EditorPrefs.SetString(GetPrefs_EnteredLicenseKey(), (info.EnteredLicenseKey ?? "").Trim());

            licenseInfo = info;
            licenseInfoFetched = true;
        }

        private static string GetPrefs_OrganizationName(int orgId)
        {
            return "OdinInspector_LicenseInfo_OrganizationName_Org" + orgId;
        }

        private static string GetPrefs_UserName(int orgId)
        {
            return "OdinInspector_LicenseInfo_UserName_Org" + orgId;
        }

        private static string GetPrefs_MachineName(int orgId)
        {
            return "OdinInspector_LicenseInfo_MachineName_Org" + orgId;
        }

        private static string GetPrefs_ClaimToken(int orgId)
        {
            return "OdinInspector_LicenseInfo_ClaimToken_Org" + orgId;
        }

        private static string GetPrefs_ClaimType(int orgId)
        {
            return "OdinInspector_LicenseInfo_ClaimType_Org" + orgId;
        }

        private static string GetPrefs_IsKeylessClaim(int orgId)
        {
            return "OdinInspector_LicenseInfo_IsKeylessClaim_Org" + orgId;
        }

        private static string GetPrefs_EnteredLicenseKey()
        {
            return "OdinInspector_LicenseInfo_ManualLicenseKey";
        }
    }
}
#endif