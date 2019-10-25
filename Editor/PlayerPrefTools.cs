using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.Diagnostics;

namespace GFrame.UnityEditor
{
    public class PlayerPrefTools
    {
        [MenuItem("Custom/Tools/Clear All Saved Data")]
        static public void ClearSavedData()
        {
            PlayerPrefs.DeleteAll();
            DirectoryInfo forder = new DirectoryInfo(FilePath.persistentDataPath4Recorder);
            forder.Delete(true);
        }


        [MenuItem("Custom/Path/OpenPersistent")]
        static public void OpenPersistent()
        {
            OpenDirectory(FilePath.persistentDataPath);
        }



        private static void OpenDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            if (!Directory.Exists(path))
            {
                UnityEngine.Debug.LogError("No Directory: " + path);
                return;
            }

            //Application.dataPath 只能在主线程中获取
            int lastIndex = Application.dataPath.LastIndexOf("/");
            string s_sheelPath = ProjectPathConfig.externalToolsPath + "OpenDir/openDir.sh";
            // 新开线程防止锁死
            Thread newThread = new Thread(new ThreadStart(() =>
            {
                CmdOpenDirectory(path, s_sheelPath);
            }));
            newThread.Start();
        }

        private static void CmdOpenDirectory(string obj, string sheelpath)
        {
            Process p = new Process();
#if UNITY_EDITOR_WIN
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c start " + obj.ToString();
#elif UNITY_EDITOR_OSX
            p.StartInfo.FileName = "/bin/sh";
            p.StartInfo.Arguments = sheelpath + " " + obj.ToString();
#endif
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = false;
            p.Start();

            p.WaitForExit();
            p.Close();
        }
    }
}
