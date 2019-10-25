using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TMonoSingletonAttribute : System.Attribute
    {
        private string m_AbsolutePath;

        public TMonoSingletonAttribute(string path)
        {
            m_AbsolutePath = path;
        }

        public string AbsolutePath
        {
            get { return m_AbsolutePath; }
        }

    }
}




