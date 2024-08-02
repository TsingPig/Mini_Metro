using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using Debug = UnityEngine.Debug;

namespace TsingPigSDK
{
    public static partial class Log
    {
        public static void CallInfo(string msg = "")
        {
            MethodBase callingMethod = new StackTrace().GetFrame(1).GetMethod();
            Type callingType = callingMethod.DeclaringType;
            //Debug.Log($"{callingType.Name} : {callingMethod.Name}    Msg:{msg}");
            Info(callingType.Name, callingMethod.Name, $"  Msg : {msg}");
        }

        public static void Error(string msg = "")
        {
            Debug.LogError($"错误：{msg}");
        }

        public static void Info(params string[] strings)
        {
            string result = string.Join(" ", strings);
            Debug.Log(result);
        }

        public static void Warning(params string[] strings)
        {
            string result = string.Join(" ", strings);
            Debug.LogWarning(result);
        }

        /// <summary>
        /// 打印SQL查询结果
        /// </summary>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        public static void LogQueryResult(DataSet queryResult)
        {
            // 输出查询结果
            if(queryResult != null && queryResult.Tables.Count > 0)
            {
                foreach(DataRow row in queryResult.Tables[0].Rows)
                {
                    List<string> rowData = new List<string>();
                    foreach(var col in row.ItemArray)
                    {
                        rowData.Add(col.ToString());
                    }
                    Info($"Query Result: {string.Join(", ", rowData)}");
                }
            }
        }
    }
}