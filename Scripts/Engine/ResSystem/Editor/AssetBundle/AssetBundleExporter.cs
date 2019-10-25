using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;

namespace GFrame.UnityEditor
{

    public class AssetBundleExporter
    {

        [MenuItem("Assets/GFrame/Asset/构建AB_Current")]
        public static void BuildAllAssetBundles()
        {
            Log.i("#Start Build All AssetBundles.");
            AssetBundleBuilder.BuildAB(FilePath.streamingAssetsPath4AB, EditorUserBuildSettings.activeBuildTarget);
            BuildDataTable();
        }

        // [MenuItem("Assets/GFrame/Asset/构建AB_IOS")]
        // public static void BuildAllAssetBundlesIOS()
        // {
        //     AssetBundleBuilder.BuildAB(outPath + "/IOS", BuildTarget.iOS);
        // }

        // [MenuItem("Assets/GFrame/Asset/构建AB_Android")]
        // public static void BuildAllAssetBundlesAndroid()
        // {
        //     AssetBundleBuilder.BuildAB(outPath + "/Android", BuildTarget.Android);
        // }

        #region 构建 AssetDataTable
        //[MenuItem("Assets/GFrame/Asset/生成Asset清单")]
        private static void BuildDataTable()
        {
            Log.i("#Start BuildDataTable");
            AssetDataTable table = new AssetDataTable();
            ProcessAssetBundleRes(table, null);
            table.Save();
        }

        [MenuItem("Assets/GFrame/Asset/清理无效AB")]
        public static void RemoveInvalidAssetBundle()
        {
            AssetDataTable table = new AssetDataTable();
            ProcessAssetBundleRes(table, null);
            Log.i("#Start Remove Invalid AssetBundle");
            RemoveInvalidAssetBundleInner(FilePath.streamingAssetsPath4AB, table);
            Log.i("#Success Remove Invalid AssetBundle.");
        }

        #endregion
        private static void ProcessAssetBundleRes(AssetDataTable table, string[] abNames)
        {
            AssetDataPackage package = null;
            //int abIndex = -1;
            AssetDatabase.RemoveUnusedAssetBundleNames();
            if (abNames == null)
            {
                abNames = AssetDatabase.GetAllAssetBundleNames();
            }

            if (abNames != null && abNames.Length > 0)
            {
                for (int i = 0; i < abNames.Length; ++i)
                {
                    //输出路径
                    string abPath = Path.Combine(FilePath.streamingAssetsPath4AB, abNames[i]);

                    string[] depends = AssetDatabase.GetAssetBundleDependencies(abNames[i], false);
                    FileInfo info = new FileInfo(abPath);
                    if (!info.Exists)
                    {
                        continue;
                    }
                    string md5 = GetMD5HashFromFile(abPath);
                    long buildTime = System.DateTime.Now.Ticks;
                    bool successAdd = table.AddAssetBundle(abNames[i], depends, md5, (int)info.Length, buildTime, out package);
                    if (!successAdd)
                    {
                        continue;
                    }

                    string[] assets = AssetDatabase.GetAssetPathsFromAssetBundle(abNames[i]);
                    foreach (var cell in assets)
                    {
                        // if (cell.EndsWith(".unity"))
                        // {
                        //     package.AddAssetData(new AssetData(AssetPath2Name(cell), eResType.kABScene));//, -1));
                        // }
                        // else
                        {
                            package.AddAssetData(new AssetData(PathHelper.Path2Name(cell), eResType.kABAsset));//, -1));
                        }
                    }
                }
            }
        }

        public static void RemoveInvalidAssetBundleInner(string path, AssetDataTable table)
        {
            string[] dirs = Directory.GetDirectories(path);
            if (dirs != null && dirs.Length > 0)
            {
                for (int i = 0; i < dirs.Length; ++i)
                {
                    RemoveInvalidAssetBundleInner(dirs[i], table);
                }
            }

            string[] files = Directory.GetFiles(path);
            if (files != null && files.Length > 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    string p = AssetBundlePath2ABName(files[i]);

                    if (!AssetFileFilter.IsAssetBundle(p))
                    {
                        continue;
                    }
                    if (table.GetABUnit(p) == null)
                    {
                        File.Delete(files[i]);
                        File.Delete(files[i] + ".meta");
                        File.Delete(files[i] + ".manifest");
                        File.Delete(files[i] + ".manifest.meta");
                        Log.e("#Delete Invalid AB:" + p);
                    }
                }

                files = Directory.GetFiles(path);
                if (files == null || files.Length == 0)
                {
                    Directory.Delete(path);
                }

            }
            else
            {
                Directory.Delete(path);
            }
        }


        private static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Log.e(ex);
            }

            return null;
        }


        private static string AssetBundlePath2ABName(string path)
        {
            return path.Replace(FilePath.streamingAssetsPath4AB, "");//(outFolderPath + path).Replace("//", "/");
        }

    }
}




