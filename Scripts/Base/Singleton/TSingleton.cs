using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1.泛型
/// 2.反射
/// 3.抽象类
/// 4.命名空间
/// </summary>
namespace GFrame
{

    //如果类 T 有new约束 你就可以在泛型类定义里使用new T()方法 否则就不能使用
    public abstract class TSingleton<T> : ISingleton where T : TSingleton<T>, new()
    {
        protected static T instance = null;
        protected static object s_Lock = new object();

        public static T S
        {
            get
            {
                return Instance();
            }
        }

        public virtual void OnSingletonInit()
        {

        }

        //这个方法用到了lock，我们希望lock的代码在同一时刻只能由一个线程访问
        public static T Instance()
        {
            if (instance == null)
            {
                lock (s_Lock)
                {
                    instance = new T();
                    instance.OnSingletonInit();
                }
            }

            return instance;
        }
    }
}
