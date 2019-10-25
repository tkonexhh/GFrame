using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace GFrame
{

    public class LogMgr : TSingleton<LogMgr>
    {
        public LogLevel logLevel = LogLevel.Max;
        public LogLevel fileLogLevel = LogLevel.Max;
        private int mainThreadID = -1;
        private FileLogOutput m_LogOutput;
        private Dictionary<LogType, LogLevel> m_LogTypeLevelDict;

        public override void OnSingletonInit()
        {
            Application.logMessageReceived += LogCallBack;
            Application.logMessageReceivedThreaded += LogThreadCallBack;
            this.mainThreadID = Thread.CurrentThread.ManagedThreadId;
            m_LogOutput = new FileLogOutput();

            m_LogTypeLevelDict = new Dictionary<LogType, LogLevel>
            {
                { LogType.Log, LogLevel.Normal },
                { LogType.Warning, LogLevel.Warning },
                { LogType.Assert, LogLevel.Assert },
                { LogType.Error, LogLevel.Error },
                { LogType.Exception, LogLevel.Error },
            };
        }

        void LogCallBack(string log, string track, LogType type)
        {
            if (this.mainThreadID == Thread.CurrentThread.ManagedThreadId)
                Output(log, track, type);
        }

        void LogThreadCallBack(string log, string track, LogType type)
        {
            if (this.mainThreadID != Thread.CurrentThread.ManagedThreadId)
                Output(log, track, type);
        }

        void Output(string log, string track, LogType type)
        {
            LogData logData = new LogData();
            logData.log = log;
            logData.track = track;
            logData.logLevel = m_LogTypeLevelDict[type];
            m_LogOutput.FileLog(logData);
        }
    }
}