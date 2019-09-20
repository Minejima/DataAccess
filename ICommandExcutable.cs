using System;
using System.Data;
using System.Threading.Tasks;

namespace DataAccess {
    public interface ICommandExcutable : IDisposable {
        /// <summary>
        /// コマンドパラメータを追加する
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        void AddParameter(string name, object value, DbType dbType);

        /// <summary>
        /// Insert, Update, Delete等を実行する
        /// </summary>
        /// <returns>影響した行数</returns>
        int ExecuteCommand(string command);

        /// <summary>
        /// Insert, Update, Delete等を実行する
        /// </summary>
        /// <returns>影響した行数</returns>
        Task<int> ExecuteCommandAsync(string command);

        /// <summary>
        /// コミット
        /// </summary>
        void Commit();
    }
}
