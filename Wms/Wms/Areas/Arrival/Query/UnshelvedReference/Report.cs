namespace Wms.Areas.Arrival.Query.UnshelvedReference
{
    using System.Collections.Generic;
    using System.Text;
    using Dapper;
    using Wms.Areas.Arrival.ViewModels.UnshelvedReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Arrival.ViewModels.UnshelvedReference.UnshelvedReferenceSearchConditions;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<UnshelvedReferenceReport> ArrivalListing(UnshelvedReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT
                            TPS.CENTER_ID
                        ,   TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') ARRIVAL_DATE
                        ,   MIS.BRAND_ID
                        ,   MB.BRAND_NAME
                        ,   TAR.VENDOR_ID
                        ,   MV.VENDOR_NAME1 VENDOR_NAME
                        ,   TPS.INVOICE_NO
                        ,   TAR.INVOICE_SEQ
                        ,   MIC.CATEGORY_NAME1
                        ,   TPS.ITEM_SKU_ID
                        ,   TPS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   TPS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   TPS.ITEM_SIZE_ID
                        ,   MIS.ITEM_SIZE_NAME
                        ,   TPS.JAN
                        ,   COUNT(TPS.BOX_NO) CASE_QTY
                        ,   SUM(TPS.STOCK_QTY) STOCK_QTY
                    FROM
                            T_PACKAGE_STOCKS TPS
                    LEFT JOIN
                            T_ARRIVE_RESULTS TAR
                    ON
                            TAR.SHIPPER_ID = TPS.SHIPPER_ID
                        AND TAR.CENTER_ID = TPS.CENTER_ID
                        AND TAR.INVOICE_NO = TPS.INVOICE_NO
                        AND TAR.ITEM_SKU_ID = TPS.ITEM_SKU_ID
                    LEFT JOIN
                            M_VENDORS MV
                    ON
                            MV.SHIPPER_ID = TAR.SHIPPER_ID
                        AND MV.VENDOR_ID = TAR.VENDOR_ID
                    LEFT JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.SHIPPER_ID = TPS.SHIPPER_ID
                        AND MIS.ITEM_SKU_ID = TPS.ITEM_SKU_ID
                    LEFT JOIN
                            M_ITEM_CATEGORIES4 MIC
                    ON
                            MIC.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MIC.CATEGORY_ID1 = MIS.CATEGORY_ID1
                        AND MIC.CATEGORY_ID2 = MIS.CATEGORY_ID2
                        AND MIC.CATEGORY_ID3 = MIS.CATEGORY_ID3
                        AND MIC.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    LEFT JOIN
                            M_COLORS MC
                    ON
                            MC.SHIPPER_ID = TPS.SHIPPER_ID
                        AND MC.ITEM_COLOR_ID = TPS.ITEM_COLOR_ID
                    LEFT JOIN
                            M_SIZES MS
                    ON
                            MS.SHIPPER_ID = TPS.SHIPPER_ID
                        AND MS.ITEM_SIZE_ID = TPS.ITEM_SIZE_ID
                    LEFT JOIN
                            M_DIVISIONS MD
                    ON
                            MD.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MD.DIVISION_ID = MIS.DIVISION_ID
                    LEFT JOIN
                            M_BRANDS MB
                    ON
                            MB.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MB.BRAND_ID = MIS.BRAND_ID
                    WHERE
                            TPS.SHIPPER_ID = :SHIPPER_ID
                        AND TPS.LOCATION_CD = SF_GET_FIXED_LOCATION(
                                                    TPS.SHIPPER_ID
                                                ,   TPS.CENTER_ID
                                                ,   :USER_ID
                                                ,   'NYK-DC1'
                                            )
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);

            // Add search condition
            // センター
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND TPS.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // 入荷開始日
            if (condition.ArrivalDateFrom != null)
            {
                query.Append(" AND TO_DATE(TO_CHAR(TPS.MAKE_DATE,'yyyy-mm-dd'),'yyyy-mm-dd') >= :MAKE_DATE_FROM ");
                parameters.Add(":MAKE_DATE_FROM", condition.ArrivalDateFrom);
            }

            // 入荷終了日
            if (condition.ArrivalDateTo != null)
            {
                query.Append(" AND TO_DATE(TO_CHAR(TPS.MAKE_DATE,'yyyy-mm-dd'),'yyyy-mm-dd') <= :MAKE_DATE_TO ");
                parameters.Add(":MAKE_DATE_TO", condition.ArrivalDateTo);
            }

            // 事業部
            if (!string.IsNullOrEmpty(condition.DivisionCd))
            {
                query.Append(" AND MD.DIVISION_ID = :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionCd);
            }

            // ブランド
            if (!string.IsNullOrEmpty(condition.BrandId))
            {
                query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                parameters.Add(":BRAND_ID", condition.BrandId + "%");
            }

            // ブランド名
            if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
            {
                query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                parameters.Add(":BRAND_NAME", condition.BrandName + "%");
            }

            // 代表仕入先
            if (!string.IsNullOrEmpty(condition.VendorId))
            {
                query.Append(" AND TAR.VENDOR_ID LIKE :VENDOR_ID ");
                parameters.Add(":VENDOR_ID", condition.VendorId + "%");
            }

            // 代表仕入先名
            if (string.IsNullOrEmpty(condition.VendorId) && !string.IsNullOrEmpty(condition.VendorName))
            {
                query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                parameters.Add(":VENDOR_NAME1", condition.VendorName + "%");
            }

            // 分類1
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            // 分類2
            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            // 分類3
            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            // 分類4
            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 納品書番号
            if (!string.IsNullOrEmpty(condition.InvoiceNo))
            {
                query.Append(" AND TPS.INVOICE_NO LIKE :INVOICE_NO ");
                parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
            }

            // ケースNo
            if (!string.IsNullOrEmpty(condition.BoxNo))
            {
                query.Append(" AND TPS.BOX_NO LIKE :BOX_NO ");
                parameters.Add(":BOX_NO", condition.BoxNo + "%");
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TPS.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TPS.JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            query.AppendLine(@"
                    GROUP BY
                            TPS.CENTER_ID
                        ,   TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD')
                        ,   MIS.BRAND_ID
                        ,   MB.BRAND_NAME
                        ,   TAR.VENDOR_ID
                        ,   MV.VENDOR_NAME1
                        ,   TPS.INVOICE_NO
                        ,   TAR.INVOICE_SEQ
                        ,   MIC.CATEGORY_NAME1
                        ,   TPS.ITEM_SKU_ID
                        ,   TPS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   TPS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   TPS.ITEM_SIZE_ID
                        ,   MIS.ITEM_SIZE_NAME
                        ,   TPS.JAN
            ");

            // Sort function
            switch (condition.ArrivalSortKey)
            {
                case ArrivalSortKeyEnum.DateVendorIdSku:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') DESC,MIS.BRAND_ID DESC,TAR.VENDOR_ID DESC,TPS.ITEM_SKU_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') ASC,MIS.BRAND_ID ASC,TAR.VENDOR_ID ASC,TPS.ITEM_SKU_ID ASC ");
                            break;
                    }

                    break;

                case ArrivalSortKeyEnum.BrandIdVendorIdSku:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MIS.BRAND_ID DESC,TAR.VENDOR_ID DESC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') DESC,TPS.ITEM_SKU_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MIS.BRAND_ID ASC,TAR.VENDOR_ID ASC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') ASC,TPS.ITEM_SKU_ID ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TPS.ITEM_SKU_ID DESC,MIS.BRAND_ID DESC,TAR.VENDOR_ID DESC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TPS.ITEM_SKU_ID ASC,MIS.BRAND_ID ASC,TAR.VENDOR_ID ASC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<UnshelvedReferenceReport>(query.ToString(), parameters);
        }

        /// <summary>
        /// 配分Excelに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PackageUnshelvedReferenceReport> PackageArrivalListing(UnshelvedReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                    SELECT
                            TPS.CENTER_ID
                        ,   TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') ARRIVAL_DATE
                        ,   MIS.BRAND_ID
                        ,   MB.BRAND_NAME
                        ,   TAR.VENDOR_ID
                        ,   MV.VENDOR_NAME1 VENDOR_NAME
                        ,   TPS.INVOICE_NO
                        ,   MIC.CATEGORY_NAME1
                        ,   TPS.ITEM_SKU_ID
                        ,   TPS.ITEM_ID
                        ,   MIS.ITEM_NAME
                        ,   TPS.ITEM_COLOR_ID
                        ,   MC.ITEM_COLOR_NAME
                        ,   TPS.ITEM_SIZE_ID
                        ,   MIS.ITEM_SIZE_NAME
                        ,   TPS.BOX_NO BOX_NO
                        ,   TPS.STOCK_QTY STOCK_QTY
                    FROM
                            T_PACKAGE_STOCKS TPS
                    LEFT JOIN
                            T_ARRIVE_RESULTS TAR
                    ON
                            TAR.SHIPPER_ID = TPS.SHIPPER_ID
                        AND TAR.CENTER_ID = TPS.CENTER_ID
                        AND TAR.INVOICE_NO = TPS.INVOICE_NO
                        AND TAR.ITEM_SKU_ID = TPS.ITEM_SKU_ID
                    LEFT JOIN
                            M_VENDORS MV
                    ON
                            MV.SHIPPER_ID = TAR.SHIPPER_ID
                        AND MV.VENDOR_ID = TAR.VENDOR_ID
                    LEFT JOIN
                            M_ITEM_SKU MIS
                    ON
                            MIS.SHIPPER_ID = TPS.SHIPPER_ID
                        AND MIS.ITEM_SKU_ID = TPS.ITEM_SKU_ID
                    LEFT JOIN
                            M_ITEM_CATEGORIES4 MIC
                    ON
                            MIC.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MIC.CATEGORY_ID1 = MIS.CATEGORY_ID1
                        AND MIC.CATEGORY_ID2 = MIS.CATEGORY_ID2
                        AND MIC.CATEGORY_ID3 = MIS.CATEGORY_ID3
                        AND MIC.CATEGORY_ID4 = MIS.CATEGORY_ID4
                    LEFT JOIN
                            M_COLORS MC
                    ON
                            MC.SHIPPER_ID = TPS.SHIPPER_ID
                        AND MC.ITEM_COLOR_ID = TPS.ITEM_COLOR_ID
                    LEFT JOIN
                            M_SIZES MS
                    ON
                            MS.SHIPPER_ID = TPS.SHIPPER_ID
                        AND MS.ITEM_SIZE_ID = TPS.ITEM_SIZE_ID
                    LEFT JOIN
                            M_DIVISIONS MD
                    ON
                            MD.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MD.DIVISION_ID = MIS.DIVISION_ID
                    LEFT JOIN
                            M_BRANDS MB
                    ON
                            MB.SHIPPER_ID = MIS.SHIPPER_ID
                        AND MB.BRAND_ID = MIS.BRAND_ID
                    WHERE
                            TPS.SHIPPER_ID = :SHIPPER_ID
                        AND TPS.LOCATION_CD = SF_GET_FIXED_LOCATION(
                                                    TPS.SHIPPER_ID
                                                ,   TPS.CENTER_ID
                                                ,   :USER_ID
                                                ,   'NYK-DC1'
                                            )
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            parameters.Add(":USER_ID", Common.Profile.User.UserId);

            // Add search condition
            // センター
            if (!string.IsNullOrEmpty(condition.CenterId))
            {
                query.Append(" AND TPS.CENTER_ID = :CENTER_ID ");
                parameters.Add(":CENTER_ID", condition.CenterId);
            }

            // 入荷開始日
            if (condition.ArrivalDateFrom != null)
            {
                query.Append(" AND TO_DATE(TO_CHAR(TPS.MAKE_DATE,'yyyy-mm-dd'),'yyyy-mm-dd') >= :MAKE_DATE_FROM ");
                parameters.Add(":MAKE_DATE_FROM", condition.ArrivalDateFrom);
            }

            // 入荷終了日
            if (condition.ArrivalDateTo != null)
            {
                query.Append(" AND TO_DATE(TO_CHAR(TPS.MAKE_DATE,'yyyy-mm-dd'),'yyyy-mm-dd') <= :MAKE_DATE_TO ");
                parameters.Add(":MAKE_DATE_TO", condition.ArrivalDateTo);
            }

            // 事業部
            if (!string.IsNullOrEmpty(condition.DivisionCd))
            {
                query.Append(" AND MD.DIVISION_ID = :DIVISION_ID ");
                parameters.Add(":DIVISION_ID", condition.DivisionCd);
            }

            // ブランド
            if (!string.IsNullOrEmpty(condition.BrandId))
            {
                query.Append(" AND MIS.BRAND_ID LIKE :BRAND_ID ");
                parameters.Add(":BRAND_ID", condition.BrandId + "%");
            }

            // ブランド名
            if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
            {
                query.Append(" AND MB.BRAND_NAME LIKE :BRAND_NAME ");
                parameters.Add(":BRAND_NAME", condition.BrandName + "%");
            }

            // 代表仕入先
            if (!string.IsNullOrEmpty(condition.VendorId))
            {
                query.Append(" AND TAR.VENDOR_ID LIKE :VENDOR_ID ");
                parameters.Add(":VENDOR_ID", condition.VendorId + "%");
            }

            // 代表仕入先名
            if (string.IsNullOrEmpty(condition.VendorId) && !string.IsNullOrEmpty(condition.VendorName))
            {
                query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                parameters.Add(":VENDOR_NAME1", condition.VendorName + "%");
            }

            // 分類1
            if (!string.IsNullOrEmpty(condition.CategoryId1))
            {
                query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
            }

            // 分類2
            if (!string.IsNullOrEmpty(condition.CategoryId2))
            {
                query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
            }

            // 分類3
            if (!string.IsNullOrEmpty(condition.CategoryId3))
            {
                query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
            }

            // 分類4
            if (!string.IsNullOrEmpty(condition.CategoryId4))
            {
                query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
            }

            // 納品書番号
            if (!string.IsNullOrEmpty(condition.InvoiceNo))
            {
                query.Append(" AND TPS.INVOICE_NO LIKE :INVOICE_NO ");
                parameters.Add(":INVOICE_NO", condition.InvoiceNo + "%");
            }

            // ケースNo
            if (!string.IsNullOrEmpty(condition.BoxNo))
            {
                query.Append(" AND TPS.BOX_NO LIKE :BOX_NO ");
                parameters.Add(":BOX_NO", condition.BoxNo + "%");
            }

            // 品番
            if (!string.IsNullOrEmpty(condition.ItemId))
            {
                query.Append(" AND TPS.ITEM_ID LIKE :ITEM_ID ");
                parameters.Add(":ITEM_ID", condition.ItemId + "%");
            }

            // JAN
            if (!string.IsNullOrEmpty(condition.Jan))
            {
                query.Append(" AND TPS.JAN LIKE :JAN ");
                parameters.Add(":JAN", condition.Jan + "%");
            }

            // Sort function
            switch (condition.PackageArrivalSortKey)
            {
                case PackageArrivalSortKeyEnum.DateVendorIdSkuCaseNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TPS.MAKE_DATE DESC,MV.VENDOR_NAME1 DESC,TPS.BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TPS.MAKE_DATE ASC,MV.VENDOR_NAME1 ASC,TPS.BOX_NO ASC ");
                            break;
                    }

                    break;

                case PackageArrivalSortKeyEnum.VendorIdDateSkuCaseNo:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY  MIS.BRAND_ID DESC,TAR.VENDOR_ID DESC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') DESC,TPS.ITEM_SKU_ID DESC,TPS.BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MIS.BRAND_ID ASC,TAR.VENDOR_ID ASC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') ASC,TPS.ITEM_SKU_ID ASC,TPS.BOX_NO ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY MIS.ITEM_SKU_ID DESC,MIS.BRAND_ID DESC,TAR.VENDOR_ID DESC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') DESC,TPS.BOX_NO DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY MIS.ITEM_SKU_ID ASC,MIS.BRAND_ID ASC,TAR.VENDOR_ID ASC,TO_CHAR(TPS.MAKE_DATE,'YYYY/MM/DD') ASC,TPS.BOX_NO ASC ");
                            break;
                    }

                    break;
            }

            // Fill data to memory
            return MvcDbContext.Current.Database.Connection.Query<PackageUnshelvedReferenceReport>(query.ToString(), parameters);
        }
    }
}