using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GFrame.UnityEditor
{

    public class BaseConfigEditor<T> where T : UnityEngine.ScriptableObject
    {
        public static void BuildConfig(string foleder, string name)
        {
            T config = default(T);
            string path = foleder + name;
            IO.CheckDirAndCreate(foleder);
            config = AssetDatabase.LoadAssetAtPath<T>(path);
            if (config == null)
            {
                config = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(config, path);
            }
            Log.i("#Create {0} In Folder:{1}", config.name, path);
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }


    }
}




