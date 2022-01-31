using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPIDataCollection
{
    public class Rootobject
    {
        public Doc[] docs { get; set; }
    }

    public class Doc
    {
        public string key { get; set; }
        public string title { get; set; }
        public int first_publish_year { get; set; }
    }
}
