using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{
    [TMonoSingletonAttribute("[Tools]/GameObjectPoolMgr")]
    public class GameObjectPoolMgr : TMonoSingleton<GameObjectPoolMgr>
    {

        Dictionary<string, GameObjectPool> m_PoolMap;

        public override void OnSingletonInit()
        {
            m_PoolMap = new Dictionary<string, GameObjectPool>();
        }
        public GameObjectPool AddPool(string poolName, GameObject prefeb, int maxCount, int initCount)
        {
            if (m_PoolMap.ContainsKey(poolName))
            {
                Log.w("Add Same pool");
            }

            GameObjectPool pool = new GameObjectPool();
            pool.InitPool(transform, poolName, prefeb, maxCount, initCount);
            m_PoolMap.Add(poolName, pool);
            return pool;
        }

        public void RemovePool(string poolName)
        {
            GameObjectPool pool = null;
            if (!m_PoolMap.TryGetValue(poolName, out pool))
            {
                Log.e("RemovePool No Pool");
            }

            pool.RemoveAllObj(true);
            m_PoolMap.Remove(poolName);
        }

        public void RemoveAllPool()
        {
            foreach (var pool in m_PoolMap)
            {
                pool.Value.RemoveAllObj(true);
            }

            m_PoolMap.Clear();
        }

        public GameObject Allocate(string poolName)
        {
            GameObjectPool pool = null;

            if (!m_PoolMap.TryGetValue(poolName, out pool))
            {
                Log.e("Allocate No Pool at:" + poolName);
                return null;
            }

            return pool.Allocate();
        }

        public void Recycle(string poolName, GameObject obj)
        {
            GameObjectPool pool = null;
            if (!m_PoolMap.TryGetValue(poolName, out pool))
            {
                Log.e("Recycle No Pool");
            }
            pool.Recycle(obj);
        }

        public void Recycle(GameObject obj)
        {
            Recycle(obj.name, obj);
        }
    }
}




