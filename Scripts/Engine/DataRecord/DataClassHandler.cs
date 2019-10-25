using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace GFrame
{

    public class DataClassHandler<T> where T : IDataClass, new()
    {
        //是否加密
        private static bool ENCRY = false;
        protected static T m_Data;
        protected static bool m_AutoSave = false;
        protected static int m_AutoSaveTimer;
        protected static string m_FileName;
        protected static string m_FileNameKey;

        public static T data
        {
            get { return m_Data; }
        }

        private bool autoSave
        {
            get { return m_AutoSave; }
            set
            {
                if (m_AutoSave == value)
                {
                    return;
                }

                m_AutoSave = value;

                if (m_AutoSave)
                {
                    EventSystem.S.Register(EngineEventID.OnAfterApplicationPauseChange, OnAutoSaveChecker);
                    EventSystem.S.Register(EngineEventID.OnApplicationQuit, OnApplicationQuit);

                    if (m_AutoSaveTimer <= 0)
                    {
                        m_AutoSaveTimer = Timer.S.Post2Really(OnAutoSaveTimer, 180, -1);
                    }
                }
                else
                {
                    EventSystem.S.UnRegister(EngineEventID.OnAfterApplicationPauseChange, OnAutoSaveChecker);
                    EventSystem.S.UnRegister(EngineEventID.OnApplicationQuit, OnApplicationQuit);

                    if (m_AutoSaveTimer > 0)
                    {
                        Timer.S.Cancel(m_AutoSaveTimer);
                        m_AutoSaveTimer = -1;
                    }
                }
            }
        }


        protected void OnAutoSaveTimer(int count)
        {
            Save();
        }

        protected void OnAutoSaveChecker(int key, params object[] args)
        {
            bool pause = (bool)args[0];
            if (pause)
            {
                Save();
            }
        }

        protected void OnApplicationQuit(int key, params object[] args)
        {
            Save();
        }

        protected static string dataFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(m_FileName))
                {
                    string fileName = typeof(T).FullName;//.GetHashCode().ToString();
                    if (ENCRY)
                    {
                        fileName = fileName.GetHashCode().ToString();
                    }

                    if (string.IsNullOrEmpty(m_FileNameKey))
                    {
                        m_FileName = string.Format("{0}{1}", FilePath.persistentDataPath4Recorder, fileName);
                    }
                    else
                    {
                        m_FileName = string.Format("{0}{1}{2}", FilePath.persistentDataPath4Recorder, fileName, m_FileNameKey);
                    }
                }

                return m_FileName;
            }
        }


        public void EnableAutoSave()
        {
            autoSave = true;
        }

        public void DisableAutoSave()
        {
            autoSave = false;
        }

        public static void SetFileNameKey(string key)
        {
            m_FileNameKey = key;
        }

        public static void Load()
        {
            if (m_Data != null)
                return;

            if (IO.IsFileExist(dataFilePath))
            {
                m_Data = SerializeHelper.DeserializeJson<T>(dataFilePath, ENCRY);
            }

            if (m_Data == null)
            {
                m_Data = new T();
                m_Data.InitWithEmptyData();
                m_Data.SetDataDirty();
            }

            if (m_Data != null)
            {
                m_Data.OnDataLoadFinish();
                // /return;
            }

            // m_Data = new T();
            // m_Data.InitWithEmptyData();
            // m_Data.SetDataDirty();
            // m_Data.OnDataLoadFinish();
        }

        public static void Save()
        {
            if (m_Data == null)
                return;
            //实例方法和未公开的属性不会被转化
            if (m_Data.GetIsDataDirty())
            {
                SerializeHelper.SerializeJson(dataFilePath, m_Data, ENCRY);
            }

        }
    }
}




