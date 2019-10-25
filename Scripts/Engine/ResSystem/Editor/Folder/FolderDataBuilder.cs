using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace GFrame.UnityEditor
{

    public class FolderDataBuilder
    {
        [MenuItem("Assets/GFrame/Asset/构建FolderData")]
        public static void BuildFolderData()
        {
            //Debug.LogError("BuildFolderData");
            FolderDataTable.S.Clear();
            string targetPath = ProjectPathConfig.FileAssetRelativePath;
            //DirectoryInfo directory = new DirectoryInfo(targetPath);

            string[] files = Directory.GetFiles(targetPath, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {

                if (AssetFileFilter.IsAsset(files[i]))
                {
                    FolderDataTable.S.AddFolderData(files[i]);
                }
            }

            FolderDataTable.S.Save();
        }
    }
}




