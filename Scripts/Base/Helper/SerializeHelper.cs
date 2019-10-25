using UnityEngine;
using LitJson;
using System;
using System.Text;
using System.IO;

namespace GFrame
{

    public class SerializeHelper
    {
        public static string GetJson(object obj, bool encry)
        {
            if (obj == null)
            {
                Log.w("#SerializeJson obj is Null.");
                return null;
            }
            string jsonValue = null;
            try
            {
                jsonValue = JsonMapper.ToJson(obj);
                if (encry)
                {
                    //jsonValue = EncryptUtil.AesStr(jsonValue, "weoizkxjkfs", "asjkdyweucn");
                }
            }
            catch (Exception e)
            {
                Log.e(e);
                return null;
            }
            return jsonValue;
        }

        public static bool SaveJson(string path, string jsonValue)
        {
            if (string.IsNullOrEmpty(path))
            {
                Log.w("#SerializeJson Without Valid Path.");
                return false;
            }
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] writeDataArray = UTF8Encoding.UTF8.GetBytes(jsonValue);
                fs.Write(writeDataArray, 0, writeDataArray.Length);
                fs.Flush();
            }
            return true;
        }

        public static bool SerializeJson(string path, object obj, bool encry = false)
        {
            string jsonValue = GetJson(obj, encry);
            if (string.IsNullOrEmpty(jsonValue))
            {
                return false;
            }
            SaveJson(path, jsonValue);

            return true;
        }

        public static T DeserializeJson<T>(string path, bool encry = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                Log.w("DeserializeJson Without Valid Path.");
                return default(T);
            }

            FileInfo fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                return default(T);
            }

            using (FileStream stream = fileInfo.OpenRead())
            {
                try
                {
                    if (stream.Length <= 0)
                    {
                        stream.Close();
                        return default(T);
                    }

                    byte[] byteData = new byte[stream.Length];

                    stream.Read(byteData, 0, byteData.Length);

                    string context = UTF8Encoding.UTF8.GetString(byteData);
                    stream.Close();

                    if (string.IsNullOrEmpty(context))
                    {
                        return default(T);
                    }

                    // if (encry)
                    // {
                    //     context = EncryptUtil.UnAesStr(context, "weoizkxjkfs", "asjkdyweucn");
                    // }

                    return JsonMapper.ToObject<T>(context);
                }
                catch (Exception e)
                {
                    Log.e(e);
                }
            }

            Log.w("DeserializeJson Failed!");
            return default(T);//返回T的默认值。
        }


        public static bool SerializeBinary(string path, object obj)
        {
            if (string.IsNullOrEmpty(path))
            {
                Log.w("#SerializeBinary Without Valid Path");
                return false;
            }

            if (obj == null)
            {
                Log.w("#SerializeBinary obj is Null");
                return false;
            }

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(fileStream, obj);
                return true;
            }
        }

        public static object DeserializeBinary(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return bf.Deserialize(fileStream);
            }
        }
    }
}




