using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class KeyCodeTracker
    {

        private Action m_DefaultProcessListener;

        public void SetDefaultProcessListener(Action callback)
        {
            m_DefaultProcessListener = callback;
        }

        private void ProcessKeyDown()
        {
            if (m_DefaultProcessListener != null)
            {
                m_DefaultProcessListener();
            }
        }


        public void OnLateUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ProcessKeyDown();
            }
        }
    }
}




