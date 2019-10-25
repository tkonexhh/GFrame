using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class ObjectPool<T> : TSingleton<ObjectPool<T>>, IPool<T> where T : IPoolAble, new()
    {

        private int m_MaxCount;
        private Stack<T> m_CacheStack;



        public void Init(int maxCount, int initCount)
        {
            if (m_MaxCount > 0)
            {
                initCount = Mathf.Min(maxCount, initCount);
            }
            m_MaxCount = maxCount;
            if (currentCount < initCount)
            {
                for (int i = currentCount; i < initCount; i++)
                {
                    Recycle(new T());
                }
            }
        }

        public int currentCount
        {
            get
            {
                if (m_CacheStack == null)
                    return 0;
                return m_CacheStack.Count;
            }
        }

        public T Allocate()
        {
            T result;
            if (m_CacheStack == null || m_CacheStack.Count == 0)
            {
                result = new T();
            }
            else
            {
                result = m_CacheStack.Pop();
            }
            return result;
        }


        public bool Recycle(T t)
        {
            if (t == null)
                return false;

            if (m_CacheStack == null)
            {
                m_CacheStack = new Stack<T>();
            }
            else if (m_MaxCount > 0)
            {
                if (m_CacheStack.Count >= m_MaxCount)
                {
                    t.OnCacheReset();
                    return false;
                }
            }

            t.OnCacheReset();
            m_CacheStack.Push(t);
            return true;
        }
    }
}




