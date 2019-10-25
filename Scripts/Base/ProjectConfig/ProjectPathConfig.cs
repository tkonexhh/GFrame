using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class ProjectPathConfig : ScriptableObject
    {

        private static ProjectPathConfig s_Instace;
        public static ProjectPathConfig S
        {
            get
            {
                if (s_Instace == null)
                {
                    s_Instace = new ProjectPathConfig();
                }
                return s_Instace;
            }
        }

        #region 
        [SerializeField] private string m_AppConfigPath = "Config/AppConfig";
        [SerializeField] private string m_AbAssetRelativePath = "Res/AB/";
        [SerializeField] private string m_FileAssetRelativePath = "Res/Resources/";
        [SerializeField] private string m_ABTableFileName = "ABTableConfig.bin";
        [SerializeField] private string m_RESTableFileName = "ResTableConfig.bin";
        [SerializeField] private string m_ExternalToolsPath = "/../../../Tools/";
        [SerializeField] private string m_ExternalTablePath = "/../../../Tables/Sources/";
        [SerializeField] private string m_TableScriptOutPutPath = "/Scripts/Game/Table";

        #endregion


        public static string appConfigPath
        {
            get
            {
                return S.m_AppConfigPath;
            }
        }

        public static string abAssetRelativePath
        {
            get
            {
                return "Assets/" + S.m_AbAssetRelativePath;
            }
        }

        public static string FileAssetRelativePath
        {
            get
            {
                return "Assets/" + S.m_FileAssetRelativePath;
            }
        }

        public static string abTableFilePath
        {
            get { return string.Format("{0}{1}", FilePath.streamingAssetsPath4AB, S.m_ABTableFileName); }
        }


        public static string resTableFilePath
        {

            get
            {
                return string.Format("{0}{1}", FilePath.streamingAssetsPath, S.m_RESTableFileName);
            }
        }

        public static string externalToolsPath
        {
            get
            {
                return Application.dataPath + S.m_ExternalToolsPath;
            }
        }

        public static string externalTablePath
        {
            get
            {
                return Application.dataPath + S.m_ExternalTablePath;
            }
        }

        public static string tableScriptOutPutPath
        {
            get
            {
                return Application.dataPath + S.m_TableScriptOutPutPath;
            }
        }


        public static string AssetBundleName2FullPath(string name)
        {
            string dependURL = FilePath.streamingAssetsPath4AB + name;
            return dependURL;
        }

    }
}




