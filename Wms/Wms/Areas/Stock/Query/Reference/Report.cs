namespace Wms.Areas.Stock.Query.Reference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Stock.ViewModels.Reference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Stock.ViewModels.Reference.ReferenceSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<ReferenceReport> StockListing(ReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
            SELECT
                       WSS.*
                    ,  ML.LOCSEC_1
                    ,  ML.LOCSEC_2
                    ,  ML.LOCSEC_3
                    ,  ML.LOCSEC_4
                    ,  ML.LOCSEC_5
                  FROM WW_STK_STOCKS01 WSS
                  LEFT OUTER JOIN 
                       M_LOCATIONS ML
                  ON
                        WSS.SHIPPER_ID = ML.SHIPPER_ID
                    AND WSS.CENTER_ID = ML.CENTER_ID
                    AND WSS.LOCATION_CD = ML.LOCATION_CD
                 WHERE WSS.SHIPPER_ID = :SHIPPER_ID
                   AND WSS.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.SortKey)
            {
                case StockSortKey.ItemSku:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSS.ITEM_SKU_ID DESC,WSS.GRADE_ID DESC,WSS.LOCATION_CD DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSS.ITEM_SKU_ID ASC,WSS.GRADE_ID ASC,WSS.LOCATION_CD ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSS.LOCATION_CD DESC,WSS.ITEM_SKU_ID DESC,WSS.GRADE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSS.LOCATION_CD ASC,WSS.ITEM_SKU_ID ASC,WSS.GRADE_ID ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<ReferenceReportPackage> PackageStockListing(ReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       WSS.*
                    ,  ML.LOCSEC_1
                    ,  ML.LOCSEC_2
                    ,  ML.LOCSEC_3
                    ,  ML.LOCSEC_4
                    ,  ML.LOCSEC_5
                    ,  WSS.STOCK_QTY * WSS.NORMAL_SELLING_PRICE_EX_TAX AS PRICE
                  FROM WW_STK_STOCKS02  WSS
                  LEFT OUTER JOIN 
                       M_LOCATIONS ML
                  ON
                        WSS.SHIPPER_ID = ML.SHIPPER_ID
                    AND WSS.CENTER_ID = ML.CENTER_ID
                    AND WSS.LOCATION_CD = ML.LOCATION_CD
                 WHERE WSS.SHIPPER_ID = :SHIPPER_ID
                   AND WSS.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // Sort function
            switch (condition.PackageSortKey)
            {
                case PackageStockSortKey.LocationBox:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSS.LOCATION_CD DESC,WSS.BOX_NO DESC,WSS.ITEM_SKU_ID DESC,WSS.GRADE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSS.LOCATION_CD ASC,WSS.BOX_NO ASC,WSS.ITEM_SKU_ID ASC,WSS.GRADE_ID ASC ");
                            break;
                    }

                    break;

                case PackageStockSortKey.BoxSku:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSS.BOX_NO DESC,WSS.ITEM_SKU_ID DESC,WSS.GRADE_ID DESC,WSS.LOCATION_CD DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSS.BOX_NO ASC,WSS.ITEM_SKU_ID ASC,WSS.GRADE_ID ASC,WSS.LOCATION_CD ASC ");
                            break;
                    }

                    break;

                case PackageStockSortKey.SkuGrade:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY ITEM_SKU_ID DESC,GRADE_ID DESC,LOCATION_CD DESC,BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY ITEM_SKU_ID ASC,GRADE_ID ASC,LOCATION_CD ASC,BOX_NO ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WSS.LOCATION_CD DESC,WSS.ITEM_SKU_ID DESC,WSS.GRADE_ID DESC,WSS.BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WSS.LOCATION_CD ASC,WSS.ITEM_SKU_ID ASC,WSS.GRADE_ID ASC,WSS.BOX_NO ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<ReferenceReportPackage>(query.ToString(), parameters);
        }
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<StockReport> StockListingForCsv(ReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                       WSS.CENTER_ID || '　' || WSS.CENTER_NAME CENTER
                      ,ML.LOCSEC_1
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,WSS.LOCATION_CD
                      ,WSS.CATEGORY_ID1
                      ,WSS.CATEGORY_NAME1
                      ,WSS.ITEM_ID
                      ,WSS.ITEM_NAME
                      ,WSS.ITEM_COLOR_ID
                      ,WSS.ITEM_COLOR_NAME
                      ,WSS.ITEM_SIZE_ID
                      ,WSS.ITEM_SIZE_NAME
                      ,WSS.GRADE_NAME
                      ,WSS.CASE_QTY
                      ,WSS.STOCK_QTY
                      ,WSS.ALLOC_QTY
                      ,WSS.JAN
                      ,0    PACKAGE_STOCK_REPORT_FLG
                      ," + (condition.DetailJanFlag ? "1 " : "0 ") + @" DETAIL_JAN_FLG
                  FROM WW_STK_STOCKS01 WSS
                 INNER JOIN M_LOCATIONS ML
                    ON WSS.SHIPPER_ID   = ML.SHIPPER_ID
                   AND WSS.CENTER_ID = ML.CENTER_ID
                   AND WSS.LOCATION_CD = ML.LOCATION_CD
                 INNER JOIN M_USERS MU
                    ON WSS.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                 WHERE WSS.SHIPPER_ID = :SHIPPER_ID
                   AND WSS.SEQ = :SEQ
                 ORDER BY ML.LOCSEC_1,WSS.LOCATION_CD,WSS.ITEM_SKU_ID,WSS.GRADE_ID
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<StockReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PackageStockReport> PackageStockListingForCsv(ReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                       WSS.CENTER_ID || '　' || WSS.CENTER_NAME CENTER
                      ,ML.LOCSEC_1
                      ,:USER_ID || '　' || MU.USER_NAME PRINT_USER
                      ,WSS.LOCATION_CD || '_' || WSS.CATEGORY_ID1 || '_' || WSS.CATEGORY_NAME1 || '_' || 
                       WSS.ITEM_ID || '_' || WSS.ITEM_NAME || '_' || WSS.ITEM_COLOR_ID || '_' || 
                       WSS.ITEM_COLOR_NAME || '_' || WSS.ITEM_SIZE_ID || '_' || WSS.ITEM_SIZE_NAME || '_' || 
                       WSS.JAN || '_' || WSS.GRADE_NAME ID
                      ,WSS.LOCATION_CD
                      ,WSS.CATEGORY_ID1
                      ,WSS.CATEGORY_NAME1
                      ,WSS.ITEM_ID
                      ,WSS.ITEM_NAME
                      ,WSS.ITEM_COLOR_ID
                      ,WSS.ITEM_COLOR_NAME
                      ,WSS.ITEM_SIZE_ID
                      ,WSS.ITEM_SIZE_NAME
                      ,WSS.GRADE_NAME
                      ,WSS.BOX_NO
                      ,WSS.STOCK_QTY
                      ,WSS.JAN
                      ,DENSE_RANK() OVER(
                            PARTITION BY
                                    ML.LOCSEC_1
                            ORDER BY
                                    ML.LOCSEC_1 
                                ,   WSS.LOCATION_CD 
                                ,   WSS.ITEM_SKU_ID 
                                ,   WSS.GRADE_NAME
                      ) AS STOCK_SEQ
                      ,1  PACKAGE_STOCK_REPORT_FLG
                      ," + (condition.DetailJanFlag ? "1 " : "0 ") + @" DETAIL_JAN_FLG
                 FROM WW_STK_STOCKS02 WSS
                 INNER JOIN M_LOCATIONS ML
                    ON WSS.SHIPPER_ID   = ML.SHIPPER_ID
                   AND WSS.CENTER_ID = ML.CENTER_ID
                   AND WSS.LOCATION_CD = ML.LOCATION_CD
                 INNER JOIN M_USERS MU
                    ON WSS.SHIPPER_ID   = MU.SHIPPER_ID
                   AND :USER_ID = MU.USER_ID
                 WHERE WSS.SHIPPER_ID = :SHIPPER_ID
                   AND WSS.SEQ = :SEQ
                 ORDER BY ML.LOCSEC_1 ,WSS.LOCATION_CD , WSS.ITEM_SKU_ID ,WSS.GRADE_NAME,WSS.BOX_NO
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":USER_ID", Profile.User.UserId);
            parameters.Add(":SEQ", condition.Seq);

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PackageStockReport>(query.ToString(), parameters);
        }
    }
}