using Kooboo.IndexedDB.Btree;
using Kooboo.IndexedDB.Columns;
using Kooboo.IndexedDB.Dynamic;
using Kooboo.IndexedDB.Indexs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kooboo.IndexedDB.Query
{
    internal interface ITableVisitor
    {
        bool TryGetColumn(string columnName, out Type dataType, out int columnLen, out Func<object, byte[]> toBytes, out int relativePosition);
        IIndex GetIndex(string IndexName);
        byte[] GetColumnsBytes(Int64 blockposition, int relativePosition, int length);
        ItemCollection GetCollection(bool ascending);
    }

    internal struct ObjectStoreVisitor : ITableVisitor
    {
        private ObjectStore _objectStore;

        public ObjectStoreVisitor(ObjectStore objectStore)
        {
            _objectStore = objectStore;
        }

        public ItemCollection GetCollection(bool ascending)
        {
            return _objectStore.GetColloction(ascending);
        }

        public bool TryGetColumn(string columnName, out Type dataType, out int columnLen, out Func<object, byte[]> toBytes, out int relativePosition)
        {
            var co = _objectStore.GetIColumn(columnName);
            if (co == null)
            {
                dataType = null;
                columnLen = 0;
                toBytes = null;
                relativePosition = 0;
                return false;
            }

            dataType = co.DataType;
            columnLen = co.Length;
            toBytes = (t) => co.GetBytes(t);
            relativePosition = co.relativePosition;
            return true;
        }

        public byte[] GetColumnsBytes(long blockposition, int relativePosition, int length)
        {
            return _objectStore.GetColumnsBytes(blockposition, relativePosition, length);
        }

        public IIndex GetIndex(string IndexName)
        {
            return _objectStore.GetIIndex(IndexName);
        }

        public static implicit operator ObjectStoreVisitor(ObjectStore objectStore)
        {
            return new ObjectStoreVisitor(objectStore);
        }
    }



    internal class TableVisitor : ITableVisitor
    {
        private Table _table;

        public TableVisitor(Table table)
        {
            _table = table;
        }

        public ItemCollection GetCollection(bool ascending)
        {
            return _table.GetCollection(ascending);
        }

        public bool TryGetColumn(string columnName, out Type dataType, out int columnLen, out Func<object, byte[]> toBytes, out int relativePosition)
        {
            FieldConverter field = this._table.ObjectConverter.Fields.Find(o => o.FieldName == columnName);

            if (field == null)
            {
                dataType = null;
                columnLen = 0;
                toBytes = null;
                relativePosition = 0;
                return false;
            }

            dataType = field.ClrType;
            columnLen = field.Length;
            toBytes = field.ToBytes;
            relativePosition = field.RelativePosition;
            return true;
        }


        public byte[] GetColumnsBytes(long blockposition, int relativePosition, int length)
        {
            return _table.BlockFile.GetCol(blockposition, relativePosition, length);
        }

        public IIndex GetIndex(string IndexName)
        {
            return _table.GetIndex(IndexName);
        }

        public static implicit operator TableVisitor(Table table)
        {
            return new TableVisitor(table);
        }
    }


    internal abstract class FieldRelationalQuery : IQuery
    {
        internal ITableVisitor store;

        //IColumn
        internal Type columnType;
        internal int columnLength;
        internal Func<object, byte[]> columnToBytes;
        internal int columnRelativePosition;
        //IIndex
        internal IIndex index;

        internal bool isColumn => columnType != null;
        internal Type fieldType => isColumn ? columnType : index.keyType;

        internal FieldRelationalQuery(string memName, ITableVisitor objectStore)
        {
            if (!objectStore.TryGetColumn(memName, out columnType, out columnLength, out columnToBytes, out columnRelativePosition))
            {
                index = objectStore.GetIndex(memName);
                if (index == null)
                    throw new Exception("only index or column are allowed in the where condition, consider adding the field into columns in order to search");
            }
            store = objectStore;
        }

        public abstract IEnumerable<long> Execute(IEnumerable<long> collection);

        internal ItemCollection DefaultColloction => store.GetCollection(true);

        internal object RealValue(object val)
        {
            if (val is decimal)//dmlValue: string, decimal, datetime
            {
                return DecimalConvertHelper.Get(fieldType)(val);
            }
            return val;
        }
    }

    internal static class DecimalConvertHelper
    {
        private static ConcurrentDictionary<Type, Func<object, object>> containers = new ConcurrentDictionary<Type, Func<object, object>>();

        public static Func<object, object> Get(Type convertToType)
        {
            return containers.GetOrAdd(convertToType, e => Build(e));
        }

        private static Func<object, object> Build(Type convertToType)
        {
            //object val = (decimal)num;
            //return (object)((convertToType)((decimal)val));

            ParameterExpression par = Expression.Parameter(typeof(object));
            var body = Expression.Convert(Expression.Convert(par, typeof(decimal)), convertToType);
            return Expression.Lambda<Func<object, object>>(Expression.Convert(body, typeof(object)), par).Compile();
        }
    }
}
