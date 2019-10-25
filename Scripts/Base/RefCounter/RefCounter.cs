using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{
    //引用计数器
    public interface IRefCounter
    {
        int refCount
        {
            get;
        }

        void AddRef();
        void SubRef();
    }


    public class Refcounter : IRefCounter
    {
        public int m_RefCount = 0;
        public int refCount
        {
            get { return m_RefCount; }
        }

        public void AddRef()
        {
            ++m_RefCount;
        }

        public void SubRef()
        {
            --m_RefCount;
            if (m_RefCount == 0)
            {
                OnZeroRef();
            }
        }

        protected virtual void OnZeroRef()
        {

        }
    }
}




