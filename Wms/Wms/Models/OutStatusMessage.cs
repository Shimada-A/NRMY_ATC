using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Wms.Models
{
    public class OutStatusMessage
    {
        private const string OutStatusName = "OUT_STATUS";

        private const string OutMessageName = "OUT_MESSAGE";

        private const int MessageSize = 4096;

        private const int NormalStatus = 0;

        public static DynamicParameters CreateParam()
        {
            var param = new DynamicParameters();

            AddOutParameterInt(param, OutStatusName, 0);
            AddOutParameterString(param, OutMessageName, string.Empty, MessageSize);

            return param;
        }

        public static void AddInParameterString(DynamicParameters param, string name, string value, bool isDbNull = false)
        {
            AddParameter(param, name, isDbNull ? null : (object)value, DbType.String, true);
        }

        public static void AddInParameterInt(DynamicParameters param, string name, int value, bool isDbNull = false)
        {
            AddParameter(param, name, isDbNull ? null : (object)value, DbType.Int32, true);
        }

        public static void AddOutParameterString(DynamicParameters param, string name, string value = null, int size = MessageSize)
        {
            AddParameter(param, name, value ?? string.Empty, DbType.String, false, size);
        }

        public static void AddOutParameterInt(DynamicParameters param, string name, int value)
        {
            AddParameter(param, name, value, DbType.Int32, false);
        }

        private static void AddParameter(DynamicParameters param, string name, object value, DbType type, bool isInput, int? size = null)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            param.Add(name, value, type, isInput ? ParameterDirection.Input : ParameterDirection.Output, size);
        }

        public OutStatusMessage(DynamicParameters param)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            OutStatus = param.Get<int>(OutStatusName);
            OutMessage = param.Get<string>(OutMessageName);
        }

        public int OutStatus { get; private set; }

        public string OutMessage { get; private set; }

        public bool IsNormalStatus()
        {
            return OutStatus == NormalStatus;
        }

        public bool IsNegativeStatus()
        {
            return OutStatus < NormalStatus;
        }

        public bool IsPositiveStatus()
        {
            return OutStatus > NormalStatus;
        }
    }
}