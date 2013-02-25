using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System.Linq.Expressions;
using Skewrl.Core.Config;

namespace Skewrl.Core.Data
{
    /// <summary>
    /// Implementation of the Repository pattern for table service entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : TableServiceEntity
    {
        void Add(T item);
        void Delete(T item);
        void Update(T item);
        T FindSingle(Func<T, bool> exp);
        T FindFirst(Func<T, bool> exp);
        List<T> FindAll(Func<T, bool> exp);
        List<T> FindN(Func<T, bool> exp, int count);
    }

    public class AzureTableRepository<T> : IRepository<T> where T : TableServiceEntity
    {
        private readonly string _tablename;
        private readonly TableServiceContext _datactx;
        private readonly CloudTable _table;

        public AzureTableRepository()
        {
            _tablename = typeof(T).Name.ToLowerInvariant();
            _datactx = SkewrlConfig.Instance.StorageAccount.CreateCloudTableClient().GetTableServiceContext();
            _table = SkewrlConfig.Instance.StorageAccount.CreateCloudTableClient().GetTableReference(_tablename);
        }

        public void Add(T item)
        {
            _datactx.AddObject(_tablename, item);
            _datactx.SaveChanges();
        }

        public void Delete(T item)
        {
            _datactx.DeleteObject(item);
            _datactx.SaveChanges();
        }

        public void Update(T item)
        {
            _datactx.UpdateObject(item);
            _datactx.SaveChanges();
        }

        public T FindSingle(Func<T, bool> exp)
        {
            IQueryable<T> query = _datactx.CreateQuery<T>(_tablename);

            return query.Where(exp).SingleOrDefault();
        }

        public T FindFirst(Func<T, bool> exp)
        {
            IQueryable<T> query = _datactx.CreateQuery<T>(_tablename);

            return query.Where(exp).FirstOrDefault();
        }

        public List<T> FindAll(Func<T, bool> exp)
        {
            IQueryable<T> query = _datactx.CreateQuery<T>(_tablename);
            return query.Where(exp).OrderBy(u=>u.RowKey).ToList();
        }

        public List<T> FindN(Func<T, bool> exp, int count)
        {
            IQueryable<T> query = _datactx.CreateQuery<T>(_tablename);

            if (exp != null)
                return query.Where(exp).Take(count).ToList();
            else
                return query.Take(count).ToList();
        }

    }
}
