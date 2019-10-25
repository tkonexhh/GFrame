using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GFrame
{

    public class AssetDataTable : TSingleton<AssetDataTable>
    {
        private List<AssetDataPackage> m_AllAssetDataPackage = new List<AssetDataPackage>();
        private Dictionary<string, AssetDataPackage> m_AllAssetDataMap = new Dictionary<string, AssetDataPackage>();


        [Serializable]
        public class SerializeData
        {
            public AssetDataPackage.SerializeData[] datas;
        }

        #region Public Func

        public void Reset()
        {
            for (int i = m_AllAssetDataPackage.Count - 1; i >= 0; --i)
            {
                m_AllAssetDataPackage[i].Reset();
            }
            //m_ActiveAssetDataPackages.Clear();
            m_AllAssetDataPackage.Clear();
        }

        public void Save()
        {
            SerializeData data = new SerializeData();
            data.datas = new AssetDataPackage.SerializeData[m_AllAssetDataPackage.Count];
            for (int i = 0; i < m_AllAssetDataPackage.Count; i++)
            {
                data.datas[i] = m_AllAssetDataPackage[i].Save();
            }
            string outPath = ProjectPathConfig.abTableFilePath;//string.Format("{0}{1}", path, ProjectPathConfig.abTableFileName);

            if (SerializeHelper.SerializeBinary(outPath, data))
            {
                Log.i("#Success Save AssetDataTable:" + outPath);
            }
            else
            {
                Log.e("#Failed Save AssetDataTable:" + outPath);
            }
        }

        #endregion
        public bool AddAssetBundle(string name, string[] depends, string md5, int fileSize, long buildTime, out AssetDataPackage package)
        {
            package = null;
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            string key = null;
            string path = null;
            GetPackageKeyFromABName(name, out key, out path);
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            package = GetAssetDataPackage(key);
            if (package == null)
            {
                package = new AssetDataPackage(key, path, DateTime.Now.Ticks);
                m_AllAssetDataPackage.Add(package);
                Log.i("#Add AssetDataPackage:" + key);
            }

            return package.AddAssetBundle(name, depends, md5, fileSize, buildTime);
        }


        private void GetPackageKeyFromABName(string name, out string key, out string path)
        {
            int index = name.LastIndexOf('/');
            if (index < 0)
            {
                key = name;
                path = key;
                return;
            }
            key = name.Substring(index + 1, name.Length - 1 - index);
            path = key;
            // string keyResult = null;
            // string pathResult = key;

            return;
        }

        private AssetDataPackage GetAssetDataPackage(string key)
        {
            // for (int i = m_AllAssetDataPackage.Count - 1; i >= 0; --i)
            // {
            //     if (m_AllAssetDataPackage[i].key.Equals(key))
            //     {
            //         return m_AllAssetDataPackage[i];
            //     }
            // }
            // return null;
            AssetDataPackage package;
            m_AllAssetDataMap.TryGetValue(key, out package);
            return package;
        }

        public ABUnit GetABUnit(string path)
        {
            ABUnit unit = null;
            for (int i = m_AllAssetDataPackage.Count - 1; i >= 0; --i)
            {
                unit = m_AllAssetDataPackage[i].GetABUnit(path);
                if (unit != null) break;
            }
            return unit;
        }

        public AssetData GetAssetData(string name)
        {
            for (int i = m_AllAssetDataPackage.Count - 1; i >= 0; --i)
            {
                AssetData data = m_AllAssetDataPackage[i].GetAssetData(name);
                if (data == null)
                {
                    continue;
                }
                //Debug.LogError("GetAssetData:" + data);
                return data;
            }
            return null;
        }

        public string GetAssetBundleNameByAssetName(string assetName)
        {
            for (int i = m_AllAssetDataPackage.Count - 1; i >= 0; --i)
            {
                string name = m_AllAssetDataPackage[i].GetAssetBundleNameByAssetName(assetName);
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }
            return null;
        }

        public void LoadPackageFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            object data = SerializeHelper.DeserializeBinary(filePath);

            if (data == null)
            {
                Log.w("#Failed Deserialize");
                return;
            }

            SerializeData sd = data as SerializeData;

            if (sd == null)
            {
                Log.e("#Failed Load AssetDataTable:" + filePath);
                return;
            }

            for (int i = 0; i < sd.datas.Length; i++)
            {
                AssetDataPackage package = BuildAssetDataPackageBySerializeData(sd.datas[i], filePath);
                string key = package.key;
                m_AllAssetDataMap.Add(key, package);
                m_AllAssetDataPackage.Add(package);
            }

        }

        private AssetDataPackage BuildAssetDataPackageBySerializeData(AssetDataPackage.SerializeData data, string path)
        {
            return new AssetDataPackage(data, path);
        }
    }
}




