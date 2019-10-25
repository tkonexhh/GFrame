using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

namespace GFrame
{

    public class FileLogOutput
    {

        static string LogPath = "Log";
        private bool m_IsRunning = false;
        private StreamWriter m_LogWriter = null;
        private Thread m_FileLogThread = null;

        private Queue<LogData> m_WritingLogQueue = null;
        private Queue<LogData> m_WaitingLogQueue = null;

        public FileLogOutput()
        {
            if (LogMgr.S.fileLogLevel == LogLevel.None)
                return;
            System.DateTime now = System.DateTime.Now;
            string logName = string.Format("Log_{0}_{1}_{2}_{3}_{4}_{5}",
                now.Year, now.Month.ToString().PadLeft(2, '0'), now.Day.ToString().PadLeft(2, '0'), now.Hour.ToString().PadLeft(2, '0'), now.Minute.ToString().PadLeft(2, '0'), now.Second).ToString().PadLeft(2, '0');
            string logPath = string.Format("{0}{1}/{2}.txt", FilePath.persistentDataPath, LogPath, logName);
            if (File.Exists(logPath))
                File.Delete(logPath);
            string logDir = Path.GetDirectoryName(logPath);
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);
            m_LogWriter = new StreamWriter(logPath);
            m_LogWriter.AutoFlush = true;
            m_WritingLogQueue = new Queue<LogData>();
            m_WaitingLogQueue = new Queue<LogData>();
            m_FileLogThread = new Thread(new ThreadStart(WriteLog));
            m_FileLogThread.Start();
            m_IsRunning = true;
        }

        void WriteLog()
        {

            while (m_IsRunning)
            {
                if (m_WritingLogQueue.Count == 0)
                {
                    var tmpQueue = m_WritingLogQueue;
                    m_WritingLogQueue = m_WaitingLogQueue;
                    m_WaitingLogQueue = tmpQueue;
                }
                else
                {
                    while (m_WritingLogQueue.Count > 0)
                    {
                        var log = this.m_WritingLogQueue.Dequeue();
                        if (log.logLevel == LogLevel.Error)
                        {
                            m_LogWriter.WriteLine("ERROR-START-----------------------------------------------------------------------------------------------------------");
                            m_LogWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + log.log + "\n");
                            m_LogWriter.WriteLine(log.track);
                            m_LogWriter.WriteLine("ERROR-END-------------------------------------------------------------------------------------------------------------");
                        }
                        else
                        {
                            m_LogWriter.WriteLine(System.DateTime.Now.ToString() + "\t" + log.log);
                        }
                    }
                }
            }
        }

        public void FileLog(LogData logData)
        {
            if (!m_IsRunning)
            {
                return;
            }
            if (string.IsNullOrEmpty(logData.log))
            {
                return;
            }
            if (logData.logLevel <= LogMgr.S.fileLogLevel)
                m_WaitingLogQueue.Enqueue(logData);
        }
    }
}




