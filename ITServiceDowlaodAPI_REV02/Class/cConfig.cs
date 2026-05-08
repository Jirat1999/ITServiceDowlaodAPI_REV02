using System;
using System.Net;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ITServiceDowlaodAPI_REV02.Models;
using ITServiceDowlaodAPI_REV02.Models.Setting;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public static class cConfig
    {
        public static cmlSettings oSettingConfig = new cmlSettings();
        public static cmlApiConfig oApiConfig = new cmlApiConfig();
        public static cmlConfigDB oConfigDB = new cmlConfigDB();
        public static List<cmlTaskList> aoC_Tasklist = new List<cmlTaskList>();

        // ฟังก์ชันอัปเดตไฟล์ appsettings.json เด้งกลับเป็น false
        public static void C_SETbManualTrigger(bool pbStatus)
        {
            try
            {
                if (oSettingConfig != null) oSettingConfig.bManualTrigger = pbStatus;

                string tPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (File.Exists(tPath))
                {
                    string tJson = File.ReadAllText(tPath);

                    if (pbStatus == false)
                    {
                        tJson = tJson.Replace("\"bManualTrigger\": true", "\"bManualTrigger\": false")
                                     .Replace("\"bManualTrigger\":true", "\"bManualTrigger\": false")
                                     .Replace("\"bManualTrigger\" : true", "\"bManualTrigger\": false")
                                     .Replace("\"bManualTrigger\" :true", "\"bManualTrigger\": false");
                    }

                    File.WriteAllText(tPath, tJson);
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_LogError("Update Config Error: " + oEx.Message);
            }
        }

        public static string C_GETtIpLocal(int pnMode)
        {
            try
            {
                string tHostName = Dns.GetHostName();
                if (pnMode == 1) return tHostName;
                var ipv4 = Dns.GetHostAddresses(tHostName).FirstOrDefault(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                return ipv4 != null ? ipv4.ToString() : "127.0.0.1";
            }
            catch { return "Offline"; }
        }
    }
}