using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{
    [Serializable]
    public class ABUnit
    {
        private string m_Name;
        public string name
        {
            get { return m_Name; }
        }
        private string[] m_Depends;
        private string m_MD5;
        private int m_FileSize;
        private long m_BuildTime;
        public ABUnit(string name, string[] depends, string md5, int fileSize, long buildTime)
        {
            m_Name = name;
            m_Depends = depends;
            m_MD5 = md5;
            m_FileSize = fileSize;
            m_BuildTime = buildTime;
        }
    }
}




