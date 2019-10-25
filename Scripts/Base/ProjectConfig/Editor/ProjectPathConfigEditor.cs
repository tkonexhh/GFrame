using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame.UnityEditor
{

    public class ProjectPathConfigEditor
    {
        private const string CONFIG_NAME = "ProjectPathConfig.asset";

        [MenuItem("Assets/GFrame/Config/Build ProjectPathConfig")]
        public static void BuildProjectPathConfig()
        {
            BaseConfigEditor<ProjectPathConfig>.BuildConfig(FilePath.projectConfigPath, CONFIG_NAME);
        }
    }
}




