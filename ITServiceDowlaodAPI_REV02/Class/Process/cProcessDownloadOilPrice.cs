namespace ITServiceDowlaodAPI_REV02.Class.Process
{
    public class cProcessDownloadOilPrice
    {
        public async Task<bool> C_PRCbProcessAsync(CancellationToken oStoppingToken)
        {
            try
            {
                var (oData, tJon) = await cApiService.C_PRCtOilPriceAsync(oStoppingToken);
                if (oData != null && !string.IsNullOrEmpty(tJon))
                {
                    oData.tRawJson = tJon;

                    await new cDatabaseService().C_PRCxDataInsertOilPriceAsync(oData);

                    return true;
                }
                cConsole.C_PRCxLogWarning(">>> Skip saving: API returned null or empty data.");
                return false;
            }
            catch (Exception ex)
            {
                cConsole.C_PRCxLogError("Error in cProcessDownloadOilPrice: " + ex.Message);
                cLog.C_PRCxLog("cProcessDownloadOilPrice", "C_PRCbProcessAsync", ex.Message);
                return false;
            }
        }
    }
}