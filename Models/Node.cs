using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class Node
    {
        public int Id { get; set; }

        public string Hostname { get; set; }
        public string IpAddress { get; set; }
        public string Vendor { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
