using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess {
    public interface IQueryExcutable : IDisposable {
        /// <summary>
        /// クエリパラメータを追加する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        void AddParameter(string name, object value, DbType dbType);

        /// <summary>
        /// クエリを発行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string queryString);

        /// <summary>
        /// (非同期)クエリを発行します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryString"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string queryString);

        /// <summary>
        /// クエリを発行します。
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string queryString);

        /// <summary>
        /// (非同期)クエリを発行します。
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryAsync(string queryString);
    }
}
