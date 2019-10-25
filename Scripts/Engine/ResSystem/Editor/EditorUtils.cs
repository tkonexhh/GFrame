using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame
{

    public class EditorUtils
    {
        public static string CurrentSelectPath
        {
            get
            {
                if (Selection.activeObject == null)
                {
                    return null;
                }
                return AssetDatabase.GetAssetPath(Selection.activeObject);
            }
        }

        public static string AssetsPath2ABSPath(string path)
        {
            string assetRootPath = System.IO.Path.GetFullPath(Application.dataPath);
            assetRootPath = assetRootPath.Substring(0, assetRootPath.Length - 6) + path;
            //Debug.LogError(assetRootPath);
            return assetRootPath.Replace("\\", "/");
        }

        public static string AssetPath2ReltivePath(string path)
        {
            if (path == null)
                return null;
            return path.Replace("Assets/", "");
        }

    }
}




