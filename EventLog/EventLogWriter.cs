using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EventLog
{
    public class EventLogWriter
    {
        /// <summary>
        /// イベントログ出力
        /// </summary>
        /// <param name="eventLogSourceName"></param>
        /// <param name="ex"></param>
        /// <param name="type"></param>
        /// <param name="args">ExpandoObjectで作られたインプット情報</param>
        /// <param name="classId"></param>
        public static void Write(string eventLogSourceName, Exception ex, EventLogEntryType type, int moduleId, Const.ClassId classId)
        {
            var eventId = GetEventId(moduleId, classId);
            Write(eventLogSourceName, ex, type, eventId);
        }

        /// <summary>
        /// イベントログ出力
        /// </summary>
        /// <param name="eventLogSourceName"></param>
        /// <param name="ex"></param>
        /// <param name="type"></param>
        /// <param name="eventId"></param>
        public static void Write(string eventLogSourceName, Exception ex, EventLogEntryType type, int eventId = 0)
        {
            var message = ex.ToString();
            Write(eventLogSourceName, message, type, eventId);
        }

        /// <summary>
        /// イベントログ出力
        /// </summary>
        /// <param name="eventLogSourceName"></param>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="eventId"></param>
        public static void Write(string eventLogSourceName, string message, EventLogEntryType type, int eventId = 0)
        {
            if (!System.Diagnostics.EventLog.SourceExists(eventLogSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventLogSourceName, string.Empty);
            }
            System.Diagnostics.EventLog.WriteEntry(eventLogSourceName, message, type, eventId);
        }

        /// <summary>
        /// イベントID取得
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public static int GetEventId(int moduleId, Const.ClassId classId)
        {
            return moduleId + (int)classId;
        }
    }
}