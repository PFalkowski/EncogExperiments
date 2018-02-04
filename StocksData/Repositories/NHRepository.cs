using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace StocksData.Repositories
{
    public class NhRepository<T> : IRepository<T> where T : class
    {
        public IEnumerable<T> Entities => Session.Query<T>();

        public ISession Session { get; }

        public NhRepository(ISession session)
        {
            Session = session;
        }
        public void Add(T entity)
        {
            Session.Save(entity);
        }

        public void AddOrUpdate(T entity)
        {
            Session.SaveOrUpdate(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Session.Save(entity);
            }
        }

        public T Get(object id)
        {
            return Session.Get<T>(id);
        }

        public T Get(Expression<Func<T>> predicate)
        {
            return Session.QueryOver<T>(predicate).SingleOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T>> predicate)
        {
            return Session.QueryOver<T>(predicate).Future();
        }

        public IList<T> GetAll()
        {
            return Entities.ToList();
        }

        public void Remove(T entity)
        {
            Session.Delete(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }
    }
}
