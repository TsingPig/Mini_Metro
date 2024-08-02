using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TsingPigSDK
{
    public static class TransfromExtension
    {
        /// <summary>
        /// 递归获得所有子孙物体
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="transList"></param>
        /// <returns></returns>
        public static List<Transform> GetAllChildsByComponent(this Transform trans, List<Transform> transList = null)
        {
            if(null == transList)
            {
                transList = new List<Transform>();
            }
            if(null != trans)
            {
                for(int i = 0; i < trans.childCount; i++)
                {
                    Transform child = trans.GetChild(i);
                    if(null != child.GetComponent<LayoutGroup>())
                    {
                        transList.Add(child);
                    }
                    GetAllChildsByComponent(child, transList);
                }
            }
            return transList;
        }
    }
}