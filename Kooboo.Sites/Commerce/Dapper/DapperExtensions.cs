using Dapper;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce
{
    public static class DapperExtensions
    {
        static readonly string _fields = "fields";
        static readonly string _sets = "sets";
        static readonly string _values = "values";

        public static ConcurrentDictionary<Type, Dictionary<string, string>> _typeMemberStrings = new ConcurrentDictionary<Type, Dictionary<string, string>>();

        #region Get
        public static T Get<T>(this IDbConnection connection, Guid id) where T : EntityBase
        {
            var type = typeof(T);
            var dic = GetTypeSqls(type);
            return connection.QueryFirstOrDefault<T>($"SELECT {dic[_fields]} FROM '{type.Name}' WHERE Id=@Id LIMIT 1", new { Id = id });
        }

        public static IEnumerable<T> GetList<T>(this IDbConnection connection) where T : class
        {
            var type = typeof(T);
            var dic = GetTypeSqls(type);
            return connection.Query<T>($"SELECT {dic[_fields]} FROM '{type.Name}'");
        }

        #endregion

        #region Insert
        public static void Insert<T>(this IDbConnection connection, T entity) where T : class
        {
            var type = typeof(T);
            var dic = GetTypeSqls(type);
            connection.Execute($"INSERT INTO '{type.Name}' ({dic[_fields]}) VALUES ({dic[_values]})", entity);
        }

        public static void InsertList<T>(this IDbConnection connection, IEnumerable<T> entities) where T : class
        {
            if (!entities.Any()) return;
            var type = typeof(T);
            var dic = GetTypeSqls(type);
            connection.Execute($"INSERT INTO '{type.Name}' ({dic[_fields]}) VALUES ({dic[_values]})", entities);
        }
        #endregion

        #region Update
        public static void Update<T>(this IDbConnection connection, T entity) where T : EntityBase
        {
            var type = typeof(T);
            var dic = GetTypeSqls(type);
            connection.Execute($"UPDATE '{type.Name}' SET {dic[_sets]} WHERE Id =@Id", entity);
        }

        public static void UpdateList<T>(this IDbConnection connection, IEnumerable<T> entities) where T : EntityBase
        {
            if (!entities.Any()) return;
            var type = typeof(T);
            var dic = GetTypeSqls(type);
            connection.Execute($"UPDATE '{type.Name}' SET {dic[_sets]} WHERE Id =@Id", entities);
        }
        #endregion

        #region Delete
        public static void Delete<T>(this IDbConnection connection, Guid id) where T : EntityBase
        {
            connection.Execute($"DELETE FROM '{typeof(T).Name}' WHERE Id =@Id", new { Id = id });
        }

        public static void Delete<T>(this IDbConnection connection, T entity) where T : EntityBase
        {
            connection.Execute($"DELETE FROM '{typeof(T).Name}' WHERE Id =@Id", entity);
        }

        public static void DeleteList<T>(this IDbConnection connection, IEnumerable<Guid> ids) where T : EntityBase
        {
            if (!ids.Any()) return;
            connection.Execute($"DELETE FROM '{typeof(T).Name}' WHERE Id IN (@Id)", ids.Select(s => new { Id = s }));
        }

        public static void DeleteList<T>(this IDbConnection connection, IEnumerable<T> entities) where T : EntityBase
        {
            if (!entities.Any()) return;
            connection.Execute($"DELETE FROM '{typeof(T).Name}' WHERE Id IN (@Id)", entities);
        }
        #endregion

        #region Exist
        public static bool Exist<T>(this IDbConnection connection, Guid id) where T : EntityBase
        {
            return connection.QuerySingleOrDefault<bool?>($"SELECT 1 FROM '{typeof(T).Name}' WHERE Id =@Id LIMIT 1", new { Id = id }) ?? false;
        }
        #endregion

        #region Count
        public static int Count<T>(this IDbConnection connection) where T : EntityBase
        {
            return connection.QuerySingleOrDefault<int>($"select count(1) from '{typeof(T).Name}'");
        }
        #endregion


        #region Transaction
        public static T ExecuteTask<T>(this IDbConnection connection, Func<IDbConnection, T> func, bool enableTransation = true, bool closeAfterExecuted = true)
        {
            try
            {
                IDbTransaction tran = null;

                if (enableTransation)
                {
                    connection.Open();
                    tran = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                }

                var result = func(connection);

                if (enableTransation)
                {
                    tran.Commit();
                    connection.Close();
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (closeAfterExecuted) connection.Dispose();
            }
        }

        public static void ExecuteTask(this IDbConnection connection, Action<IDbConnection> action, bool enableTransation = false, bool closeAfterExecuted = true)
        {
            try
            {
                IDbTransaction tran = null;

                if (enableTransation)
                {
                    connection.Open();
                    tran = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                }

                action(connection);

                if (enableTransation)
                {
                    tran.Commit();
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (closeAfterExecuted) connection.Dispose();
            }
        }

        #endregion

        private static Dictionary<string, string> GetTypeSqls(Type type)
        {
            return _typeMemberStrings.GetOrAdd(type, _ =>
            {
                var properties = type.GetProperties()
                    .Where(w => !w.IsDefined(typeof(IgnoreAttribute), true))
                    .Select(s => new
                    {
                        s.Name,
                        NotUpdate = s.IsDefined(typeof(NotUpdateAttribute), true),
                    });

                return new Dictionary<string, string> {
                    {_fields,string.Join(",",properties.Select(s=>$@"""{s.Name}"""))},
                    {_sets,string.Join(",",properties.Where(w=>!w.NotUpdate).Select(s=>$@"""{s.Name}""=@{s.Name}"))},
                    {_values,string.Join(",",properties.Select(s=>$@"@{s.Name}"))},
                };
            });
        }
    }
}
