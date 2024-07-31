//-----------------------------------------------------------------------
// <copyright file="LicenseInfo.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Licensing
{
    using System;

    [Serializable]
    public struct LicenseInfo
    {
        public static readonly LicenseInfo Default = new LicenseInfo()
        {
            ClaimType = ClaimedLicenseType.Unclaimed,
            ClaimToken = "",
            MachineName = "",
            UserName = "",
        };

        public ClaimedLicenseType ClaimType;
        public string ClaimToken;
        public string MachineName;
        public string UserName;
        public string EnteredLicenseKey;
        public bool IsKeylessClaim;
    }
}
#endif