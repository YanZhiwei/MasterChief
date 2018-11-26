using MasterChief.DotNet4.Utilities.Common;
using System.Collections;

namespace MasterChief.DotNet4.Utilities.DesignPattern
{
    /// <summary>
    /// 业务工厂
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// 时间：2016-01-07 13:43
    /// 备注：
    public class BusinessFactory<T>
        where T : class, new()
    {
        #region Fields

        private static Hashtable _businessCache = new Hashtable();
        private static readonly object syncRoot = new object();

        #endregion Fields

        #region Properties

        /// <summary>
        /// 实例化
        /// </summary>
        /// 时间：2016-01-07 13:45
        /// 备注：
        public static T Instance
        {
            get
            {
                string fullName = typeof(T).FullName;
                T business = (T)_businessCache[fullName];

                if (business == null)
                {
                    lock (syncRoot)
                    {
                        if (business == null)
                        {
                            business = ReflectHelper.CreateInstance<T>(typeof(T).FullName, typeof(T).Assembly.FullName);
                            _businessCache.Add(typeof(T).FullName, business);
                        }
                    }
                }

                return business;
            }
        }

        #endregion Properties
    }
}