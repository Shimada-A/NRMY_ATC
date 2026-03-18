using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutDetail
    {
        public string ConvertQuery()
        {
            if (!Digit.HasValue)
                return $",TBL.{ColumnId}";

            string value;
            if (DataType == (byte)Common.DataType.Number)
            {
                // 数値型であれば桁数MAXまでの値で返す
                var maxNum = Math.Pow(10, Digit.GetValueOrDefault()) - 1;
                value = $@"
                    (CASE
                        WHEN TBL.{ColumnId} > {maxNum} THEN
                            {maxNum}
                        ELSE
                            ROUND(TBL.{ColumnId})
                    END)
                ";
            }
            else
            {
                // 文字列型であれば、桁数までで返す
                value =  $"SUBSTR(tbl.{ColumnId},1,{Digit})";
            }

            if (!PadDirection.HasValue || PadDirection.Value == 0)
                return "," + value;

            if (PadDirection.Value == (byte)Common.PadDirection.PadLeft)
            {
                return $", LPAD({value},{Digit},'{PadChar}')";
            }
            else
            {
                return $", RPAD({value},{Digit},'{PadChar}')";
            }
        }
    }
}