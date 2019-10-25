using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{
    public class DataDirtyRecorder
    {
        private bool m_IsDirty;
        public void SetIsDirty(bool dirty)
        {
            m_IsDirty = dirty;
        }

        public bool GetIsDirty()
        {
            return m_IsDirty;
        }
    }

    public class DataDirtyHandler
    {
        protected DataDirtyRecorder m_Recorder;

        public void SetDirtyRecorder(DataDirtyRecorder recorder)
        {
            m_Recorder = recorder;
        }

        public void SetDataDirty()
        {
            if (m_Recorder == null)
            {
                Log.e("Has No DataDirtyRecorder");
                return;
            }
            m_Recorder.SetIsDirty(true);
        }

        public bool GetIsDataDirty()
        {
            if (m_Recorder == null)
            {
                Log.e("Has No DataDirtyRecorder");
                return false;
            }
            return m_Recorder.GetIsDirty();
        }

        public void ResetDataDirty()
        {
            if (m_Recorder == null)
            {
                Log.e("Has No DataDirtyRecorder");
                return;
            }
            m_Recorder.SetIsDirty(false);
        }
    }
}
