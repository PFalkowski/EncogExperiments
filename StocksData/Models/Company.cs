using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Models
{
    public class Company
    {
        [Key]
        [ForeignKey(nameof(Quotes))]
        public string Ticker { get; set; }
        public virtual ICollection<StockQuote> Quotes { get; set; }

        public override string ToString()
        {
            return Ticker;
        }
    }
}
