namespace MasterChief.DotNet4.Utilities.DesignPattern
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 泛型单列
    /// </summary>
    /// <seealso cref="MasterChief.DotNet4.Utilities.DesignPattern.Singleton" />
    public class Singleton<T> : Singleton
        where T : class
    {
        #region Fields

        private static readonly object _syncRoot = new object();

        private static T _instance;

        #endregion Fields

        #region Properties

        /// <summary>
        /// 泛型实例
        /// </summary>
        public static T Instance
        {
            get => _instance;

            set
            {
                _instance = value;
                AllSingletons[typeof(T)] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 若对象为NULL，则会自动创建
        /// </summary>
        /// <returns></returns>
        public static T CreateInstance()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = Activator.CreateInstance<T>();
                        AllSingletons[typeof(T)] = _instance;
                    }
                }
            }

            return _instance;
        }

        #endregion Methods
    }

    /// <summary>
    /// 单例字典
    /// </summary>
    /// <seealso cref="MasterChief.DotNet4.Utilities.DesignPattern.Singleton" />
    public class Singleton
    {
        #region Fields

        /// <summary>
        /// 所有字典单例
        /// </summary>
        public static IDictionary<Type, object> AllSingletons => allSingletons;

        private static readonly IDictionary<Type, object> allSingletons;

        #endregion Fields

        #region Constructors

        static Singleton()
        {
            allSingletons = new Dictionary<Type, object>();
        }

        #endregion Constructors
    }

    /// <summary>
    /// 单例字典
    /// </summary>
    /// <typeparam name="TKey">字典Key类型</typeparam>
    /// <typeparam name="TValue">字典Value类型.</typeparam>
    public class SingletonDictionary<TKey, TValue> : Singleton<IDictionary<TKey, TValue>>
    {
        #region Fields

        /// <summary>
        /// 泛型实例
        /// </summary>
        public static new IDictionary<TKey, TValue> Instance => Singleton<Dictionary<TKey, TValue>>.Instance;

        #endregion Fields

        #region Constructors

        static SingletonDictionary()
        {
            Singleton<Dictionary<TKey, TValue>>.Instance = new Dictionary<TKey, TValue>();
        }

        #endregion Constructors
    }

    /// <summary>
    /// 单例列表
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    public class SingletonList<T> : Singleton<IList<T>>
        where T : class, new()
    {
        #region Fields

        /// <summary>
        /// 泛型实例
        /// </summary>
        public static new IList<T> Instance => Singleton<IList<T>>.Instance;

        #endregion Fields

        #region Constructors

        static SingletonList()
        {
            Singleton<IList<T>>.Instance = new List<T>();
        }

        #endregion Constructors
    }
}