using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{
    [Serializable]
    public class ResData
    {
        private string m_ResName;

        public string assetName
        {
            get { return m_ResName; }
        }

        public ResData(string name)
        {
            m_ResName = name;
        }
    }
}




