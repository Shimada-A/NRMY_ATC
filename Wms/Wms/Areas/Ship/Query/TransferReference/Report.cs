using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Ship.ViewModels.TransferReference;
using Wms.Models;
using Wms.Query;

namespace Wms.Areas.Ship.Query.TransferReference
{
    public class Report : BaseQuery
    {
        public IEnumerable<PickInfoResult> GetPickInfo(TransferReferenceSearchConditions condition)
        {
            var param = new DynamicParameters(new
            {
                SHIPPER_ID = Common.Profile.User.ShipperId,
                CENTER_ID = condition.CenterId,
                BATCH_NO = condition.BatchNo
            });

            var sql = GetPickInfoSql();
            return MvcDbContext.Current.Database.Connection.Query<PickInfoResult>(sql, param);
        }

        private static string GetPickInfoSql()
        {
            return @"
                SELECT
                    T.LOCATION_CD,
                    T.BATCH_NO,
                    T.JAN,
                    MAX(T.ITEM_NAME) ITEM_NAME,
                    SUM(T.HIKI_QTY) PIC_QTY
                FROM
                T_PIC T
                WHERE
                    T.SHIPPER_ID = :SHIPPER_ID
                    AND T.CENTER_ID = :CENTER_ID
                    AND T.BATCH_NO = :BATCH_NO
                GROUP BY
                    T.BATCH_NO
                    ,T.LOCATION_CD
                    ,T.JAN
                ORDER BY
                    T.LOCATION_CD
                    ,T.JAN
                ";
        }
    }
}