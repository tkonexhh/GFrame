using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;


namespace GFrame
{

    /// <summary>
    /// 反射工具类
    /// </summary>
    public static class ReflectionHelper
    {

        public static Type GetType(string name)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //assemblies.get
            var defaultAssembly = assemblies.First(assembly => assembly.GetName().Name == "Assembly-CSharp");
            Type type = defaultAssembly.GetType(name);
            if (type == null)
            {
                Log.e("#Not Find Type at:" + name);
            }
            #region 
            // Type type = null;
            // foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            // {
            //     Type tt = asm.GetType(name);
            //     if (tt != null)
            //     {
            //         type = tt;
            //         break;
            //     }
            // }
            #endregion

            return type;
        }

        public static void Invoke(object obj, Type type, String methodName, object[] parameters = null)
        {
            //object obj = Instance(type);
            MethodInfo method = type.GetMethod(methodName);
            if (method == null) return;
            method.Invoke(obj, parameters);
        }


        /// <summary>
        /// 如果传递的type具有泛型形参, 那么返回泛型形参对象的实例, 否则返回当前type的实例
        /// </summary>
        /// <param name="type"></param> 
        /// <returns></returns>
        public static Object Instance(Type type)
        {
            Type paramType = type.GetGenericArguments()[0];
            type = paramType == null ? type : paramType;
            return Activator.CreateInstance(type, true);
        }

        /// <summary>
        /// 获取所有方法(公有方法和私有方法)
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethods(Type type)
        {
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
            return type.GetMethods(bindingFlags);
        }


        /// <summary>
        /// 获取包括父类（但是除开Object）的所有字段
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<FieldInfo> GetFields(Type type)
        {
            List<FieldInfo> list = new List<FieldInfo>();
            list.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));
            while ((type = type.BaseType) != typeof(Object))
            {
                list.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static));
            }
            return list;
        }
    }
}




