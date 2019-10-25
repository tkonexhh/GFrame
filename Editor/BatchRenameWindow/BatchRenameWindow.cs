using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BatchRenameWindow : EditorWindow
{
    [MenuItem("Custom/Window/批量重命名")]
    private static void BatchRename()
    {
        EditorWindow.GetWindow<BatchRenameWindow>(true, "批量修改字体", true);
    }

    private static string m_TargetPath;
    private static bool m_IsFront;
    private static int m_InputTxtCount = 0;
    private static string m_InputAddTxt = "";
    void OnGUI()
    {
        EditorGUILayout.LabelField(string.Format("目标文件夹 path:{0}", m_TargetPath));
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

        m_IsFront = EditorGUILayout.ToggleLeft("前部/后部", m_IsFront);


        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("删除N个字符");
        int.TryParse(EditorGUILayout.TextField(m_InputTxtCount.ToString()), out m_InputTxtCount);
        if (GUILayout.Button("GO!"))
        {
            RenameDel();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("--------------------");

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("添加字符");
        m_InputAddTxt = EditorGUILayout.TextField(m_InputAddTxt);
        if (GUILayout.Button("Go!"))
        {
            RenameAdd();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("--------------------");
        EditorGUILayout.EndVertical();
    }

    private void RenameDel()
    {
        if (string.IsNullOrEmpty(m_TargetPath))
        {
            return;
        }
        if (m_InputTxtCount <= 0)
        {
            return;
        }

        string[] guids = AssetDatabase.FindAssets("", new string[] { m_TargetPath });
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            EditorUtility.DisplayProgressBar("Hold on", path, (float)(i + 1) / guids.Length);
            int index = path.LastIndexOf("/");

            string fileName = path.Substring(index + 1);

            if (m_IsFront)
            {
                fileName = fileName.Substring(m_InputTxtCount);
            }
            else
            {
                index = fileName.LastIndexOf(".");
                fileName = fileName.Substring(0, index);
                Debug.LogError(fileName);
                fileName = fileName.Substring(0, fileName.Length - m_InputTxtCount);
            }

            AssetDatabase.RenameAsset(path, fileName);
        }
        EditorUtility.ClearProgressBar();
    }

    private void RenameAdd()
    {
        if (string.IsNullOrEmpty(m_TargetPath))
        {
            return;
        }

        if (string.IsNullOrEmpty(m_InputAddTxt))
        {
            return;
        }

        string[] guids = AssetDatabase.FindAssets("", new string[] { m_TargetPath });
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            EditorUtility.DisplayProgressBar("Hold on", path, (float)(i + 1) / guids.Length);
            int index = path.LastIndexOf("/");

            string fileName = path.Substring(index + 1);

            if (m_IsFront)
            {
                fileName = m_InputAddTxt + fileName;
            }
            else
            {
                index = fileName.LastIndexOf(".");
                fileName = fileName.Substring(0, index);
                Debug.LogError(fileName);
                fileName = fileName + m_InputAddTxt;
            }

            AssetDatabase.RenameAsset(path, fileName);
        }
        EditorUtility.ClearProgressBar();
    }
}




