using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer.Utilities
{
    public enum LogType { Info, Warning, Error }

    public static class Logger
    {
        /*
         * log-5.json
         */

        #region Fields
        static string logFolder = "";
        static int maxLogSize = 0;
        static int currentLogNum = -1;

        static string tolog = "";

        static StackTrace trace;

        static StreamWriter writer;
        #endregion

        #region Funcs
        public static void Init(string LogFolder, int MaxLogSize)
        {
            logFolder = LogFolder;
            maxLogSize = MaxLogSize;

            int lastLogIndex = findLastLogIndex(logFolder);
            if (lastLogIndex == -1) // create log file, if there is no logs
                currentLogNum = 0;
            else // open existing log file
                currentLogNum = lastLogIndex;

            writer = new StreamWriter(logFolder + fullLogName, true);

            trace = new StackTrace();
        }
        public static void Init(int MaxLogSize)
        {
            Init("./", MaxLogSize);
        }
        public static void Init()
        {
            Init("./", 
                1024 // kB
                * 1024 //mB
                * 250 // == 250mB
                );
        }

        public static void Log(LogType type, string msg)
        {
            tolog =
                '[' + DateTime.Now.ToString() + ']' +
                '[' + type.ToString() + ']' +
                '[' + trace.GetFrame(1).GetMethod().DeclaringType.Name + ']' +
                '[' + msg + ']';

            var met = trace.GetFrame(1).GetMethod();

            writeLog(tolog);
        }
        
        private static int findLastLogIndex(string folder)
        {
            int maxLogNum = -1;
            List<string> files = new List<string>(Directory.GetFiles(folder));
            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                int num = -1;

                if (file.IndexOf(logPrefix) != -1)
                    num = Convert.ToInt32(file.Substring(6, file.IndexOf('.', 6) - 6));

                if (num > maxLogNum)
                    maxLogNum = num;
            }

            return maxLogNum;
        }
        private static void writeLog(string data)
        {
            // if upcoming file size more then maxLogSize, create new log
            if (new FileInfo(logFolder + fullLogName).Length + data.Length > maxLogSize)
            {
                writer.Close();
                writer.Dispose();

                currentLogNum++;
                writer = new StreamWriter(logFolder + fullLogName, true);
                writer.Write(data + "\n");
                writer.Flush();
            }
            else
            {
                writer.Write(data + "\n");
                writer.Flush();
            }
        }
        #endregion

        #region Props
        private static string logPrefix { get { return "log-"; } }
        private static string logExtension { get { return ".json"; } }
        private static string fullLogName { get { return logPrefix + currentLogNum + logExtension; } }
        #endregion
    }
}
