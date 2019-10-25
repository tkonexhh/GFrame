using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class SignSystem : TSingleton<SignSystem>
    {
        private SignState m_WeekSign;
        private SignState m_MonthSign;

        public SignState weekSign
        {
            get
            {
                if (m_WeekSign == null)
                {
                    m_WeekSign = new SignState("week", 6);
                }
                return m_WeekSign;
            }
        }

        public SignState monthSign
        {
            get
            {
                if (m_MonthSign == null)
                {
                    m_MonthSign = new SignState("week", 29);
                }
                return m_MonthSign;
            }
        }

        public void InitSignSystem()
        {

        }

        #region 内部签到类
        public class SignState
        {
            private string m_SignKey;
            private string m_TimeKey;
            private string m_Name;
            private int m_MaxIndex;//最大天数，从0开始
            private int m_LastSignIndex = -1;
            private int m_SignableIndex = 0;
            public SignState(string name, int maxIndex)
            {
                m_Name = name;
                m_MaxIndex = maxIndex;
                m_SignKey = string.Format("sign_key_{0}", m_Name);
                m_TimeKey = string.Format("sign_time_{0}", m_Name);
                Load();
            }

            public bool isSignAble
            {
                get
                {
                    return m_SignableIndex >= 0;
                }
            }

            public int signAbleIndex
            {
                get
                {
                    return m_SignableIndex;
                }
            }

            public void Reset()
            {
                m_LastSignIndex = -1;
                m_SignableIndex = 0;
                Save();
            }

            public void Save()
            {
                PlayerPrefs.SetInt(m_SignKey, m_SignableIndex);
                PlayerPrefs.SetString(m_TimeKey, DateTime.Today.ToShortDateString());
            }

            public void Load()
            {
                m_LastSignIndex = PlayerPrefs.GetInt(m_SignKey, -1);
                string timeStr = PlayerPrefs.GetString(m_TimeKey, "");
                DateTime lastSignDate;
                if (!string.IsNullOrEmpty(timeStr))
                {
                    if (DateTime.TryParse(timeStr, out lastSignDate))
                    {
                        DateTime today = DateTime.Today;
                        TimeSpan pass = today - lastSignDate;
                        if (pass.Days < 1)//今天已经签到过
                        {
                            m_SignableIndex = -1;
                        }
                        else if (pass.Days == 1)
                        {
                            m_SignableIndex = m_LastSignIndex + 1;
                            if (m_SignableIndex > m_MaxIndex)
                            {
                                Reset();
                            }
                        }
                        else
                        {
                            //断签
                            Reset();
                        }
                    }
                    else
                    {
                        Reset();
                    }
                }
                else
                {
                    Reset();
                }

            }

            public void Sign()
            {
                if (!isSignAble) return;
                if (m_SignableIndex == -1) return;
                Save();
                m_LastSignIndex = m_SignableIndex;
                m_SignableIndex = -1;
            }
        }

        #endregion
    }
}




