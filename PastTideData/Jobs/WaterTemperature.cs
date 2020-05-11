using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PastTideData.POCOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PastTideData.Jobs
{
    public class WaterTemperature
    {
        int[] _stations = {8775296,8774230,8775241,8776604,8775870,8771013,8772471,8772447,8771341,8771450,8771486,8770808,8770777,8773767,8773146,8770613,8775244,8775792,8775237,
                            8770475,8779770,8773259,8778490,8773701,8770520,8779280,8777812,8774770,8770971,8776139,8770570,8771972,8772985,8773037,8779748,8779749,8770822};

        string _connStr = "server=test1.ce8cn9mhhgds.us-east-1.rds.amazonaws.com;user=Wallen;database=whattodo;port=3306;password=MyRDSdb1";

        public void SeedWaterTemp()
        { 
            Console.WriteLine(DateTime.Now);

            DataTable dt = new DataTable("StationWaterTemperature");

            dt.Columns.Add("ReadingDatetime", typeof(DateTime));
            dt.Columns.Add("StationId", typeof(int));
            dt.Columns.Add("WaterTemperature", typeof(string));
            dt.Columns.Add("WaterTemperatureF", typeof(string));

            for (int year = 2010; year < 2021; year++)
            {
                Console.WriteLine(year);

                foreach (int station in _stations)
                {
                    string apiPath = "https://tidesandcurrents.noaa.gov/api/datagetter?product=water_temperature&begin_date=" + year + "0101&end_date="+year+"1231&datum=MLLW&interval=h&station=" + station + "&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL";

                    var waterTempPOCO = ReadAPI(apiPath);

                    DataRow workRow;

                    if (object.ReferenceEquals(waterTempPOCO.Data, null))
                    {
                        continue;
                    }


                    foreach (WaterData waterData in waterTempPOCO.Data)
                    {
                        try
                        {
                            workRow = dt.NewRow();
                            workRow[0] = waterData.t;
                            workRow[1] = station;
                            workRow[2] = waterData.v;
                            workRow[3] = waterData.f;
                            dt.Rows.Add(workRow);
                            //Console.WriteLine(waterData.v);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{waterData.t} -- {station} -- {waterData.v} -- {waterData.f}");
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(apiPath);
                            Console.ReadLine();
                            continue;
                        }
                    }
                }
            }

            using (var connection = new MySqlConnection(_connStr + ";AllowLoadLocalInfile=True"))
            {
                connection.Open();
                var bulkCopy = new MySqlBulkCopy(connection);
                bulkCopy.DestinationTableName = "StationWaterTemperature";
                bulkCopy.WriteToServer(dt);

            }

            Console.WriteLine(DateTime.Now);
        }


        public WaterTempPOCO ReadAPI(string apiPath)
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

            WaterTempPOCO waterTempPOCO = JsonConvert.DeserializeObject<WaterTempPOCO>(strObj);

            return waterTempPOCO;
        }


        //public List<PK> GenerateAllDateTimes()
        //{

        //    List<PK> primaryKeys = new List<PK>();

        //    foreach (int station in _stations)
        //    {
        //        DateTime begin = new DateTime(2010, 1, 1);
        //        DateTime end = new DateTime(2020, 1, 1);
        //        for (DateTime date = begin; date <= end; date = date.AddHours(1))
        //        {
        //            PK pK = new PK();

        //            pK.ReadingDatetime = date;
        //            pK.Station = station;

        //            primaryKeys.Add(pK);

        //        }

        //    }

        //    return primaryKeys;
        //}

            /*
        public void JoinLinqForFun()
        {
            var waterTempData = ReadAPI("https://tidesandcurrents.noaa.gov/api/datagetter?product=water_temperature&begin_date=20170428&end_date=20180428&datum=MLLW&interval=h&station=8774230&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL");
           // var times = GenerateAllDateTimes();

            var justWaterData = waterTempData.Data;

            foreach (WaterData wd in waterTempData.Data)
            {
                Console.WriteLine(wd.v);
            }



            //var result = times.Join(justWaterData, arg => arg.ReadingDatetime, arg => arg.t,
            //    (first, second) => new { ReadTime = first.ReadingDatetime, Stat = first.Station, second.v, second.f });

            //foreach (var r in result)
            //{
            //    Console.WriteLine($"{r.ReadTime} -- {r.Stat} -- {r.v} -- {r.f}");
            //}
        } */


        //public void test()
        //{
        //    IList<string> strList1 = new List<string>() {
        //    "One",
        //    "Two",
        //    "Three",
        //    "Four"
        //    };

        //    IList<string> strList2 = new List<string>() {
        //    "One",
        //    "Two",
        //    "Five",
        //    "Six"
        //    };

        //    var innerJoinResult = strList1.Join(// outer sequence 
        //                  strList2,  // inner sequence 
        //                  str1 => str1,    // outerKeySelector
        //                  str2 => str2,  // innerKeySelector
        //                  (str1, str2) => str1);

        //    foreach (var str in innerJoinResult)
        //    {
        //        Console.WriteLine("{0} ", str);
        //    }
        //}

    }

    public class StationWaterTemperature
    {
        public DateTime ReadingDatetime { get; set; }
        public int StationId { get; set; }
        public decimal WaterTemperature { get; set; }
        public string WaterTemperatureF { get; set; }
    }

    public class PK
    {
        public DateTime ReadingDatetime { get; set; }
        public int Station { get; set; }
    }
}
