using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class UIData
    {
        string m_Name;
        int m_UIID;

        bool m_ISABMode = false;


        public UIData(int uiId, string name, bool abMode)
        {
            m_UIID = uiId;
            m_Name = name;
            m_ISABMode = abMode;
        }

        public string name
        {
            get { return m_Name; }
        }
        public int UIID
        {
            get { return m_UIID; }
        }

        public string fullPath
        {
            get
            {
                if (m_ISABMode)
                {
                    return m_Name;
                }

                return m_Name;
            }
        }
    }

    public class UIDataTable
    {
        private static Dictionary<int, UIData> m_UIDataMap = new Dictionary<int, UIData>();

        private static bool s_IsABMode = false;

        public static void SetABMode(bool abMode)
        {
            s_IsABMode = abMode;
        }

        public static void AddPanelData<T>(T uiid, string name) where T : IConvertible
        {
            Add(new UIData(uiid.ToInt32(null), name, s_IsABMode));
        }

        public static void Add(UIData data)
        {
            if (data == null)
            {
                return;
            }

            if (m_UIDataMap.ContainsKey(data.UIID))
            {
                Log.w("Already Add UIData:" + data.UIID);
                return;
            }

            m_UIDataMap.Add(data.UIID, data);
        }

        public static UIData Get<T>(T uiId) where T : IConvertible
        {
            UIData result = null;
            if (m_UIDataMap.TryGetValue(uiId.ToInt32(null), out result))
            {
                return result;
            }
            Log.e("Not found UIData by ID:" + uiId);
            return null;
        }
    }
}
