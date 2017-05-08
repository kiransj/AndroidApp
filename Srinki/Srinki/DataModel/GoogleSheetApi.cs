using System;
using System.Collections.Generic;
using System.Text;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace Srinki.DataModel
{
    class GoogleSheetApi
    {
        SheetsService service;
        const string spreadSheetId = "1EOAWeb9vXzMpBcgprX1J_UL7JPjVsuIKW3sUNU4Lev4";

        public GoogleSheetApi()
        {
            service = new SheetsService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyD6vtsw_5qftsNOV-q2ND2B0_wb3mMq3dg_1",
                ApplicationName = "Srinki"
            });
        }

        public IList<IList<Object>> getSheetData(string sheetName, string range)
        {
            ValueRange valuRange;
            String sheetRange = string.Format("'{0}'!{1}", sheetName, range);
            try
            {
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadSheetId, sheetRange);                
                valuRange = request.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Google Sheet API failed\nDue to " + ex.Message);
            }

            IList<IList<Object>> values = valuRange.Values;
            if (values == null) throw new Exception("Failed to fetch data from google sheet. Values is NULL");
            if (values.Count <= 0) throw new Exception("Failed to fetch data from google sheet. Values is 0");
            return values;
        }
    }
}
