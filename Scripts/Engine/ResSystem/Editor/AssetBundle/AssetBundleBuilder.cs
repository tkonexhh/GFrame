using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace GFrame
{

    public class AssetBundleBuilder
    {

        public static void BuildAB(string dirPath, BuildTarget buildTarget)
        {
            IO.CheckDirAndCreate(dirPath);
            BuildPipeline.BuildAssetBundles(dirPath, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
            // 刚创建的文件夹和目录能马上再Project视窗中出现
            AssetDatabase.Refresh();
        }
    }
}




