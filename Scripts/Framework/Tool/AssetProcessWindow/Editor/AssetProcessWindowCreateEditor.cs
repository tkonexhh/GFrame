using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame.UnityEditor
{

    public class AssetProcessWindowCreateEditor
    {
        private const string CONFIG_NAME = "AssetProcessWindow.asset";

        [MenuItem("Assets/GFrame/Tools/Build AssetProcessWindow")]
        public static void BuildAppConfig()
        {
            BaseConfigEditor<AssetProcessWindow>.BuildConfig(FilePath.projectToolPath, CONFIG_NAME);
        }
    }
}





