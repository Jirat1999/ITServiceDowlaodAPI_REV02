using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public class cLog
    {
        public static void C_WRTxLog(string ptClass, string ptFunction, string ptMessage)
        {
            try
            {
                string tPath = AppDomain.CurrentDomain.BaseDirectory + "Log";

                if (!Directory.Exists(tPath))
                {
                    Directory.CreateDirectory(tPath);
                }

                string tFileName = tPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                string tLogMsg = $"{DateTime.Now:HH:mm:ss} | Class: {ptClass} | Func: {ptFunction} | Msg: {ptMessage}";

                using (StreamWriter w = File.AppendText(tFileName))
                {
                    w.WriteLine(tLogMsg);
                }
            }
            catch
            {
                
            }
        }
    }
}
