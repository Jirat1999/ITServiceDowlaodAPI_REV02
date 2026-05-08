namespace ITServiceDowlaodAPI_REV02.Models.Database
{
    public class cmlTCNM_ERROR_ErrorLogs
    {
        public long nFNErrorId { get; set; }
        public string tFTProcessName { get; set; }
        public string tFTErrorMessage { get; set; }
        public string tFTStackTrace { get; set; }
        public DateTime dFDErrorDate { get; set; } = DateTime.Now;
    }
}
