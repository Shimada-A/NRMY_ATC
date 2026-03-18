namespace Wms.Areas.Stock.Query.Adjust
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Master.Models;
    using Wms.Areas.Stock.Models;
    using Wms.Areas.Stock.ViewModels.Adjust;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using Wms.Resources;
    using static Wms.Areas.Stock.ViewModels.Adjust.AdjustSearchConditions;

    public class AdjustQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertStkAjust(AdjustSearchConditions condition)
        {
            // ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_STK_ADJUST01 (
                               MAKE_DATE
                              ,MAKE_USER_ID
                              ,MAKE_PROGRAM_NAME
                              ,UPDATE_DATE
                              ,UPDATE_USER_ID
                              ,UPDATE_PROGRAM_NAME
                              ,UPDATE_COUNT
                              ,SHIPPER_ID
                              ,SEQ
                              ,LINE_NO
                              ,CENTER_ID
                              ,CENTER_NAME
                              ,LOCATION_CD
                              ,CATEGORY_ID1
                              ,CATEGORY_NAME1
                              ,ITEM_SKU_ID
                              ,ITEM_ID
                              ,ITEM_NAME
                              ,ITEM_COLOR_ID
                              ,ITEM_COLOR_NAME
                              ,ITEM_SIZE_ID
                              ,ITEM_SIZE_NAME
                              ,JAN
                              ,GRADE_ID
                              ,GRADE_NAME
                              ,CASE_QTY
                              ,STOCK_QTY
                              ,ALLOC_QTY
                        )
                        SELECT
                               SYSTIMESTAMP
                              ,:USER_ID
                              ,:PROGRAM_NAME
                              ,SYSTIMESTAMP
                              ,:USER_ID
                              ,:PROGRAM_NAME
                              ,0
                              ,TS.SHIPPER_ID
                              ,:SEQ
                              ,ROWNUM
                              ,TS.CENTER_ID
                              ,MW.CENTER_NAME1 || MW.CENTER_NAME2 LOC_NAME
                              ,TS.LOCATION_CD
                              ,MIS.CATEGORY_ID1
                              ,MIC1.CATEGORY_NAME1
                              ,TS.ITEM_SKU_ID
                              ,TS.ITEM_ID
                              ,MIS.ITEM_NAME
                              ,TS.ITEM_COLOR_ID
                              ,MC.ITEM_COLOR_NAME
                              ,TS.ITEM_SIZE_ID
                              ,MIS.ITEM_SIZE_NAME
                              ,TS.JAN
                              ,TS.GRADE_ID
                              ,MG.GRADE_NAME
                              ,TPS.STOCK_QTY CASE_QTY
                              ,TS.STOCK_QTY
                              ,TS.ALLOC_QTY
                          FROM T_STOCKS TS ");
                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(@" 
                        INNER JOIN (SELECT SHIPPER_ID
                                           ,ITEM_SKU_ID
                                           ,LOCATION_CD
                                           ,CENTER_ID
                                       FROM T_PACKAGE_STOCKS
                                      WHERE BOX_NO IS NOT NULL
                                        AND BOX_NO LIKE :BOX_NO
                                      GROUP BY SHIPPER_ID
                                              ,ITEM_SKU_ID
                                              ,LOCATION_CD
                                              ,CENTER_ID) TPS1
                            ON TS.SHIPPER_ID = TPS1.SHIPPER_ID
                           AND TS.ITEM_SKU_ID = TPS1.ITEM_SKU_ID
                           AND TS.LOCATION_CD = TPS1.LOCATION_CD
                           AND TS.CENTER_ID = TPS1.CENTER_ID ");
                        parameters.Add(":BOX_NO", condition.BoxNo + "%");
                    }

                    query.Append(@"
                         LEFT JOIN M_CENTERS MW
                            ON TS.SHIPPER_ID   = MW.SHIPPER_ID
                           AND TS.CENTER_ID       = MW.CENTER_ID
                          LEFT JOIN M_ITEM_SKU MIS
                            ON TS.SHIPPER_ID   = MIS.SHIPPER_ID
                           AND TS.ITEM_SKU_ID  = MIS.ITEM_SKU_ID
                          LEFT JOIN (SELECT SHIPPER_ID
                                           ,CATEGORY_ID1
                                           ,MAX(CATEGORY_NAME1) CATEGORY_NAME1
                                       FROM M_ITEM_CATEGORIES4
                                      GROUP BY SHIPPER_ID,CATEGORY_ID1) MIC1
                            ON MIS.SHIPPER_ID   = MIC1.SHIPPER_ID
                           AND MIS.CATEGORY_ID1 = MIC1.CATEGORY_ID1
                          LEFT JOIN M_COLORS MC
                            ON TS.SHIPPER_ID   = MC.SHIPPER_ID
                           AND TS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                          LEFT JOIN M_SIZES MS
                            ON TS.SHIPPER_ID   = MS.SHIPPER_ID
                           AND TS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                          LEFT JOIN M_GRADES MG
                            ON TS.SHIPPER_ID   = MG.SHIPPER_ID
                           AND TS.GRADE_ID     = MG.GRADE_ID
                          LEFT JOIN (SELECT SHIPPER_ID
                                           ,ITEM_SKU_ID
                                           ,LOCATION_CD
                                           ,CENTER_ID
                                           ,COUNT(DISTINCT(BOX_NO)) STOCK_QTY
                                       FROM T_PACKAGE_STOCKS
                                      WHERE BOX_NO <> ' ' ");

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo))
                    {
                        query.Append(" AND BOX_NO LIKE :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo + "%");
                    }

                    query.Append(@"
                          GROUP BY SHIPPER_ID,ITEM_SKU_ID,LOCATION_CD,CENTER_ID) TPS
                            ON TS.SHIPPER_ID   = TPS.SHIPPER_ID
                           AND TS.ITEM_SKU_ID  = TPS.ITEM_SKU_ID
                           AND TS.LOCATION_CD  = TPS.LOCATION_CD
                           AND TS.CENTER_ID       = TPS.CENTER_ID
                          LEFT JOIN M_LOCATIONS ML
                            ON TS.SHIPPER_ID   = ML.SHIPPER_ID
                           AND TS.CENTER_ID       = ML.CENTER_ID
                           AND TS.LOCATION_CD  = ML.LOCATION_CD
                           AND ML.INVENTORY_CONFIRM_FLAG IN (0, 2, 3)
                          LEFT JOIN M_BRANDS MB
                            ON MIS.SHIPPER_ID  = MB.SHIPPER_ID
                           AND MIS.BRAND_ID    = MB.BRAND_ID
                          LEFT JOIN M_VENDORS MV
                            ON MIS.SHIPPER_ID = MV.SHIPPER_ID
                           AND MIS.MAIN_VENDOR_ID = MV.VENDOR_ID
                         WHERE TS.SHIPPER_ID   = :SHIPPER_ID
                           AND TS.CENTER_ID = :CENTER_ID
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertStkAjust");
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
                        query.Append(" AND MIS.BRAND_ID = :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId);
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
                        query.Append(" AND MIS.MAIN_VENDOR_ID = :MAIN_VENDOR_ID ");
                        parameters.Add(":MAIN_VENDOR_ID", condition.MainVendorId);
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
                        query.Append(" AND TS.LOCATION_CD >= :LOCATION_CD ");
                        parameters.Add(":LOCATION_CD", condition.LocationCdFrom);
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

                    // サイズ名
                    if (!string.IsNullOrEmpty(condition.ItemSizeName))
                    {
                        query.Append(" AND MIS.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                        parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
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
                        parameters.Add(":ITEM_SKU_ID", condition.ItemSkuId + "%");
                    }

                    // カラー
                    if (!string.IsNullOrEmpty(condition.ItemColorId))
                    {
                        query.Append(" AND TS.ITEM_COLOR_ID = :ITEM_COLOR_ID ");
                        parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId);
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
                        query.Append(" AND TS.ITEM_SIZE_ID = :ITEM_SIZE_ID ");
                        parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId);
                    }

                    //// サイズ名
                    //if (string.IsNullOrEmpty(condition.ItemSizeId) && !string.IsNullOrEmpty(condition.ItemSizeName))
                    //{
                    //    query.Append(" AND MS.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                    //    parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
                    //}

                    // 格付
                    if (!string.IsNullOrEmpty(condition.GradeId))
                    {
                        query.Append(" AND TS.GRADE_ID = :GRADE_ID ");
                        parameters.Add(":GRADE_ID", condition.GradeId);
                    }

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// Get Adjust List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<AdjustResultRow> GetData(AdjustSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT WW.*
                     , ML.LOCATION_CLASS
                  FROM WW_STK_ADJUST01 WW
                LEFT OUTER JOIN
                       M_LOCATIONS ML
                    ON ML.LOCATION_CD = WW.LOCATION_CD
                   AND ML.CENTER_ID = WW.CENTER_ID
                   AND ML.SHIPPER_ID = WW.SHIPPER_ID
                LEFT OUTER JOIN
                       M_GRADES MG
                    ON MG.GRADE_ID = WW.GRADE_ID
                   AND MG.SHIPPER_ID = WW.SHIPPER_ID
                 WHERE WW.SHIPPER_ID = :SHIPPER_ID
                   AND WW.SEQ = :SEQ ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<AdjustResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case AdjustSortKey.SkuGradeLocation:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.ITEM_SKU_ID DESC, MG.DISPLAY_ORDER DESC, WW.LOCATION_CD DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.ITEM_SKU_ID ASC, MG.DISPLAY_ORDER ASC, WW.LOCATION_CD ASC ");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WW.LOCATION_CD DESC, WW.ITEM_SKU_ID DESC, MG.DISPLAY_ORDER DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WW.LOCATION_CD ASC, WW.ITEM_SKU_ID ASC, MG.DISPLAY_ORDER ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var Adjusts = MvcDbContext.Current.Database.Connection.Query<AdjustResultRow>(query.ToString(), parameters);
            var stkAdjust01s = MvcDbContext.Current.StkAdjust01s.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId);
            condition.LocationSum = stkAdjust01s.Select(x => x.LocationCd).Distinct().Count();
            condition.ItemSkuSum = stkAdjust01s.Select(x => x.ItemSkuId).Distinct().Count();
            condition.StockQtySum = stkAdjust01s.Select(x => x.StockQty).Sum();
            condition.AllocQtySum = stkAdjust01s.Select(x => x.AllocQty).Sum();

            // Excute paging
            return new StaticPagedList<AdjustResultRow>(Adjusts, condition.Page, condition.PageSize, totalCount);
        }
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertStkAdjust02(long seq02, long seq, long lineNo)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_STK_ADJUST02 (
                           MAKE_DATE
                          ,MAKE_USER_ID
                          ,MAKE_PROGRAM_NAME
                          ,UPDATE_DATE
                          ,UPDATE_USER_ID
                          ,UPDATE_PROGRAM_NAME
                          ,UPDATE_COUNT
                          ,SHIPPER_ID
                          ,SEQ
                          ,LINE_NO
                          ,CENTER_ID
                          ,ITEM_SKU_ID
                          ,JAN
                          ,ITEM_ID
                          ,ITEM_NAME
                          ,ITEM_COLOR_ID
                          ,ITEM_COLOR_NAME
                          ,ITEM_SIZE_ID
                          ,ITEM_SIZE_NAME
                          ,LOCATION_CD
                          ,LOCATION_CLASS
                          ,GRADE_ID
                          ,GRADE_NAME
                          ,BEFORE_STOCK_QTY
                          ,ALLOC_QTY
                          ,CASE_CLASS
                          ,CASE_CLASS_NAME
                          ,BOX_NO
                          ,STOCK_QTY
                          ,TPS_UPDATE_DATE
                          ,TPS_UPDATE_USER_ID
                          ,TPS_UPDATE_PROGRAM_NAME
                          ,TPS_UPDATE_COUNT
                          ,INVOICE_NO
                          ,TPS_ALLOC_QTY
                    )
                    SELECT
                           SYSTIMESTAMP
                          ,:USER_ID
                          ,:PROGRAM_NAME
                          ,SYSTIMESTAMP
                          ,:USER_ID
                          ,:PROGRAM_NAME
                          ,0
                          ,TPS.SHIPPER_ID
                          ,:SEQ02
                          ,ROWNUM
                          ,TPS.CENTER_ID
                          ,TPS.ITEM_SKU_ID
                          ,TPS.JAN
                          ,TPS.ITEM_ID
                          ,MIS.ITEM_NAME
                          ,TPS.ITEM_COLOR_ID
                          ,MC.ITEM_COLOR_NAME
                          ,TPS.ITEM_SIZE_ID
                          ,MIS.ITEM_SIZE_NAME
                          ,TPS.LOCATION_CD
                          ,ML.LOCATION_CLASS
                          ,TPS.GRADE_ID
                          ,MGR.GRADE_NAME
                          ,WSA.STOCK_QTY BEFORE_STOCK_QTY
                          ,WSA.ALLOC_QTY
                          ,ML.CASE_CLASS
                          ,MG.GEN_NAME CASE_CLASS_NAME
                          ,TPS.BOX_NO
                          ,TPS.STOCK_QTY
                          ,TPS.UPDATE_DATE TPS_UPDATE_DATE
                          ,TPS.UPDATE_USER_ID TPS_UPDATE_USER_ID
                          ,TPS.UPDATE_PROGRAM_NAME TPS_UPDATE_PROGRAM_NAME
                          ,TPS.UPDATE_COUNT TPS_UPDATE_COUNT
                          ,TPS.INVOICE_NO
                          ,CASE WHEN ML.LOCATION_CLASS = '02'
                                THEN TPS.ALLOC_QTY
                                ELSE NULL
                           END TPS_ALLOC_QTY
                      FROM WW_STK_ADJUST01 WSA 
                      INNER JOIN T_PACKAGE_STOCKS TPS
                        ON WSA.SHIPPER_ID   = TPS.SHIPPER_ID
                       AND WSA.CENTER_ID    = TPS.CENTER_ID
                       AND WSA.LOCATION_CD  = TPS.LOCATION_CD
                       AND WSA.ITEM_SKU_ID  = TPS.ITEM_SKU_ID
                      LEFT JOIN M_LOCATIONS ML
                        ON WSA.SHIPPER_ID   = ML.SHIPPER_ID
                       AND WSA.CENTER_ID    = ML.CENTER_ID
                       AND WSA.LOCATION_CD  = ML.LOCATION_CD
                      LEFT JOIN M_GENERALS MG
                        ON ML.SHIPPER_ID = MG.SHIPPER_ID
                       AND MG.REGISTER_DIVI_CD = '1'
                       AND MG.CENTER_ID = '@@@'
                       AND MG.GEN_DIV_CD = 'CASE_CLASS'
                       AND (CASE WHEN ML.CASE_CLASS <> 9 THEN ML.CASE_CLASS
                                 ELSE (CASE WHEN TRIM(TPS.BOX_NO) IS NULL THEN 2
                                            ELSE 1 END)
                            END) = MG.GEN_CD
                      LEFT JOIN M_ITEM_SKU MIS
                        ON TPS.SHIPPER_ID   = MIS.SHIPPER_ID
                       AND TPS.ITEM_SKU_ID  = MIS.ITEM_SKU_ID
                      LEFT JOIN M_COLORS MC
                        ON TPS.SHIPPER_ID   = MC.SHIPPER_ID
                       AND TPS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                      LEFT JOIN M_GRADES MGR
                        ON TPS.SHIPPER_ID   = MGR.SHIPPER_ID
                       AND TPS.GRADE_ID     = MGR.GRADE_ID
                     WHERE WSA.SHIPPER_ID   = :SHIPPER_ID
                       AND WSA.SEQ = :SEQ
                       AND WSA.LINE_NO = :LINE_NO
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertStkAdjust02");
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":SEQ", seq);
                    parameters.Add(":SEQ02", seq02);
                    parameters.Add(":LINE_NO", lineNo);

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }
            return true;
        }

        /// <summary>
        /// Get Adjust List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public AdjustInputViewModel AdjustInputGetData(long seq)
        {
            var stkAdjust02 = MvcDbContext.Current.StkAdjust02s.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == seq).FirstOrDefault();
            AdjustInputViewModel vm = new AdjustInputViewModel();
            if (stkAdjust02 != null)
            {
                vm.Seq = seq;
                vm.CenterId = stkAdjust02.CenterId;
                vm.ItemId = stkAdjust02.ItemId;
                vm.ItemName = stkAdjust02.ItemName;
                vm.ItemColorId = stkAdjust02.ItemColorId;
                vm.ItemColorName = stkAdjust02.ItemColorName;
                vm.ItemSizeId = stkAdjust02.ItemSizeId;
                vm.ItemSizeName = stkAdjust02.ItemSizeName;
                vm.LocationCd = stkAdjust02.LocationCd;
                vm.LocationClass = stkAdjust02.LocationClass;
                vm.Jan = stkAdjust02.Jan;
                vm.GradeId = stkAdjust02.GradeId;
                vm.GradeName = stkAdjust02.GradeName;
                vm.BeforeStockQty = stkAdjust02.BeforeStockQty;
                vm.AllocQty = stkAdjust02.AllocQty;
                vm.AllocQty = stkAdjust02.AllocQty;
            }
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_STK_ADJUST02
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ 
                 ORDER BY BOX_NO ASC");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", seq);

            // Fill data to memory
            vm.AdjustInputs = MvcDbContext.Current.Database.Connection.Query<StkAdjust02>(query.ToString(), parameters).ToList();
            vm.StockQtySum = vm.AdjustInputs.Select(x => x.StockQty).Sum();
            vm.AdjustQtyToSum = vm.AdjustInputs.Select(x => x.AdjustQtyTo).Sum();

            // Excute paging
            return vm;
        }

        /// <summary>
        /// 排他チェック
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool ExclusiveCheck(AdjustInputViewModel vm)
        {
            var stkAdjust02s = MvcDbContext.Current.StkAdjust02s.Where(x => x.ShipperId == Common.Profile.User.ShipperId && x.Seq == vm.Seq).ToList();
            foreach (var cModel in stkAdjust02s.Select((v, i) => new { v, i }))
            {
                if (vm.AdjustInputs.Where(x => x.Seq == cModel.v.Seq && x.LineNo == cModel.v.LineNo && x.AdjustQtyTo != cModel.v.StockQty).Count() == 1)
                {
                    if (!MvcDbContext.Current.PackageStocks.Where(x => x.ShipperId == cModel.v.ShipperId
                                                                 && x.CenterId == cModel.v.CenterId
                                                                 && x.LocationCd == cModel.v.LocationCd
                                                                 && x.ItemSkuId == cModel.v.ItemSkuId
                                                                 && x.BoxNo == cModel.v.BoxNo
                                                                 && x.InvoiceNo == cModel.v.InvoiceNo
                                                                 && x.UpdateDate == cModel.v.TpsUpdateDate
                                                                 && x.UpdateProgramName == cModel.v.TpsUpdateProgramName
                                                                 && x.UpdateUserId == cModel.v.TpsUpdateUserId
                                                                 && x.UpdateCount == cModel.v.TpsUpdateCount).Any())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 「在庫調整ワーク」更新処理
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool UpdateStkAdjust02(AdjustInputViewModel vm)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var cModel in vm.AdjustInputs.Select((v, i) => new { v, i }))
                {
                    var stkAdjust02 =
                  MvcDbContext.Current.StkAdjust02s
                  .Where(m => m.ShipperId == cModel.v.ShipperId && m.Seq == cModel.v.Seq && m.LineNo == cModel.v.LineNo)
                  .SingleOrDefault();
                    if (stkAdjust02 == null)
                    {
                        return false;
                    }

                    stkAdjust02.SetBaseInfoUpdate();
                    stkAdjust02.AdjustQtyTo = cModel.v.AdjustQtyTo;

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
        /// 「在庫・荷姿別在庫」更新処理
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public string UpdateStock(AdjustInputViewModel vm)
        {
            var message = string.Empty;
            var param = new DynamicParameters();
            // トランザクション開始
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                //// 調査ロケ取得処理
                //var locations = MvcDbContext.Current.Locations.Where(x => x.ShipperId == Common.Profile.User.ShipperId
                //                                                     && x.LocationClass == "16");
                //// 「在庫調整ワーク02」より更新対象データ取得
                //var stkAdjust02s = MvcDbContext.Current.StkAdjust02s.Where(x=>x.Seq==vm.Seq && x.StockQty != x.AdjustQtyTo).ToList();
                //foreach (var cModel in stkAdjust02s.Select((v, i) => new { v, i }))
                //{
                // 「在庫、荷姿別在庫」更新処理起動
                param = new DynamicParameters();
                param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                param.Add("IN_CENTER_ID", vm.CenterId, DbType.String, ParameterDirection.Input);
                param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                param.Add("IN_SLIP_NO", vm.SlipNo, DbType.String, ParameterDirection.Input);
                param.Add("IN_ITEM_SKU_ID", "", DbType.String, ParameterDirection.Input);
                param.Add("IN_LOCATION_CD", vm.LocationCd, DbType.String, ParameterDirection.Input);
                param.Add("IN_RESULT_QTY", vm.AdjustQtyToSum, DbType.Int32, ParameterDirection.Input);
                param.Add("IN_WORK_ID", vm.Seq, DbType.Int32, ParameterDirection.Input);
                param.Add("IN_ADJUST_REASON_CD",string.IsNullOrEmpty( vm.AdjustReasonCd) ? 0 : Convert.ToInt32(vm.AdjustReasonCd) , DbType.Int32, ParameterDirection.Input);
                param.Add("IN_NOTE", vm.Note, DbType.String, ParameterDirection.Input);
                param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                var db = MvcDbContext.Current.Database;
                db.Connection.Execute(
                    "pk_stk_adjustment.SP_H_STK_T_STOCK_RSC_CHK_UPD",
                    param,
                    commandType: CommandType.StoredProcedure);

                if (param.Get<int>("OUT_STATUS") != (int)ProcedureStatus.Success)
                {
                    return param.Get<string>("OUT_MESSAGE");
                }

                //    //「在庫、荷姿別在庫」在庫調整ロケ更新処理起動
                //    param = new DynamicParameters();
                //    param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                //    param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                //    param.Add("IN_CENTER_ID", Profile.User.CenterId, DbType.String, ParameterDirection.Input);
                //    param.Add("IN_ITEM_SKU_ID", cModel.v.ItemSkuId, DbType.String, ParameterDirection.Input);
                //    param.Add("IN_LOCATION_CD", cModel.v.LocationCd, DbType.String, ParameterDirection.Input);
                //    param.Add("IN_BOX_NO", cModel.v.BoxNo, DbType.String, ParameterDirection.Input);
                //    param.Add("IN_INVOICE_NO", cModel.v.InvoiceNo, DbType.String, ParameterDirection.Input);
                //    param.Add("IN_ADJUST_QTY_TO", cModel.v.AdjustQtyTo, DbType.Int32, ParameterDirection.Input);
                //    param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                //    param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                //    var db1 = MvcDbContext.Current.Database;
                //    db1.Connection.Execute(
                //        "SP_H_STK_T_STOCKS_UPD",
                //        param,
                //        commandType: CommandType.StoredProcedure);

                //    if (param.Get<int>("OUT_STATUS") == (int)ProcedureStatus.Error)
                //    {
                //        return MessageResource.ERR_STOCK_UPDATE;
                //    }

                //}

                //// 「在庫訂正」登録処理

                //// 「在庫調査結果明細」登録処理
                //param = new DynamicParameters();
                //param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                //param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                //param.Add("IN_CENTER_ID", Profile.User.CenterId, DbType.String, ParameterDirection.Input);
                //param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                //param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                //var db2 = MvcDbContext.Current.Database;
                //db2.Connection.Execute(
                //    "SP_H_STK_T_STOCKS_UPD",
                //    param,
                //    commandType: CommandType.StoredProcedure);

                //if (param.Get<int>("OUT_STATUS") == 1)
                //{
                //    return param.Get<string>("OUT_MESSAGE");
                //}
                //else if (param.Get<int>("OUT_STATUS") != 1 && param.Get<int>("OUT_STATUS") != 0)
                //{
                //    return MessageResource.ERR_STOCK_RESEARCH_UPDATE;
                //}


                trans.Commit();
            }

            return message;
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
        /// 格付データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListGrades()
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

        /// <summary>
        /// アイテムコードデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Items
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .OrderBy(m => m.ItemCode)
                .Select(m => new SelectListItem
                {
                    Value = m.ItemCode,
                    Text = m.ItemCode + ":" + m.ItemCodeName
                });
        }

        /// <summary>
        /// 在庫作成処理
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public string CreateStock(AdjustCreateViewModel vm)
        {
            // トランザクション開始
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                // 在庫作成処理
                var param = new DynamicParameters();
                param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                param.Add("IN_CENTER_ID", vm.CenterId, DbType.String, ParameterDirection.Input);
                param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                param.Add("IN_LOCATION_CD", vm.LocationCd, DbType.String, ParameterDirection.Input);
                param.Add("IN_BOX_NO", vm.BoxNo ?? " ", DbType.String, ParameterDirection.Input);
                param.Add("IN_ITEM_SKU_ID", vm.ItemSkuId, DbType.String, ParameterDirection.Input);
                param.Add("IN_JAN", vm.Jan, DbType.String, ParameterDirection.Input);
                param.Add("IN_QTY", vm.StockQty, DbType.Int32, ParameterDirection.Input);
                param.Add("IN_ADJUST_REASON_CD", vm.AdjustReasonCd, DbType.Int32, ParameterDirection.Input);
                param.Add("IN_NOTE", vm.Note, DbType.String, ParameterDirection.Input);
                param.Add("IN_SLIP_NO", vm.SlipNo, DbType.String, ParameterDirection.Input);
                param.Add("IN_INVOICE_NO", vm.InvoiceNo, DbType.String, ParameterDirection.Input);
                param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                var db = MvcDbContext.Current.Database;
                db.Connection.Execute(
                    "PK_STK_ADJUSTMENT.SP_H_STK_CREATE_STOCKS",
                    param,
                    commandType: CommandType.StoredProcedure);

                if (param.Get<int>("OUT_STATUS") != (int)ProcedureStatus.Success)
                {
                    return param.Get<string>("OUT_MESSAGE");
                }

                trans.Commit();
            }

            return string.Empty;
        }

        /// <summary>
        /// ロケーションマスタを取得する
        /// </summary>
        /// <param name="centerId">センターID</param>
        /// <param name="locationCd">ロケーションコード</param>
        /// <returns></returns>
        public Location GetLocation(string centerId, string locationCd)
        {
            return MvcDbContext.Current.Locations
                .Where(x => x.LocationCd == locationCd && x.CenterId == centerId && x.ShipperId == Profile.User.ShipperId)
                .SingleOrDefault();
        }

        /// <summary>
        /// ケースNoが登録済みのロケーションコードを取得する
        /// </summary>
        /// <param name="centerId">センターID</param>
        /// <param name="boxNo">ケースNo</param>
        /// <returns></returns>
        public string GetLocationCdBoxNo(string centerId, string boxNo)
        {
            return MvcDbContext.Current.PackageStocks
                        .Where(x => x.BoxNo == boxNo && x.CenterId == centerId && x.ShipperId == Profile.User.ShipperId)
                        .FirstOrDefault()?.LocationCd;
        }
    }
}