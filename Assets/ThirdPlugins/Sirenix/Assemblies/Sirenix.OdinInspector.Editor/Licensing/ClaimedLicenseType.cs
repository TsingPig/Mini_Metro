//-----------------------------------------------------------------------
// <copyright file="ClaimedLicenseType.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Licensing
{
    public enum ClaimedLicenseType
    {
        Unclaimed,
        Disabled,
        ClaimViaOrgToken,
        ClaimViaLicenseKeyEntered,
        ClaimViaUserLogin,
    }
}
#endif