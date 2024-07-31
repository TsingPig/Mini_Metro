//-----------------------------------------------------------------------
// <copyright file="OdinActivationWindow.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Licensing
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    public class OdinActivationWindow : OdinEditorWindow
    {
        static Color green = new Color(0x22 / 255f, 0x78 / 255f, 0);
        static Color red = new Color(0x78 / 255f, 0x16 / 255f, 0);
        static Color orange = new Color(1 * 0.7f, 0.5f * 0.7f, 0, 1);

        private static GUIStyle paddingStyle;
        private static GUIStyle statusTextError;
        private static GUIStyle statusTextSuccess;
        private static GUIStyle statusTextInfo;

        [NonSerialized] private ActivationModes selectedActivationMode;
        [NonSerialized] private ActivationMethod activationMethod;
        [NonSerialized] private StatusMessage currentStatusMessage;
        [NonSerialized] private WebsiteAPI.ReachableStatus? prevReachabilityStatus;
        [NonSerialized] private bool verify1, verify2, verify3, verify4;
        [NonSerialized] private string availableLicensesText = null;
        [NonSerialized] private int availableLicensesTotal;
        [NonSerialized] private int availableLicensesAvailable;
        [NonSerialized] private LicenseInfo currLicenseInfo;
        [NonSerialized] private WebsiteAPI.ReachableStatus reachability;

        private WebsiteAPI.MessageBoxMessageType messageBoxMessageType;
        private string messageBoxMessageTitle;
        private string messageBoxMessage;
        private string enteredLicenseKey;
        private string enteredMachineName;
        private string enteredUsername;
        private string enteredPassword;

        [MenuItem("Tools/Odin Inspector/License Management")]
        public static void Open()
        {
            var w = GetWindow<OdinActivationWindow>(true);
            w.position = GUIHelper.GetEditorWindowRect()
                .AlignCenter(600, 500);
        }

        [MenuItem("Tools/Odin Inspector/License Management", true)]
        private static bool OpenValidate()
        {
            return LicenseManager.LicensingIsEnabled;
        }

        private static void InitStyles()
        {
            paddingStyle = paddingStyle ?? new GUIStyle() { padding = new RectOffset(20, 20, 10, 10) };
            statusTextError = statusTextError ?? new GUIStyle(SirenixGUIStyles.BoldTitleRight) { normal = new GUIStyleState() { textColor = Color.red }, wordWrap = true };
            statusTextSuccess = statusTextSuccess ?? new GUIStyle(SirenixGUIStyles.BoldTitleRight) { normal = new GUIStyleState() { textColor = green }, wordWrap = true };
            statusTextInfo = statusTextInfo ?? new GUIStyle(SirenixGUIStyles.TitleRight) { wordWrap = true };
        }

        public static OdinActivationWindow OpenWindow(string message)
        {
            var w = GetWindow<OdinActivationWindow>(true);
            w.position = GUIHelper.GetEditorWindowRect()
                .AlignCenter(600, 500);

            return w;
        }

        public static void OpenWindow(WebsiteAPI.LicenseKeyStatusResponse response)
        {
            var wnd = OpenWindow(string.Empty);
            if (response.HasMessageBoxMessage)
            {
                wnd.messageBoxMessageTitle = response.MessageBoxMessageTitle;
                wnd.messageBoxMessageType = response.MessageBoxMessageType;
                wnd.messageBoxMessage = response.MessageBoxMessage;
            }

            wnd.currentStatusMessage = new StatusMessage(response.ReplyMessage, false, response.Success ? StatusMessageType.Success : StatusMessageType.Error);
        }

        private void UpdateOrgAvailableSeatCount()
        {
            if (LicenseManager.GetBakedOrgClaimIsEnabled())
            {
                WebsiteAPI.GetAvailableOrgSeatsAsync(LicenseManager.OrgInfo.OrgId, LicenseManager.OrgInfo.OrgClaimToken, x =>
                {
                    if (x.Success)
                    {
                        this.availableLicensesText = Math.Max(0, x.Total - x.Available) + " / " + x.Total;
                        this.availableLicensesTotal = x.Total;
                        this.availableLicensesAvailable = x.Available;

                        // Update the local org name if it's changed online (this change is stored in editorprefs, not committed to a file in the project)
                        if (x.OrganizationName != LicenseManager.OrgInfo.OrgName)
                        {
                            LicenseManager.SetOrgName(x.OrganizationName);
                        }
                    }
                    else
                    {
                        this.availableLicensesText = "Error: " + x.ReplyMessage;
                        this.availableLicensesTotal = 0;
                        this.availableLicensesAvailable = 0;
                    }

                    this.UpdateMessageBoxMessage(x);
                });
            }
        }

        private void UpdateMessageBoxMessage(WebsiteAPI.WebsiteResponse x)
        {
            if (x.HasMessageBoxMessage)
            {
                this.messageBoxMessageTitle = x.MessageBoxMessageTitle;
                this.messageBoxMessageType = x.MessageBoxMessageType;
                this.messageBoxMessage = x.MessageBoxMessage;
            }
            else
            {
                this.ClearMessage();
            }
        }

        private void ClearMessage()
        {
            this.messageBoxMessageTitle = null;
            this.messageBoxMessageType = WebsiteAPI.MessageBoxMessageType.Info;
            this.messageBoxMessage = null;
        }

        private static ActivationMethod GetDefaultActivationMethod()
        {
            if (LicenseManager.GetBakedOrgClaimIsEnabled())
                return ActivationMethod.AutoClaimFromOrganization;

            return ActivationMethod.OdinUserAccount;
        }

        protected override void Initialize()
        {
            if (LicenseManager.LicensingIsEnabled)
            {
                this.EnableAutomaticHeightAdjustment(900, false);
            }
            else
            {
                this.EnableAutomaticHeightAdjustment(900, true);
            }

            this.WindowPadding = new Vector4(0, 0, 0, 0);
            this.titleContent = new GUIContent("Activate Odin Inspector");
            this.currLicenseInfo = LicenseManager.LicenseInfo;

            if (this.currLicenseInfo.ClaimType == ClaimedLicenseType.ClaimViaLicenseKeyEntered)
                this.activationMethod = ActivationMethod.ActivationKey;
            else if (this.currLicenseInfo.ClaimType == ClaimedLicenseType.ClaimViaOrgToken)
                this.activationMethod = ActivationMethod.AutoClaimFromOrganization;
            else if (this.currLicenseInfo.ClaimType == ClaimedLicenseType.ClaimViaUserLogin)
                this.activationMethod = ActivationMethod.OdinUserAccount;
            else
                this.activationMethod = GetDefaultActivationMethod();

            this.currentStatusMessage = new StatusMessage()
            {
                Message = "Connecting",
                IsLoading = true,
                MessageType = StatusMessageType.Info
            };

            this.enteredMachineName = this.currLicenseInfo.MachineName;
            this.enteredMachineName = this.currLicenseInfo.MachineName;
            this.enteredUsername = this.currLicenseInfo.UserName;

            this.UpdateOrgAvailableSeatCount();
        }

        protected override void DrawEditor(int index)
        {
            InitStyles();

            if (!LicenseManager.LicensingIsEnabled)
            {
                GUIHelper.PushGUIEnabled(false);
                SirenixEditorGUI.MessageBox("License management has been disabled in this build of Odin.");
                GUIHelper.PopGUIEnabled();
                return;
            }

            // Init();
            UpdateStatus();
            var canClaim = this.currLicenseInfo.ClaimType == ClaimedLicenseType.Unclaimed;

            TempGUIHelpers.BeginWindowHeaderBox(paddingStyle);
            {
                GUILayout.Space(10);
                GUILayout.BeginHorizontal(GUILayoutOptions.ExpandWidth(false));

                GUIHelper.PushGUIEnabled(this.currLicenseInfo.ClaimType != ClaimedLicenseType.Disabled);
                if (TempGUIHelpers.FlatTabButton("Activate Odin", this.selectedActivationMode == ActivationModes.Activate))
                {
                    this.selectedActivationMode = ActivationModes.Activate;
                    this.ClearMessage();
                }
                GUIHelper.PopGUIEnabled();

                if (TempGUIHelpers.FlatTabButton("I Don't use Odin", this.selectedActivationMode == ActivationModes.IDontUseOdin))
                {
                    this.selectedActivationMode = ActivationModes.IDontUseOdin;
                    this.ClearMessage();
                }

                GUILayout.Space(10);
                GUILayout.EndHorizontal();

                GUILayout.Space(-10);
                this.DrawBottomCurrentLicenseStatusMessage();
            }
            GUIHelper.RequestRepaint();
            TempGUIHelpers.EndWindowHeaderBox();

            GUIHelper.PushLabelWidth(130);
            {
                this.DrawServerMessageBox();

                if (this.selectedActivationMode == ActivationModes.Activate)
                {
                    GUIHelper.PushGUIEnabled(this.currentStatusMessage.IsLoading == false);
                    GUILayout.BeginVertical(paddingStyle);
                    {
                        GUILayout.Label("Activate license with", SirenixGUIStyles.BoldTitle);
                        GUIHelper.PushGUIEnabled(GUI.enabled && canClaim);
                        this.activationMethod = TempGUIHelpers.HorizontalEnumRadioButtonList(this.activationMethod);
                        GUIHelper.PopGUIEnabled();
                    }
                    GUILayout.EndVertical();

                    GUIHelper.PushGUIEnabled(GUI.enabled && WebsiteAPI.Reachability == WebsiteAPI.ReachableStatus.Reachable);
                    {
                        if (this.activationMethod == ActivationMethod.AutoClaimFromOrganization)
                        {
                            DrawAutoClaimFromOrganization();
                        }
                        else if (this.activationMethod == ActivationMethod.ActivationKey)
                        {
                            DrawClaimUsingActivationKey();
                        }
                        else if (this.activationMethod == ActivationMethod.OdinUserAccount)
                        {
                            DrawClaimUsingUserAccount();
                        }
                    }
                    GUIHelper.PopGUIEnabled();
                    GUIHelper.PopGUIEnabled();
                }
                else
                {
                    DrawDisableOdinLicense();
                }
            }
            GUIHelper.PopLabelWidth();

            this.DrawCurrentReplyMessage();
        }

        private bool IsNullOrWhiteSpace(string str)
        {
            if (string.IsNullOrEmpty(str)) return true;
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsWhiteSpace(str[i])) return false;
            }
            return true;
        }

        private void DrawServerMessageBox()
        {
            if (!IsNullOrWhiteSpace(this.messageBoxMessageTitle) && !IsNullOrWhiteSpace(this.messageBoxMessage))
            {
                GUILayout.BeginVertical(paddingStyle);
                {
                    SirenixEditorGUI.BeginBox(this.messageBoxMessageTitle);
                    var icon = EditorIcons.UnityWarningIcon;
                    if (this.messageBoxMessageType == WebsiteAPI.MessageBoxMessageType.Error)
                    {
                        icon = EditorIcons.UnityErrorIcon;
                    }
                    else if (this.messageBoxMessageType == WebsiteAPI.MessageBoxMessageType.Info)
                    {
                        icon = EditorIcons.UnityInfoIcon;
                    }

                    GUILayout.Label(new GUIContent(this.messageBoxMessage, icon));
                    SirenixEditorGUI.EndBox();
                }
                GUILayout.EndVertical();
            }
        }

        private void DrawCurrentReplyMessage()
        {
            GUILayout.BeginHorizontal(SirenixGUIStyles.ContentPadding);
            var rect = GUIHelper.GetCurrentLayoutRect();

            string status;
            Color color;
            switch (this.currLicenseInfo.ClaimType)
            {
                case ClaimedLicenseType.Unclaimed:
                    status = "No license has been claimed for this machine";
                    color = red;
                    break;
                case ClaimedLicenseType.Disabled:
                    status = "Odin has been disabled for this machine";
                    color = orange;
                    break;
                case ClaimedLicenseType.ClaimViaOrgToken:
                    status = "A license has been automatically claimed from your organization";
                    color = green;
                    break;
                case ClaimedLicenseType.ClaimViaLicenseKeyEntered:
                    status = "License key entered";
                    color = green;
                    break;
                case ClaimedLicenseType.ClaimViaUserLogin:
                    status = "A license has been claimed via account login";
                    color = green;
                    break;
                default: throw new NotImplementedException(this.currLicenseInfo.ClaimType.ToString());
            }

            EditorGUI.DrawRect(rect, color);
            SirenixEditorGUI.DrawBorders(rect, 1);

            //GUILayout.Label("<b>Status:</b> ", new GUIStyle(SirenixGUIStyles.RichTextLabel)
            //{
            //    normal = new GUIStyleState()
            //    {
            //        textColor = color
            //    },
            //    alignment = TextAnchor.MiddleCenter
            //});

            GUILayout.Label(status, new GUIStyle(SirenixGUIStyles.BoldLabelCentered) { normal = new GUIStyleState() { textColor = Color.white } });
            GUILayout.EndHorizontal();
        }

        private void UpdateStatus()
        {
            if (Event.current.type == EventType.Layout)
            {
                this.reachability = WebsiteAPI.Reachability;
                this.currLicenseInfo = LicenseManager.LicenseInfo;
            }

            if (this.prevReachabilityStatus != reachability)
            {
                this.prevReachabilityStatus = reachability;

                if (reachability == WebsiteAPI.ReachableStatus.Unreachable)
                {
                    this.currentStatusMessage = new StatusMessage()
                    {
                        Message = WebsiteAPI.UnreachableReason,
                        MessageType = StatusMessageType.Error,
                        IsLoading = false,
                    };
                }
                else if (reachability == WebsiteAPI.ReachableStatus.Waiting)
                {
                    this.currentStatusMessage = new StatusMessage()
                    {
                        Message = "Connecting to " + WebsiteAPI.API_ENDPOINT + "...",
                        MessageType = StatusMessageType.Info,
                        IsLoading = true,
                    };
                }
                else
                {
                    this.currentStatusMessage = new StatusMessage();
                }
            }

            switch (this.currLicenseInfo.ClaimType)
            {
                case ClaimedLicenseType.Unclaimed: break;
                case ClaimedLicenseType.Disabled:
                    this.selectedActivationMode = ActivationModes.IDontUseOdin;
                    break;
                case ClaimedLicenseType.ClaimViaOrgToken:
                    this.activationMethod = ActivationMethod.AutoClaimFromOrganization;
                    break;
                case ClaimedLicenseType.ClaimViaLicenseKeyEntered:
                    this.activationMethod = ActivationMethod.ActivationKey;
                    break;
                case ClaimedLicenseType.ClaimViaUserLogin:
                    this.activationMethod = ActivationMethod.OdinUserAccount;
                    break;
            }
        }

        private void DrawDisableOdinLicense()
        {
            var info = this.currLicenseInfo;

            if (info.ClaimType == ClaimedLicenseType.Unclaimed)
            {
                GUILayout.BeginVertical(paddingStyle);
                {
                    GUILayout.Label("Odin Inspector requires a license", SirenixGUIStyles.BoldLabel);
                    GUILayout.Label(
                        "Odin Inspector Enterprise is installed in this project. If you use it in any capacity, you will need a license. " +
                        "If what you're doing never requires you to interact with Odin, then you can click the \"I don't use Odin\" button below to deactivate Odin.'"
                    , SirenixGUIStyles.MultiLineLabel);
                    GUILayout.Space(20);
                    this.verify1 = EditorGUILayout.ToggleLeft("I don't use the Inspector Window, or only use it for inspectors where Odin is inactive.", this.verify1);
                    this.verify2 = EditorGUILayout.ToggleLeft("I don't use any editor windows made with Odin.", this.verify2);
                    this.verify4 = EditorGUILayout.ToggleLeft("I don't use any Odin plugins such as the Odin Project Validator.", this.verify4);
                    this.verify3 = EditorGUILayout.ToggleLeft("I don't build tools or write code using Odin.", this.verify3);
                }
                GUILayout.EndVertical();

                var enable = this.verify1 && this.verify2 && this.verify3 && this.verify4;
                GUIHelper.PushGUIEnabled(enable);
                if (Button("I don't use Odin"))
                {
                    info.ClaimType = ClaimedLicenseType.Disabled;
                    LicenseManager.SetLicenseInfo(info);
                    this.ClearMessage();
                }
                GUIHelper.PopGUIEnabled();
            }
            else if (info.ClaimType == ClaimedLicenseType.Disabled)
            {
                GUILayout.BeginVertical(paddingStyle);
                {
                    GUILayout.Label("Odin Inspector is disabled", SirenixGUIStyles.BoldLabel);
                    GUILayout.Label(
                        "Odin Inspector has been disabled on this machine, and a warning will be displayed if you attempt to make active use of any Odin Inspector features."
                        , SirenixGUIStyles.MultiLineLabel);
                    GUILayout.Space(20);
                    GUILayout.EndVertical();
                }

                if (Button("Enable Odin Inspector"))
                {
                    info.ClaimType = ClaimedLicenseType.Unclaimed;
                    LicenseManager.SetLicenseInfo(info);
                    this.selectedActivationMode = ActivationModes.Activate;
                    this.activationMethod = GetDefaultActivationMethod();
                    this.ClearMessage();
                }
            }
            else
            {
                this.DrawReturnLicenseButton();
            }
        }

        private void DrawClaimUsingUserAccount()
        {
            var canClaim = this.currLicenseInfo.ClaimType == ClaimedLicenseType.Unclaimed;
            var isClaimed = this.currLicenseInfo.ClaimType == ClaimedLicenseType.ClaimViaUserLogin;

            if (isClaimed)
            {
                GUILayout.BeginVertical(paddingStyle);
                var info = this.currLicenseInfo;

                TempGUIHelpers.DrawInfoLabel("Machine Name", info.MachineName);
                TempGUIHelpers.DrawInfoLabel("Username", info.UserName);

                GUILayout.EndVertical();
                this.DrawReturnLicenseButton();
            }
            else if (canClaim)
            {
                GUILayout.BeginVertical(paddingStyle);
                this.DrawMachineNameTextField();
                this.enteredUsername = SirenixEditorFields.TextField("Username", this.enteredUsername);
                this.enteredPassword = EditorGUILayout.PasswordField("Password", this.enteredPassword);
                GUILayout.EndVertical();

                if (Button("Activate"))
                {
                    this.currentStatusMessage = new StatusMessage("Authenticating...", true, StatusMessageType.Info);

                    WebsiteAPI.ClaimSeatViaLoginAsync(this.enteredUsername, this.enteredPassword, this.enteredMachineName, response =>
                    {
                        this.HandleSeatClaimResponse(response);
                        if (response.Success && response.Status == WebsiteAPI.SeatClaimResponse.ResponseStatus.Accepted)
                        {
                            var licenseInfo = this.currLicenseInfo;
                            licenseInfo.MachineName = this.enteredMachineName;
                            licenseInfo.UserName = this.enteredUsername;
                            licenseInfo.ClaimType = ClaimedLicenseType.ClaimViaUserLogin;
                            licenseInfo.ClaimToken = response.ClaimToken;
                            licenseInfo.IsKeylessClaim = response.IsKeylessClaim;
                            LicenseManager.SetLicenseInfo(licenseInfo);
                        }
                        this.UpdateOrgAvailableSeatCount();
                        this.UpdateMessageBoxMessage(response);
                    });

                }
            }

        }

        private void DrawReturnLicenseButton()
        {
            var info = this.currLicenseInfo;

            if (Button("Return License"))
            {
                this.currentStatusMessage = new StatusMessage("Returning license...", true, StatusMessageType.Info);
                WebsiteAPI.ReturnLicenseAsync(this.currLicenseInfo, response =>
                {
                    if (response.Success)
                    {
                        if (response.ReturnLicense)
                        {
                            info.ClaimType = ClaimedLicenseType.Unclaimed;
                            LicenseManager.SetLicenseInfo(info);
                        }

                        if (response.Status == WebsiteAPI.ReturnLicenseResponse.ResponseStatus.Success)
                        {
                            this.currentStatusMessage = new StatusMessage("License has been successfully returned", false, StatusMessageType.Success);
                        }
                        else
                        {
                            var err = response.ReplyMessage;
                            if (response.ReturnLicense)
                            {
                                err += "\n License has been removed";
                            }

                            this.currentStatusMessage = new StatusMessage(err, false, StatusMessageType.Error);
                        }
                    }
                    else
                    {
                        this.currentStatusMessage = new StatusMessage(response.ReplyMessage, false, StatusMessageType.Error);
                    }
                    this.UpdateMessageBoxMessage(response);
                    this.UpdateOrgAvailableSeatCount();
                });
            }
        }

        private void DrawClaimUsingActivationKey()
        {
            var canClaim = this.currLicenseInfo.ClaimType == ClaimedLicenseType.Unclaimed;
            var isClaimed = this.currLicenseInfo.ClaimType == ClaimedLicenseType.ClaimViaLicenseKeyEntered;

            if (isClaimed)
            {
                this.DrawReturnLicenseButton();
            }
            else if (canClaim)
            {
                GUILayout.BeginVertical(paddingStyle);
                this.DrawMachineNameTextField();
                this.enteredLicenseKey = SirenixEditorFields.TextField("License Key", this.enteredLicenseKey);
                GUILayout.EndVertical();

                if (Button("Activate"))
                {
                    this.currentStatusMessage = new StatusMessage("Activating...", true, StatusMessageType.Info);

                    WebsiteAPI.ClaimSeatViaLicenseKeyAsync(this.enteredLicenseKey, this.enteredMachineName, response =>
                    {
                        this.HandleSeatClaimResponse(response);
                        if (response.Success && response.Status == WebsiteAPI.SeatClaimResponse.ResponseStatus.Accepted)
                        {
                            var licenseInfo = this.currLicenseInfo;
                            licenseInfo.MachineName = this.enteredMachineName;
                            licenseInfo.EnteredLicenseKey = this.enteredLicenseKey;
                            licenseInfo.ClaimType = ClaimedLicenseType.ClaimViaLicenseKeyEntered;
                            licenseInfo.ClaimToken = response.ClaimToken;
                            licenseInfo.IsKeylessClaim = response.IsKeylessClaim;
                            LicenseManager.SetLicenseInfo(licenseInfo);
                        }

                        this.UpdateMessageBoxMessage(response);
                        this.UpdateOrgAvailableSeatCount();
                    });
                }
            }
        }

        private void DrawAutoClaimFromOrganization()
        {
            var orgInfo = LicenseManager.OrgInfo;
            var canClaim = this.currLicenseInfo.ClaimType == ClaimedLicenseType.Unclaimed;
            var isClaimed = this.currLicenseInfo.ClaimType == ClaimedLicenseType.ClaimViaOrgToken;

            if (!LicenseManager.GetBakedOrgClaimIsEnabled())
            {
                GUILayout.BeginVertical(paddingStyle);
                {
                    GUILayout.Label("Organization claims are not available", SirenixGUIStyles.BoldLabel);
                    GUILayout.Label(
                            "Automatic organization claims have been disabled for this distribution of Odin. " +
                            "To enable it, go to your organization's settings on https://odininspector.com/account/manage/licenses and enable Automatic Seat Claiming. " +
                            "Then go to https://odininspector.com/download and download a new build of Odin, making sure to enable the Auto Assign Licenses checkbox."
                            , SirenixGUIStyles.MultiLineLabel);

                }
                GUILayout.EndVertical();
            }
            else if (isClaimed)
            {
                GUILayout.BeginVertical(paddingStyle);
                TempGUIHelpers.DrawInfoLabel("Organization Name", LicenseManager.OrgInfo.OrgName);
                this.DrawAvailableLicenses();
                GUILayout.EndVertical();
                this.DrawReturnLicenseButton();
            }
            else if (canClaim)
            {
                GUILayout.BeginVertical(paddingStyle);
                {
                    this.DrawMachineNameTextField();
                    TempGUIHelpers.DrawInfoLabel("Organization Name", LicenseManager.OrgInfo.OrgName);
                    this.DrawAvailableLicenses();
                }
                GUILayout.EndVertical();

                if (Button("Activate"))
                {
                    this.currentStatusMessage = new StatusMessage("Activating...", true, StatusMessageType.Info);

                    WebsiteAPI.ClaimOrgSeatAsync(orgInfo.OrgId, orgInfo.OrgClaimToken, this.enteredMachineName, response =>
                    {
                        this.HandleSeatClaimResponse(response);

                        if (response.Success && response.Status == WebsiteAPI.SeatClaimResponse.ResponseStatus.Accepted)
                        {
                            var licenseInfo = this.currLicenseInfo;
                            licenseInfo.MachineName = this.enteredMachineName;
                            licenseInfo.ClaimType = ClaimedLicenseType.ClaimViaOrgToken;
                            licenseInfo.ClaimToken = response.ClaimToken;
                            licenseInfo.IsKeylessClaim = response.IsKeylessClaim;
                            LicenseManager.SetLicenseInfo(licenseInfo);
                        }

                        this.UpdateMessageBoxMessage(response);
                        this.UpdateOrgAvailableSeatCount();
                    });
                }
            }
        }

        private void DrawAvailableLicenses()
        {
            var text = LicenseManager.OrgInfo.OrgName == null ? "Information not available" : this.availableLicensesText;
            TempGUIHelpers.DrawLoadingInfoLabel("Claimed Licenses", text);

            if (this.availableLicensesTotal > 0 && this.availableLicensesAvailable == 0)
            {
                TempGUIHelpers.DrawInfoLabel("  ", "Your organization has no unclaimed licenses left. You can still activate Odin by making a licence-less claim from your organization; if you do this, your organization manager will be notified that more seat purchases are required.");
            }
        }

        private void DrawMachineNameTextField()
        {
            //TempGUIHelpers.DrawInfoLabel("Machine Id", SystemInfo.deviceUniqueIdentifier);
            //GUILayout.Space(10);
            this.enteredMachineName = SirenixEditorFields.TextField("Machine Name", this.enteredMachineName);
        }

        private void DrawBottomCurrentLicenseStatusMessage()
        {
            var message = this.currentStatusMessage;
            var rect = GUIHelper.GetCurrentLayoutRect()
                    .HorizontalPadding(paddingStyle.padding.right);

            rect.xMin += 190;

            if (message.Message != null)
            {
                if (message.IsLoading)
                {
                    var iconRect = rect.AlignRight(20).AlignCenterY(20);
                    rect.width -= iconRect.width + 10;
                    TempGUIHelpers.DrawSpinningIcon(iconRect);
                }

                var style = statusTextInfo;

                if (message.MessageType == StatusMessageType.Error)
                    style = statusTextError;
                else if (message.MessageType == StatusMessageType.Success)
                    style = statusTextSuccess;

                GUI.Label(rect, message.Message, style);
            }
        }

        private bool Button(string text)
        {
            GUILayout.Space(5);
            SirenixEditorGUI.HorizontalLineSeparator();
            GUILayout.BeginVertical(paddingStyle);
            GUILayout.BeginHorizontal();

            GUIHelper.PushGUIEnabled(true);
            GUILayout.BeginVertical(GUILayoutOptions.ExpandWidth(false));
            GUILayout.Space(9);
            GUILayout.BeginHorizontal();
            GUIHelper.PushColor(new Color(1, 1, 1, 1));
            if (GUILayout.Button("  Help  "))
            {
                Application.OpenURL("https://odininspector.com/tutorials/license-management");
            }
            if (GUILayout.Button("  Online License Management  "))
            {
                Application.OpenURL("https://odininspector.com/account/manage/licenses");
            }
            GUIHelper.PopColor();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUIHelper.PopGUIEnabled();

            GUILayout.FlexibleSpace();
            var result = GUILayout.Button(text, GUILayoutOptions.Height(30));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(5);
            return result;
        }

        private void HandleSeatClaimResponse(WebsiteAPI.SeatClaimResponse response)
        {
            GUIHelper.RequestRepaint();
            if (response.Success)
            {
                if (response.Status == WebsiteAPI.SeatClaimResponse.ResponseStatus.Denied)
                {
                    this.currentStatusMessage = new StatusMessage()
                    {
                        Message = "License activation failed",
                        MessageType = StatusMessageType.Error,
                    };

                    if (!string.IsNullOrEmpty(response.ReplyMessage))
                    {
                        this.currentStatusMessage.Message += "\nReason: " + response.ReplyMessage;
                    }
                }
                else if (response.Status == WebsiteAPI.SeatClaimResponse.ResponseStatus.Accepted)
                {
                    this.currentStatusMessage = new StatusMessage()
                    {
                        Message = "Odin has been successfully activated",
                        MessageType = StatusMessageType.Success,
                    };
                }
            }
            else
            {
                this.currentStatusMessage = new StatusMessage()
                {
                    Message = response.ReplyMessage,
                    MessageType = StatusMessageType.Error,
                };
            }
        }

        private struct StatusMessage
        {
            public string Message;
            public bool IsLoading;
            public StatusMessageType MessageType;

            public StatusMessage(string message, bool isLoading, StatusMessageType messageType)
            {
                this.Message = message;
                this.IsLoading = isLoading;
                this.MessageType = messageType;
            }
        }

        private enum StatusMessageType
        {
            Info,
            Success,
            Error
        }
    }

    internal static class TempGUIHelpers
    {
        private static GUIStyle infoLabelStyle;

        const float DEFAULT_RADIO_LIST_SPACING = 10f;

        public static void DrawSpinningIcon(Rect iconRect, float speed = 360, Texture icon = null)
        {
            var angle = (float)EditorApplication.timeSinceStartup;
            var mat = GUI.matrix;
            GUIHelper.RequestRepaint();
            icon = icon ?? EditorIcons.Refresh.Highlighted;
            EditorGUIUtility.RotateAroundPivot(angle * speed, iconRect.center);
            GUI.DrawTexture(iconRect, icon);
            GUI.matrix = mat;
        }

        public static void BeginWindowHeaderBox(GUIStyle style)
        {
            if (style != null)
            {
                GUILayout.BeginVertical(style);
            }
            else
            {
                GUILayout.BeginVertical();
            }
            var rect = GUIHelper.GetCurrentLayoutRect().Expand(4, 4, 2, 0);
            EditorGUI.DrawRect(rect, SirenixGUIStyles.HeaderBoxBackgroundColor);
        }

        public static void EndWindowHeaderBox()
        {
            var rect = GUIHelper.GetCurrentLayoutRect().Expand(4, 4, 2, 0);
            SirenixEditorGUI.DrawBorders(rect, 1);
            GUILayout.EndVertical();
        }

        public static int HorizontalRadioButtonList(string label, int selectedIndex, IEnumerable<string> options, float spacing = DEFAULT_RADIO_LIST_SPACING, GUIStyle style = null)
        {
            return HorizontalRadioButtonList(GUIHelper.TempContent(label), selectedIndex, options, spacing, style);
        }

        public static int HorizontalRadioButtonList(GUIContent label, int selectedIndex, IEnumerable<string> options, float spacing = DEFAULT_RADIO_LIST_SPACING, GUIStyle style = null)
        {
            var rect = EditorGUILayout.GetControlRect(false);
            rect = EditorGUI.PrefixLabel(rect, -1, new GUIContent("Get license from:"));
            return HorizontalRadioButtonList(rect, selectedIndex, options, spacing, style);
        }

        public static int HorizontalRadioButtonList(int selectedIndex, IEnumerable<string> options, float spacing = DEFAULT_RADIO_LIST_SPACING, GUIStyle style = null)
        {
            var rect = EditorGUILayout.GetControlRect(false);
            return HorizontalRadioButtonList(rect, selectedIndex, options, spacing, style);
        }

        public static int HorizontalRadioButtonList(Rect rect, int selectedIndex, IEnumerable<string> options, float spacing = DEFAULT_RADIO_LIST_SPACING, GUIStyle style = null)
        {
            style = style ?? EditorStyles.radioButton;
            var i = 0;
            foreach (var item in options)
            {
                var size = EditorStyles.radioButton.CalcSize(GUIHelper.TempContent(item));
                rect.width = size.x;
                var wasSelected = selectedIndex == i;
                var selected = GUI.Toggle(rect, wasSelected, item, style);
                if (selected != wasSelected && selected)
                {
                    selectedIndex = i;
                }

                rect.x += size.x + spacing;
                i++;
            }

            return selectedIndex;
        }

        public static T HorizontalEnumRadioButtonList<T>(string label, T enumValue, float spacing = DEFAULT_RADIO_LIST_SPACING, GUIStyle style = null)
        {
            var index = EnumTypeUtilities<T>.GetIndexOfEnumValue(enumValue);
            index = HorizontalRadioButtonList(GUIHelper.TempContent(label), index, EnumTypeUtilities<T>.NiceNames, spacing, style);
            return EnumTypeUtilities<T>.AllEnumMemberInfos[index].Value;
        }

        public static T HorizontalEnumRadioButtonList<T>(GUIContent label, T enumValue, float spacing = DEFAULT_RADIO_LIST_SPACING, GUIStyle style = null)
        {
            var index = EnumTypeUtilities<T>.GetIndexOfEnumValue(enumValue);
            index = HorizontalRadioButtonList(label, index, EnumTypeUtilities<T>.NiceNames, spacing, style);
            return EnumTypeUtilities<T>.AllEnumMemberInfos[index].Value;
        }

        public static T HorizontalEnumRadioButtonList<T>(T enumValue, float spacing = DEFAULT_RADIO_LIST_SPACING, GUIStyle style = null)
        {
            var index = EnumTypeUtilities<T>.GetIndexOfEnumValue(enumValue);
            index = HorizontalRadioButtonList(index, EnumTypeUtilities<T>.NiceNames, spacing, style);
            return EnumTypeUtilities<T>.AllEnumMemberInfos[index].Value;
        }

        public static void DrawInfoLabel(string labelText, string fieldText)
        {
            if (infoLabelStyle == null)
            {
                infoLabelStyle = new GUIStyle(SirenixGUIStyles.LeftAlignedGreyMiniLabel)
                {
                    wordWrap = true
                };
            }

            EditorGUILayout.LabelField(labelText, fieldText, infoLabelStyle);
        }

        public static void DrawLoadingInfoLabel(string labelText, string fieldText, string loadingText = "Loading...")
        {
            if (fieldText == string.Empty)
                fieldText = null;

            EditorGUILayout.LabelField(labelText, fieldText ?? ("     " + loadingText), SirenixGUIStyles.LeftAlignedGreyMiniLabel);
            if (fieldText == null)
            {
                var r = GUILayoutUtility.GetLastRect();
                r.xMin = r.x + GUIHelper.CurrentIndentAmount;
                r.width = EditorGUIUtility.labelWidth - GUIHelper.CurrentIndentAmount;
                r.x = r.xMax;
                r = r.AlignLeft(r.height);
                GUIHelper.PushColor(new Color(1, 1, 1, 0.3f));
                DrawSpinningIcon(r);
                GUIHelper.PopColor();
            }
        }

        private static GUIStyle FlatTabButtonTextStyle;

        public static bool FlatTabButton(string buttonText, bool isActive)
        {
            FlatTabButtonTextStyle = FlatTabButtonTextStyle ?? new GUIStyle(SirenixGUIStyles.LabelCentered)
            {
                normal = new GUIStyleState() { textColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0.7f) }
            };

            var size = FlatTabButtonTextStyle.CalcSize(GUIHelper.TempContent(buttonText));
            var padding = new Vector2(25, 8);
            var rect = GUILayoutUtility.GetRect(size.x + padding.x * 2, size.y + padding.y * 2, GUIStyle.none, GUILayoutOptions.ExpandWidth(false))
                .Expand(0, 1, 0, 0);
            var bgColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.19f) : new Color(0, 0, 0, 0.19f);
            var mouseOver = GUI.enabled && rect.Contains(Event.current.mousePosition);
            var textBlendColor = (mouseOver || isActive) ? Color.white : new Color(1, 1, 1, 0.45f);

            if (!isActive)
            {
                rect = rect.AlignBottom(rect.height - 4);
                bgColor.a = 0;
            }

            EditorGUI.DrawRect(rect, bgColor);
            SirenixEditorGUI.DrawBorders(rect, 1);
            GUIHelper.PushColor(textBlendColor, true);
            GUI.Label(rect, buttonText, FlatTabButtonTextStyle);
            GUIHelper.PopColor();
            return GUI.Button(rect, GUIContent.none, GUIStyle.none);
        }
    }
}
#endif