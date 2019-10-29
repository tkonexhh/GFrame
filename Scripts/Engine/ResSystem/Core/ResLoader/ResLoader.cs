using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class ResLoader : IResLoader, IPoolAble, IPoolType
    {
        private static List<ResLoader> s_ActiveLoaderLst = new List<ResLoader>();


        private List<IRes> m_ResLst = new List<IRes>();
        private LinkedList<IRes> m_WaitLoadList = new LinkedList<IRes>();
        private string m_LoaderName;
        private int m_LoadingCount;

        public static ResLoader Allocate(string name = null)
        {
            ResLoader loader = ObjectPool<ResLoader>.S.Allocate();
            loader.m_LoaderName = name;
            s_ActiveLoaderLst.Add(loader);
            return loader;
        }

        public void PreloadRes()
        {
            while (m_WaitLoadList.Count > 0)
            {
                IRes first = m_WaitLoadList.First.Value;
                m_WaitLoadList.RemoveFirst();
                --m_LoadingCount;
                first.LoadSync();
            }
        }

        public UnityEngine.Object LoadSync(string name)
        {
            Add2Load(name);
            PreloadRes();

            IRes res = ResMgr.S.GetRes(name);

            if (res == null)
            {
                Log.e("#Failed to Load Res:" + name);
                return null;
            }

            return res.asset;
        }

        public void LoadSync()
        {

        }

        public void LoadAsync()//异步加载资源
        {
            //DoLoadAsync();
            IRes res = m_WaitLoadList.First.Value;
        }


        public bool Add2Load(string name, Action<bool, IRes> listener = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                Log.e("#Res Name Is Null");
                return false;
            }

            IRes res = FindResInArray(name);
            if (res != null)
            {
                //已经加载完毕了

                return true;
            }

            res = ResMgr.S.GetRes(name);
            if (res == null)
            {
                return false;
            }
            if (listener != null)
            {
                res.RegisterResListener(listener);
            }
            AddResToArray(res);

            return false;
        }

        public void Add2Load(List<string> names)
        {
            if (names == null)
            {
                return;
            }

            for (int i = 0; i < names.Count; i++)
            {
                Add2Load(names[i]);
            }
        }


        private IRes FindResInArray(string name)
        {
            for (int i = m_ResLst.Count - 1; i >= 0; --i)
            {
                if (m_ResLst[i].name.Equals(name))
                {
                    return m_ResLst[i];
                }
            }
            return null;
        }

        private void AddResToArray(IRes res)
        {
            res.AddRef();
            m_ResLst.Add(res);
            ++m_LoadingCount;
            m_WaitLoadList.AddLast(res);
        }

        public void ReleaseAllRes()
        {
            for (int i = m_ResLst.Count - 1; i >= 0; --i)
            {
                m_ResLst[i].SubRef();
                //ReleaseRes(m_ResLst[i]);
            }
            m_ResLst.Clear();
            m_LoadingCount = 0;
            m_WaitLoadList.Clear();
        }

        // public void ReleaseRes(string assetName)
        // {

        // }

        public void OnCacheReset()
        {
            s_ActiveLoaderLst.Remove(this);
            ReleaseAllRes();

        }

        public void Recycle2Cache()
        {
            ObjectPool<ResLoader>.S.Recycle(this);
        }
    }
}




