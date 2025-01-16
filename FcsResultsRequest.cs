using Oracle.DataAccess.Client;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace FcsResultsRequestV2
{


    public class FcsResultsRequest
    //public partial class FcsResultsRequest : UserControl, IExtensionWindow
    {
        private const string PHRASE_HEADER = "UrlService_FCS";
        private const string PHRASE_ENTRY = "FcsResultRequest";
        private OracleConnection _oraCon;




        public FcsResultsRequest(OracleConnection oraCon)
        {
            this._oraCon = oraCon;
        }



        string GetUrl()
        {

            string url = string.Empty;
            string sqlQuery = string.Format("SELECT phrase_description FROM lims_sys.PHRASE_HEADER ph, lims_sys.Phrase_Entry pe where " +
" ph.NAME = '{0}' and  pe.PHRASE_ID = ph.PHRASE_ID and Phrase_Name = '{1}'", PHRASE_HEADER, PHRASE_ENTRY);


            using (OracleCommand cmd = new OracleCommand(sqlQuery, _oraCon))
            {

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        url = (string)reader["phrase_description"].ToString().Trim();
                    }
                }
            }
            return url;

        }



        public void SendResult2NautilusWS(string coaReport)
        {
            try
            {

                Debugger.Launch();
                string Url = GetUrl();
                if (Url == null) { return; }

                string apiReqResult = "";

                string fullUrl = GetUrl() + coaReport;

                // API request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullUrl);
                request.Method = "POST";
                request.ContentLength = 0;
                using (WebResponse response = request.GetResponse())
                {
                    var a = response;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {

                            apiReqResult = reader.ReadToEnd();

                        }
                    }
                }


            }
            catch
            {

            }
        }





    }
}



