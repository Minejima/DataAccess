using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess {
    /// <summary>
    /// Dapperで実装
    /// </summary>
    public class DapperDao : ICommandExcutable, IQueryExcutable {
        protected DynamicParameters _parameters;

        protected DbConnection _conn;

        protected DbTransaction _trans;

        /// <summary>
        /// コマンド影響行数
        /// </summary>
        protected int _count;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbConnection"></param>
        public DapperDao(DbConnection dbConnection) {
            _conn = dbConnection;
            _count = 0;
        }

        /// <summary>
        /// Dapperのパラメータ追加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        public virtual void AddParameter(string name, object value, DbType dbType) {
            _parameters.Add(name, value, dbType);
        }

        /// <summary>
        /// コミット
        /// </summary>
        public virtual void Commit() {
            try {
                _trans.Commit();
                _count = 0;
            } catch (Exception ex) {
                throw new Exception("Commit failed", ex);
            }
        }

        /// <summary>
        /// ロールバックする
        /// </summary>
        protected virtual void Rollback() {
            try {
                _trans.Rollback();
                _count = 0;
            } catch (Exception ex) {
                throw new Exception("Rollback faild", ex);
            }
        }

        /// <summary>
        /// トランザクション作成
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        protected virtual DbTransaction CreateTransaction(DbConnection connection) {
            return _conn.BeginTransaction();
        }

        public virtual int ExecuteCommand(string command) {
            try {
                _trans = CreateTransaction(_conn);
                var cnt = _conn.Execute(command, _parameters, _trans);
                _count += cnt;
                return cnt;
            } catch (Exception ex) {
                if (_count > 0) {
                    Rollback();
                }
                throw new Exception($"command:{command}", ex);
            }
        }

        public virtual async Task<int> ExecuteCommandAsync(string command) {
            try {
                _trans = CreateTransaction(_conn);
                var cnt = await _conn.ExecuteAsync(command, _parameters, _trans);
                _count += cnt;
                return cnt;
            } catch (Exception ex) {
                if (_count > 0) {
                    Rollback();
                }
                throw new Exception($"command:{command}", ex);
            }
        }

        public virtual IEnumerable<T> Query<T>(string queryString) {
            try {
                return _conn.Query<T>(queryString, _parameters);
            } catch (Exception ex) {
                throw new Exception($"query:{queryString}", ex);
            } finally {
                _parameters = new DynamicParameters();
            }
        }

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string queryString) {
            try {
                return await _conn.QueryAsync<T>(queryString, _parameters);
            } catch (Exception ex) {
                throw new Exception($"query:{queryString}", ex);
            } finally {
                _parameters = new DynamicParameters();
            }
        }

        public virtual IEnumerable<dynamic> Query(string queryString) {
            try {
                return _conn.Query(queryString, _parameters);
            } finally {
                _parameters = new DynamicParameters();
            }
        }

        public virtual async Task<IEnumerable<dynamic>> QueryAsync(string queryString) {
            try {
                return await _conn.QueryAsync(queryString, _parameters);
            } catch (Exception ex) {
                throw new Exception($"query:{queryString}", ex);
            } finally {
                _parameters = new DynamicParameters();
            }
        }

        #region IDisposable supported
        private bool disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    _trans?.Dispose();
                    _conn?.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
