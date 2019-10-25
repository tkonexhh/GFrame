using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace GFrame
{

    public class SqilteEditor
    {
        [MenuItem("Custom/SQlite/创建表ClassData")]
        private static void CreateClassByDB()
        {
            var path = SqliteMgr.DataBasePath;

            string dbName = path.Substring(path.LastIndexOf("/"));
            Debug.LogError(dbName + "---" + path);
            SqliteDatabase m_sqlDB = new SqliteDatabase(path);
        }

        [MenuItem("Custom/SQlite/删除DB")]
        private static void DelDB()
        {
            DirectoryInfo forder = new DirectoryInfo(FilePath.persistentDataPath4DataBase);
            forder.Delete(true);
        }
    }
}




