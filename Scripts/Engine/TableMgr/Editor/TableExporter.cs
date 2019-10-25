using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using LitJson;

namespace GFrame.UnityEditor
{

    public class TableExporter
    {
        //[MenuItem("Assets/GFrame/Table/Build TXT")]
        public static void BuildTxt()
        {

        }
        [MenuItem("Assets/GFrame/Table/Build C#")]
        public static void BuildCCode()
        {
            string dataFolderPath = ProjectPathConfig.externalTablePath;

            string[] allTableFiles = Directory.GetFiles(dataFolderPath, "*.xlsx", SearchOption.AllDirectories);
            if (allTableFiles == null || allTableFiles.Length <= 0)
            {
                Debug.LogError("#No Table Found in" + dataFolderPath);
                return;
            }
            for (int i = 0; i < allTableFiles.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Hold on", "Processing", (float)(i + 1) / allTableFiles.Length);
                if (PathHelper.Path2Name(allTableFiles[i]).StartsWith("~"))
                {
                    continue;
                }
                ExcelUtility excel = new ExcelUtility(allTableFiles[i]);
                excel.WriteDataFile(allTableFiles[i]);
            }
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();

        }

        [MenuItem("Assets/GFrame/Table/Build JSON")]
        static void ExcelToJson()
        {
            string dataFolderPath = ProjectPathConfig.externalTablePath;
            string outJsonPath = FilePath.streamingAssetsPath4Config;
            IO.CheckDirAndCreate(dataFolderPath);

            string[] allTableFiles = Directory.GetFiles(dataFolderPath, "*.xlsx", SearchOption.AllDirectories);
            if (allTableFiles == null || allTableFiles.Length <= 0)
            {
                Debug.LogError("#No Table Found in" + dataFolderPath);
                return;
            }
            IO.CheckDirAndCreate(outJsonPath);

            for (int i = 0; i < allTableFiles.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Hold on", "Processing", (float)(i + 1) / allTableFiles.Length);
                if (PathHelper.Path2Name(allTableFiles[i]).StartsWith("~"))
                {
                    continue;
                }

                string dictName = new DirectoryInfo(Path.GetDirectoryName(allTableFiles[i])).Name;
                string fileName = Path.GetFileNameWithoutExtension(allTableFiles[i]);

                //构造Excel工具类
                ExcelUtility excel = new ExcelUtility(allTableFiles[i]);
                //判断编码类型
                Encoding encoding = Encoding.GetEncoding("utf-8");
                //判断输出类型
                string output = outJsonPath + fileName + ".json";
                excel.ConvertToJson(output, encoding);

            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }

    }
}




