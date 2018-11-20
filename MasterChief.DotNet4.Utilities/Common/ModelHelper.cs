namespace MasterChief.DotNet4.Utilities.Common
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    /// <summary>
    /// 实体类帮助类
    /// </summary>
    public static class ModelHelper
    {
        #region Methods

        /// <summary>
        /// 实体类数值内容比较
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类对象</param>
        /// <param name="othModel">实体类对象</param>
        /// <returns>是否数值内容比较</returns>
        /// 时间：2016/8/25 13:32
        /// 备注：
        public static bool CompletelyEqual<T>(T model, T othModel)
            where T : class
        {
            if (null == model || null == othModel)
            {
                return false;
            }

            return SerializeToString(model).Equals(SerializeToString(othModel));
        }

        /// <summary>
        /// 对象深拷贝
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类对象</param>
        /// <returns>对象</returns>
        public static T DeepCopy<T>(T model)
            where T : class
        {
            /*
             *相关知识：
             *C# 支持两种类型：“值类型”和“引用类型”。
             *值类型（Value Type)（如 char、int 和 float）、枚举类型和结构类型。
             *引用类型(Reference Type) 包括类 (Class) 类型、接口类型、委托类型和数组类型。
             *如何来划分它们？
             *以它们在计算机内存中如何分配来划分
             *1,值类型的变量直接包含其数据。
             *2,引用类型的变量则存储对象引用。
             *对于引用类型，两个变量可能引用同一个对象，因此对一个变量的操作可能影响另一个变量所引用的对象。
             *对于值类型，每个变量都有自己的数据副本，对一个变量的操作不可能影响另一个变量。
             *
             *堆栈(stack)是一种先进先出的数据结构，在内存中，变量会被分配在堆栈上来进行操作。
             *堆(heap)是用于为类型实例(对象)分配空间的内存区域，在堆上创建一个对象，会将对象的地址传给堆栈上的变量(反过来叫变量指向此对象，或者变量引用此对象)。
             *
             *浅拷贝：是指将对象中的所有字段逐字复杂到一个新对象
             *对值类型字段只是简单的拷贝一个副本到目标对象，改变目标对象中值类型字段的值不会反映到原始对象中，因为拷贝的是副本。
             *对引用型字段则是指拷贝他的一个引用到目标对象。改变目标对象中引用类型字段的值它将反映到原始对象中，因为拷贝的是指向堆是上的一个地址。
             *
             *深拷贝：深拷贝与浅拷贝不同的是对于引用字段的处理，深拷贝将会在新对象中创建一个新的对象和
             *原始对象中对应字段相同（内容相同）的字段，也就是说这个引用和原始对象的引用是不同， 我们改变新
             *对象中这个字段的时候是不会影响到原始对象中对应字段的内容。
             *
             *浅复制： 实现浅复制需要使用Object类的MemberwiseClone方法用于创建一个浅表副本
             *深复制： 须实现 ICloneable接口中的Clone方法，且需要需要克隆的对象加上[Serializable]特性
             *以上参考：http://www.cnblogs.com/huangting2009/archive/2009/03/13/1410634.html
             */

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, model);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 将对象序列化成字符串
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="model">实体类对象</param>
        /// <returns>字符串</returns>
        /// 时间：2016/8/25 13:25
        /// 备注：
        public static string SerializeToString<T>(T model)
            where T : class
        {
            Type type = typeof(T);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            StringBuilder builder = new StringBuilder();

            foreach (FieldInfo field in fields)
            {
                object fvalue = field.GetValue(model);
                builder.Append(field.Name + ":" + fvalue + ";");
            }

            return builder.ToString();
        }

        #endregion Methods
    }
}