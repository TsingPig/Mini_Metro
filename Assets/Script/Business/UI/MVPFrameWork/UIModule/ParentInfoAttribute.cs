using System;

namespace MVPFrameWork
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ParentInfoAttribute : Attribute
    {
        public FindType type = FindType.None;

        public string param = string.Empty;

        public ParentInfoAttribute()
        {
        }

        public ParentInfoAttribute(FindType type, string param)
        {
            this.type = type;
            this.param = param;
        }
    }
}