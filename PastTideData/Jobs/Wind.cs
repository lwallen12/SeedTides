using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PastTideData.POCOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PastTideData.Jobs
{
    public class Wind
    {
        int[] stations = {8775296,8774230,8775241,8776604,8775870,8771013,8772471,8772447,8771341,8771450,8771486,8770808,8770777,8773767,8773146,8770613,8775244,8775792,8775237,
                            8770475,8779770,8773259,8778490,8773701,8770520,8779280,8777812,8774770,8770971,8776139,8770570,8771972,8772985,8773037,8779748,8779749,8770822};


        public void UpdateWindRecords()
        {
            Console.WriteLine(DateTime.Now);

            string apiPath = "https://tidesandcurrents.noaa.gov/api/datagetter?product=wind&begin_date=20170428&end_date=20180428&datum=MLLW&interval=h&station=8774230&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL";
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
            Console.WriteLine(strObj); //Definitely gives us our data

            WindPOCO windPOCO = JsonConvert.DeserializeObject<WindPOCO>(strObj);



            string _connStr = "server=test1.ce8cn9mhhgds.us-east-1.rds.amazonaws.com;user=Wallen;database=whattodo;port=3306;password=MyRDSdb1";
            using (var connection = new MySqlConnection(_connStr))
            {

                connection.Open();
                foreach (int station in stations)
                {
                    foreach (Data w in windPOCO.data)
                    {


                        //Console.WriteLine("WindSpeed: " + w.s);
                        //Console.WriteLine("Wind Direction: " + w.d);
                        //Console.WriteLine("Wind Direction Desc: " + w.dr);

                        //w.s = w.s == "" ? (Object)Convert.DBNull : w.s;



                        //Console.WriteLine(w.t.ToString("yyyy-MM-dd HH:mm:ss"));

                        //DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")

                        //string query = "SELECT * FROM PastTidalStation WHERE ReadingDatetime = '" + w.t.ToString("yyyy-MM-dd HH:mm:ss") + "' AND StationId = " + 8774230;
                        //string query = "SELECT * FROM PastTidalStation WHERE StationId = " + 8774230;

                        //Console.WriteLine(query);
                        //Console.WriteLine(stmt);

                        //MySqlCommand cmd = new MySqlCommand(query, connection);
                        //MySqlDataReader reader = cmd.ExecuteReader();



                        //while (reader.Read())
                        //{
                        //    Console.WriteLine(reader.GetDateTime(0) + ": "
                        //        + reader.GetInt32(1));
                        //}

                        //Console.ReadLine();

                        //reader.Close();




                        //cmd.Parameters.Add("@WindSpeed", MySqlDbType.Decimal);
                        //cmd.Parameters.Add("@WindDegrees", MySqlDbType.Decimal);
                        //cmd.Parameters.Add("@WindDirectionDescription", MySqlDbType.VarChar);
                        //cmd.Parameters.Add("@ReadingDatetime", MySqlDbType.DateTime);
                        //cmd.Parameters.Add("@StationId", MySqlDbType.Int32);

                        //cmd.Parameters["@WindSpeed"].Value = w.s;
                        //cmd.Parameters["@WindDegrees"].Value = w.d;
                        //cmd.Parameters["@WindDirectionDescription"].Value = w.dr;
                        //cmd.Parameters["@ReadingDatetime"].Value = w.t;
                        //cmd.Parameters["@StationId"].Value = 8774230;

                        string stmt = "UPDATE PastTidalStation SET WindSpeed = " + w.s + ", WindDegrees = " + w.d + ", WindDirectionDescription = '" + w.dr + "' WHERE ReadingDatetime = '" + w.t.ToString("yyyy-MM-dd HH:mm:ss") + "' AND StationId = " + station;

                        MySqlCommand cmd = new MySqlCommand(stmt, connection);

                        try
                        {
                            if (w.s == "")
                            {
                                continue;
                            }
                            cmd.ExecuteNonQuery();
                        }
                        catch (MySqlException ex)
                        {
                            Console.WriteLine(stmt);
                            Console.WriteLine(ex.Message);
                            continue;
                        }

                    }
                }
            }

            Console.WriteLine(DateTime.Now);
        }
    }
}
