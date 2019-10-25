using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class Timer : TMonoSingleton<Timer>
    {
        List<TimeItem> m_UnScaleTimeItems = new List<TimeItem>();
        List<TimeItem> m_ScaleTimeItems = new List<TimeItem>();
        private float m_CurrentUnScaleTime = -1;
        private float m_CurrentScaleTime = -1;

        public override void OnSingletonInit()
        {
            m_UnScaleTimeItems.Clear();
            m_ScaleTimeItems.Clear();
            m_CurrentUnScaleTime = Time.unscaledTime;
            m_CurrentScaleTime = Time.time;
        }

        public void ResetMgr()
        {
            m_UnScaleTimeItems.Clear();
            m_ScaleTimeItems.Clear();
        }


        #region 投递受时间缩放影响的定时器
        public int Post2Scale(Run<int> callback, float delay, int repeat = 1)
        {
            TimeItem item = TimeItem.Allocate(callback, delay, repeat);
            Post2Scale(item);
            return item.id;
        }

        private void Post2Scale(TimeItem item)
        {
            item.SortScore = m_CurrentScaleTime + item.DelayTime();
            m_ScaleTimeItems.Add(item);
        }
        #endregion

        #region 投递不受时间缩放影响的定时器
        public int Post2Really(Run<int> callback, float delay, int repeat = 1)
        {
            TimeItem item = TimeItem.Allocate(callback, delay, repeat);
            Post2Really(item);
            return item.id;
        }

        private void Post2Really(TimeItem item)
        {
            item.SortScore = m_CurrentUnScaleTime + item.DelayTime();
            m_UnScaleTimeItems.Add(item);
        }
        #endregion

        public bool Cancel(int id)
        {
            return true;
        }

        private void Update()
        {
            m_CurrentUnScaleTime = Time.unscaledTime;
            m_CurrentScaleTime = Time.time;

            TickTimeItems(m_UnScaleTimeItems, m_CurrentUnScaleTime);
            TickTimeItems(m_ScaleTimeItems, m_CurrentScaleTime);

        }

        private void TickTimeItems(List<TimeItem> items, float time)
        {
            if (items.Count == 0) return;
            for (int i = items.Count - 1; i >= 0; --i)
            {
                var item = items[i];
                if (item.SortScore < time)
                {
                    items.Remove(item);
                    item.OnTimeTick();

                    if (item.NeedRepeat())
                    {
                        Post2Really(item);
                    }
                    else
                    {
                        item.Recycle2Cache();
                    }
                }
            }
        }
    }
}




