using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PastTideData
{
    public class DataSeederTides
    {

        string _connStr = "server=test1.ce8cn9mhhgds.us-east-1.rds.amazonaws.com;user=Wallen;database=whattodo;port=3306;password=MyRDSdb1";

        //For each station in this list
        //For each day in this loop... insert this day

        //The other method should be a bunch of updates

        int[] stations = {8775296,8774230,8775241,8776604,8775870,8771013,8772471,8772447,8771341,8771450,8771486,8770808,8770777,8773767,8773146,8770613,8775244,8775792,8775237,
                            8770475,8779770,8773259,8778490,8773701,8770520,8779280,8777812,8774770,8770971,8776139,8770570,8771972,8772985,8773037,8779748,8779749,8770822};


        public void SeedTable()
        {
            using (var connection = new MySqlConnection(_connStr))
            {
                connection.Open();
                foreach (int station in stations)
                {
                    DateTime begin = new DateTime(2010, 1, 1);
                    DateTime end = new DateTime(2020, 1, 1);

                    //I realize no input so no way to SQL Inject?
                    for (DateTime date = begin; date <= end; date = date.AddHours(1))
                    {
                        //Console.WriteLine($"Station: {station}, -- Datetime: {date}");
                        string stmt = "INSERT INTO PastTidalStation (ReadingDatetime, StationId) VALUES (@ReadingDatetime, @StationId)";
                        MySqlCommand cmd = new MySqlCommand(stmt, connection);

                        //cmd.Parameters.Add("@ReadingDatetime", MySqlDbType.DateTime);
                        //cmd.Parameters.Add("@StationId", MySqlDbType.Int32);

                        //cmd.Parameters["@ReadingDateTime"].Value = date;
                        //cmd.Parameters["@StationId"].Value = station;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void SeedTableVersionTwo()
        {
            DataTable dt = new DataTable("PastTidalStation");

            dt.Columns.Add("ReadingDatetime", typeof(DateTime));
            dt.Columns.Add("StationId", typeof(int));

            
            foreach (int station in stations)
            {
                DataRow workRow;
                DateTime begin = new DateTime(2010, 1, 1);
                DateTime end = new DateTime(2020, 1, 1);
                for (DateTime date = begin; date <= end; date = date.AddHours(1))
                {
                    workRow = dt.NewRow();
                    workRow[0] = date;
                    workRow[1] = station;
                    dt.Rows.Add(workRow);

                }

                Console.WriteLine(station);

            }

            //workRow = dt.NewRow();
            //workRow[0] = DateTime.Now;
            //workRow[1] = 8043538;
            //dt.Rows.Add(workRow);


            using (var connection = new MySqlConnection(_connStr + ";AllowLoadLocalInfile=True"))
            {
                //using (MySqlBulkLoader mySqlBulkLoader = new MySqlBulkLoader(connection))
                //{
                //    mySqlBulkLoader.TableName = dt.TableName;
                //    mySqlBulkLoader.Load(dt);
                //}

                connection.Open();
                var bulkCopy = new MySqlBulkCopy(connection);
                bulkCopy.DestinationTableName = "PastTidalStation";
                bulkCopy.WriteToServer(dt);

            }


        }
    }

}
