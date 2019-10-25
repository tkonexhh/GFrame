using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class AbstractRes : Refcounter, IRes, IPoolAble
    {
        protected string m_Name;
        protected UnityEngine.Object m_Asset;
        private event Action<bool, IRes> m_ResListener;
        public string name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public UnityEngine.Object asset
        {
            get { return m_Asset; }
            //set { m_Asset = value; }
        }

        protected AbstractRes(string name)
        {
            m_Name = name;
        }

        protected AbstractRes()
        {

        }

        protected override void OnZeroRef()
        {
            Debug.LogError("OnZeroRef:" + m_Name);
            ReleaseRes();
        }


        public void RegisterResListener(Action<bool, IRes> listener)
        {
            if (listener == null)
            {
                return;
            }

            m_ResListener += listener;
        }
        public void UnRegisterResListener(Action<bool, IRes> listener)
        {
            if (listener == null)
            {
                return;
            }
            if (m_ResListener == null)
            {
                return;
            }
            m_ResListener -= listener;
        }


        public virtual bool LoadSync()//同步加载
        {
            return false;
        }
        public virtual void LoadAsync()//异步加载
        {

        }

        public string[] GetDependResList()//获取AB的依赖项
        {
            return null;
        }

        public bool ReleaseRes()
        {
            m_ResListener = null;
            return true;
        }


        public virtual void Recycle2Cache()
        {

        }

        public virtual void OnCacheReset()
        {

        }
    }
}




