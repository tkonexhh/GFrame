using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class TimeItem : IPoolAble, IPoolType
    {
        private static int s_NextID;
        private static Dictionary<int, TimeItem> s_TimeItemMap = new Dictionary<int, TimeItem>();


        private float m_SortScore;

        private Run<int> m_Callback;
        private int m_RepeatCount;
        private float m_DelayTime;
        private int m_CallbackTick;//循环次数

        private int m_ID = -1;
        public int id
        {
            get { return m_ID; }
            private set { m_ID = value; }
        }

        public float SortScore
        {
            get { return m_SortScore; }
            set { m_SortScore = value; }
        }

        public float DelayTime()
        {
            return m_DelayTime;
        }

        public bool NeedRepeat()
        {
            return m_RepeatCount != 0;
        }

        public static TimeItem Allocate(Run<int> callback, float delayTime, int repeatCount = 1)
        {
            TimeItem item = ObjectPool<TimeItem>.S.Allocate();
            item.Set(callback, delayTime, repeatCount);
            return item;
        }

        public void Set(Run<int> callback, float delayTime, int repeatCount = 1)
        {
            m_CallbackTick = 0;
            m_Callback = callback;
            m_DelayTime = delayTime;
            m_RepeatCount = repeatCount;
            RegisterActiveTimeItem(this);
        }

        public void Cancel()
        {

        }

        public void OnTimeTick()
        {
            if (m_Callback != null)
            {
                m_Callback(++m_CallbackTick);
            }

            if (m_RepeatCount > 0)
            {
                --m_RepeatCount;
            }
        }

        private void RegisterActiveTimeItem(TimeItem timeItem)
        {
            timeItem.id = ++s_NextID;
            s_TimeItemMap.Add(timeItem.id, timeItem);
        }

        private void UnRegisterActiveTimeItem(TimeItem timeItem)
        {
            if (s_TimeItemMap.ContainsKey(timeItem.id))
            {
                s_TimeItemMap.Remove(timeItem.id);
            }

            timeItem.id = -1;
        }


        public void Recycle2Cache()
        {
            ObjectPool<TimeItem>.S.Recycle(this);
        }

        public void OnCacheReset()
        {
            m_CallbackTick = 0;
            m_Callback = null;
            m_DelayTime = 0;
            m_RepeatCount = 0;
            UnRegisterActiveTimeItem(this);
            m_ID = -1;
        }
    }
}




