using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame.UnityEditor
{

    public class FolderDataConfigEditor
    {
        private const string CONFIG_NAME = "FolderDataConfig.asset";

        [MenuItem("Assets/GFrame/Config/Build FolderDataConfig")]
        public static void BuildFolderDataConfig()
        {
            BaseConfigEditor<FolderDataConfig>.BuildConfig(FilePath.projectConfigPath, CONFIG_NAME);
        }
    }
}




