using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTransformer.Utilities
{
    public static class Logger
    {
        /*
         * log-5.txt
         */

        #region Fields
        static string logFolder = "";
        static int maxLogSize = 0;
        static int logNumber = 0;

        static StackTrace trace;

        static StreamWriter stream;
        #endregion

        #region Funcs
        public static void Init(string LogFolder, int MaxLogSize)
        {
            logFolder = LogFolder;
            maxLogSize = MaxLogSize; 

            List<string> files = new List<string>(Directory.GetFiles(logFolder));
            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                string num = "";

                if (file.IndexOf("log-") != -1)
                    num = file.Substring(4, file.IndexOf('.'));

            }

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
        #endregion

        public static void Log(EventLogEntryType type, string msg)
        {
            //trace.GetFrame(1).GetMethod().Name
        }
    }
}
