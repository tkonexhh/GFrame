using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace GFrame
{

    public class FileMgr : TSingleton<FileMgr>
    {
        //private ZipFile m_ZipFile = null;
        public override void OnSingletonInit()
        {
            // if (Platform.IsAndroid && !Platform.IsEditor)
            // {
            //     if (m_ZipFile == null)
            //     {

            //         m_ZipFile = new ZipFile(File.Open(Application.dataPath, FileMode.Open, FileAccess.Read));
            //     }
            // }
        }

        // public Stream OpenStreamInZip(string absPath)
        // {
        //     string tag = "!/assets/";
        //     string androidFolder = absPath.Substring(0, absPath.IndexOf(tag));

        //     int startIndex = androidFolder.Length + tag.Length;
        //     string relativePath = absPath.Substring(startIndex, absPath.Length - startIndex);

        //     ZipEntry zipEntry = m_ZipFile.GetEntry(string.Format("assets/{0}", relativePath));

        //     if (zipEntry != null)
        //     {
        //         return m_ZipFile.GetInputStream(zipEntry);
        //     }
        //     else
        //     {
        //         Log.e(string.Format("Can't Find File {0}", absPath));
        //     }

        //     return null;
        // }



        public string GetFileInInner(string fileName)
        {
            if (Platform.IsAndroid && !Platform.IsEditor)
            {
                return fileName;
            }

            return GetFullPath(fileName);
        }

        private string GetFileInZip(ZipFile zipFile, string fileName)
        {
            int totalCount = 0;

            foreach (var entry in zipFile)
            {
                ++totalCount;
                ICSharpCode.SharpZipLib.Zip.ZipEntry e = entry as ICSharpCode.SharpZipLib.Zip.ZipEntry;
                if (e != null)
                {
                    if (e.IsFile)
                    {
                        Debug.LogError("ZIP:" + e.Name);
                        if (e.Name.EndsWith(fileName))
                        {
                            return zipFile.Name + "/!/" + e.Name;
                        }
                    }
                }
            }
            return null;
        }



        public static string GetFullPath(string fileName)
        {
            if (!IO.IsFileExist(fileName))
            {
                return null;
            }
            FileInfo file = new FileInfo(fileName);
            return file.FullName;
        }

        public static void GetFileInFolder(string dirName, string fileName, List<string> outResult)
        {
            if (outResult == null)
            {
                return;
            }

            DirectoryInfo directory = new DirectoryInfo(dirName);
            //if(directory.Parent!=null)

            FileInfo[] files = directory.GetFiles();
            string fname = string.Empty;
            for (int i = 0; i < files.Length; i++)
            {
                fname = files[i].Name;
                if (fname == fileName)
                {
                    outResult.Add(files[i].FullName);
                }
            }

        }
    }
}




