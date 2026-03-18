namespace Wms.Areas.Stock.Query.Reference
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Stock.Models;
    using Wms.Areas.Stock.ViewModels.Reference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Stock.ViewModels.Reference.ReferenceSearchConditions;

    public class ReferenceQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertStkStock(ReferenceSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;

                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    // 在庫明細
                    if (condition.ResultType == ResultTypes.Stock)
                    {
                        query = new StringBuilder(@"
                        INSERT INTO WW_STK_STOCKS01(
                                MAKE_DATE
                            ,   MAKE_USER_ID
                            ,   MAKE_PROGRAM_NAME
                            ,   UPDATE_DATE
                            ,   UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME
                            ,   UPDATE_COUNT
                            ,   SHIPPER_ID
                            ,   SEQ
                            ,   LINE_NO
                            ,   IS_CHECK
                            ,   CENTER_ID
                            ,   CENTER_NAME
                            ,   LOCATION_CD
                            ,   BOX_NO
                            ,   DIVISION_ID
                            ,   CATEGORY_ID1
                            ,   CATEGORY_NAME1
                            ,   CATEGORY_NAME2
                            ,   CATEGORY_NAME3
                            ,   CATEGORY_NAME4
                            ,   ITEM_SEASON_NAME
                            ,   ITEM_SKU_ID
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_COLOR_NAME
                            ,   ITEM_SIZE_ID
                            ,   ITEM_SIZE_NAME
                            ,   NORMAL_SELLING_PRICE_EX_TAX
                            ,   JAN
                            ,   GRADE_ID
                            ,   GRADE_NAME
                            ,   CASE_QTY
                            ,   STOCK_QTY
                            ,   ALLOC_QTY
                            ,   UN_ALLOC_QTY
                            ,   ITEM_CODE
                            ,   ITEM_SEASON_YEAR
                            ,   BRAND_ID
                            ,   ITEM_CODE_NAME
                            ,   BRAND_NAME
                            ,   PRICE
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   :SHIPPER_ID
                            ,   :SEQ
                            ,   ROWNUM AS LINE_NO
                            ,   0 AS IS_CHECK
                            ,   TS.CENTER_ID
                            ,   MW.CENTER_NAME1 || MW.CENTER_NAME2 CENTER_NAME
                            ,   TS.LOCATION_CD
                            ,   '' BOX_NO
                            ,   MIS.DIVISION_ID
                            ,   MIS.CATEGORY_ID1
                            ,   MIC.CATEGORY_NAME1
                            ,   MIC.CATEGORY_NAME2
                            ,   MIC.CATEGORY_NAME3
                            ,   MIC.CATEGORY_NAME4
                            ,   MGEN.GEN_NAME AS ITEM_SEASON_NAME
                            ,   TS.ITEM_SKU_ID
                            ,   TS.ITEM_ID
                            ,   MIS.ITEM_NAME
                            ,   TS.ITEM_COLOR_ID
                            ,   MC.ITEM_COLOR_NAME
                            ,   TS.ITEM_SIZE_ID
                            ,   MIS.ITEM_SIZE_NAME
                            ,   MIS.NORMAL_SELLING_PRICE_EX_TAX
                            ,   TS.JAN
                            ,   TS.GRADE_ID
                            ,   MG.GRADE_NAME
                            ,   TPS.CASE_QTY
                            ,   TS.STOCK_QTY
                            ,   TS.ALLOC_QTY
                            ,   (TS.STOCK_QTY - TS.ALLOC_QTY) AS UN_ALLOC_QTY
                            ,   MIS.ITEM_CODE
                            ,   MIS.SEASON_YEAR
                            ,   MIS.BRAND_ID
                            ,   MI.ITEM_CODE_NAME
                            ,   MB.BRAND_NAME
                            ,   TS.STOCK_QTY * MIS.NORMAL_SELLING_PRICE_EX_TAX
                        FROM
                                T_STOCKS TS ");
                        // 格納状態
                        if (condition.StorageStatus)
                        {
                            query.Append(@" 
                        INNER JOIN (
                                SELECT
                                        TST.CENTER_ID
                                    ,   TST.ITEM_SKU_ID
                                FROM
                                        T_STOCKS TST
                                INNER JOIN
                                        M_LOCATIONS ML
                                ON
                                        TST.SHIPPER_ID = ML.SHIPPER_ID
                                    AND TST.CENTER_ID = ML.CENTER_ID
                                    AND TST.LOCATION_CD = ML.LOCATION_CD
                                WHERE
                                        TST.SHIPPER_ID  = :SHIPPER_ID
                                    AND TST.CENTER_ID   = :CENTER_ID
                                    AND TST.STOCK_QTY   > 0
                                    AND ML.LOCATION_CLASS IN('01','03')
                                GROUP BY
                                        TST.CENTER_ID
                                    ,   ML.LOCATION_CLASS
                                    ,   TST.ITEM_SKU_ID
                                HAVING
                                        COUNT(DISTINCT(TST.LOCATION_CD)) > 1
                            ) T
                        ON
                                TS.CENTER_ID = T.CENTER_ID
                            AND TS.ITEM_SKU_ID = T.ITEM_SKU_ID ");
                        }

                        // ケースNo
                        if (!string.IsNullOrEmpty(condition.BoxNo))
                        {
                            query.Append(@" 
                        INNER JOIN (
                                SELECT
                                        SHIPPER_ID
                                    ,   ITEM_SKU_ID
                                    ,   LOCATION_CD
                                    ,   CENTER_ID
                                FROM
                                        T_PACKAGE_STOCKS
                                WHERE
                                        BOX_NO IS NOT NULL
                                    AND BOX_NO = :BOX_NO
                                GROUP BY
                                        SHIPPER_ID
                                    ,   ITEM_SKU_ID
                                    ,   LOCATION_CD
                                    ,   CENTER_ID
                            ) TPS1
                        ON
                                TS.SHIPPER_ID = TPS1.SHIPPER_ID
                           AND TS.ITEM_SKU_ID = TPS1.ITEM_SKU_ID
                           AND TS.LOCATION_CD = TPS1.LOCATION_CD
                           AND TS.CENTER_ID = TPS1.CENTER_ID ");
                            parameters.Add(":BOX_NO", condition.BoxNo);
                        }

                        query.Append(@"
                        LEFT JOIN
                                M_CENTERS MW
                        ON
                                TS.SHIPPER_ID = MW.SHIPPER_ID
                            AND TS.CENTER_ID = MW.CENTER_ID
                        LEFT JOIN
                                M_ITEM_SKU MIS
                        ON
                                TS.SHIPPER_ID   = MIS.SHIPPER_ID
                            AND TS.ITEM_SKU_ID  = MIS.ITEM_SKU_ID
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
                                TS.SHIPPER_ID   = MC.SHIPPER_ID
                            AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        LEFT JOIN
                                M_SIZES MS
                        ON
                                TS.SHIPPER_ID   = MS.SHIPPER_ID
                            AND TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                        LEFT JOIN
                                M_GRADES MG
                        ON
                                TS.SHIPPER_ID   = MG.SHIPPER_ID
                            AND TS.GRADE_ID     = MG.GRADE_ID
                        LEFT JOIN
                                M_GENERALS MGEN
                        ON
                                TS.SHIPPER_ID = MGEN.SHIPPER_ID
                            AND MGEN.CENTER_ID = '@@@'
                            AND MGEN.REGISTER_DIVI_CD = '1'
                            AND MGEN.GEN_DIV_CD = 'SEASON_NAME'
                            AND MIS.ITEM_SEASON_ID = MGEN.GEN_CD
                        LEFT JOIN
                                M_ITEM_CODE MI
                        ON
                                TS.SHIPPER_ID = MI.SHIPPER_ID
                            AND MIS.ITEM_CODE = MI.ITEM_CODE
                        LEFT JOIN
                                M_BRANDS MB
                        ON
                                TS.SHIPPER_ID = MB.SHIPPER_ID
                            AND MIS.BRAND_ID = MB.BRAND_ID
                        LEFT JOIN (
                                SELECT
                                        SHIPPER_ID
                                    ,   ITEM_SKU_ID
                                    ,   LOCATION_CD
                                    ,   CENTER_ID
                                    ,   COUNT(DISTINCT(BOX_NO)) CASE_QTY
                                FROM
                                        T_PACKAGE_STOCKS
                                WHERE
                                        BOX_NO <> ' ' ");

                        // ケースNo
                        if (!string.IsNullOrEmpty(condition.BoxNo))
                        {
                            query.Append(" AND BOX_NO = :BOX_NO ");
                            parameters.Add(":BOX_NO", condition.BoxNo);
                        }

                        query.Append(@"
                        GROUP BY
                                SHIPPER_ID
                            ,   ITEM_SKU_ID
                            ,   LOCATION_CD
                            ,   CENTER_ID
                        ) TPS
                        ON
                                TS.SHIPPER_ID   = TPS.SHIPPER_ID
                            AND TS.ITEM_SKU_ID  = TPS.ITEM_SKU_ID
                            AND TS.LOCATION_CD  = TPS.LOCATION_CD
                            AND TS.CENTER_ID       = TPS.CENTER_ID
                        LEFT JOIN
                                M_LOCATIONS ML
                        ON
                                TS.SHIPPER_ID   = ML.SHIPPER_ID
                            AND TS.CENTER_ID       = ML.CENTER_ID
                            AND TS.LOCATION_CD  = ML.LOCATION_CD
                        LEFT JOIN
                                M_BRANDS MB
                        ON
                                MIS.SHIPPER_ID  = MB.SHIPPER_ID
                            AND MIS.BRAND_ID    = MB.BRAND_ID
                        LEFT JOIN
                                M_VENDORS MV
                        ON
                                MIS.SHIPPER_ID = MV.SHIPPER_ID
                            AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                        WHERE
                                TS.SHIPPER_ID   = :SHIPPER_ID
                            AND TS.CENTER_ID = :CENTER_ID
                            AND TS.LOCATION_CD <> SF_GET_FIXED_LOCATION(:SHIPPER_ID, :CENTER_ID, :USER_ID, 'ZZZ')
                        ");

                        parameters.Add(":USER_ID", Profile.User.UserId);
                        parameters.Add(":PROGRAM_NAME", "InsertStkStock");
                        parameters.Add(":SEQ", condition.Seq);
                        parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                        parameters.Add(":CENTER_ID", condition.CenterId);

                        // 事業部
                        if (!string.IsNullOrEmpty(condition.DivisionId))
                        {
                            query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                            parameters.Add(":DIVISION_ID", condition.DivisionId);
                        }

                        // ロケーション区分
                        if (!string.IsNullOrEmpty(condition.LocationClass))
                        {
                            query.Append(" AND ML.LOCATION_CLASS = :LOCATION_CLASS ");
                            parameters.Add(":LOCATION_CLASS", condition.LocationClass);
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

                        // エリア
                        if (!string.IsNullOrEmpty(condition.Locsec1))
                        {
                            query.Append(" AND ML.LOCSEC_1 LIKE :LOCSEC_1 ");
                            parameters.Add(":LOCSEC_1", condition.Locsec1 + "%");
                        }

                        // 代表仕入先
                        if (!string.IsNullOrEmpty(condition.MainVendorId))
                        {
                            query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                            parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
                        }

                        // 代表仕入先名
                        if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
                        {
                            query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                            parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
                        }

                        // ロケーション
                        if (!string.IsNullOrEmpty(condition.LocationCdFrom) && string.IsNullOrEmpty(condition.LocationCdTo))
                        {
                            query.Append(" AND TS.LOCATION_CD LIKE :LOCATION_CD ");
                            parameters.Add(":LOCATION_CD", condition.LocationCdFrom + "%");
                        }

                        if (!string.IsNullOrEmpty(condition.LocationCdFrom) && !string.IsNullOrEmpty(condition.LocationCdTo))
                        {
                            query.Append(@" AND TS.LOCATION_CD >= :LOCATION_CD_FROM
                                             AND TS.LOCATION_CD <= :LOCATION_CD_TO ");
                            parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
                            parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
                        }

                        // 分類
                        if (!string.IsNullOrEmpty(condition.CategoryId1))
                        {
                            query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                            parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId2))
                        {
                            query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                            parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId3))
                        {
                            query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                            parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId4))
                        {
                            query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                            parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                        }

                        // アイテムコード
                        if (!string.IsNullOrEmpty(condition.ItemCode))
                        {
                            query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                            parameters.Add(":ITEM_CODE", condition.ItemCode);
                        }

                        // JAN
                        if (!string.IsNullOrEmpty(condition.Jan))
                        {
                            query.Append(" AND TS.JAN LIKE :JAN ");
                            parameters.Add(":JAN", condition.Jan + "%");
                        }

                        // 品番
                        if (!string.IsNullOrEmpty(condition.ItemId))
                        {
                            query.Append(" AND TS.ITEM_ID LIKE :ITEM_ID ");
                            parameters.Add(":ITEM_ID", condition.ItemId + "%");
                        }

                        // SKU
                        if (!string.IsNullOrEmpty(condition.ItemSkuId))
                        {
                            query.Append(" AND TS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                            parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId.ToString() + "%");
                        }

                        // カラー
                        if (!string.IsNullOrEmpty(condition.ItemColorId))
                        {
                            query.Append(" AND TS.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID ");
                            parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId + "%");
                        }

                        // カラー名
                        if (string.IsNullOrEmpty(condition.ItemColorId) && !string.IsNullOrEmpty(condition.ItemColorName))
                        {
                            query.Append(" AND MC.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME ");
                            parameters.Add(":ITEM_COLOR_NAME", condition.ItemColorName + "%");
                        }

                        // サイズ
                        if (!string.IsNullOrEmpty(condition.ItemSizeId))
                        {
                            query.Append(" AND TS.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID ");
                            parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId + "%");
                        }

                        // サイズ名
                        if (string.IsNullOrEmpty(condition.ItemSizeId) && !string.IsNullOrEmpty(condition.ItemSizeName))
                        {
                            query.Append(" AND MIS.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                            parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
                        }

                        // 格付
                        if (!string.IsNullOrEmpty(condition.GradeId))
                        {
                            query.Append(" AND TS.GRADE_ID = :GRADE_ID ");
                            parameters.Add(":GRADE_ID", condition.GradeId);
                        }

                        // 在庫数 在庫数0を表示する　
                        if (!condition.StockQtyFlag)
                        {
                            query.Append(" AND TS.STOCK_QTY <> 0 ");
                        }
                    }
                    else
                    {
                        query = new StringBuilder(@"
                        INSERT INTO WW_STK_STOCKS02 (
                                MAKE_DATE
                            ,   MAKE_USER_ID
                            ,   MAKE_PROGRAM_NAME
                            ,   UPDATE_DATE
                            ,   UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME
                            ,   UPDATE_COUNT
                            ,   SHIPPER_ID
                            ,   SEQ
                            ,   LINE_NO
                            ,   CENTER_ID
                            ,   CENTER_NAME
                            ,   LOCATION_CD
                            ,   BOX_NO
                            ,   DIVISION_ID
                            ,   CATEGORY_ID1
                            ,   CATEGORY_NAME1
                            ,   CATEGORY_NAME2
                            ,   CATEGORY_NAME3
                            ,   CATEGORY_NAME4
                            ,   ITEM_SEASON_NAME
                            ,   ITEM_SKU_ID
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_COLOR_NAME
                            ,   ITEM_SIZE_ID
                            ,   ITEM_SIZE_NAME
                            ,   NORMAL_SELLING_PRICE_EX_TAX
                            ,   JAN
                            ,   GRADE_ID
                            ,   GRADE_NAME
                            ,   STOCK_QTY
                            ,   INVOICE_NO
                            ,   ALLOC_QTY
                            ,   UN_ALLOC_QTY
                            ,   ITEM_CODE
                            ,   ITEM_SEASON_YEAR
                            ,   BRAND_ID
                            ,   ITEM_CODE_NAME
                            ,   BRAND_NAME
                            ,   PRICE
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   :SHIPPER_ID
                            ,   :SEQ
                            ,   ROWNUM AS LINE_NO
                            ,   TPS.CENTER_ID
                            ,   MW.CENTER_NAME1 || MW.CENTER_NAME2 CENTER_NAME
                            ,   TPS.LOCATION_CD
                            ,   TPS.BOX_NO
                            ,   MIS.DIVISION_ID
                            ,   MIS.CATEGORY_ID1
                            ,   MIC.CATEGORY_NAME1
                            ,   MIC.CATEGORY_NAME2
                            ,   MIC.CATEGORY_NAME3
                            ,   MIC.CATEGORY_NAME4
                            ,   MGEN.GEN_NAME AS ITEM_SEASON_NAME
                            ,   TPS.ITEM_SKU_ID
                            ,   TPS.ITEM_ID
                            ,   MIS.ITEM_NAME
                            ,   TPS.ITEM_COLOR_ID
                            ,   MC.ITEM_COLOR_NAME
                            ,   TPS.ITEM_SIZE_ID
                            ,   MIS.ITEM_SIZE_NAME
                            ,   MIS.NORMAL_SELLING_PRICE_EX_TAX
                            ,   TPS.JAN
                            ,   TPS.GRADE_ID
                            ,   MG.GRADE_NAME
                            ,   TPS.STOCK_QTY
                            ,   TPS.INVOICE_NO
                            ,   TPS.ALLOC_QTY
                            ,   (TPS.STOCK_QTY - TPS.ALLOC_QTY) AS UN_ALLOC_QTY
                            ,   MIS.ITEM_CODE
                            ,   MIS.SEASON_YEAR
                            ,   MIS.BRAND_ID
                            ,   MI.ITEM_CODE_NAME
                            ,   MB.BRAND_NAME
                            ,   TPS.STOCK_QTY * MIS.NORMAL_SELLING_PRICE_EX_TAX
                        FROM
                                T_PACKAGE_STOCKS TPS ");
                        // 格納状態
                        if (condition.StorageStatus)
                        {
                            query.Append(@" 
                        INNER JOIN (
                                SELECT
                                        TST.CENTER_ID
                                    ,   TST.ITEM_SKU_ID
                                FROM
                                        T_STOCKS TST
                                INNER JOIN 
                                        M_LOCATIONS ML
                                ON
                                        TST.SHIPPER_ID = ML.SHIPPER_ID
                                    AND TST.CENTER_ID = ML.CENTER_ID
                                    AND TST.LOCATION_CD = ML.LOCATION_CD
                                WHERE
                                        TST.SHIPPER_ID  = :SHIPPER_ID
                                    AND TST.CENTER_ID   = :CENTER_ID
                                    AND TST.STOCK_QTY   > 0
                                    AND ML.LOCATION_CLASS IN('01','03')
                                GROUP BY
                                        TST.CENTER_ID
                                    ,   ML.LOCATION_CLASS
                                    ,   TST.ITEM_SKU_ID
                                HAVING
                                        COUNT(DISTINCT(TST.LOCATION_CD)) > 1
                        ) T
                        ON
                                TPS.CENTER_ID = T.CENTER_ID
                            AND TPS.ITEM_SKU_ID = T.ITEM_SKU_ID ");
                        }
                        query.Append(@" 
                        LEFT JOIN
                                M_CENTERS MW
                        ON
                                TPS.SHIPPER_ID   = MW.SHIPPER_ID
                            AND TPS.CENTER_ID  = MW.CENTER_ID
                        LEFT JOIN
                                M_ITEM_SKU MIS
                        ON
                                TPS.SHIPPER_ID   = MIS.SHIPPER_ID
                            AND TPS.ITEM_SKU_ID  = MIS.ITEM_SKU_ID
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
                                TPS.SHIPPER_ID   = MC.SHIPPER_ID
                            AND TPS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        LEFT JOIN
                                M_SIZES MS
                        ON
                                TPS.SHIPPER_ID   = MS.SHIPPER_ID
                            AND TPS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                        LEFT JOIN
                                M_GRADES MG
                        ON
                                TPS.SHIPPER_ID   = MG.SHIPPER_ID
                            AND TPS.GRADE_ID     = MG.GRADE_ID
                        LEFT JOIN
                                M_LOCATIONS ML
                        ON
                                TPS.SHIPPER_ID   = ML.SHIPPER_ID
                            AND TPS.CENTER_ID       = ML.CENTER_ID
                            AND TPS.LOCATION_CD  = ML.LOCATION_CD
                        LEFT JOIN
                                M_BRANDS MB
                        ON
                                MIS.SHIPPER_ID  = MB.SHIPPER_ID
                            AND MIS.BRAND_ID    = MB.BRAND_ID
                        LEFT JOIN
                                M_VENDORS MV
                        ON
                                MIS.SHIPPER_ID = MV.SHIPPER_ID
                            AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                        LEFT JOIN
                                M_GENERALS MGEN
                        ON
                                TPS.SHIPPER_ID = MGEN.SHIPPER_ID
                            AND MGEN.CENTER_ID = '@@@'
                            AND MGEN.REGISTER_DIVI_CD = '1'
                            AND MGEN.GEN_DIV_CD = 'SEASON_NAME'
                            AND MIS.ITEM_SEASON_ID = MGEN.GEN_CD
                        LEFT JOIN
                                M_ITEM_CODE MI
                        ON
                                TPS.SHIPPER_ID = MI.SHIPPER_ID
                            AND MIS.ITEM_CODE = MI.ITEM_CODE
                        WHERE
                                TPS.SHIPPER_ID   = :SHIPPER_ID
                            AND TPS.CENTER_ID = :CENTER_ID
                            AND TPS.LOCATION_CD <> SF_GET_FIXED_LOCATION(:SHIPPER_ID, :CENTER_ID, :USER_ID, 'ZZZ')
                        ");
                        parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                        parameters.Add(":USER_ID", Profile.User.UserId);
                        parameters.Add(":PROGRAM_NAME", "InsertStkStock");
                        parameters.Add(":SEQ", condition.Seq);
                        parameters.Add(":CENTER_ID", condition.CenterId);

                        // Add search condition

                        // 事業部
                        if (!string.IsNullOrEmpty(condition.DivisionId))
                        {
                            query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                            parameters.Add(":DIVISION_ID", condition.DivisionId);
                        }

                        // ロケーション区分
                        if (!string.IsNullOrEmpty(condition.LocationClass))
                        {
                            query.Append(" AND ML.LOCATION_CLASS = :LOCATION_CLASS ");
                            parameters.Add(":LOCATION_CLASS", condition.LocationClass);
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

                        // エリア
                        if (!string.IsNullOrEmpty(condition.Locsec1))
                        {
                            query.Append(" AND ML.LOCSEC_1 LIKE :LOCSEC_1 ");
                            parameters.Add(":LOCSEC_1", condition.Locsec1 + "%");
                        }

                        // 代表仕入先
                        if (!string.IsNullOrEmpty(condition.MainVendorId))
                        {
                            query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                            parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
                        }

                        // 代表仕入先名
                        if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
                        {
                            query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                            parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
                        }

                        // ロケーション
                        if (!string.IsNullOrEmpty(condition.LocationCdFrom) && string.IsNullOrEmpty(condition.LocationCdTo))
                        {
                            query.Append(" AND TPS.LOCATION_CD LIKE :LOCATION_CD ");
                            parameters.Add(":LOCATION_CD", condition.LocationCdFrom + "%");
                        }

                        if (!string.IsNullOrEmpty(condition.LocationCdFrom) && !string.IsNullOrEmpty(condition.LocationCdTo))
                        {
                            query.Append(@" AND TPS.LOCATION_CD >= :LOCATION_CD_FROM
                                     AND TPS.LOCATION_CD <= :LOCATION_CD_TO ");
                            parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
                            parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
                        }

                        // 分類
                        if (!string.IsNullOrEmpty(condition.CategoryId1))
                        {
                            query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                            parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId2))
                        {
                            query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                            parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId3))
                        {
                            query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                            parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                        }

                        if (!string.IsNullOrEmpty(condition.CategoryId4))
                        {
                            query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                            parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                        }

                        // アイテムコード
                        if (!string.IsNullOrEmpty(condition.ItemCode))
                        {
                            query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                            parameters.Add(":ITEM_CODE", condition.ItemCode);
                        }

                        // JAN
                        if (!string.IsNullOrEmpty(condition.Jan))
                        {
                            query.Append(" AND TPS.JAN LIKE :JAN ");
                            parameters.Add(":JAN", condition.Jan + "%");
                        }

                        // ケースNo
                        if (!string.IsNullOrEmpty(condition.BoxNo))
                        {
                            query.Append(" AND TPS.BOX_NO = :BOX_NO ");
                            parameters.Add(":BOX_NO", condition.BoxNo);
                        }

                        // 品番
                        if (!string.IsNullOrEmpty(condition.ItemId))
                        {
                            query.Append(" AND TPS.ITEM_ID LIKE :ITEM_ID ");
                            parameters.Add(":ITEM_ID", condition.ItemId + "%");
                        }

                        // SKU
                        if (!string.IsNullOrEmpty(condition.ItemSkuId))
                        {
                            query.Append(" AND TPS.ITEM_SKU_ID LIKE :ITEM_SKU_ID ");
                            parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId.ToString() + "%");
                        }

                        // カラー
                        if (!string.IsNullOrEmpty(condition.ItemColorId))
                        {
                            query.Append(" AND TPS.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID ");
                            parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId + "%");
                        }

                        // カラー名
                        if (string.IsNullOrEmpty(condition.ItemColorId) && !string.IsNullOrEmpty(condition.ItemColorName))
                        {
                            query.Append(" AND MC.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME ");
                            parameters.Add(":ITEM_COLOR_NAME", condition.ItemColorName + "%");
                        }

                        // サイズ
                        if (!string.IsNullOrEmpty(condition.ItemSizeId))
                        {
                            query.Append(" AND TPS.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID ");
                            parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId + "%");
                        }

                        // サイズ名
                        if (string.IsNullOrEmpty(condition.ItemSizeId) && !string.IsNullOrEmpty(condition.ItemSizeName))
                        {
                            query.Append(" AND MIS.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                            parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
                        }

                        // 格付
                        if (!string.IsNullOrEmpty(condition.GradeId))
                        {
                            query.Append(" AND TPS.GRADE_ID = :GRADE_ID ");
                            parameters.Add(":GRADE_ID", condition.GradeId);
                        }

                        // 在庫数 在庫数0を表示する　
                        if (!condition.StockQtyFlag)
                        {
                            query.Append(" AND TPS.STOCK_QTY <> 0 ");
                        }
                    }
                    // 2.検索・ワーク作成
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
                return true;
            }
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertStkStock02(ReferenceSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    var keys = MvcDbContext.Current.StkStock01s.Where(x => x.IsCheck && x.Seq == condition.Seq).Select(x => new { x.ShipperId, x.CenterId, x.LocationCd, x.ItemSkuId }).ToList();

                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_STK_STOCKS02 (
                                MAKE_DATE
                            ,   MAKE_USER_ID
                            ,   MAKE_PROGRAM_NAME
                            ,   UPDATE_DATE
                            ,   UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME
                            ,   UPDATE_COUNT
                            ,   SHIPPER_ID
                            ,   SEQ
                            ,   LINE_NO
                            ,   CENTER_ID
                            ,   CENTER_NAME
                            ,   LOCATION_CD
                            ,   BOX_NO
                            ,   DIVISION_ID
                            ,   CATEGORY_ID1
                            ,   CATEGORY_NAME1
                            ,   CATEGORY_NAME2
                            ,   CATEGORY_NAME3
                            ,   CATEGORY_NAME4
                            ,   ITEM_SEASON_NAME
                            ,   ITEM_SKU_ID
                            ,   ITEM_ID
                            ,   ITEM_NAME
                            ,   ITEM_COLOR_ID
                            ,   ITEM_COLOR_NAME
                            ,   ITEM_SIZE_ID
                            ,   ITEM_SIZE_NAME
                            ,   NORMAL_SELLING_PRICE_EX_TAX
                            ,   JAN
                            ,   GRADE_ID
                            ,   GRADE_NAME
                            ,   STOCK_QTY
                            ,   INVOICE_NO
                            ,   ALLOC_QTY
                            ,   UN_ALLOC_QTY
                            ,   ITEM_CODE
                            ,   ITEM_SEASON_YEAR
                            ,   BRAND_ID
                            ,   ITEM_CODE_NAME
                            ,   BRAND_NAME
                            ,   PRICE
                        )
                        SELECT
                                SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   SYSTIMESTAMP
                            ,   :USER_ID
                            ,   :PROGRAM_NAME
                            ,   0
                            ,   TPS.SHIPPER_ID
                            ,   :SEQ
                            ,   ROWNUM
                            ,   TPS.CENTER_ID
                            ,   MW.CENTER_NAME1 || MW.CENTER_NAME2 CENTER_NAME
                            ,   TPS.LOCATION_CD
                            ,   TPS.BOX_NO
                            ,   MIS.DIVISION_ID
                            ,   MIS.CATEGORY_ID1
                            ,   MIC.CATEGORY_NAME1
                            ,   MIC.CATEGORY_NAME2
                            ,   MIC.CATEGORY_NAME3
                            ,   MIC.CATEGORY_NAME4
                            ,   MGEN.GEN_NAME
                            ,   TPS.ITEM_SKU_ID
                            ,   TPS.ITEM_ID
                            ,   MIS.ITEM_NAME
                            ,   TPS.ITEM_COLOR_ID
                            ,   MC.ITEM_COLOR_NAME
                            ,   TPS.ITEM_SIZE_ID
                            ,   MIS.ITEM_SIZE_NAME
                            ,   MIS.NORMAL_SELLING_PRICE_EX_TAX
                            ,   TPS.JAN
                            ,   TPS.GRADE_ID
                            ,   MG.GRADE_NAME
                            ,   TPS.STOCK_QTY
                            ,   TPS.INVOICE_NO
                            ,   TPS.ALLOC_QTY
                            ,   TPS.STOCK_QTY - TPS.ALLOC_QTY
                            ,   MIS.ITEM_CODE
                            ,   MIS.SEASON_YEAR
                            ,   MIS.BRAND_ID
                            ,   MI.ITEM_CODE_NAME
                            ,   MB.BRAND_NAME
                            ,   TPS.STOCK_QTY * MIS.NORMAL_SELLING_PRICE_EX_TAX
                        FROM
                                T_PACKAGE_STOCKS TPS ");
                    // 格納状態
                    if (condition.StorageStatus)
                    {
                        query.Append(@" 
                            INNER JOIN (
                                    SELECT
                                            TST.CENTER_ID
                                        ,   TST.ITEM_SKU_ID
                                    FROM
                                            T_STOCKS TST
                                    INNER JOIN
                                            M_LOCATIONS ML
                                    ON
                                            TST.SHIPPER_ID = ML.SHIPPER_ID
                                        AND TST.CENTER_ID = ML.CENTER_ID
                                        AND TST.LOCATION_CD = ML.LOCATION_CD
                                    WHERE
                                            TST.SHIPPER_ID  = :SHIPPER_ID
                                        AND TST.CENTER_ID   = :CENTER_ID
                                        AND TST.STOCK_QTY   > 0
                                        AND ML.LOCATION_CLASS IN('01','03')
                                    GROUP BY
                                            TST.CENTER_ID
                                        ,   ML.LOCATION_CLASS
                                        ,   TST.ITEM_SKU_ID
                                    HAVING
                                            COUNT(DISTINCT(TST.LOCATION_CD)) > 1
                            ) T
                            ON
                                    TPS.CENTER_ID = T.CENTER_ID
                                AND TPS.ITEM_SKU_ID = T.ITEM_SKU_ID ");
                    }
                    query.Append(@" 
                        LEFT JOIN
                                M_CENTERS MW
                        ON
                                TPS.SHIPPER_ID   = MW.SHIPPER_ID
                            AND TPS.CENTER_ID       = MW.CENTER_ID
                        LEFT JOIN
                                M_ITEM_SKU MIS
                        ON
                                TPS.SHIPPER_ID   = MIS.SHIPPER_ID
                            AND TPS.ITEM_SKU_ID  = MIS.ITEM_SKU_ID
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
                                TPS.SHIPPER_ID   = MC.SHIPPER_ID
                            AND TPS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                        LEFT JOIN
                                M_SIZES MS
                        ON
                                TPS.SHIPPER_ID   = MS.SHIPPER_ID
                            AND TPS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                        LEFT JOIN
                                M_GRADES MG
                        ON
                                TPS.SHIPPER_ID   = MG.SHIPPER_ID
                            AND TPS.GRADE_ID     = MG.GRADE_ID
                        LEFT JOIN
                                M_LOCATIONS ML
                        ON
                                TPS.SHIPPER_ID   = ML.SHIPPER_ID
                            AND TPS.CENTER_ID       = ML.CENTER_ID
                            AND TPS.LOCATION_CD  = ML.LOCATION_CD
                        LEFT JOIN
                                M_BRANDS MB
                        ON
                                MIS.SHIPPER_ID  = MB.SHIPPER_ID
                            AND MIS.BRAND_ID    = MB.BRAND_ID
                        LEFT JOIN
                                M_VENDORS MV
                        ON
                                MIS.SHIPPER_ID = MV.SHIPPER_ID
                            AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                        LEFT JOIN
                                M_GENERALS MGEN
                        ON
                                MIS.SHIPPER_ID = MGEN.SHIPPER_ID
                            AND MGEN.CENTER_ID = '@@@'
                            AND MGEN.REGISTER_DIVI_CD = '1'
                            AND MGEN.GEN_DIV_CD = 'SEASON_NAME'
                            AND MIS.ITEM_SEASON_ID = MGEN.GEN_CD
                        LEFT JOIN
                                M_ITEM_CODE MI
                        ON
                                MIS.SHIPPER_ID = MI.SHIPPER_ID
                            AND MIS.ITEM_CODE = MI.ITEM_CODE
                       WHERE
                                TPS.SHIPPER_ID   = :SHIPPER_ID
                            AND TPS.CENTER_ID || TPS.LOCATION_CD || TPS.ITEM_SKU_ID IN :KEYS
                    ");
                    // ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertStkStock02");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);

                    string[] selectedkeys =  keys.Select(x => x.CenterId + x.LocationCd + x.ItemSkuId).ToArray();
                    parameters.Add(":KEYS", selectedkeys);

                    // Add search condition
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.Append(" AND TPS.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionId))
                    {
                        query.Append(" AND MIS.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    // ロケーション区分
                    if (!string.IsNullOrEmpty(condition.LocationClass))
                    {
                        query.Append(" AND ML.LOCATION_CLASS = :LOCATION_CLASS ");
                        parameters.Add(":LOCATION_CLASS", condition.LocationClass);
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

                    // エリア
                    if (!string.IsNullOrEmpty(condition.Locsec1))
                    {
                        query.Append(" AND ML.LOCSEC_1 LIKE :LOCSEC_1 ");
                        parameters.Add(":LOCSEC_1", condition.Locsec1 + "%");
                    }

                    // 代表仕入先
                    if (!string.IsNullOrEmpty(condition.MainVendorId))
                    {
                        query.Append(" AND MIS.MAIN_VENDOR_ID LIKE :MAIN_VENDOR_ID ");
                        parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId + "%");
                    }

                    // 代表仕入先名
                    if (string.IsNullOrEmpty(condition.MainVendorId) && !string.IsNullOrEmpty(condition.MainVendorName))
                    {
                        query.Append(" AND MV.VENDOR_NAME1 LIKE :VENDOR_NAME1 ");
                        parameters.Add(":VENDOR_NAME1", condition.MainVendorName + "%");
                    }

                    // ロケーション
                    if (!string.IsNullOrEmpty(condition.LocationCdFrom) && string.IsNullOrEmpty(condition.LocationCdTo))
                    {
                        query.Append(" AND TPS.LOCATION_CD LIKE :LOCATION_CD ");
                        parameters.Add(":LOCATION_CD", condition.LocationCdFrom + "%");
                    }

                    if (!string.IsNullOrEmpty(condition.LocationCdFrom) && !string.IsNullOrEmpty(condition.LocationCdTo))
                    {
                        query.Append(@" AND TPS.LOCATION_CD >= :LOCATION_CD_FROM
                                     AND TPS.LOCATION_CD <= :LOCATION_CD_TO ");
                        parameters.Add(":LOCATION_CD_FROM", condition.LocationCdFrom);
                        parameters.Add(":LOCATION_CD_TO", condition.LocationCdTo);
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.Append(" AND MIS.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.Append(" AND MIS.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.Append(" AND MIS.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.Append(" AND MIS.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // アイテムコード
                    if (!string.IsNullOrEmpty(condition.ItemCode))
                    {
                        query.Append(" AND MIS.ITEM_CODE = :ITEM_CODE ");
                        parameters.Add(":ITEM_CODE", condition.ItemCode);
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.Append(" AND TPS.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(" AND TPS.BOX_NO = :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.Append(" AND TPS.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // SKU
                    if (!string.IsNullOrEmpty(condition.ItemSkuId))
                    {
                        query.Append(" AND TPS.ITEM_SKU_ID = :ITEM_SKU_ID ");
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId);
                    }

                    // カラー
                    if (!string.IsNullOrEmpty(condition.ItemColorId))
                    {
                        query.Append(" AND TPS.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID ");
                        parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId + "%");
                    }

                    // カラー名
                    if (string.IsNullOrEmpty(condition.ItemColorId) && !string.IsNullOrEmpty(condition.ItemColorName))
                    {
                        query.Append(" AND MC.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME ");
                        parameters.Add(":ITEM_COLOR_NAME", condition.ItemColorName + "%");
                    }

                    // サイズ
                    if (!string.IsNullOrEmpty(condition.ItemSizeId))
                    {
                        query.Append(" AND TPS.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID ");
                        parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId + "%");
                    }

                    // サイズ名
                    if (string.IsNullOrEmpty(condition.ItemSizeId) && !string.IsNullOrEmpty(condition.ItemSizeName))
                    {
                        query.Append(" AND MIS.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                        parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
                    }

                    // 格付
                    if (!string.IsNullOrEmpty(condition.GradeId))
                    {
                        query.Append(" AND TPS.GRADE_ID = :GRADE_ID ");
                        parameters.Add(":GRADE_ID", condition.GradeId);
                    }

                    // 在庫数 在庫数0を表示する　
                    if (!condition.StockQtyFlag)
                    {
                        query.Append(" AND TPS.STOCK_QTY <> 0 ");
                    }
                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertStkStock02");
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// Get Reference List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<ReferenceResultRow> GetData(ReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            // 在庫明細
            if (condition.ResultType == ResultTypes.Stock)
            {
                StringBuilder query = new StringBuilder(@"
                SELECT WS.*
                  FROM WW_STK_STOCKS01 WS
                  INNER JOIN
                       M_GRADES MG
                  ON
                       WS.SHIPPER_ID = MG.SHIPPER_ID
                   AND WS.GRADE_ID = MG.GRADE_ID
                 WHERE WS.SHIPPER_ID = :SHIPPER_ID
                   AND WS.SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<ReferenceResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.SortKey)
                {
                    case StockSortKey.ItemSku:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY WS.ITEM_SKU_ID DESC,MG.DISPLAY_ORDER DESC,WS.LOCATION_CD DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY WS.ITEM_SKU_ID ASC,MG.DISPLAY_ORDER ASC,WS.LOCATION_CD ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY WS.LOCATION_CD DESC,WS.ITEM_SKU_ID DESC,MG.DISPLAY_ORDER DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY WS.LOCATION_CD ASC,WS.ITEM_SKU_ID ASC,MG.DISPLAY_ORDER ASC ");
                                break;
                        }

                        break;
                }

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", condition.PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

                // Fill data to memory
                var References = MvcDbContext.Current.Database.Connection.Query<ReferenceResultRow>(query.ToString(), parameters);
                var stock01s = MvcDbContext.Current.StkStock01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.LocationSum = stock01s.Select(x => x.LocationCd).Distinct().Count();
                condition.ItemSkuSum = stock01s.Select(x => x.ItemSkuId).Distinct().Count();
                condition.StockQtySum = stock01s.Select(x => x.StockQty).Sum();
                condition.AllocQtySum = stock01s.Select(x => x.AllocQty).Sum();
                condition.SelectedCnt = MvcDbContext.Current.StkStock01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

                // Excute paging
                return new StaticPagedList<ReferenceResultRow>(References, condition.Page, condition.PageSize, totalCount);
            }

            // ケース明細
            else
            {
                StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_STK_STOCKS02 WS
                  INNER JOIN
                       M_GRADES MG
                  ON
                       WS.SHIPPER_ID = MG.SHIPPER_ID
                   AND WS.GRADE_ID = MG.GRADE_ID
                 WHERE WS.SHIPPER_ID = :SHIPPER_ID
                   AND WS.SEQ = :SEQ ");
                parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                parameters.Add(":SEQ", condition.Seq);

                // 全レコード数を取得
                int totalCount = MvcDbContext.Current.Database.Connection.Query<ReferenceResultRow>(query.ToString(), parameters).Count();

                // Sort function
                switch (condition.PackageSortKey)
                {
                    case PackageStockSortKey.LocationBox:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY WS.LOCATION_CD DESC,WS.BOX_NO DESC,WS.ITEM_SKU_ID DESC,MG.DISPLAY_ORDER DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY WS.LOCATION_CD ASC,WS.BOX_NO ASC,WS.ITEM_SKU_ID ASC,MG.DISPLAY_ORDER ASC ");
                                break;
                        }

                        break;

                    case PackageStockSortKey.BoxSku:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY WS.BOX_NO DESC,WS.ITEM_SKU_ID DESC,MG.DISPLAY_ORDER DESC,WS.LOCATION_CD DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY WS.BOX_NO ASC,WS.ITEM_SKU_ID ASC,MG.DISPLAY_ORDER ASC,WS.LOCATION_CD ASC ");
                                break;
                        }

                        break;

                    case PackageStockSortKey.SkuGrade:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY WS.ITEM_SKU_ID DESC,MG.DISPLAY_ORDER DESC,WS.LOCATION_CD DESC,WS.BOX_NO DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY WS.ITEM_SKU_ID ASC,MG.DISPLAY_ORDER ASC,WS.LOCATION_CD ASC,WS.BOX_NO ASC ");
                                break;
                        }

                        break;

                    default:
                        switch (condition.Sort)
                        {
                            case AscDescSort.Desc:
                                query.AppendLine(@"ORDER BY WS.LOCATION_CD DESC,WS.ITEM_SKU_ID DESC,MG.DISPLAY_ORDER DESC,WS.BOX_NO DESC ");
                                break;

                            default:
                                query.AppendLine(@"ORDER BY WS.LOCATION_CD ASC,WS.ITEM_SKU_ID ASC,MG.DISPLAY_ORDER ASC,WS.BOX_NO ASC ");
                                break;
                        }

                        break;
                }

                query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
                parameters.Add(":PAGE_SIZE", condition.PageSize);

                // Choose data corresponding on each page
                parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

                // Fill data to memory
                var References = MvcDbContext.Current.Database.Connection.Query<ReferenceResultRow>(query.ToString(), parameters);
                var stock02s = MvcDbContext.Current.StkStock02s.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
                condition.LocationSum = stock02s.Select(x => x.LocationCd).Distinct().Count();
                condition.ItemSkuSum = stock02s.Select(x => x.ItemSkuId).Distinct().Count();
                condition.StockQtySum = stock02s.Select(x => x.StockQty).Sum();

                // Excute paging
                return new StaticPagedList<ReferenceResultRow>(References, condition.Page, condition.PageSize, totalCount);
            }
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool UpdateStkStock(IList<SelectedReferenceViewModel> References)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in References)
                {
                    // 在庫明細
                    var stock01 = MvcDbContext.Current.StkStock01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (stock01 == null)
                    {
                        return false;
                    }

                    stock01.SetBaseInfoUpdate();
                    stock01.IsCheck = u.IsCheck;
                    try
                    {
                        MvcDbContext.Current.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                }

                trans.Commit();
            }

            return true;
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="References"></param>
        /// <returns></returns>
        public bool StkStockAllChange(ReferenceSearchConditions conditions, bool check)
        {
            var dbContext = MvcDbContext.Current.Database.Connection;

            using (System.Data.Common.DbTransaction tran = dbContext.BeginTransaction())
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    DynamicParameters parameters = new DynamicParameters();

                    // 更新用
                    sql.Append(@"
                        UPDATE
                                WW_STK_STOCKS01
                        SET
                                UPDATE_DATE = SYSTIMESTAMP
                            ,   UPDATE_USER_ID = :USER_ID
                            ,   UPDATE_PROGRAM_NAME = :PROGRAM_NAME
                            ,   UPDATE_COUNT = UPDATE_COUNT + 1
                            ,   IS_CHECK = :IS_CHECK
                        WHERE
                                SHIPPER_ID = :SHIPPER_ID
                            AND SEQ = :SEQ
                    ");

                    parameters.Add(":USER_ID", Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "StkStockAllChange");
                    parameters.Add(":IS_CHECK", check ? 1 : 0);
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":SEQ", conditions.Seq);

                    dbContext.Execute(sql.ToString(), parameters);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }

                tran.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.StkStock01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + ":" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// ロケーション区分データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListLocationClasses()
        {
            return MvcDbContext.Current.LocationClasses
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.LocationClass,
                    Text = m.LocationName
                }).Distinct()
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId1,
                    Text = m.CategoryId1.ToString() + ":" + m.CategoryName1
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類2データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys2(string categoryId1 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId2,
                    Text = m.CategoryId2.ToString() + ":" + m.CategoryName2
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類3データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys3(string categoryId1 = "", string categoryId2 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId3,
                    Text = m.CategoryId3.ToString() + ":" + m.CategoryName3
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類4データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys4(string categoryId1 = "", string categoryId2 = "", string categoryId3 = "")
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Profile.User.ShipperId
                && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1)
                && (categoryId2 == null ? 1 == 1 : m.CategoryId2 == categoryId2)
                && (categoryId3 == null ? 1 == 1 : m.CategoryId3 == categoryId3))
                .Select(m => new SelectListItem
                {
                    Value = m.CategoryId4,
                    Text = m.CategoryId4.ToString() + ":" + m.CategoryName4
                })
                .Distinct().OrderBy(m => m.Value);
        }

        /// <summary>
        /// アイテムデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Items
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.ItemCode,
                    Text = m.ItemCode + ":" + m.ItemCodeName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 格付データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListGrades()
        {
            try
            {
                return MvcDbContext.Current.Grades
                      .Where(m => m.ShipperId == Profile.User.ShipperId)
                      .OrderBy(m => m.DisplayOrder)
                      .Select(m => new SelectListItem
                      {
                          Value = m.GradeId,
                          Text = m.GradeName
                      });
            }
            catch (System.Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "GetSelectListGrades");
                throw ex;
            };


        }
    }
}