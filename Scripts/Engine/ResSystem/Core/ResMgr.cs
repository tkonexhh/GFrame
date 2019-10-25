using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{
    public enum LoadResMode
    {
        AssetBundle,
        Res,
        Mix,
    }

    public class ResMgr : TMonoSingleton<ResMgr>
    {

        private Dictionary<string, IRes> m_ResMap = new Dictionary<string, IRes>();
        private List<IRes> m_ResList = new List<IRes>();


        // public int totalResCount
        // {
        //     get { return m_ResList.Count; }
        // }


        public override void OnSingletonInit()
        {

        }


        public void InitResMgr()
        {
            Log.i("#Init[ResMgr]");
            ReloadABTable();
            ReloadResTable();
        }

        private void ReloadResTable()
        {
            FolderDataTable.S.Reset();
            bool IsFileExist = IO.IsFileExist(FileMgr.S.GetFileInInner(ProjectPathConfig.resTableFilePath));
            FolderDataTable.S.LoadPackageFromeFile(FileMgr.S.GetFileInInner(ProjectPathConfig.resTableFilePath));
        }

        private void ReloadABTable()
        {

            AssetDataTable.S.Reset();
            AssetDataTable.S.LoadPackageFromFile(FileMgr.S.GetFileInInner(ProjectPathConfig.abTableFilePath));
            //List<string> outResult = new List<string>();
            // FileMgr.GetFileInFolder(FilePath.streamingAssetsPath4AB, ProjectPathConfig.abTableFileName, outResult);
            // for (int i = 0; i < outResult.Count; ++i)
            // {
            //     Debug.LogError(outResult[i]);
            //     AssetDataTable.S.LoadPackageFromFile(FileMgr.GetFullPath(ProjectPathConfig.abTableFilePath));
            // }
            // FilePath.GetFileInFolder(FilePath.persistentDataPath);
        }

        public IRes GetRes(string name)
        {
            IRes res = null;
            if (m_ResMap.TryGetValue(name, out res))
            {
                return res;
            }

            res = ResFactory.Create(name);
            if (res != null)
            {
                m_ResMap.Add(name, res);
                m_ResList.Add(res);
            }
            return res;
        }

        // public T GetRes<T>(string name) where T : IRes
        // {
        //     IRes res = null;
        //     if (m_ResMap.TryGetValue(name, out res))
        //     {
        //         return (T)res;
        //     }

        //     return default(T);
        // }
    }
}




