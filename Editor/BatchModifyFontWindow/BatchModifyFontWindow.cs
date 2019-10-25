using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 将目标文件夹下所有Prefab的丢失、默认字体的位置输出，并替换成目标字体
/// </summary>
public class BatchModifyFontWindow : EditorWindow
{

    [MenuItem("Custom/Window/BatchModifyFontWindow")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow<BatchModifyFontWindow>(true, "批量修改字体", true);
    }
    private static Font m_OldFont;
    private static Font m_NewFont;
    private static string m_TargetPath;

    public void Awake()
    {
        //在资源中读取一张贴图
        //texture = Resources.Load("1") as Texture;
    }

    //绘制窗口时调用
    void OnGUI()
    {
        //文本框显示鼠标在窗口的位置
        EditorGUILayout.LabelField(string.Format("目标文件夹 path:{0}", m_TargetPath));

        m_OldFont = EditorGUILayout.ObjectField("原始字体", m_OldFont, typeof(Font), true) as Font;
        m_NewFont = EditorGUILayout.ObjectField("目标字体", m_NewFont, typeof(Font), true) as Font;
        EditorGUILayout.LabelField("目标文件夹(拖至下方)");
        Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(350));

        m_TargetPath = EditorGUI.TextField(rect, m_TargetPath);
        //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内  
        if ((Event.current.type == EventType.DragUpdated
          || Event.current.type == EventType.DragExited)
          && rect.Contains(Event.current.mousePosition))
        {
            //改变鼠标的外表  
            DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            {
                m_TargetPath = DragAndDrop.paths[0];
            }
        }


        if (GUILayout.Button("修改字体！"))
        {
            Change();
        }

        if (GUILayout.Button("关闭窗口"))
        {
            //关闭窗口
            this.Close();
        }

    }

    public static void Change()
    {
        if (m_OldFont == null || m_NewFont == null)
        {
            Debug.LogError("Font is Null");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { m_TargetPath });
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            EditorUtility.DisplayProgressBar("Hold on", path, (float)(i + 1) / guids.Length);
            bool hasModifyFont = false;
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            //Debug.LogError(go);
            List<Text> lstTxts = new List<Text>();
            GetLabels(lstTxts, go.transform);
            for (int j = 0; j < lstTxts.Count; j++)
            {
                Text text = lstTxts[j];
                string fontName;
                if (text.font)
                {
                    fontName = text.font.name;
                }
                else
                {
                    fontName = "Missing";
                }
                //Debug.LogError(fontName);
                if ((fontName == "Missing") || (text.font.name == m_OldFont.name))
                {
                    Debug.LogError("Change-->" + go);
                    //if (needReplace)
                    {
                        text.font = m_NewFont;
                        hasModifyFont = true;
                        //EditorUtility.SetDirty(text);
                    }
                }

                if (hasModifyFont)
                {
                    EditorUtility.SetDirty(go);
                }
            }

        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

    }



    static void GetLabels(List<Text> list, Transform target)
    {
        if (target.childCount > 0)
        {
            foreach (Transform child in target)
            {
                GetLabels(list, child);
            }
        }
        Text l = target.GetComponent<Text>();
        if (l)
        {

            list.Add(l);
        }
    }

    // //更新
    // void Update()
    // {

    // }

    // void OnFocus()
    // {
    //     Debug.Log("当窗口获得焦点时调用一次");
    // }

    // void OnLostFocus()
    // {
    //     Debug.Log("当窗口丢失焦点时调用一次");
    // }

    // void OnHierarchyChange()
    // {
    //     Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
    // }

    // void OnProjectChange()
    // {
    //     Debug.Log("当Project视图中的资源发生改变时调用一次");
    // }

    // void OnInspectorUpdate()
    // {
    //     //Debug.Log("窗口面板的更新");
    //     //这里开启窗口的重绘，不然窗口信息不会刷新
    //     this.Repaint();
    // }

    // void OnSelectionChange()
    // {
    //     //当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
    //     foreach (Transform t in Selection.transforms)
    //     {
    //         //有可能是多选，这里开启一个循环打印选中游戏对象的名称
    //         Debug.Log("OnSelectionChange" + t.name);
    //     }
    // }

    void OnDestroy()
    {
        Debug.Log("当窗口关闭时调用");
    }

}