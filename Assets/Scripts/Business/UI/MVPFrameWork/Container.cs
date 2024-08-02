using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MVPFrameWork
{
    public static class Container
    {
        private interface ITypeNode
        {
        }

        private class SingletonTypeNode : ITypeNode
        {
            private object _obj;

            private Type _type;

            public object Obj
            {
                get
                {
                    if(_obj == null)
                    {
                        _obj = Activator.CreateInstance(_type);
                        GenerateInterfaceField(_obj);
                    }

                    return _obj;
                }
            }

            public SingletonTypeNode(Type objType)
            {
                _type = objType;
            }

            public SingletonTypeNode(object obj)
            {
                _obj = obj;
            }
        }

        private class NormalTypeNode : ITypeNode
        {
            public Type objType;

            public NormalTypeNode(Type objType)
            {
                this.objType = objType;
            }
        }

        private static readonly Dictionary<Type, Dictionary<string, ITypeNode>> _dic = new Dictionary<Type, Dictionary<string, ITypeNode>>(16);

        public static void Regist<T, V>(string name = null) where V : class, T, new()
        {
            Regist<T>(name, new NormalTypeNode(typeof(V)));
        }

        public static void Regist<T, V>(object name) where V : class, T, new()
        {
            Regist<T, V>(name?.ToString());
        }

        public static void RegistSingleton<T, V>(V obj, string name) where V : class, T, new()
        {
            SingletonTypeNode singletonTypeNode = null;
            singletonTypeNode = ((obj == null) ? new SingletonTypeNode(typeof(V)) : new SingletonTypeNode(obj));
            Regist<T>(name, singletonTypeNode);
        }

        public static void RegistSingleton<T, V>() where V : class, T, new()
        {
            SingletonTypeNode node = new SingletonTypeNode(typeof(V));
            Regist<T>(string.Empty, node);
        }

        public static void RegistSingleton<T, V>(string name) where V : class, T, new()
        {
            RegistSingleton<T, V>(null, name);
        }

        public static void RegistSingleton<T, V>(V obj) where V : class, T, new()
        {
            RegistSingleton<T, V>(obj, string.Empty);
        }

        public static T Resolve<T>(string name = null)
        {
            return (T)Resolve(typeof(T), name);
        }

        public static T Resolve<T>(object name)
        {
            return Resolve<T>(name?.ToString());
        }

        public static object Resolve(Type type, string name = null)
        {
            object obj = null;
            name = (string.IsNullOrEmpty(name) ? string.Empty : name);
            Dictionary<string, ITypeNode> value = null;

            if(_dic.TryGetValue(type, out value))
            {
                ITypeNode value2 = null;
                value?.TryGetValue(name, out value2);
                if(value2 is NormalTypeNode)
                {
                    NormalTypeNode normalTypeNode = value2 as NormalTypeNode;
                    obj = Activator.CreateInstance(normalTypeNode.objType);
                    GenerateInterfaceField(obj);
                }
                else if(value2 is SingletonTypeNode)
                {
                    SingletonTypeNode singletonTypeNode = value2 as SingletonTypeNode;
                    obj = singletonTypeNode.Obj;
                }
            }

            if(obj == null)
            {
                Debug.LogErrorFormat("<Ming> ## Uni Error ## Cls:Container Func:Resolve Type:{0}{1} Info:Unregistered", type, (!string.IsNullOrEmpty(name)) ? (" Name:" + name) : string.Empty);
            }

            return obj;
        }

        private static void Regist<T>(string name, ITypeNode node)
        {
            name = (string.IsNullOrEmpty(name) ? string.Empty : name);
            Type typeFromHandle = typeof(T);
            if(!_dic.TryGetValue(typeFromHandle, out var value))
            {
                value = new Dictionary<string, ITypeNode>();
                _dic[typeFromHandle] = value;
            }
            value[name] = node;
        }

        private static void GenerateInterfaceField(object target)
        {
            GenerateInterfaceField(target, target?.GetType());
        }

        private static void GenerateInterfaceField(object target, Type type)
        {
            if(target == null || type == null)
            {
                return;
            }

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if(fields != null && fields.Length != 0)
            {
                foreach(FieldInfo fieldInfo in fields)
                {
                    if(!(fieldInfo != null) || !fieldInfo.FieldType.IsInterface)
                    {
                        continue;
                    }

                    object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(AutoBuildAttribute), inherit: true);
                    string name = null;
                    if(customAttributes == null || customAttributes.Length == 0)
                    {
                        continue;
                    }

                    object[] array = customAttributes;
                    for(int j = 0; j < array.Length; j++)
                    {
                        AutoBuildAttribute autoBuildAttribute = (AutoBuildAttribute)array[j];
                        if(autoBuildAttribute != null)
                        {
                            name = autoBuildAttribute.name;
                            break;
                        }
                    }

                    fieldInfo.SetValue(target, Resolve(fieldInfo.FieldType, name));
                }
            }

            GenerateInterfaceField(target, type.BaseType);
        }
    }
}