using System;

namespace MVPFrameWork
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoBuildAttribute : Attribute
    {
        public string name;

        public AutoBuildAttribute()
        {
        }

        public AutoBuildAttribute(string name)
        {
            this.name = name;
        }
    }
}