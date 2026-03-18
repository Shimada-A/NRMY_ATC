using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutCondition
    {
        public string ConvertQuery()
        {
            if ((Common.ConditionClass)ConditionClass == Common.ConditionClass.Today)
            {
                return $" TBL.{ColumnId} >= TO_CHAR(SYSDATE, 'YYYYMMDD')  AND TBL.{ColumnId} < TO_CHAR(SYSDATE+1, 'YYYYMMDD') ";
            }

            var cond = ConvertConditionClassToValue();
            var value = ConvertConditionValue();
            if (string.IsNullOrEmpty(cond))
                return string.Empty;

            return $" TBL.{ColumnId } {cond} {value} ";
        }

        public string ConvertOrder()
        {
            return $"TBL.{ColumnId} {(Common.SortDirection)SortDirection}";
        }

        private string ConvertConditionClassToValue()
        {
            switch ((Common.ConditionClass)ConditionClass)
            {
                case Common.ConditionClass.Equal:
                    return "=";
                case Common.ConditionClass.NotEqual:
                    return "<>";
                case Common.ConditionClass.GreaterEqual:
                    return "<=";
                case Common.ConditionClass.Today:
                case Common.ConditionClass.LessEqual:
                    return ">=";
                case Common.ConditionClass.Like:
                    return "LIKE";
                case Common.ConditionClass.ThisMonth:
                case Common.ConditionClass.Range:
                    return "BETWEEN";
            }

            return string.Empty;
        }

        /// <summary>
        /// 比較対象の値
        /// </summary>
        /// <returns></returns>
        private string ConvertConditionValue()
        {
            switch ((Common.ConditionClass)ConditionClass)
            {
                case Common.ConditionClass.Equal:
                case Common.ConditionClass.NotEqual:
                case Common.ConditionClass.GreaterEqual:
                case Common.ConditionClass.LessEqual:
                    return $"'{ConditionValueFrom}'";
                case Common.ConditionClass.Today:
                    return "TO_CHAR(SYSDATE, 'YYYYMMDD') AND TO_CHAR(round(sysdate) - 1/86400,'YYYYMMDDHHMMSS')";
                case Common.ConditionClass.ThisMonth:
                    return "TO_CHAR(SYSDATE, 'YYYYMM') || '01' AND TO_CHAR(SYSDATE, 'YYYYMM') || '31'";
                case Common.ConditionClass.Like:
                    return $"'%' || '{ConditionValueFrom}' || '%'";
                case Common.ConditionClass.Range:
                    return $"'{ConditionValueFrom}' AND '{ConditionValueTo}'";
            }

            return string.Empty;
        }
    }
}