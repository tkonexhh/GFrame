using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{
    //根据资源进出维护一个资源数据库
    public class FolderDataTable : TSingleton<FolderDataTable>
    {
        [Serializable]
        public class SerializeData
        {
            public FolderData.SerializeData[] m_Datas;
        }


        string m_OutPath = ProjectPathConfig.resTableFilePath;
        private List<FolderData> m_FolderDataLst = new List<FolderData>();
        private Dictionary<string, FolderData> m_FolderDataMap = new Dictionary<string, FolderData>();

        public List<FolderData> GetFolderDataLst()
        {
            return m_FolderDataLst;
        }

        public FolderData GetAssetData(string assetKey)
        {
            assetKey = assetKey.ToLower();
            FolderData data = null;
            if (m_FolderDataMap.TryGetValue(assetKey, out data))
            {
                return data;
            }
            return null;
        }


        public void AddFolderData(string path)
        {
            string assetKey = PathHelper.Path2Name(path);
            assetKey = assetKey.ToLower();
            FolderData data = null;
            if (m_FolderDataMap.TryGetValue(assetKey, out data))
            {
                Log.e("#Already Add Asset In" + path);
                return;
            }
            data = new FolderData(assetKey, path);
            m_FolderDataLst.Add(data);
            m_FolderDataMap.Add(assetKey, data);
            //Save();
        }


        public void RemoveFolderData(string path)
        {
            //Load();
            string assetKey = PathHelper.Path2Name(path);
            FolderData data = null;
            if (!m_FolderDataMap.TryGetValue(assetKey, out data))
            {
                Log.e("#Not Find Asset In" + path);
                return;
            }
            m_FolderDataLst.Remove(data);
            m_FolderDataMap.Remove(assetKey);
            //Save();
            // Save();
        }

        public void Save()
        {
            IO.DelFile(ProjectPathConfig.resTableFilePath);
            SerializeData totalData = new SerializeData();
            totalData.m_Datas = new FolderData.SerializeData[m_FolderDataLst.Count];
            for (int i = 0; i < m_FolderDataLst.Count; i++)
            {
                FolderData.SerializeData data = GetSerializeFolderData(m_FolderDataLst[i]);
                totalData.m_Datas[i] = data;
            }
            SerializeHelper.SerializeBinary(m_OutPath, totalData);
            //FolderDataConfig.S.Refesh();
            //Load();
        }

        public void Reset()
        {
            m_FolderDataLst.Clear();
            m_FolderDataMap.Clear();
        }

        public void LoadPackageFromeFile(string path)
        {
            Log.i("#Init FolderDataTable");
            object obj = SerializeHelper.DeserializeBinary(path);
            if (obj == null)
            {
                Log.e("#Deserialize Failed At:" + path);
            }

            SerializeData data = obj as SerializeData;
            for (int i = 0; i < data.m_Datas.Length; ++i)
            {

                FolderData folderData = GetFolderData(data.m_Datas[i]);
                m_FolderDataLst.Add(folderData);
                m_FolderDataMap.Add(folderData.assetName, folderData);
            }

        }

        public void Clear()
        {
            IO.DelFile(m_OutPath);
        }

        private FolderData.SerializeData GetSerializeFolderData(FolderData data)
        {
            FolderData.SerializeData floderData = new FolderData.SerializeData();
            floderData.path = data.path;
            return floderData;
        }

        private FolderData GetFolderData(FolderData.SerializeData serializeData)
        {
            FolderData data = new FolderData(serializeData.name, serializeData.path);
            return data;
        }
    }
}




