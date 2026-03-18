namespace Share.Extensions.Classes
{
    using System;
    using Oracle.ManagedDataAccess.Client;

    public static class OracleExtention
    {
        /// <summary>
        /// 一意制約違反かどうかを返します。
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns>True:一意制約/False:その他の例外</returns>
        /// <remarks>Oracleの事前定義例外:DUP_VAL_ON_INDEX</remarks>
        public static bool DuplicateValueOnIndexException(Exception ex)
        {
            var oracleEx = GetOracleException(ex);
            if (oracleEx == null)
            {
                return false;
            }
            else
            {
                if (oracleEx.Number == 1)
                {
                    // ORA-00001:一意制約違反
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 例外からOracleExceptionを返します。
        /// </summary>
        /// <param name="ex">例外</param>
        /// <returns>OracleException</returns>
        private static OracleException GetOracleException(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            if (ex is OracleException)
            {
                return ex as OracleException;
            }
            else
            {
                return GetOracleException(ex.InnerException);
            }
        }
    }
}