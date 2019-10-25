using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{

    public class PathHelper
    {
        // 上一级目录
        public static string GetParentDir(string dir, int floor = 1)
        {
            string subDir = dir;

            for (int i = 0; i < floor; ++i)
            {
                int last = subDir.LastIndexOf('/');
                subDir = subDir.Substring(0, last);
            }

            return subDir;
        }

        public static string GetParentDirName(string dir, int floor = 1)
        {
            string subDir = dir;
            string tempStr = null;
            for (int i = 0; i < floor; ++i)
            {
                int last = subDir.LastIndexOf('/');
                if (i == floor - 1)
                {
                    tempStr = subDir.Substring(0, last);
                    int last_before = tempStr.LastIndexOf('/');
                    int length = last - last_before - 1;
                    subDir = subDir.Substring(last_before + 1, length);
                }
                else
                {
                    subDir = subDir.Substring(0, last);
                }
            }
            return subDir;
        }

        ///无后缀的文件名
        public static string FileNameWithoutExtend(string name)
        {
            if (name == null)
                return null;

            int endIndex = name.LastIndexOf('.');
            if (endIndex > 0)
            {
                return name.Substring(0, endIndex);
            }

            return name;
        }

        //根据路径获取文件名字
        public static string Path2Name(string path)
        {
            int startIndex = path.LastIndexOf("/") + 1;
            int endIndex = path.LastIndexOf(".");
            if (endIndex > 0)
            {
                int length = endIndex - startIndex;
                return path.Substring(startIndex, length).ToLower();
            }
            return path.Substring(startIndex).ToLower();
        }

        public static string GetSamePart(string a, string b)
        {
            string[] a1 = a.Split('/');
            string[] b1 = b.Split('/');
            string c = "";
            for (int i = 0; i < a1.Length; i++)
            {
                for (int j = 0; j < b1.Length; j++)
                {
                    //Debug.LogError(a1[i] + b1[j]);
                    if (string.Equals(a1[i], b1[j]))
                    {
                        c = c + a1[i] + "/";
                        break;
                    }
                }
            }

            return c;
        }
    }
}




