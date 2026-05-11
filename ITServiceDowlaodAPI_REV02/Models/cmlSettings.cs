namespace ITServiceDowlaodAPI_REV02.Models
{
    public class cmlSettings
    {
        public string? tAppCode { get; set; }
        public string? tAppName { get; set; }
        public string? tMode { get; set; } = string.Empty;
        public int nCycleTime { get; set; }
        public bool bManualTrigger { get; set; }
    }
}
