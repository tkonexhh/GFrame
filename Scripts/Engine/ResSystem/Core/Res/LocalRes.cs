using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class LocalRes : AbstractRes
    {
        public static LocalRes Allocate(string name)
        {
            LocalRes res = ObjectPool<LocalRes>.S.Allocate();
            if (res != null)
            {
                res.name = name;
            }
            return res;
        }

        public override bool LoadSync()//同步加载
        {
            FolderData data = FolderDataTable.S.GetAssetData(m_Name);

            string samePart = PathHelper.GetSamePart(data.path, ProjectPathConfig.FileAssetRelativePath);
            Debug.LogError(samePart);
            //int length = Application.dataPath.LastIndexOf("/") + ProjectPathConfig.FileAssetRelativePath.Length;
            string filename = data.path.Substring(samePart.Length);
            string key = PathHelper.FileNameWithoutExtend(filename);

            Object asset = Resources.Load(key);
            Debug.LogError(data.path + "--" + filename);
            m_Asset = asset;
            if (m_Asset != null)
            {
                return true;
            }
            return false;
        }
        public override void LoadAsync()//异步加载
        {

        }

        public override void Recycle2Cache()
        {
            ObjectPool<LocalRes>.S.Recycle(this);
        }

        public override void OnCacheReset()
        {

        }
    }
}




