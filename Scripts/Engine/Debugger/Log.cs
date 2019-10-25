using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GFrame
{

    public enum LogLevel
    {
        None = 0,
        Normal,
        Warning,
        Assert,
        Error,
        Max,
    }


    public class Log
    {

        static bool CheckLogLevel(LogLevel level)
        {

            if (LogMgr.S.logLevel < level)
                return false;

            return true;
        }

        public static void e(object msg)
        {
            if (CheckLogLevel(LogLevel.Error))
                Debug.LogError(msg);
        }

        public static void e(string msg, params object[] args)
        {
            if (CheckLogLevel(LogLevel.Error))
                Debug.LogErrorFormat(msg, args);
        }

        public static void w(object msg)
        {
            if (CheckLogLevel(LogLevel.Warning))
                Debug.LogWarning(msg);
        }

        public static void w(string msg, params object[] args)
        {
            if (CheckLogLevel(LogLevel.Warning))
                Debug.LogWarningFormat(msg, args);
        }

        public static void i(object msg)
        {
            if (CheckLogLevel(LogLevel.Normal))
                Debug.Log(msg);
        }

        public static void i(string msg, params object[] args)
        {
            if (CheckLogLevel(LogLevel.Normal))
                Debug.LogFormat(msg, args);
        }

        public static void Assert(bool condition, object msg)
        {
            if (CheckLogLevel(LogLevel.Assert))
                Debug.Assert(condition, msg);
        }
    }


    public class LogData
    {
        public LogLevel logLevel;
        public string log;
        public string track;
    }
}
