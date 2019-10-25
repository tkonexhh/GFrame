using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{
    [Serializable]
    public class AssetData : ResData
    {
        // private string m_AssetName;
        private short m_ResType;
        //private int m_Index;

        // public string assetName
        // {
        //     get { return m_AssetName; }
        // }

        public short assetType
        {
            get { return m_ResType; }
        }
        // public int assetBundleIndex
        // {
        //     get { return m_Index; }
        // }
        public AssetData(string name, short resType) : base(name)//, int index)
        {
            //m_AssetName = name;
            m_ResType = resType;
            //m_Index = index;
        }

    }
}




