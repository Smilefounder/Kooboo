using Kooboo.IndexedDB.Btree;
using Kooboo.IndexedDB.Indexs;
using System;
using System.Collections.Generic;

namespace Kooboo.IndexedDB.Query
{
    internal class QueryOrder : IQuery
    {
        private IIndex index;
        private bool ascending;
        public QueryOrder(string field, bool ascending, ITableVisitor store)
        {
            this.index = store.GetIndex(field);
            if (index == null)
                throw new Exception("Only Index can use OrderBy");
            this.ascending = ascending;
        }

        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            var indexOrder = index.AllItems(ascending);
            if (collection == null)
                return indexOrder;
            return OrderEach(indexOrder, collection);
        }

        private static IEnumerable<long> OrderEach(IEnumerable<long> indexOrder, IEnumerable<long> collection)
        {
            HashSet<long> hash = new HashSet<long>(collection);
            foreach (var item in indexOrder)
            {
                if (hash.Contains(item))
                    yield return item;
            }
        }
    }

    internal class QueryOrderPrimaryKey<TKey, TValue> : IQuery
    {
        private ITableVisitor _store;
        private bool ascending;
        public QueryOrderPrimaryKey(bool ascending, ITableVisitor store)
        {
            this._store = store;
            this.ascending = ascending;
        }

        public IEnumerable<long> Execute(IEnumerable<long> collection)
        {
            var indexOrder = _store.GetCollection(ascending);
            if (collection == null)
                return indexOrder;
            return OrderEach(indexOrder, collection);
        }

        private static IEnumerable<long> OrderEach(IEnumerable<long> indexOrder, IEnumerable<long> collection)
        {
            HashSet<long> hash = new HashSet<long>(collection);
            foreach (var item in indexOrder)
            {
                if (hash.Contains(item))
                    yield return item;
            }
        }
    }
}
