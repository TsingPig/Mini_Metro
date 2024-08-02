using System;

namespace MVPFrameWork.Singleton
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T _instance;

        private static object _locker = new object();

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(_locker)
                    {
                        if(_instance == null)
                        {
                            _instance = Activator.CreateInstance<T>();
                        }
                    }
                }

                return _instance;
            }
        }

        protected Singleton()
        {
            if(GetType() != typeof(T))
            {
                throw new InvalidOperationException($"type:{GetType()},T:{typeof(T)}");
            }
        }
    }
}