using System;
using System.Collections.Generic;
using System.Text;

namespace PastTideData.POCOs
{
    public class Metadata
    {
        public string id { get; set; }
        public string name { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }

    }
    public class Data
    {
        public DateTime t { get; set; }
        public string s { get; set; }
        public string d { get; set; }
        public string dr { get; set; }
        public string g { get; set; }
        public string f { get; set; }

    }
    public class WindPOCO
    {
        public Metadata metadata { get; set; }
        public IList<Data> data { get; set; }

    }
}
