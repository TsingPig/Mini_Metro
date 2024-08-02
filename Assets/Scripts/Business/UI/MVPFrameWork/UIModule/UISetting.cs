using MVPFrameWork.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MVPFrameWork
{
    public static class UISetting
    {
        public static IResourceLoader DefaultResoucesLoader = Singleton<ResManager>.Instance;

        public static ParentParam DefaultParentParam = ParentParam.Default;
    }

    internal static class NodeContainer
    {
        private static Dictionary<string, Transform> _tagDic = new Dictionary<string, Transform>();

        private static Dictionary<string, Transform> _nameDic = new Dictionary<string, Transform>();

        public static Transform FindNodeWithTag(string tag)
        {
            Transform value = null;
            _tagDic?.TryGetValue(tag, out value);
            if(value == null)
            {
                value = GameObject.FindWithTag(tag).transform;
                _tagDic[tag] = value;
            }

            return value;
        }

        public static Transform FindNodeWithName(string name)
        {
            Transform value = null;
            _nameDic?.TryGetValue(name, out value);
            if(value == null)
            {
                GameObject gameObject = GameObject.Find(name);
                Assert.IsNotNull(gameObject, "NodeContainer.FindNodeWithName error:" + name);
                value = gameObject.transform;
                _nameDic[name] = value;
            }

            return value;
        }
    }

    public class ParentParam
    {
        private static ParentParam _default = new ParentParam(FindType.FindWithName, "Canvas");

        public FindType findType;

        public string param;

        public static ParentParam Default => _default;

        public ParentParam(FindType findType, string param)
        {
            this.findType = findType;
            this.param = param;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ResInfoAttribute : Attribute
    {
        public string assetPath;

        public bool async = false;

        public ResInfoAttribute()
        {
        }

        public ResInfoAttribute(string assetPath)
        {
            this.assetPath = assetPath;
        }

        public ResInfoAttribute(string assetPath, bool async)
        {
            this.assetPath = assetPath;
            this.async = async;
        }
    }
}