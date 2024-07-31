//-----------------------------------------------------------------------
// <copyright file="WebsiteAPI.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Editor.Licensing
{
    using Sirenix.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Networking;

    public static class WebsiteAPI
    {
        private static readonly MethodInfo UnityWebRequest_SendWebRequest_Method;
        private static readonly EventInfo AsyncOperation_Completed_Event;

        private static readonly PropertyInfo UnityWebRequest_IsError_Property;
        private static readonly PropertyInfo UnityWebRequest_IsHttpError_Property;
        private static readonly PropertyInfo UnityWebRequest_IsNetworkError_Property;
        private static readonly PropertyInfo UnityWebRequest_Result_Property;

        public static readonly bool RequiredUnityApiIsAvailable = true;

        static WebsiteAPI()
        {
            UnityWebRequest_SendWebRequest_Method = 
                typeof(UnityWebRequest).GetMethod("SendWebRequest", Flags.InstancePublic, null, new Type[] { }, null) 
                ?? typeof(UnityWebRequest).GetMethod("Send", Flags.InstancePublic, null, new Type[] { }, null);

            if (UnityWebRequest_SendWebRequest_Method == null)
            {
                RequiredUnityApiIsAvailable = false;
            }

            AsyncOperation_Completed_Event = typeof(AsyncOperation).GetEvent("completed", Flags.InstancePublic);

            UnityWebRequest_IsError_Property = typeof(UnityWebRequest).GetProperty("isError", Flags.InstancePublic);
            UnityWebRequest_IsHttpError_Property = typeof(UnityWebRequest).GetProperty("isHttpError", Flags.InstancePublic);
            UnityWebRequest_IsNetworkError_Property = typeof(UnityWebRequest).GetProperty("isNetworkError", Flags.InstancePublic);
            UnityWebRequest_Result_Property = typeof(UnityWebRequest).GetProperty("result", Flags.InstancePublic);

            if (UnityWebRequest_IsError_Property == null && UnityWebRequest_IsHttpError_Property == null && UnityWebRequest_IsNetworkError_Property == null && UnityWebRequest_Result_Property == null)
            {
                RequiredUnityApiIsAvailable = false;
            }
        }

        private static AsyncOperation SendWebRequest(UnityWebRequest request)
        {
            return (AsyncOperation)UnityWebRequest_SendWebRequest_Method.Invoke(request, null);
        }

        private static bool RequestIsError(UnityWebRequest request)
        {
            if (UnityWebRequest_Result_Property != null)
            {
                object value = UnityWebRequest_Result_Property.GetValue(request, null);
                int enumValue = Convert.ToInt32(value);

                // 0 = InProgress
                // 1 = Success
                // Rest = errors of various kinds
                return enumValue > 1; 
            }

            if (UnityWebRequest_IsNetworkError_Property != null && (bool)UnityWebRequest_IsNetworkError_Property.GetValue(request, null))
            {
                return true;
            }

            if (UnityWebRequest_IsHttpError_Property != null && (bool)UnityWebRequest_IsHttpError_Property.GetValue(request, null))
            {
                return true;
            }

            if (UnityWebRequest_IsError_Property != null && (bool)UnityWebRequest_IsError_Property.GetValue(request, null))
            {
                return true;
            }

            return false;
        }

        private static void SubscribeOnCompleted(AsyncOperation operation, Action<AsyncOperation> action)
        {
            if (operation.isDone)
            {
                try
                {
                    action(operation);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
                return;
            }

            if (AsyncOperation_Completed_Event != null)
            {
                AsyncOperation_Completed_Event.AddEventHandler(operation, action);
            }
            else
            {
                EditorApplication.CallbackFunction update = null;
                
                update = () =>
                {
                    if (operation.isDone)
                    {
                        EditorApplication.update -= update;
                        try
                        {
                            action(operation);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogException(ex);
                        }
                    }
                };

                EditorApplication.update += update;
            }
        }

        public const string API_ENDPOINT = "https://odininspector.com/";
        //public const string API_ENDPOINT = "https://localhost:5001/";
        //public const string API_ENDPOINT = "https://localhost:44334/";

        public enum ReachableStatus
        {
            Waiting,
            Unreachable,
            Reachable
        }

        private static bool isPinging;

        [InitializeOnLoadMethod]
        private static void Init()
        {
            if (LicenseManager.LicensingIsEnabled)
            {
                TryUpdateReachability();
            }
        }

        public static bool TryUpdateReachability()
        {
            if (isPinging || !RequiredUnityApiIsAvailable) return false;

            isPinging = true;
            Reachability = ReachableStatus.Waiting;

            var request = UnityWebRequest.Get(API_ENDPOINT.TrimEnd('/') + "/api/ping");
            try
            {
                request.SetRequestHeader("User-Agent", "odin-inspector-unity-editor/1.0");
            }
            catch { }
            request.timeout = 5000;

            var op = SendWebRequest(request);

            SubscribeOnCompleted(op, a =>
            {
                isPinging = false;

                if (request.downloadHandler.text == "pong")
                {
                    UnreachableReason = null;
                    Reachability = ReachableStatus.Reachable;
                }
                else
                {
                    Reachability = ReachableStatus.Unreachable;
                    UnreachableReason = "Unable to ping licensing server";
                }
            });

            return true;
        }

        public static ReachableStatus Reachability { get; private set; }

        public static string UnreachableReason { get; private set; }

        public static AsyncOperation DoAPICall<TArgs, TResult>(TArgs args, string api, Action<TResult> onCompleted)
            where TArgs : WebsiteArgs
            where TResult : WebsiteResponse, new()
        {
            if (!RequiredUnityApiIsAvailable) return null;

            args.OdinVersion = OdinInspectorVersion.Version;

            var json = JsonUtility.ToJson(args);
            var request = UnityWebRequest.Post(API_ENDPOINT.TrimEnd('/') + "/api/" + api, new Dictionary<string, string>()
            {
                { "json", json }
            });

            try
            {
                request.SetRequestHeader("User-Agent", "odin-inspector-unity-editor/1.0");
            }
            catch { }

            var op = SendWebRequest(request);

            SubscribeOnCompleted(op, a =>
            {
                try
                {
                    if (RequestIsError(request))
                    {
                        onCompleted(new TResult()
                        {
                            Success = false,
                            ReplyMessage = request.error,
                        });
                    }
                    else if (request.downloadHandler == null)
                    {
                        onCompleted(new TResult()
                        {
                            Success = false,
                            ReplyMessage = "No reply data received",
                        });
                    }
                    else
                    {
                        var replyJson = request.downloadHandler.text;
                        TResult result;

                        try
                        {
                            result = JsonUtility.FromJson<TResult>(replyJson);
                        }
                        catch
                        {
                            result = new TResult()
                            {
                                Success = false,
                                ReplyMessage = "Invalid json reply",
                            };
                        }
#if SIRENIX_INTERNAL
                        Debug.Log(api + ": " + replyJson);
#endif
                        onCompleted(result);
                    }
                }
                finally
                {
                    request.Dispose();
                }
            });

            return op;
        }

        public static AsyncOperation GetLicenseKeyStatusAsync(LicenseInfo license, Action<LicenseKeyStatusResponse> onComplete)
        {
            return DoAPICall<LicenseKeyStatusArgs, LicenseKeyStatusResponse>(new LicenseKeyStatusArgs()
            {
                ClaimToken = license.ClaimToken,
                MachineId = SystemInfo.deviceUniqueIdentifier,
                MachineName = license.MachineName,
                OdinVersion = OdinInspectorVersion.Version,
            }, "get-license-claim-status", onComplete);
        }

        public static AsyncOperation GetAvailableOrgSeatsAsync(int orgID, string orgClaimToken, Action<AvailableOrgSeatsResponse> onComplete)
        {
            return DoAPICall<WebsiteArgs, AvailableOrgSeatsResponse>(new WebsiteArgs()
            {
                OrgId = orgID,
                OrgClaimToken = orgClaimToken
            }, "get-available-org-seats", onComplete);

        }

        public static AsyncOperation ClaimOrgSeatAsync(int orgId, string orgClaimToken, string machineName, Action<SeatClaimResponse> onComplete)
        {
            return DoAPICall<WebsiteArgs, SeatClaimResponse>(new WebsiteArgs()
            {
                OrgId = orgId,
                OrgClaimToken = orgClaimToken,
                MachineName = machineName,
                MachineId = SystemInfo.deviceUniqueIdentifier
            }, "claim-org-seat", onComplete);
        }

        public static AsyncOperation ClaimSeatViaLoginAsync(string username, string password, string machineName, Action<SeatClaimResponse> onComplete)
        {
            var org = LicenseManager.OrgInfo;

            return DoAPICall<WebsiteArgs, SeatClaimResponse>(new WebsiteArgs()
            {
                Username = username,
                Password = password,
                MachineName = machineName,
                MachineId = SystemInfo.deviceUniqueIdentifier,
                OrgId = org.OrgId,
                OrgClaimToken = org.OrgClaimToken,
            }, "claim-login-seat", onComplete);
        }

        public static AsyncOperation ClaimSeatViaLicenseKeyAsync(string licenseKey, string machineName, Action<SeatClaimResponse> onComplete)
        {
            var org = LicenseManager.OrgInfo;

            return DoAPICall<WebsiteArgs, SeatClaimResponse>(new WebsiteArgs()
            {
                LicenseKey = licenseKey,
                MachineName = machineName,
                MachineId = SystemInfo.deviceUniqueIdentifier,
                OrgId = org.OrgId,
                OrgClaimToken = org.OrgClaimToken,
            }, "claim-license-key-seat", onComplete);
        }

        public static AsyncOperation ReturnLicenseAsync(LicenseInfo license, Action<ReturnLicenseResponse> onComplete)
        {
            return DoAPICall<WebsiteArgs, ReturnLicenseResponse>(new WebsiteArgs()
            {
                ClaimToken = license.ClaimToken,
                MachineId = SystemInfo.deviceUniqueIdentifier,
            }, "return-license", onComplete);
        }

        public class WebsiteArgs
        {
            public int OrgId;
            public string OrgClaimToken;
            public string MachineName;
            public string MachineId;
            public string Username;
            public string Password;
            public string LicenseKey;
            public string ClaimToken;
            public string OdinVersion;
            public bool IsKeylessClaim;
        }

        public class WebsiteResponse
        {
            public bool Success = true;
            public string ReplyMessage;
            public MessageBoxMessageType MessageBoxMessageType;
            public string MessageBoxMessageTitle;
            public string MessageBoxMessage;
            public string SuperAnnoyingPopupMessage;

            public bool HasMessageBoxMessage
            {
                get
                {
                    return !string.IsNullOrEmpty(this.MessageBoxMessageTitle) && !string.IsNullOrEmpty(this.MessageBoxMessage) && !string.IsNullOrEmpty(this.MessageBoxMessageTitle.Trim()) && !string.IsNullOrEmpty(this.MessageBoxMessage.Trim());
                }
            }
        }

        public enum MessageBoxMessageType
        {
            Info,
            Warning,
            Error
        }

        public class SeatClaimResponse : WebsiteResponse
        {
            public ResponseStatus Status;
            public string ClaimToken;
            public bool IsKeylessClaim;

            public enum ResponseStatus
            {
                ReplyInvalid,
                Accepted,
                Denied,
            }
        }

        public class ReturnLicenseResponse : WebsiteResponse
        {
            public ResponseStatus Status;
            public bool ReturnLicense;

            public enum ResponseStatus
            {
                Failure,
                Success,
            }
        }

        public class AvailableOrgSeatsResponse : WebsiteResponse
        {
            public string OrganizationName;
            public int Available;
            public int Total;
        }

        public class LicenseKeyStatusArgs : WebsiteArgs
        {
        }

        public class LicenseKeyStatusResponse : WebsiteResponse
        {
            public KeyState LicenseState;
            public bool IsKeylessClaim;
        }

        public enum KeyState
        {
            ReplyInvalid,
            Active,
            Disabled,
            Expired,
            Invalid,
        }
    }
}
#endif