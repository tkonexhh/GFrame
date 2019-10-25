using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class KeyboardInputter : IKeyboardInputter
    {

        private List<KeyCodeMonitor> m_MonitorList;
        private bool m_IsEnable = true;
        public bool isEnable
        {
            get { return m_IsEnable; }
            set { m_IsEnable = value; }
        }


        public void RegisterKeyCodeMonitor(KeyCode code, Run begin, Run process, Run end)//普通点击
        {
            AddKeyCodeMonitor(KeyBoardInputType.Click, new KeyCode[] { code }, begin, process, end);
        }
        public void RegisterShortcuts(KeyCode[] code, Run end)//快捷键
        {
            AddKeyCodeMonitor(KeyBoardInputType.Shortcuts, code, null, null, end);
        }
        public void RegisterKeyCodeQueue(KeyCode[] code, Run end)//连招
        {
            AddKeyCodeMonitor(KeyBoardInputType.Sequeue, code, null, null, end);
        }

        private void AddKeyCodeMonitor(KeyBoardInputType type, KeyCode[] code, Run begin, Run process, Run end)
        {
            KeyCodeMonitor monitor = null;
            if (type == KeyBoardInputType.Click)
            {
                monitor = new KeyCodeMonitor_Click(code, begin, process, end);
            }
            else if (type == KeyBoardInputType.Shortcuts)
            {
                monitor = new KeyCodeMonitor_Shortcut(code, end);
            }
            else if (type == KeyBoardInputType.Sequeue)
            {
                monitor = new KeyCodeMonitor_Sequeue(code, end);
            }

            if (m_MonitorList == null)
            {
                m_MonitorList = new List<KeyCodeMonitor>();
            }
            m_MonitorList.Add(monitor);
        }

        public void OnLateUpdate()
        {
            if (!m_IsEnable)
            {
                return;
            }

            if (m_MonitorList == null || m_MonitorList.Count == 0)
            {
                return;
            }

            //处理各种键盘事件
            for (int i = m_MonitorList.Count - 1; i >= 0; --i)
            {
                m_MonitorList[i].OnLateUpdate();
            }
        }



        #region 按键监控

        private enum KeyBoardInputType
        {
            Click,
            Shortcuts,
            Sequeue,
        }

        private class KeyCodeMonitor_Click : KeyCodeMonitor
        {
            public KeyCodeMonitor_Click(KeyCode[] code, Run begin, Run process, Run end) : base(KeyBoardInputType.Shortcuts, code, begin, process, end)
            {
            }

            public override void OnLateUpdate()
            {
                if (Input.GetKey(m_Codes[0]))
                {
                    if (!m_IsPrecessing)
                    {
                        m_IsPrecessing = true;
                        if (m_BeginDelegate != null)
                        {
                            m_BeginDelegate();
                        }
                    }
                    else
                    {
                        if (m_PrecessingDelegate != null)
                        {
                            m_PrecessingDelegate();
                        }
                    }
                }
                else
                {
                    if (m_IsPrecessing)
                    {
                        m_IsPrecessing = false;
                        if (m_EndDelegate != null)
                        {
                            m_EndDelegate();
                        }
                    }
                }
            }
        }

        private class KeyCodeMonitor_Shortcut : KeyCodeMonitor
        {
            public KeyCodeMonitor_Shortcut(KeyCode[] code, Run end) : base(KeyBoardInputType.Shortcuts, code, null, null, end)
            {
            }

            public override void OnLateUpdate()
            {
                IterationClick(0);
            }

            private void IterationClick(int index)
            {
                if (Input.GetKey(m_Codes[index]))
                {
                    if (index < m_Codes.Length - 1)
                    {
                        IterationClick(index + 1);
                    }
                    else //最后一个按键了
                    {
                        if (!m_IsPrecessing)
                        {
                            m_IsPrecessing = true;
                            if (m_EndDelegate != null)
                            {
                                m_EndDelegate();
                            }
                        }
                    }
                }
                else
                {
                    if (m_IsPrecessing)
                    {
                        m_IsPrecessing = false;
                    }
                }
            }
        }

        private class KeyCodeMonitor_Sequeue : KeyCodeMonitor
        {
            private List<KeyCode> m_InputKeyCodes;
            private int m_Frame;
            private int MAX_FRAME = 50;
            public KeyCodeMonitor_Sequeue(KeyCode[] code, Run end) : base(KeyBoardInputType.Sequeue, code, null, null, end)
            {

                m_InputKeyCodes = new List<KeyCode>();
                m_Frame = 0;

            }

            public override void OnLateUpdate()
            {
                if (Input.anyKeyDown)
                {
                    if (m_InputKeyCodes.Count < m_Codes.Length)
                    {
                        if (Input.GetKeyDown(m_Codes[m_InputKeyCodes.Count]))
                        {
                            m_Frame = 0;
                            m_InputKeyCodes.Add(m_Codes[m_InputKeyCodes.Count]);
                            if (m_InputKeyCodes.Count == m_Codes.Length)
                            {
                                ResetSequeue();
                                if (m_EndDelegate != null)
                                {
                                    m_EndDelegate();
                                }
                            }
                        }
                    }
                }

                m_Frame++;
                if (m_Frame >= MAX_FRAME)
                {
                    ResetSequeue();
                    m_IsPrecessing = false;
                }
            }

            private void ResetSequeue()
            {
                m_Frame = 0;
                m_InputKeyCodes.Clear();
            }
        }

        private class KeyCodeMonitor
        {
            private KeyBoardInputType m_InputType;
            protected Run m_BeginDelegate;
            protected Run m_PrecessingDelegate;
            protected Run m_EndDelegate;
            protected bool m_IsPrecessing = false;
            protected KeyCode[] m_Codes;

            public KeyCodeMonitor(KeyBoardInputType type, KeyCode[] code, Run begin, Run process, Run end)
            {
                m_IsPrecessing = false;
                m_InputType = type;
                m_Codes = code;
                m_BeginDelegate = begin;
                m_PrecessingDelegate = process;
                m_EndDelegate = end;
            }

            public virtual void OnLateUpdate()
            {
            }
        }
        #endregion
    }
}




