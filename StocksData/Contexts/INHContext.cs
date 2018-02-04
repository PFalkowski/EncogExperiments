using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace StocksData.Contexts
{
    public interface INhContext
    {
        ISessionFactory SessionFactory { get; }
    }
}
