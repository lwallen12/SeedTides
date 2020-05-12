using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PastTideData.Jobs
{
    public abstract class BaseSeedJob
    {
        public int[] _stations = {8775296,8774230,8775241,8776604,8775870,8771013,8772471,8772447,8771341,8771450,8771486,8770808,8770777,8773767,8773146,8770613,8775244,8775792,8775237,
                            8770475,8779770,8773259,8778490,8773701,8770520,8779280,8777812,8774770,8770971,8776139,8770570,8771972,8772985,8773037,8779748,8779749,8770822};

        public string _connStr = "server=test1.ce8cn9mhhgds.us-east-1.rds.amazonaws.com;user=Wallen;database=whattodo;port=3306;password=MyRDSdb1";

        public String ReadAPI(string apiPath)
        {
            //string apiPath = "https://tidesandcurrents.noaa.gov/api/datagetter?product=water_temperature&begin_date=20170428&end_date=20180428&datum=MLLW&interval=h&station=8774230&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL";
            WebRequest requestObject = WebRequest.Create(apiPath);
            requestObject.Method = "GET";

            HttpWebResponse responseObject = null;
            responseObject = (HttpWebResponse)requestObject.GetResponse();

            string strObj;

            using (Stream stream = responseObject.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);

                strObj = sr.ReadToEnd();
            }

            //Don't do with entire object if doing whole year!
            // Console.WriteLine(strObj); //Definitely gives us our data

            //WaterTempPOCO waterTempPOCO = JsonConvert.DeserializeObject<WaterTempPOCO>(strObj);

            return strObj;
        }

    }
}
