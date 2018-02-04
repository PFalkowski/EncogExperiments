using NHibernate;

namespace StocksData.Contexts
{
    public interface IStockNHContext
    {
        ISessionFactory SessionFactory { get; }
    }
}