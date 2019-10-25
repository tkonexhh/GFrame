using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using OfficeOpenXml;

namespace GFrame.UnityEditor
{

    //在面板上创建表，表名，注释，列名 类型
    //添加数据
    public class TableCreater : EditorWindow
    {
        [MenuItem("Custom/Window/表格创建工具")]//TableCreaterWindow
        public static void CreateTable()
        {
            EditorWindow.GetWindow<TableCreater>(true, "创建配置表", true);
        }

        private const int START_WIDTH = 1;
        private const int START_HEIGHT = 4;
        private string m_InputWidth;
        private string m_InputHeight;

        static int m_Width = START_WIDTH;
        static int m_Height = START_HEIGHT;

        string m_TableName;
        string[,] m_values;
        int[] m_SelectIndex;
        int[] m_SelectType;

        string[] m_TypeOption = new string[] { "A", "N" };
        static string[] m_TypeChoices = new string[] { "int", "float", "string", "bool", "List<int>", "List<float>", "List<string>" };


        private void Awake()
        {
            m_TableName = "Test";
            m_values = new string[m_Width, m_Height];
            m_SelectIndex = new int[m_Width];
            m_SelectType = new int[m_Width];
        }

        void OnGUI()
        {

            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));
            m_TableName = EditorGUI.TextField(rect, "表名", m_TableName);
            if (GUILayout.Button("读取配置表", GUILayout.Width(80)))
            {
                ReadTable();
            }

            EditorGUILayout.BeginHorizontal();

            m_InputWidth = EditorGUILayout.TextField(m_InputWidth, GUILayout.Width(100));
            m_InputHeight = EditorGUILayout.TextField(m_InputHeight, GUILayout.Width(100));
            if (GUILayout.Button("设置长宽", GUILayout.Width(80)))
            {
                RefeshData(int.Parse(m_InputWidth), int.Parse(m_InputHeight));
                // /Reset(int.Parse(m_InputWidth), int.Parse(m_InputHeight));
            }
            EditorGUILayout.EndHorizontal();

            //return;
            EditorGUILayout.BeginScrollView(new Vector2(50, 50));
            int posx = 20;
            int width = 80;//Screen.width / (m_Width + 1);
            for (int x = 0; x < m_Width + 2; x++)
            {
                GUILayout.BeginArea(new Rect(posx + x * width, 20, width, Screen.height - 80));
                if (x == 0)
                {
                    for (int y = 0; y < START_HEIGHT; y++)
                    {
                        EditorGUILayout.LabelField(GetTipByIndex(y));
                    }
                }
                else if (x > 0 && x < m_Width + 1)
                {
                    for (int y = 0; y < m_Height; y++)
                    {
                        if (y == 1)
                        {
                            m_SelectIndex[x - 1] = EditorGUILayout.Popup(m_SelectIndex[x - 1], m_TypeOption, GUILayout.Width(width));
                        }
                        else if (y == 2)
                        {
                            m_SelectType[x - 1] = EditorGUILayout.Popup(m_SelectType[x - 1], m_TypeChoices, GUILayout.Width(width));
                        }
                        else
                        {
                            //EditorGUILayout.TextField("", GUILayout.Width(width));
                            m_values[x - 1, y] = EditorGUILayout.TextField(m_values[x - 1, y], GUILayout.Width(width));
                        }

                    }
                }
                else if (x == m_Width + 1)
                {
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        AddColumn();
                    }
                }

                if (x == 1)
                {
                    if (GUILayout.Button("+", GUILayout.Width(30)))
                    {
                        AddRow();
                    }
                }

                GUILayout.EndArea();
            }


            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Reset"))
            {
                Reset();
            }
            if (GUILayout.Button("创建xlsx"))
            {
                CreateXls();
            }
        }

        private void AddColumn()
        {
            RefeshData(m_Width + 1, m_Height);
        }

        private void AddRow()
        {
            RefeshData(m_Width, m_Height + 1);
        }

        private void RefeshData(int targetWidth, int targetHeight)
        {
            var oldValues = m_values;
            var oldValues_select = m_SelectIndex;
            var oldValues_type = m_SelectType;
            int oldWidth = m_Width;
            int oldHeight = m_Height;
            m_Width = targetWidth;
            m_Height = targetHeight;
            m_values = new string[m_Width, m_Height];
            for (int x = 0; x < Mathf.Min(m_Width, oldWidth); x++)
            {
                for (int y = 0; y < Mathf.Min(m_Height, oldHeight); y++)
                {
                    m_values[x, y] = oldValues[x, y];
                }
            }

            if (oldWidth != m_Width)
            {
                m_SelectIndex = new int[m_Width];
                for (int x = 0; x < Mathf.Min(m_Width, oldWidth); x++)
                {
                    m_SelectIndex[x] = oldValues_select[x];
                }

                m_SelectType = new int[m_Width];
                for (int x = 0; x < Mathf.Min(m_Width, oldWidth); x++)
                {
                    m_SelectType[x] = oldValues_type[x];
                }
            }

        }


        void CreateXls()
        {
            if (string.IsNullOrEmpty(m_TableName))
            {
                Debug.LogError("文件名为空");
                return;
            }

            string path = Path.Combine(ProjectPathConfig.externalTablePath, m_TableName + ".xlsx");
            Debug.LogError(path);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(path);
            }

            using (ExcelPackage package = new ExcelPackage(file))
            {
                //在excel空文件添加新sheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("sheet1");

                for (int x = 0; x < m_Width; x++)
                {
                    for (int y = 0; y < m_Height; y++)
                    {

                        if (y == 1)
                        {
                            worksheet.Cells[y + 1, x + 1].Value = m_TypeOption[m_SelectIndex[x]];
                        }
                        else if (y == 2)
                        {
                            worksheet.Cells[y + 1, x + 1].Value = m_TypeChoices[m_SelectType[x]];
                        }
                        else
                        {
                            worksheet.Cells[y + 1, x + 1].Value = m_values[x, y];
                        }
                    }
                }

                package.Save();
            }
        }

        void ReadTable()
        {

        }

        string GetTipByIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return "注释";
                case 1:
                    return "是否注释列";
                case 2:
                    return "类型";
                case 3:
                    return "变量名";
            }
            return "";
        }

        void Reset(int width = START_WIDTH, int height = START_HEIGHT)
        {
            m_Width = width;
            m_Height = Mathf.Max(START_HEIGHT, height);
            m_SelectIndex = new int[m_Width];
            m_SelectType = new int[m_Width];
            m_values = new string[m_Width, m_Height];
        }
    }


}




