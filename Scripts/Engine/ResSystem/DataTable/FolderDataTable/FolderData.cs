using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class FolderData : ResData
    {
        [Serializable]
        public class SerializeData
        {

            private string m_Path;
            public string path
            {
                get { return m_Path; }
                set { m_Path = value; }
            }
            public string name
            {
                get { return PathHelper.Path2Name(m_Path); }
            }
        }
        private string m_Path;
        public string path
        {
            get { return m_Path; }
        }

        public FolderData(string name, string path) : base(name)
        {
            m_Path = path;
        }
    }
}




