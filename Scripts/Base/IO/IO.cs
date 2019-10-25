using System.IO;
using System.Text;

namespace GFrame
{

    public class IO
    {
        #region File
        public static bool IsFileExist(string filePath)
        {
            return File.Exists(filePath);
        }

        public static bool DelFile(string filePath)
        {
            if (IsFileExist(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }

        #endregion

        #region Directory
        public static bool IsDirExist(string dirPath)
        {
            return Directory.Exists(dirPath);
        }


        public static bool DelDir(string dirPath, bool recursive)
        {
            if (IsDirExist(dirPath))
            {
                Directory.Delete(dirPath, recursive);
                return true;
            }

            return false;
        }

        public static void CheckDirAndCreate(string dirPath)
        {
            if (!IsDirExist(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }


        public static void WriteFile(string path, string content, bool isOverWrite)
        {
            if (!isOverWrite)
            {
                if (IsFileExist(path))
                {
                    return;
                }
            }


            DelFile(path);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(content);
            sw.Close();
            fs.Close();
        }

        #endregion



        // static private void ModifFile(string go, string path, List<AttrInfo> attrInfos)
        // {
        //     string insertTxt = "";
        //     for (int i = 0; i < attrInfos.Count; i++)
        //     {
        //         insertTxt += "[SerializeField] private " + attrInfos[i].typeName + " " + attrInfos[i].attrName + ";\n";
        //     }

        //     string content = File.ReadAllText(path);
        //     string findStr = "MonoBehaviour\n";
        //     int startIndex = content.IndexOf(findStr, 100);
        //     if (startIndex > 0)
        //     {
        //         Debug.LogError(startIndex);
        //         content = content.Insert(startIndex + findStr.Length + 5, insertTxt);
        //         File.WriteAllText(path, content);
        //         EditorPrefs.SetString(EDITKEY, go);
        //         AssetDatabase.Refresh();
        //     }
        // }
    }
}




