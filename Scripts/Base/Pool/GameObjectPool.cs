using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class GameObjectPool : IPool<GameObject>, IPoolType
    {
        private Transform m_Root;
        private string m_PoolName;
        private GameObject m_Prefeb;
        private int m_MaxCount;
        private Stack<GameObject> m_CacheStack;

        public void InitPool(Transform parent, string poolName, GameObject prefeb, int maxCount, int initCount)
        {
            if (m_Prefeb != null)
            {
                Log.w("Already exist same pool");
                return;
            }

            if (prefeb == null)
            {
                Log.e("Prefeb:{0} not exist", prefeb);
                return;
            }

            m_PoolName = poolName;
            m_Prefeb = prefeb;
            m_MaxCount = maxCount;

            GameObject rootGO = new GameObject();
            rootGO.name = m_PoolName;
            m_Root = rootGO.transform;
            m_Root.SetParent(parent);

            if (initCount > 0)
            {
                for (int i = 0; i < initCount; i++)
                {
                    Recycle(CreateNewGameObject());
                }
            }
        }

        private GameObject CreateNewGameObject()
        {
            GameObject gameObject = GameObject.Instantiate(m_Prefeb);
            gameObject.name = m_PoolName;
            gameObject.transform.SetParent(m_Root);
            return gameObject;
        }

        public GameObject Allocate()
        {
            GameObject result;
            if (m_CacheStack == null || m_CacheStack.Count == 0)
            {
                result = CreateNewGameObject();
            }
            else
            {
                result = m_CacheStack.Pop();
            }
            return result;
        }


        public bool Recycle(GameObject t)
        {
            if (t == null)
                return false;
            if (m_CacheStack == null)
                m_CacheStack = new Stack<GameObject>();
            else if (m_MaxCount > 0)
            {
                if (m_CacheStack.Count >= m_MaxCount)
                {
                    return false;
                }
            }
            m_CacheStack.Push(t);
            return true;
        }

        public void RemoveAllObj(bool destroySelf)
        {
            if (destroySelf)
            {
                GameObject.Destroy(m_Root.gameObject);
            }


            if (m_CacheStack == null || m_CacheStack.Count == 0)
                return;

            while (m_CacheStack.Count > 0)
            {
                GameObject obj = m_CacheStack.Pop();
                GameObject.Destroy(obj);
            }
        }

        public void Recycle2Cache()
        {

        }

    }
}




