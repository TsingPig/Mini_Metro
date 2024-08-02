using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TsingPigSDK
{
    public static class LayoutGroupExtension
    {
        /// <summary>
        /// 强制刷新布局
        /// </summary>
        /// <param name="layout"></param>
        public static void RebuildLayout(this LayoutGroup layout)
        {
            Transform root = layout.transform;
            List<Transform> transList = root.GetAllChildsByComponent();
            foreach(Transform trans in transList)
            {
                RectTransform rectTrans = trans.GetComponent<RectTransform>();
                if(null != rectTrans)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(rectTrans);
                }
            }
        }
    }
}