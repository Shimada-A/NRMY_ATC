namespace Share.Extensions.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using Dapper;

    public static class DbConnectionExtension
    {
        /// <summary>
        /// 1ページ分のデータと全ページのレコード数を取得します。
        /// </summary>
        /// <typeparam name="T">取得したデータをマップする型</typeparam>
        /// <param name="cnn">DBコネクション</param>
        /// <param name="sql">SQL</param>
        /// <param name="param">SQLのパラメーター</param>
        /// <param name="pageNumber">取得する</param>
        /// <param name="pageSize"></param>
        /// <param name="recordCount">全ページのレコード数</param>
        /// <returns>1ページ分のデータ</returns>
        public static IEnumerable<T> FetchWithRecordCountQuery<T>(
            this IDbConnection cnn,
            string sql,
            object param,
            int pageNumber,
            int pageSize,
            out int recordCount)
        {
            // 全ページ分のレコード数を取得する
            recordCount = GetTotalRecordCount(cnn, sql, param);

            // 1ページ分のレコードを取得する
            return cnn.Query<T>(MakeSqlForPaging(sql), CreateFetchParameterForPaging(param, pageNumber, pageSize));
        }

        /// <summary>
        /// レコード数を取得する
        /// </summary>
        /// <param name="cnn">DBコネクション</param>
        /// <param name="sql">SQL</param>
        /// <param name="param">SQLのパラメーター</param>
        /// <returns>レコード数</returns>
        public static int GetTotalRecordCount(
            this IDbConnection cnn,
            string sql,
            object param)
        {
            var count = new StringBuilder();
            count.AppendLine("SELECT");
            count.AppendLine("    COUNT(*) RECORD_COUNT");
            count.AppendLine("FROM(");
            count.AppendLine(sql);
            count.AppendLine(")");

            return cnn.QuerySingle<int>(count.ToString(), param);
        }

        /// <summary>
        /// 1ページ分を取得するSQLを生成します。
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns>1ページ分を取得するSQ</returns>
        private static string MakeSqlForPaging(string sql)
        {
            var fetch = new StringBuilder(sql);
            fetch.AppendLine("OFFSET :OFFSET ROWS FETCH NEXT :FETCH ROWS ONLY");
            return fetch.ToString();
        }

        /// <summary>
        /// 1ページ分を取得するSQLに必要なパラメーターを生成します。
        /// </summary>
        /// <param name="param">ページング以外のパラメーター</param>
        /// <param name="pageNumber">取得するページ番号</param>
        /// <param name="pageSize">1ページあたりのレコード数</param>
        /// <returns>1ページ分を取得するSQLに必要なパラメーター</returns>
        private static DynamicParameters CreateFetchParameterForPaging(object param, int pageNumber, int pageSize)
        {
            var parameters = new DynamicParameters(param);
            parameters.AddDynamicParams(new { OFFSET = (pageNumber - 1) * pageSize });
            parameters.AddDynamicParams(new { FETCH = pageSize });
            return parameters;
        }

        #region splitOn対応分

        public static IEnumerable<TResult> FetchWithRecordCountQuery<T1, T2, TResult>(
            this IDbConnection cnn,
            string sql,
            Func<T1, T2, TResult> map,
            object param,
            string split,
            int pageNumber,
            int pageSize,
            out int recordCount)
        {
            recordCount = GetTotalRecordCount(cnn, sql, param);
            return cnn.Query(MakeSqlForPaging(sql), map, CreateFetchParameterForPaging(param, pageNumber, pageSize), splitOn: split);
        }

        public static IEnumerable<TResult> FetchWithRecordCountQuery<T1, T2, T3, TResult>(
            this IDbConnection cnn,
            string sql,
            Func<T1, T2, T3, TResult> map,
            object param,
            string split,
            int pageNumber,
            int pageSize,
            out int recordCount)
        {
            recordCount = GetTotalRecordCount(cnn, sql, param);
            return cnn.Query(MakeSqlForPaging(sql), map, CreateFetchParameterForPaging(param, pageNumber, pageSize), splitOn: split);
        }

        public static IEnumerable<TResult> FetchWithRecordCountQuery<T1, T2, T3, T4, TResult>(
            this IDbConnection cnn,
            string sql,
            Func<T1, T2, T3, T4, TResult> map,
            object param,
            string split,
            int pageNumber,
            int pageSize,
            out int recordCount)
        {
            recordCount = GetTotalRecordCount(cnn, sql, param);
            return cnn.Query(MakeSqlForPaging(sql), map, CreateFetchParameterForPaging(param, pageNumber, pageSize), splitOn: split);
        }

        public static IEnumerable<TResult> FetchWithRecordCountQuery<T1, T2, T3, T4, T5, TResult>(
            this IDbConnection cnn,
            string sql,
            Func<T1, T2, T3, T4, T5, TResult> map,
            object param,
            string split,
            int pageNumber,
            int pageSize,
            out int recordCount)
        {
            recordCount = GetTotalRecordCount(cnn, sql, param);
            return cnn.Query(MakeSqlForPaging(sql), map, CreateFetchParameterForPaging(param, pageNumber, pageSize), splitOn: split);
        }

        public static IEnumerable<TResult> FetchWithRecordCountQuery<T1, T2, T3, T4, T5, T6, TResult>(
            this IDbConnection cnn,
            string sql,
            Func<T1, T2, T3, T4, T5, T6, TResult> map,
            object param,
            string split,
            int pageNumber,
            int pageSize,
            out int recordCount)
        {
            recordCount = GetTotalRecordCount(cnn, sql, param);
            return cnn.Query(MakeSqlForPaging(sql), map, CreateFetchParameterForPaging(param, pageNumber, pageSize), splitOn: split);
        }

        public static IEnumerable<TResult> FetchWithRecordCountQuery<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IDbConnection cnn,
            string sql,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> map,
            object param,
            string split,
            int pageNumber,
            int pageSize,
            out int recordCount)
        {
            recordCount = GetTotalRecordCount(cnn, sql, param);
            return cnn.Query(MakeSqlForPaging(sql), map, CreateFetchParameterForPaging(param, pageNumber, pageSize), splitOn: split);
        }

        #endregion
    }
}