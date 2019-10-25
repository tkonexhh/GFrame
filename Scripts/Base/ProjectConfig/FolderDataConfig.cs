using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame
{
    public class FolderDataConfig : ScriptableObject
    {

        [SerializeField] private List<string> m_DataLst;


        #region 初始化过程
        private static FolderDataConfig s_Instance = null;

        public static FolderDataConfig S
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = new FolderDataConfig();
                }

                return s_Instance;
            }
        }

        #endregion


        // public void Refesh()
        // {
        //     if (m_DataLst == null)
        //     {
        //         m_DataLst = new List<string>();
        //     }
        //     m_DataLst.Clear();
        //     List<FolderData> dataLst = FolderDataTable.S.GetFolderDataLst();

        //     for (int i = 0; i < dataLst.Count; i++)
        //     {
        //         m_DataLst.Add(dataLst[i].assetName);
        //         // EditorUtility.SetDirty(this);
        //         // AssetDatabase.SaveAssets();
        //     }
        // }
    }
}




