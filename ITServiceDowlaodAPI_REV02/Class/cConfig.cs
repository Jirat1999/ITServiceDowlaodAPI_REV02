using ITServiceDowlaodAPI_REV02.Models;
using ITServiceDowlaodAPI_REV02.Models.Setting;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ITServiceDowlaodAPI_REV02.Class
{
    public static class cConfig
    {
        public static cmlSettings oC_SettingConfig = new cmlSettings();
        public static cmlApiConfig oC_ApiConfig = new cmlApiConfig();
        public static cmlConfigDB oC_ConfigDB = new cmlConfigDB();
        public static List<cmlTaskList> aoC_Tasklist = new List<cmlTaskList>();

        // ฟังก์ชันอัปเดตไฟล์ appsettings.json เด้งกลับเป็น false
        public static void C_PRCxSetManualTrigger(bool pbStatus)
        {
            try
            {
                if (oC_SettingConfig != null) oC_SettingConfig.bManualTrigger = pbStatus;

                string tPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (File.Exists(tPath))
                {
                    string tJsonContent = File.ReadAllText(tPath);
                    var oJsonNode = JsonNode.Parse(tJsonContent);

                    if (oJsonNode?["oSettingConfig"] != null)
                    {
                        oJsonNode["oSettingConfig"]["bManualTrigger"] = pbStatus;

                        var oOptions = new JsonSerializerOptions { WriteIndented = true };
                        File.WriteAllText(tPath, oJsonNode.ToJsonString(oOptions));
                    }
                }
            }
            catch (Exception oEx)
            {
                cConsole.C_PRCxLogError("Update Config Error: " + oEx.Message);
            }
        }

        public static string C_PRCxGetIpLocal(int pnMode)
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