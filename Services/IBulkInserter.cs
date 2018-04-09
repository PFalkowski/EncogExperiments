using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public abstract class BulkInserter<T> where T : class, new()
    {
        public string ConnectionString { get; protected set; }

        public abstract void BulkInsert(IEnumerable<T> payload);

        public void BulkInsert(params T[] payload)
        {
            BulkInsert(payload.AsEnumerable());
        }
    }
}
