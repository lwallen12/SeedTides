using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using PastTideData.POCOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace PastTideData.Jobs
{
    public class Wind : BaseSeedJob
    {
        

        public void SeedWind()
        {
            Console.WriteLine(DateTime.Now);

            DataTable dt = new DataTable("StationWind");

            dt.Columns.Add("ReadingDatetime", typeof(DateTime));
            dt.Columns.Add("StationId", typeof(int));
            dt.Columns.Add("WindSpeed", typeof(string));
            dt.Columns.Add("WindDegrees", typeof(string));
            dt.Columns.Add("WindDirectionDescription", typeof(string));
            dt.Columns.Add("WindGust", typeof(string));
            dt.Columns.Add("WindF", typeof(string));

            for (int year = 2010; year < 2021; year++)
            {
                Console.WriteLine(year);

                foreach (int station in _stations)
                {
                    string apiPath = "https://tidesandcurrents.noaa.gov/api/datagetter?product=wind&begin_date=" + year + "0101&end_date=" + year + "1231&datum=MLLW&interval=h&station=" + station + "&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL";

                    var windStringObj = ReadAPI(apiPath);
                    WindPOCO windPOCO = JsonConvert.DeserializeObject<WindPOCO>(windStringObj);

                    DataRow workRow;

                    if (object.ReferenceEquals(windPOCO.data, null))
                    {
                        continue;
                    }


                    foreach (Data d in windPOCO.data)
                    {
                        try
                        {
                            workRow = dt.NewRow();
                            workRow[0] = d.t;
                            workRow[1] = station;
                            workRow[2] = d.s;
                            workRow[3] = d.d;
                            workRow[4] = d.dr;
                            workRow[5] = d.g;
                            workRow[6] = d.f;
                            dt.Rows.Add(workRow);
                            //Console.WriteLine(waterData.v);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{d.t} -- {station} -- {d.s} -- {d.d} -- {d.dr} -- {d.g} -- {d.f}");
                            Console.WriteLine(ex.Message);
                            Console.WriteLine(apiPath);
                            Console.ReadLine();
                            continue;
                        }
                    }
                }
            }

            Console.WriteLine(DateTime.Now);

            using (var connection = new MySqlConnection(_connStr + ";AllowLoadLocalInfile=True"))
            {
                connection.Open();
                var bulkCopy = new MySqlBulkCopy(connection);
                bulkCopy.DestinationTableName = "StationWind";
                bulkCopy.WriteToServer(dt);

            }

            Console.WriteLine(DateTime.Now);
        }

    }
}
