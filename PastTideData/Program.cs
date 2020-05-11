using PastTideData.POCOs;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PastTideData.Jobs;

namespace PastTideData
{

    //I want to take a list of all the stations we are going to get tidal data for,
    //Loop through each, and get all hourly tidal readings for the past 10 years basically... back to January 2010

    //so... more specifically... hourly height, water levels, current movement?
    


    class Program
    {
        static void Main(string[] args)
        {

            //https://tidesandcurrents.noaa.gov/api/datagetter?product=water_level
            //&begin_date=20200428&end_date=20200429
            //&datum=MLLW&interval=h
            //&station=8770520&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL


            //https://tidesandcurrents.noaa.gov/api/datagetter?product=hourly_height
            //&begin_date=20170428&end_date=20170430
            //&datum=MLLW
            //&station=8770520&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL

            /*Can add interval=60*/
            //https://tidesandcurrents.noaa.gov/api/datagetter?product=predictions
            //&begin_date=20170428&end_date=20170430
            //&datum=MLLW&interval=h
            //&station=8770520&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL

            //https://tidesandcurrents.noaa.gov/api/datagetter?product=water_temperature
            //&begin_date=20170428&end_date=20170430
            //&datum=MLLW&interval=h
            //&station=8774230&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL

            /*Nothing found thus far*/
            //https://tidesandcurrents.noaa.gov/api/datagetter?product=salinity
            //&begin_date=20170428&end_date=20170430
            //&datum=MLLW&interval=h
            //&station=8774230&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL

            //https://tidesandcurrents.noaa.gov/api/datagetter?product=air_pressure
            //&begin_date=20170428&end_date=20170430
            //&datum=MLLW&interval=h
            //&station=8774230&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL

            //https://tidesandcurrents.noaa.gov/api/datagetter?product=high_low
            //&begin_date=20170428&end_date=20170430
            //&datum=MLLW&interval=h
            //&station=8774230&time_zone=GMT&units=english&format=json&application=NOS.COOPS.TAC.WL

            WaterTemperature waterTemperature = new WaterTemperature();
            waterTemperature.SeedWaterTemp();
            //waterTemperature.test();

            //waterTemperature.JoinLinqForFun();


            //Wind wind = new Wind();
            //wind.UpdateWindRecords();


            //DataSeederTides dataSeeder = new DataSeederTides();
            //dataSeeder.SeedTableVersionTwo();

        }
    }
}
