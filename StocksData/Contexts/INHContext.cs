using NHibernate;

namespace StocksData.Contexts
{
    public interface INhContext
    {
        ISessionFactory SessionFactory { get; }
    }
}
