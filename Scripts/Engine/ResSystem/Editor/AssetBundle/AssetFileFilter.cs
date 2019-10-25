using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class AssetFileFilter
    {
        public static bool IsAsset(string fileName)
        {
            if (fileName.EndsWith(".meta") || fileName.EndsWith(".DS_Store"))
            {
                return false;
            }
            return true;
        }

        public static bool IsAssetBundle(string fileName)
        {
            if (fileName.EndsWith(EngineDefine.BUNDLE_EXTEND))
            {
                return true;
            }
            return false;
        }
    }
}




