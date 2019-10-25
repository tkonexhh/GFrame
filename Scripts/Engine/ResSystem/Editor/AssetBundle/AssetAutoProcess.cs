using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace GFrame.UnityEditor
{

    public class AssetAutoProcess : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            ProcessImportedAssets(importedAsset);
            ProcessMovedAssets(movedAssets);
            //processDeletedAssets(deletedAssets);
        }

        /**
         * @description: 
         * @param {type} 
         * @return: 
         */
        private static void ProcessImportedAssets(string[] assets)
        {
            if (assets == null || assets.Length == 0)
                return;

            for (int i = 0; i < assets.Length; ++i)
            {
                if (CheckIsRes4AssetBundle(assets[i]))
                {
                    ProcessAssetBundleTag(assets[i], true);
                    //ProcessForderAsset(assets[i], true);
                }
            }
        }

        private static void ProcessMovedAssets(string[] assets)
        {
            if (assets == null || assets.Length == 0)
                return;

            for (int i = 0; i < assets.Length; ++i)
            {
                if (CheckIsRes4AssetBundle(assets[i]))
                {
                    ProcessAssetBundleTag(assets[i], true);
                    //ProcessForderAsset(assets[i], true);
                }
                else if (CheckIsRes4Resources(assets[i]))
                {
                    ProcessAssetBundleTag(assets[i], false);
                    //ProcessForderAsset(assets[i], false);
                }
            }
        }

        // private static void processDeletedAssets(string[] assets)
        // {
        //     if (assets == null || assets.Length == 0)
        //         return;

        //     for (int i = 0; i < assets.Length; ++i)
        //     {
        //         if (CheckIsRes4AssetBundle(assets[i]))
        //         {
        //             ProcessForderAsset(assets[i], false);
        //         }

        //     }
        // }


        private static bool CheckIsRes4AssetBundle(string path)
        {
            if (path.StartsWith(ProjectPathConfig.abAssetRelativePath))
            {
                return true;
            }
            return false;
        }

        private static bool CheckIsRes4Resources(string path)
        {
            if (path.StartsWith("Assets/Resources/") || path.StartsWith(ProjectPathConfig.FileAssetRelativePath))
            {
                return true;
            }
            return false;
        }

        private static void ProcessAssetBundleTag(string path, bool tag)
        {
            //获得 开头为Assets/的资源
            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (importer == null)
            {
                Log.e("#Not Find Asset:" + path);
                return;
            }

            string fullPath = EditorUtils.AssetsPath2ABSPath(path);
            if (IO.IsDirExist(fullPath))
            {
                return;
            }

            if (tag)
            {
                string dirName = Path.GetDirectoryName(path);
                //Debug.LogError(dirName);
                string assetBundleName = EditorUtils.AssetPath2ReltivePath(dirName).ToLower();
                //Debug.LogError(assetBundleName);

                if (path.Contains("FolderMode"))
                {
                    importer.assetBundleName = assetBundleName + EngineDefine.BUNDLE_EXTEND;
                }
                else
                {
                    importer.assetBundleName = string.Format("{0}/{1}{2}", assetBundleName, PathHelper.FileNameWithoutExtend(Path.GetFileName(path)), EngineDefine.BUNDLE_EXTEND);
                }
            }
            else
            {
                importer.assetBundleName = string.Empty;
            }
        }

        // private static void ProcessForderAsset(string path, bool isAdd)
        // {
        //     if (isAdd)
        //     {
        //         FolderDataTable.S.AddFolderData(path);
        //     }
        //     else
        //     {
        //         FolderDataTable.S.RemoveFolderData(path);
        //     }

        // }
    }
}