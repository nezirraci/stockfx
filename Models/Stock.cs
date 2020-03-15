using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElcomManage.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int StockLocationId { get; set; }
        public StockLocation StockLocation { get; set; }
        public int Quantity { get; set; }
    }
}
