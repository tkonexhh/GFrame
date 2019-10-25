using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace GFrame
{

    public class SqliteMgr : TMonoSingleton<SqliteMgr>
    {
        private static string m_DataBaseName = "DataBase.db";
        public static string DataBasePath
        {
            get
            {
                return FilePath.streamingAssetsPath + m_DataBaseName;
            }
        }

        private SqliteDatabase m_sqlDB;


        public override void OnSingletonInit()
        {
            m_sqlDB = new SqliteDatabase(DataBasePath);
        }

        public void Init()
        {

        }

        public void ExecuteNonQuery(string sql)
        {
            m_sqlDB.ExecuteNonQuery(sql);
        }

        public DataBaseTable ExecuteQuery(string sql)
        {
            return m_sqlDB.ExecuteQuery(sql);
        }

        public void Insert(string tablename, AbstractSqliteDataClass data)
        {
            if (string.IsNullOrEmpty(tablename))
            {
                Debug.LogError("tablename is error");
                return;
            }

            if (data == null)
            {
                Debug.LogError("data is null");
                return;
            }

            var pair = data.GetAttrStr();
            string attr = pair.Key;
            string value = pair.Value;
            if (string.IsNullOrEmpty(attr) || string.IsNullOrEmpty(value))
            {
                Debug.LogError("data is error");
                return;
            }
            string sql = string.Format("INSERT INTO {0}({1}) VALUES({2})", tablename, attr, value);
            m_sqlDB.ExecuteNonQuery(sql);
        }

        public void UpdateData(string tablename, int index)
        {
            if (string.IsNullOrEmpty(tablename))
            {
                Debug.LogError("tablename is error");
                return;
            }
            //"_rowid_"
            string sql = string.Format("SELECT 1 FROM " + tablename);
            DataBaseTable data = m_sqlDB.ExecuteQuery(sql);
            //Debug.LogError(data.Rows.Count);

            //INSERT OR REPLACE INTO Employee ("id", "name", "role") VALUES (1, "John Foo", "CEO")
        }

        private DataBaseTable GetTable(string tablename)
        {
            DataBaseTable dt = m_sqlDB.ExecuteQuery("SELECT * FROM " + tablename);
            return dt;
        }

        public int GetTableDataCount(string tablename)
        {
            return GetTable(tablename).Rows.Count;
        }

        public List<T> ReadTableData<T>(string tablename) where T : AbstractSqliteDataClass, new()
        {

            List<T> lst = new List<T>();
            DataBaseTable dt = GetTable(tablename);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                lst.Add(JsonMapper.ToObject<T>(JsonMapper.ToJson(row)));
            }
            return lst;
        }
    }
}