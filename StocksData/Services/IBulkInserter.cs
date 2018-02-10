using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace StocksData.Services
{
    public abstract class IBulkInserter<T> where T : class, new()
    {
        public string ConnectionString { get; protected set; }

        public abstract void BulkInsert(IEnumerable<T> payload);

        public void BulkInsert(params T[] payload)
        {
            BulkInsert(payload.AsEnumerable());
        }
    }
}
