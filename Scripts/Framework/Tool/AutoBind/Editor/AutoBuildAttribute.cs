using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System;
using System.Reflection;
using UnityEditor.Callbacks;
using System.IO;

namespace GFrame
{

    public class AutoBuildAttribute
    {
        private const string NAMESPACE = EngineDefine.NAMESPACE;
        private const string EDITKEY = "AutoBindAttribute";
        private const string EDITKEY_BIND = "AutoBindAttribute_BIND";
        static Dictionary<string, string> dicType = new Dictionary<string, string>()
        {
            {"Img", "Image"},
            {"Btn", "Button"},
            {"Txt", "Text"},
            {"Trans", "Transform"},
            {"Obj", "GameObject"},
            {"Toggle", "Toggle"}
        };

        #region  Bind绑定
        [MenuItem("Custom/Tools/AutoBind/根据AutoBind脚本绑定")]
        static public void AutoCreateAttributeByBind()
        {
            if (Selection.gameObjects.Length == 0)
                return;
            GameObject selectobj = Selection.gameObjects[0];
            WriteCode(selectobj);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorPrefs.SetString(EDITKEY_BIND, selectobj.name);
        }

        static private void WriteCode(GameObject obj)
        {
            string fileName = obj.name + "_AutoBind.cs";
            string dicName = Application.dataPath + "/Scripts/Game/UIScripts/" + obj.name;
            IO.CheckDirAndCreate(dicName);

            List<AttrInfo> attrInfos = GetAttrInfosInGameObjectByBind(obj);

            string filePath = Path.Combine(dicName, fileName);
            string fileMonoPath = Path.Combine(dicName, obj.transform.name + ".cs");
            IO.WriteFile(filePath, WriteNewFile(obj.transform.name, attrInfos), true);
            if (!IO.IsFileExist(fileMonoPath))
            {
                IO.WriteFile(fileMonoPath, WriteEmptyMono(obj.transform.name), true);
            }

        }

        [DidReloadScripts]
        static void BindScriptsToObjByBind()
        {
            string objName = EditorPrefs.GetString(EDITKEY_BIND, "");
            if (!string.IsNullOrEmpty(objName))
            {
                EditorPrefs.SetString(EDITKEY_BIND, "");

                GameObject targetObj = GameObject.Find(objName);
                string nameStr = NAMESPACE + "." + objName;
                Type type = ReflectionHelper.GetType(nameStr);
                if (type == null)
                {
                    Log.e("#Not Find Type For" + nameStr);
                    return;
                }

                var targetComponent = targetObj.GetComponent(type);
                if (targetObj.GetComponent(type) == null)
                {
                    targetComponent = targetObj.AddComponent(type);
                }
                List<AttrInfo> attrInfos = GetAttrInfosInGameObjectByBind(targetObj);

                BindProperties(targetComponent, targetObj, type, attrInfos);
            }
        }

        static private List<AttrInfo> GetAttrInfosInGameObjectByBind(GameObject obj)
        {
            AutoBind[] uiBinds = obj.GetComponentsInChildren<AutoBind>();
            List<AttrInfo> attrInfos = new List<AttrInfo>();
            foreach (AutoBind uiBind in uiBinds)
            {
                Transform targetTrans = uiBind.transform;
                string path = "";
                while (targetTrans.parent != obj.transform)
                {
                    targetTrans = targetTrans.parent;
                    path = targetTrans.name + "/" + path;
                }
                path += uiBind.transform.name;

                AttrInfo attrInfo = new AttrInfo();
                if (!string.IsNullOrEmpty(uiBind.attrName))
                    attrInfo.attrName = AutoBindHelper.AutoName(uiBind);
                else
                {
                    string fieldName = uiBind.transform.name;
                    fieldName = GetFieldName(fieldName);
                    attrInfo.attrName = fieldName;
                }
                attrInfo.typeName = uiBind.type.ToString();
                attrInfo.objName = uiBind.transform.name;
                attrInfo.path = path.ToString();
                attrInfos.Add(attrInfo);
            }
            return attrInfos;
        }

        #endregion


        [MenuItem("Custom/Tools/AutoBind/自动生成代码并绑定变量")]
        static public void AutoCreateAttribute()
        {
            //子物体以这种名字开头的话，则会被构建属性
            //如Img_Title Btn_Close

            //可能和已有的类重复
            if (Selection.gameObjects.Length == 0)
                return;
            GameObject selectobj = Selection.gameObjects[0];
            string name = selectobj.name;

            List<AttrInfo> attrInfos = GetAttrInfosInGameObject(selectobj);
            if (attrInfos.Count == 0) return;

            var script = selectobj.GetComponent(name);
            string path = Application.dataPath + "/";
            if (script != null)
            {
                var monoScript = MonoScript.FromMonoBehaviour((MonoBehaviour)script);
                path = AssetDatabase.GetAssetPath(monoScript.GetInstanceID());
                path = PathHelper.GetParentDir(path) + "/";
            }
            Debug.LogError(path);
            //string path = Application.dataPath + "/";
            string filePath = path + name + "_AutoBind.cs";
            string fileMonoPath = path + name + ".cs";
            if (!IO.IsFileExist(fileMonoPath))
            {
                IO.WriteFile(fileMonoPath, WriteEmptyMono(name), true);
            }
            IO.DelFile(filePath);
            IO.WriteFile(filePath, WriteNewFile(name, attrInfos), true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorPrefs.SetString(EDITKEY, name);
        }

        static private string WriteEmptyMono(string className)
        {
            string namespaceStr = NAMESPACE;
            StringBuilder code = new StringBuilder();
            code.Append("using System;\n");
            code.Append("using System.Collections;\n");
            code.Append("using UnityEngine;\n");
            code.Append("using UnityEngine.UI;\n");
            code.Append("using GFrame;\n");
            code.Append("\n");
            code.Append("namespace " + namespaceStr + "\n");
            code.Append("{\n");
            code.Append("\tpublic partial class " + className + " : MonoBehaviour\n");
            code.Append("\t{\n");
            code.Append("\t}\n");
            code.Append("}");

            return code.ToString();
        }

        static private string WriteNewFile(string className, List<AttrInfo> attrInfos)
        {
            string namespaceStr = NAMESPACE;
            StringBuilder code = new StringBuilder();
            code.Append("using System;\n");
            code.Append("using System.Collections;\n");
            code.Append("using UnityEngine;\n");
            code.Append("using UnityEngine.UI;\n");
            code.Append("using GFrame;\n");
            code.Append("\n");
            code.Append("namespace " + namespaceStr + "\n");
            code.Append("{\n");
            code.Append("\tpublic partial class " + className + " : MonoBehaviour\n");
            code.Append("\t{\n");

            foreach (var attr in attrInfos)
            {
                code.Append("\t\t[SerializeField] private " + attr.typeName + " " + attr.attrName + ";\n");
            }
            code.Append("\t}\n");
            code.Append("}");

            return code.ToString();
        }

        [DidReloadScripts]
        static void BindScriptsToObj()
        {
            string objName = EditorPrefs.GetString(EDITKEY, "");
            if (!string.IsNullOrEmpty(objName))
            {
                EditorPrefs.SetString(EDITKEY, "");
                GameObject targetObj = GameObject.Find(objName);

                string nameStr = NAMESPACE + "." + objName;
                Type type = ReflectionHelper.GetType(nameStr);
                if (type == null)
                {
                    Log.e("#Not Find Type For" + nameStr);
                    return;
                }

                var targetComponent = targetObj.GetComponent(type);
                if (targetObj.GetComponent(type) == null)
                {
                    targetComponent = targetObj.AddComponent(type);
                }

                List<AttrInfo> attrInfos = GetAttrInfosInGameObject(targetObj);
                BindProperties(targetComponent, targetObj, type, attrInfos);
            }
        }

        static private string GetFieldName(string fieldName)
        {
            fieldName = fieldName.Replace(" ", "");
            fieldName = fieldName.Replace("(", "");
            fieldName = fieldName.Replace(")", "");
            fieldName = fieldName.Replace("_", "");
            fieldName = fieldName[0].ToString().ToUpper() + fieldName.Substring(1);
            fieldName = "m_" + fieldName;
            return fieldName;
        }



        static private List<AttrInfo> GetAttrInfosInGameObject(GameObject obj)
        {
            string name = obj.name;
            Transform[] childs = obj.GetComponentsInChildren<Transform>();

            List<AttrInfo> attrInfos = new List<AttrInfo>();
            foreach (Transform child in childs)
            {
                if (child.name.Contains("_"))
                {
                    string key = child.name.Split('_')[0];
                    string value;
                    if (dicType.TryGetValue(key, out value))
                    {
                        string fieldName = child.name;
                        fieldName = GetFieldName(fieldName);

                        Transform targetTrans = child;
                        string path = "";
                        while (targetTrans.parent != obj.transform)
                        {
                            targetTrans = targetTrans.parent;
                            path = targetTrans.name + "/" + path;
                        }
                        path += child.name;
                        AttrInfo attrInfo = new AttrInfo();
                        attrInfo.attrName = fieldName;
                        attrInfo.typeName = value;
                        attrInfo.objName = child.name;
                        attrInfo.path = path.ToString();
                        attrInfos.Add(attrInfo);
                    }
                }
            }

            return attrInfos;
        }

        static private void BindProperties(object obj, GameObject go, Type type, List<AttrInfo> attrInfos)
        {
            List<AttrInfo> tempChild = new List<AttrInfo>(attrInfos);
            FieldInfo[] infos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (int i = 0; i < infos.Length; i++)
            {
                string attrName = infos[i].Name;
                for (int x = tempChild.Count - 1; x >= 0; --x)
                {

                    if (tempChild[x].attrName == attrName)
                    {
                        BindPropertie(obj, go, infos[i], tempChild[x]);
                        tempChild.Remove(tempChild[x]);
                    }
                }
            }
        }

        static private void BindPropertie(object obj, GameObject go, FieldInfo info, AttrInfo attrInfo)
        {
            var child = go.transform.Find(attrInfo.path);
            info.SetValue(obj, GetObjByAttrType(child, attrInfo.typeName));
        }

        static private object GetObjByAttrType(Transform trans, string typeName)
        {
            switch (typeName)
            {
                case "GameObject":
                    return trans.gameObject;
                case "Transform":
                    return trans;
                default:
                    return trans.GetComponent(typeName);
            }
        }

        private class AttrInfo
        {
            public string typeName;//变量类型名字
            public string attrName;//文件中的变量名
            public string objName;//游戏物体名字
            public string path;//在父节点中的路径
        }
    }
}




