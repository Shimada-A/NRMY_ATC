namespace Wms.Areas.Master.ViewModels.VendorReturnSearchModal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;
    using PagedList;
    using Wms.Models;
    using static Wms.Areas.Master.ViewModels.VendorReturnSearchModal.VendorReturnSearchCondition;

    public partial class VendorReturnViewModel
    {
        /// <summary>
        /// 検索結果取得
        /// </summary>
        /// <param name="condition">画面検索条件</param>
        /// <param name="pageSize">最大ページサイズ</param>
        /// <returns>検索結果</returns>
        public List<VendorReturnViewModel> Listing(string centerId,ref int TotalItemCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                        MV.VENDOR_ID
                    ,   MAX(MV.VENDOR_NAME1) AS VENDOR_NAME1
                    ,   MAX(MV.VENDOR_NAME2) AS VENDOR_NAME2
                    ,   NVL(SUM(TS.STOCK_QTY),0) AS RETURN_STOCK_QTY
                FROM
                        T_STOCKS TS
                INNER JOIN
                        M_ITEM_SKU MIS
                ON
                        TS.SHIPPER_ID = MIS.SHIPPER_ID
                    AND TS.ITEM_SKU_ID = MIS.ITEM_SKU_ID
                INNER JOIN
                        M_VENDORS MV
                ON 
                        TS.SHIPPER_ID = MV.SHIPPER_ID
                    AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                WHERE
                        TS.SHIPPER_ID = :SHIPPER_ID
                    AND TS.CENTER_ID = :CENTER_ID
                    AND TS.LOCATION_CD = (SELECT SF_GET_FIXED_LOCATION(:SHIPPER_ID,:CENTER_ID,:MAKE_USER_ID,'MKH') FROM DUAL)
                    AND TS.STOCK_QTY > 0
                GROUP BY
                        TS.SHIPPER_ID
                    ,   TS.CENTER_ID
                    ,   MV.VENDOR_ID
                ORDER BY  MV.VENDOR_ID ASC
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":MAKE_USER_ID", Common.Profile.User.UserId);


            // 全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<VendorReturnViewModel>(sql.ToString(), parameters).Count();
            TotalItemCount = totalCount;

            return MvcDbContext.Current.Database.Connection.Query<VendorReturnViewModel>(sql.ToString(), parameters).ToList();

        }
    }
}