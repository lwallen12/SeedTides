using System;
using System.Collections.Generic;
using System.Text;

namespace PastTideData.POCOs
{
    public class WaterMetadata
    {
        public string id { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }

    }
    public class WaterData
    {
        public DateTime t { get; set; }
        public string v { get; set; }
        public string f { get; set; }

    }
    public class WaterTempPOCO
    {
        public WaterMetadata metadata { get; set; }
        public IList<WaterData> Data { get; set; }

    }


}
