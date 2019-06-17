using System.Collections;
using System.Collections.Generic;
using System.Windows.Documents;

namespace WpfApp1
{
    public class RootObject
    {
        public string Kind { set; get; }
        public string etag { set; get; }
        public PageInfo pageInfo { get; set; }
        public List<Items> items { get; set; }
        //public List<Dictionary<string, object>> Data { get; set; }

    }
}