using Dapper;
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
        public static ConcurrentDictionary<Type, string[]> _typeMemberStrings = new ConcurrentDictionary<Type, string[]>();

        #region Get
        public static T Get<T>(this IDbConnection connection, Guid id) where T : EntityBase
        {
            string[] strs = GetTypeMemberString<T>();
            return connection.QueryFirstOrDefault<T>($"SELECT {strs[0]} FROM {typeof(T).Name} WHERE Id=@Id LIMIT 1", new { Id = id });
        }

        public static IEnumerable<T> Get<T>(this IDbConnection connection) where T : EntityBase
        {
            string[] strs = GetTypeMemberString<T>();
            return connection.Query<T>($"SELECT {strs[0]} FROM {typeof(T).Name}");
        }
        #endregion

        #region Insert
        public static void Insert<T>(this IDbConnection connection, T entity) where T : EntityBase
        {
            string[] strs = GetTypeMemberString<T>();
            connection.Execute($"INSERT INTO {typeof(T).Name} ({strs[0]}) VALUES ({strs[2]})", entity);
        }

        public static void Insert<T>(this IDbConnection connection, IEnumerable<T> entities) where T : EntityBase
        {
            string[] strs = GetTypeMemberString<T>();
            connection.Execute($"INSERT INTO {typeof(T).Name} ({strs[0]}) VALUES ({strs[2]})", entities);
        }
        #endregion

        #region Update
        public static void Update<T>(this IDbConnection connection, T entity) where T : EntityBase
        {
            string[] strs = GetTypeMemberString<T>();
            connection.Execute($"UPDATE {typeof(T).Name} SET {strs[1]} WHERE Id =@Id", entity);
        }

        public static void Update<T>(this IDbConnection connection, IEnumerable<T> entities) where T : EntityBase
        {
            string[] strs = GetTypeMemberString<T>();
            connection.Execute($"UPDATE {typeof(T).Name} SET {strs[1]} WHERE Id =@Id", entities);
        }
        #endregion

        #region Delete
        public static void Delete<T>(this IDbConnection connection, Guid id) where T : EntityBase
        {
            connection.Execute($"DELETE FROM {typeof(T).Name} WHERE Id =@Id", new { Id = id });
        }

        public static void Delete<T>(this IDbConnection connection, T entity) where T : EntityBase
        {
            connection.Execute($"DELETE FROM {typeof(T).Name} WHERE Id =@Id", entity);
        }

        public static void Delete<T>(this IDbConnection connection, IEnumerable<Guid> entity) where T : EntityBase
        {
            connection.Execute($"DELETE FROM {typeof(T).Name} WHERE Id IN (@Id)", entity.Select(s => new { Id = s }));
        }

        public static void Delete<T>(this IDbConnection connection, IEnumerable<T> entity) where T : EntityBase
        {
            connection.Execute($"DELETE FROM {typeof(T).Name} WHERE Id IN (@Id)", entity);
        }
        #endregion

        private static string[] GetTypeMemberString<T>() where T : EntityBase
        {
            return _typeMemberStrings.GetOrAdd(typeof(T), type =>
            {
                var properties = type.GetProperties()
                                     .Where(w => w.CustomAttributes.All(a => a.AttributeType != typeof(IgnoreColumnAttribute)))
                                     .Select(s => s.Name)
                                     .ToArray();

                return new string[] {
                    string.Join(",",properties.Select(s=>$@"""{s}""")),
                    string.Join(",",properties.Select(s=>$@"""{s}""=@{s}")),
                    string.Join(",",properties.Select(s=>$@"@{s}"))
                };
            });
        }
    }
}
