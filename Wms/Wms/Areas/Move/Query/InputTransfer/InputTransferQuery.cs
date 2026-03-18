namespace Wms.Areas.Move.Query.InputTransfer
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
    using Wms.Areas.Move.Models;
    using Wms.Areas.Move.Resources;
    using Wms.Areas.Move.ViewModels.InputTransfer;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Move.ViewModels.InputTransfer.InputTransfer01SearchConditions;
    using static Wms.Areas.Move.ViewModels.InputTransfer.InputTransfer02SearchConditions;

    public class InputTransferQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertMovInputTransfer01(InputTransfer01SearchConditions condition)
        {
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_ARR_TRANS_INPUT01 (
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
                        ,   TRANSFER_CLASS
                        ,   ARRIVE_PLAN_DATE
                        ,   TRANS_FROM_STORE_CLASS
                        ,   TRANSFER_FROM_STORE_ID
                        ,   TRANSFER_FROM_STORE_NAME
                        ,   SLIP_NO
                        ,   SLIP_SEQ
                        ,   ITEM_ID
                        ,   ITEM_COLOR_ID
                        ,   ITEM_SIZE_ID
                        ,   JAN
                        ,   PLAN_SLIP_NO
                        ,   RESULT_SLIP_NO
                        ,   ITEM_SKU_ID
                        ,   ARRIVE_PLAN_QTY
                        ,   RESULT_QTY
                        ,   STATUS
                        ,   CONFIRM_DATE
                        ,   IF_STATE
                        ,   UNPLANNED_FLAG
                        ,   TRANSFER_RESULT_DATE
                        ,   SLIP_DATE
                        ,   BOX_NO
                        ,   BRAND_ID
                        ,   BRAND_SHORT_NAME
                    )
                    WITH
                        TARGET_SLIP_NO AS ( --検索条件の入荷予定日または入荷実績日に該当する移動伝票番号を絞り込む
                            SELECT
                                    T.SHIPPER_ID
                                ,   T.CENTER_ID
                                ,   T.SLIP_NO
                                ,   NVL(MAX(T.ARRIVE_PLAN_DATE),MAX(T.TRANSFER_RESULT_DATE)) AS ARRIVE_PLAN_DATE
                                ,   MAX(T.TRANSFER_CLASS) AS TRANSFER_CLASS
                                ,   MAX(T.SHIP_DATE) AS SHIP_DATE
                                ,   MAX(T.TRANSFER_FROM_STORE_ID) AS TRANSFER_FROM_STORE_ID
                                ,   CASE
                                        WHEN MAX(T.STATUS) = 0 THEN 1 --1:未入荷
                                        WHEN MAX(T.STATUS) > 0 AND MIN(T.STATUS) = 0 THEN 2 --2:一部検品済
                                        WHEN MIN(T.STATUS) > 0 AND MIN(T.IF_STATE) = 2 THEN 4 --4:実績送信済
                                        ELSE 3 --3:検品済
                                    END AS TRANSFER_STATUS
                            FROM
                                    V_TRANSFER T
                            WHERE
                                    T.SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertMovInputTransfer01");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.AppendLine("            AND T.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    //移動区分
                    if (condition.StoreReturnFlag || condition.BaseMoveFlag || condition.BaseMoveNoWmsCenterFlag)
                    {
                        string transferclassQuery = "";
                        string transferclassTmp = "";
                        if (condition.StoreReturnFlag)
                        {
                            transferclassQuery = "2";
                            transferclassTmp = ",";
                        }
                        if (condition.BaseMoveFlag)
                        {
                            transferclassQuery = transferclassQuery + transferclassTmp + "1";
                            transferclassTmp = ",";
                        }
                        if (condition.BaseMoveNoWmsCenterFlag)
                        {
                            transferclassQuery = transferclassQuery + transferclassTmp + "3";
                        }

                        query.AppendLine(@"                        AND T.TRANSFER_CLASS IN ( " + transferclassQuery + " )");
                    }

                    // 移動元店舗
                    // 移動元センター
                    if (!string.IsNullOrEmpty(condition.TransferFromStoreId) || !string.IsNullOrEmpty(condition.TransferFromCenterId))
                    {
                        string transferFromStoreIdQuery = "";
                        string transferFromStoreIdTmp = "";
                        if (!string.IsNullOrEmpty(condition.TransferFromStoreId))
                        {
                            string[] shipToIds = (condition.TransferFromStoreId ?? condition.TransferFromStoreId).Split(',');
                            transferFromStoreIdQuery = " T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_STORE_ID ";
                            parameters.Add(":TRANSFER_FROM_STORE_ID", shipToIds);
                            transferFromStoreIdTmp = "OR";
                        }
                        if (!string.IsNullOrEmpty(condition.TransferFromCenterId) && condition.BaseMoveFlag)
                        {
                            transferFromStoreIdQuery = transferFromStoreIdQuery + transferFromStoreIdTmp + " T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_CENTER_ID ";
                            parameters.Add(":TRANSFER_FROM_CENTER_ID", condition.TransferFromCenterId);
                        }
                        query.AppendLine(@"                        AND(" + transferFromStoreIdQuery + ")");
                    }

                    // 入荷予定日または入荷実績日(検索・From-To)
                    if (condition.ArriveDateClass == ArriveDateClasses.ArrivePlanDate)
                    {
                        if (condition.DenpyoDateFrom != null)
                        {
                            query.AppendLine(@"                        AND T.SHIP_DATE >= :DENPYO_DATE_FROM ");
                            parameters.Add(":DENPYO_DATE_FROM", condition.DenpyoDateFrom);
                        }
                        if (condition.DenpyoDateTo != null)
                        {
                            query.AppendLine(@"                        AND T.SHIP_DATE <= :DENPYO_DATE_TO ");
                            parameters.Add(":DENPYO_DATE_TO", condition.DenpyoDateTo);
                        }
                    }
                    else if (condition.ArriveDateClass == ArriveDateClasses.TransferResultDate)
                    {
                        if (condition.TransferResultDateFrom != null)
                        {
                            query.AppendLine("                        AND T.TRANSFER_RESULT_DATE >= :TRANSFER_RESULT_DATE_FROM ");
                            parameters.Add(":TRANSFER_RESULT_DATE_FROM", condition.TransferResultDateFrom);
                        }
                        if (condition.TransferResultDateTo != null)
                        {
                            query.AppendLine("                        AND T.TRANSFER_RESULT_DATE <= :TRANSFER_RESULT_DATE_TO ");
                            parameters.Add(":TRANSFER_RESULT_DATE_TO", condition.TransferResultDateTo);
                        }
                    }

                    query.Append(@"
                            GROUP BY
                                    T.SHIPPER_ID
                                ,   T.CENTER_ID
                                ,   T.SLIP_NO
                    )
                    SELECT
                            SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   SYSTIMESTAMP
                        ,   :USER_ID
                        ,   :PROGRAM_NAME
                        ,   0
                        ,   T_SLIP.SHIPPER_ID
                        ,   :SEQ
                        ,   ROWNUM
                        ,   T_SLIP.CENTER_ID
                        ,   T_SLIP.TRANSFER_CLASS
                        ,   T_SLIP.ARRIVE_PLAN_DATE
                        ,   T.TRANS_FROM_STORE_CLASS
                        ,   T_SLIP.TRANSFER_FROM_STORE_ID
                        ,   VSTS.SHIP_TO_STORE_NAME1 AS TRANSFER_FROM_STORE_NAME
                        ,   T_SLIP.SLIP_NO
                        ,   T.SLIP_SEQ
                        ,   T.ITEM_ID
                        ,   T.ITEM_COLOR_ID
                        ,   T.ITEM_SIZE_ID
                        ,   T.JAN
                        ,   T.PLAN_SLIP_NO
                        ,   T.RESULT_SLIP_NO
                        ,   T.ITEM_SKU_ID
                        ,   T.ARRIVE_PLAN_QTY
                        ,   T.RESULT_QTY
                        ,   T.STATUS
                        ,   T.CONFIRM_DATE
                        ,   T.IF_STATE
                        ,   T.UNPLANNED_FLAG
                        ,   T.TRANSFER_RESULT_DATE
                        ,   T_SLIP.SHIP_DATE
                        ,   T.BOX_NO
                        ,   T.BRAND_ID
                        ,   T.BRAND_SHORT_NAME
                    FROM
                            V_TRANSFER T
                    INNER JOIN
                            TARGET_SLIP_NO T_SLIP
                    ON
                            T_SLIP.SHIPPER_ID = T.SHIPPER_ID
                        AND T_SLIP.CENTER_ID = T.CENTER_ID
                        AND T_SLIP.SLIP_NO = T.SLIP_NO
                    LEFT OUTER JOIN
                            V_SHIP_TO_STORES VSTS
                    ON
                            VSTS.SHIPPER_ID = T_SLIP.SHIPPER_ID
                        AND VSTS.SHIP_TO_STORE_ID = T_SLIP.TRANSFER_FROM_STORE_ID
                        AND VSTS.DELETE_FLAG = 0
                    WHERE
                            1 = 1
                    ");

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionId))
                    {
                        query.AppendLine("            AND T.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    // ブランド
                    if (!string.IsNullOrEmpty(condition.BrandId))
                    {
                        query.AppendLine("                        AND T.BRAND_ID LIKE :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId + "%");
                    }

                    // ブランド名
                    if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                    {
                        query.AppendLine("                        AND T.BRAND_NAME LIKE :BRAND_NAME ");
                        parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.AppendLine("                        AND T.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.AppendLine("                        AND T.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.AppendLine("                        AND T.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.AppendLine("                        AND T.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.AppendLine("                        AND T.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // カラー
                    if (!string.IsNullOrEmpty(condition.ItemColorId))
                    {
                        query.AppendLine("                        AND T.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID ");
                        parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId + "%");
                    }

                    // カラー名
                    if (string.IsNullOrEmpty(condition.ItemColorId) && !string.IsNullOrEmpty(condition.ItemColorName))
                    {
                        query.AppendLine("                        AND T.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME ");
                        parameters.Add(":ITEM_COLOR_NAME", condition.ItemColorName + "%");
                    }

                    // アイテム
                    if (!string.IsNullOrEmpty(condition.ItemCode))
                    {
                        query.AppendLine(@"                        AND T.ITEM_CODE = :ITEM_CODE ");
                        parameters.Add(":ITEM_CODE", condition.ItemCode);
                    }

                    // サイズ
                    if (!string.IsNullOrEmpty(condition.ItemSizeId))
                    {
                        query.AppendLine("                        AND T.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID ");
                        parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId + "%");
                    }

                    // サイズ名
                    if (string.IsNullOrEmpty(condition.ItemSizeId) && !string.IsNullOrEmpty(condition.ItemSizeName))
                    {
                        query.AppendLine("                        AND T.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                        parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.AppendLine("                        AND T.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // 伝票番号
                    if (!string.IsNullOrEmpty(condition.DenpyoNo))
                    {
                        query.AppendLine(@"                         AND T.SLIP_NO LIKE :DENPYO_NO ");
                        parameters.Add(":DENPYO_NO", condition.DenpyoNo + "%");
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.SlipNo))
                    {
                        query.AppendLine(@"                        AND T.BOX_NO LIKE :SLIP_NO ");
                        parameters.Add(":SLIP_NO", condition.SlipNo + "%");
                    }

                    // 状況
                    if (!string.IsNullOrEmpty(condition.TransferStatus))
                    {
                        query.AppendLine(@"                        AND TO_CHAR(T_SLIP.TRANSFER_STATUS) = :TRANSFER_STATUS ");
                        parameters.Add(":TRANSFER_STATUS", condition.TransferStatus);
                    }
                    else
                    {
                        query.AppendLine(@"                        AND T_SLIP.TRANSFER_STATUS <> 4 ");
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
        /// Get InputTransfer List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<InputTransfer01ResultRow> InputTransfer01GetData(InputTransfer01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            var orderBySql = "";

            // Sort function
            switch (condition.SortKey)
            {
                case InputTransfer01Sort.DenpyoDateTransferClassStoreId:
                    switch (condition.Sort)
                    {
                        case InputTransfer01SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY @SLIP_DATE DESC, @TRANSFER_CLASS DESC, @TRANSFER_FROM_STORE_ID DESC, @SLIP_NO DESC, @BRAND_ID DESC, @BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY @SLIP_DATE ASC, @TRANSFER_CLASS ASC, @TRANSFER_FROM_STORE_ID ASC, @SLIP_NO ASC, @BRAND_ID ASC, @BOX_NO ASC ";
                            break;
                    }

                    break;
                case InputTransfer01Sort.StoreIdDenpyoDate:
                    switch (condition.Sort)
                    {
                        case InputTransfer01SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY @TRANSFER_FROM_STORE_ID DESC, @SLIP_DATE DESC, @SLIP_NO DESC, @BRAND_ID DESC, @BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY @TRANSFER_FROM_STORE_ID ASC, @SLIP_DATE ASC, @SLIP_NO ASC, @BRAND_ID ASC, @BOX_NO ASC ";
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case InputTransfer01SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY @TRANSFER_CLASS DESC, @SLIP_DATE DESC, @TRANSFER_FROM_STORE_ID DESC, @SLIP_NO DESC, @BRAND_ID DESC, @BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY @TRANSFER_CLASS ASC, @SLIP_DATE ASC, @TRANSFER_FROM_STORE_ID ASC, @SLIP_NO ASC, @BRAND_ID ASC, @BOX_NO ASC ";
                            break;
                    }

                    break;
            }

            StringBuilder query = new StringBuilder($@"
            WITH
                WORK_DATA AS (
                    SELECT
                            *
                    FROM
                            WW_ARR_TRANS_INPUT01 WW
                    WHERE
                            WW.SHIPPER_ID = :SHIPPER_ID
                        AND WW.SEQ = :SEQ
                )
                ,SKU_WORK_DATA AS (
                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   ITEM_SKU_ID
                        ,   MAX(ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY
                        ,   MAX(BRAND_ID) AS BRAND_ID
                    FROM
                            WORK_DATA
                    GROUP BY
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   ITEM_SKU_ID
                )
                ,BRAND_WORK_DATA AS (
                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   BRAND_ID
                        ,   SUM(ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY
                    FROM
                            SKU_WORK_DATA
                    GROUP BY
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   BRAND_ID
                )
                ,WORK_TBL AS (
                    SELECT
                            LISTAGG(WW.LINE_NO,',') AS LINE_NO
                        ,   MAX(WW.SEQ) AS SEQ
                        ,   WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   CASE MAX(TRANSFER_CLASS)
                                WHEN 1 THEN MAX(WW.TRANSFER_FROM_STORE_ID)
                                WHEN 2 THEN NULL
                                WHEN 3 THEN NULL
                            END AS TRANSFER_FROM_CENTER_ID
                        ,   MAX(WW.TRANSFER_FROM_STORE_ID) AS TRANSFER_FROM_STORE_ID
                        ,   MAX(WW.TRANSFER_FROM_STORE_NAME) AS TRANSFER_FROM_STORE_NAME
                        ,   NVL(COUNT(DISTINCT(WW.PLAN_SLIP_NO)),0) AS SLIP_NO_PLAN_QTY
                        ,   COUNT(DISTINCT(WW.RESULT_SLIP_NO)) AS SLIP_NO_RESULT_QTY
                        ,   COUNT(DISTINCT(WW.ITEM_SKU_ID)) AS ITEM_SKU_QTY
                        ,   NVL(SUM(WW.ARRIVE_PLAN_QTY),0) AS ARRIVE_PLAN_QTY
                        ,   SUM(WW.RESULT_QTY) AS RESULT_QTY
                        ,   CASE
                                WHEN SUM(WW.STATUS) = 0 THEN 1 --1:未入荷
                                WHEN MAX(WW.STATUS) > 0 AND MIN(WW.STATUS) = 0 THEN 2 --2:一部検品済
                                WHEN MIN(WW.STATUS) > 0 AND MIN(WW.IF_STATE) = 2 THEN 4 --4:実績送信済
                                ELSE 3 --3:検品済
                            END AS TRANSFER_STATUS
                        ,   MAX(WW.CONFIRM_DATE) AS CONFIRM_DATE
                        ,   NVL(MIN(WW.UNPLANNED_FLAG),0) AS UNPLANNED_FLAG
                        ,   MAX(WW.TRANSFER_RESULT_DATE) AS TRANSFER_RESULT_DATE
                        ,   MAX(WW.TRANSFER_CLASS) AS TRANSFER_CLASS
                        ,   WW.SLIP_NO
                        ,   MAX(WW.SLIP_DATE) AS SLIP_DATE
                        ,   WW.BOX_NO
                        ,   WW.BRAND_ID
                        ,   MAX(WW.BRAND_SHORT_NAME) AS BRAND_SHORT_NAME
                        ,   MAX(WW.SELECTED_FLAG) AS IS_CHECK
                        ,   MAX(WW.UPDATE_COUNT) AS UPDATE_COUNT
                    FROM
                            WORK_DATA WW
                    GROUP BY
                            WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   WW.SLIP_NO
                        ,   WW.BRAND_ID
                        ,   WW.BOX_NO
                )
                ,INTER_TABLE AS (
                    SELECT
                            WORK_TBL.*
                        ,   TRANSFER_STATUS.GEN_NAME AS TRANSFER_STATUS_NAME 
                        ,   BRAND_WORK_DATA.ARRIVE_PLAN_QTY AS BRAND_ARRIVE_PLAN_QTY
                        ,   ROW_NUMBER() OVER (PARTITION BY WORK_TBL.SLIP_NO, WORK_TBL.BRAND_ID {orderBySql.Replace("@", "WORK_TBL.")}) AS BRAND_ROW_NUMBER
                    FROM
                            WORK_TBL
                    INNER JOIN
                            BRAND_WORK_DATA
                    ON
                            BRAND_WORK_DATA.SHIPPER_ID = WORK_TBL.SHIPPER_ID
                        AND BRAND_WORK_DATA.CENTER_ID = WORK_TBL.CENTER_ID
                        AND BRAND_WORK_DATA.SLIP_NO = WORK_TBL.SLIP_NO
                        AND BRAND_WORK_DATA.BRAND_ID = WORK_TBL.BRAND_ID
                    LEFT OUTER JOIN
                            M_GENERALS TRANSFER_STATUS
                    ON
                            TRANSFER_STATUS.SHIPPER_ID = WORK_TBL.SHIPPER_ID
                        AND TRANSFER_STATUS.CENTER_ID = '@@@'
                        AND TRANSFER_STATUS.REGISTER_DIVI_CD = '1'
                        AND TRANSFER_STATUS.GEN_DIV_CD = 'TRANSFER_STATUS'
                        AND TRANSFER_STATUS.GEN_CD = TO_CHAR(WORK_TBL.TRANSFER_STATUS)
                )
            SELECT
                    LINE_NO
                ,   SEQ
                ,   SHIPPER_ID
                ,   CENTER_ID
                ,   TRANSFER_FROM_CENTER_ID
                ,   TRANSFER_FROM_STORE_ID
                ,   TRANSFER_FROM_STORE_NAME
                ,   SLIP_NO_PLAN_QTY
                ,   SLIP_NO_RESULT_QTY
                ,   ITEM_SKU_QTY
                ,   CASE WHEN BRAND_ROW_NUMBER = 1 THEN BRAND_ARRIVE_PLAN_QTY ELSE NULL END AS ARRIVE_PLAN_QTY
                ,   RESULT_QTY
                ,   TRANSFER_STATUS
                ,   CONFIRM_DATE
                ,   UNPLANNED_FLAG
                ,   TRANSFER_RESULT_DATE
                ,   TRANSFER_CLASS
                ,   SLIP_NO
                ,   SLIP_DATE
                ,   BOX_NO
                ,   BRAND_ID
                ,   BRAND_SHORT_NAME
                ,   IS_CHECK
                ,   UPDATE_COUNT
                ,   TRANSFER_STATUS_NAME 
            FROM
                    INTER_TABLE
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            var allRecords = MvcDbContext.Current.Database.Connection.Query<InputTransfer01ResultRow>(query.ToString(), parameters);

            query.AppendLine(orderBySql.Replace("@",""));
            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var InputTransfer01s = MvcDbContext.Current.Database.Connection.Query<InputTransfer01ResultRow>(query.ToString(), parameters);

            condition.ArrivePlanQtySum = allRecords.Sum(x => x.ArrivePlanQty) ?? 0;
            condition.ResultQtySum = allRecords.Sum(x => x.ResultQty) ?? 0;
            condition.SelectedCnt = InputTransfer01s.Where(x => x.IsCheck).Count();

            var movTransInput01s = GetDetailData01(condition);
            condition.ItemSkuQtySum = movTransInput01s.ItemSkuQtySum;
            condition.SlipNoPlanQtySum = movTransInput01s.SlipNoPlanQtySum;
            condition.SlipNoResultQtySum = movTransInput01s.SlipNoResultQtySum;

            // Excute paging
            return new StaticPagedList<InputTransfer01ResultRow>(InputTransfer01s, condition.Page, condition.PageSize, allRecords.Count());
        }

        /// <summary>
        /// 明細ヘッダ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public InputTransfer01SearchConditions GetDetailData01(InputTransfer01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            WITH
                WORK_DATA AS (
                    SELECT
                            *
                    FROM
                            WW_ARR_TRANS_INPUT01 WW
                    WHERE
                            WW.SHIPPER_ID = :SHIPPER_ID
                        AND WW.SEQ = :SEQ
                )
            SELECT
                    COUNT(DISTINCT(WW.ITEM_SKU_ID)) AS ITEM_SKU_QTY_SUM
                ,   COUNT(DISTINCT(WW.PLAN_SLIP_NO)) AS SLIP_NO_PLAN_QTY_SUM
                ,   COUNT(DISTINCT(WW.RESULT_SLIP_NO)) AS SLIP_NO_RESULT_QTY_SUM
            FROM
                    WORK_DATA WW
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            return MvcDbContext.Current.Database.Connection.Query<InputTransfer01SearchConditions>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrInputTransfer02(InputTransfer02SearchConditions condition)
        {
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_ARR_TRANS_INPUT02 (
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
                        ,   TRANSFER_CLASS
                        ,   ARRIVE_PLAN_DATE
                        ,   TRANS_FROM_STORE_CLASS
                        ,   TRANSFER_FROM_STORE_ID
                        ,   TRANSFER_FROM_STORE_NAME
                        ,   SLIP_NO
                        ,   SLIP_SEQ
                        ,   CATEGORY_ID1
                        ,   CATEGORY_NAME1
                        ,   ITEM_ID
                        ,   ITEM_NAME
                        ,   ITEM_COLOR_ID
                        ,   ITEM_COLOR_NAME
                        ,   ITEM_SIZE_ID
                        ,   ITEM_SIZE_NAME
                        ,   JAN
                        ,   ITEM_SKU_ID
                        ,   ARRIVE_PLAN_QTY
                        ,   RESULT_QTY
                        ,   CONFIRM_DATE
                        ,   TTR_UPDATE_COUNT
                        ,   UNPLANNED_FLAG
                        ,   INPUT_RESULT_QTY
                        ,   TRANSFER_STATUS
                        ,   TRANSFER_RESULT_DATE
                        ,   SLIP_DATE
                        ,   BOX_NO
                        ,   TRANSFER_STATUS_NAME
                    )
                    WITH
                        TARGET_SLIP_NO AS ( --検索条件の入荷予定日または入荷実績日に該当する移動伝票番号を絞り込む
                            SELECT
                                    T.SHIPPER_ID
                                ,   T.CENTER_ID
                                ,   T.SLIP_NO
                                ,   NVL(MAX(T.ARRIVE_PLAN_DATE),MAX(T.TRANSFER_RESULT_DATE)) AS ARRIVE_PLAN_DATE
                                ,   MAX(T.TRANS_FROM_STORE_CLASS) AS TRANS_FROM_STORE_CLASS
                                ,   MAX(T.TRANSFER_FROM_STORE_ID) AS TRANSFER_FROM_STORE_ID
                            FROM
                                    V_TRANSFER T
                            WHERE
                                    T.SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertArrInputTransfer02");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.AppendLine("            AND T.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    //移動区分
                    if (condition.StoreReturnFlag || condition.BaseMoveFlag || condition.BaseMoveNoWmsCenterFlag)
                    {
                        string transferclassQuery = "";
                        string tmp = "";
                        if (condition.StoreReturnFlag)
                        {
                            transferclassQuery = "2";
                            tmp = ",";
                        }
                        if (condition.BaseMoveFlag)
                        {
                            transferclassQuery = transferclassQuery + tmp + "1";
                            tmp = ",";
                        }
                        if (condition.BaseMoveNoWmsCenterFlag)
                        {
                            transferclassQuery = transferclassQuery + tmp + "3";
                        }

                        query.AppendLine(@"                        AND T.TRANSFER_CLASS IN ( " + transferclassQuery + " )");
                    }

                    // 移動元店舗
                    // 移動元センター
                    if (!string.IsNullOrEmpty(condition.TransferFromStoreId) || !string.IsNullOrEmpty(condition.TransferFromCenterId))
                    {
                        string transferFromStoreIdQuery = "";
                        string transferFromStoreIdTmp = "";
                        if (!string.IsNullOrEmpty(condition.TransferFromStoreId))
                        {
                            string[] shipToIds = (condition.TransferFromStoreId ?? condition.TransferFromStoreId).Split(',');
                            transferFromStoreIdQuery = " T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_STORE_ID ";
                            parameters.Add(":TRANSFER_FROM_STORE_ID", shipToIds);
                            transferFromStoreIdTmp = "OR";
                        }
                        if (!string.IsNullOrEmpty(condition.TransferFromCenterId) && condition.BaseMoveFlag)
                        {
                            transferFromStoreIdQuery = transferFromStoreIdQuery + transferFromStoreIdTmp + " T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_CENTER_ID ";
                            parameters.Add(":TRANSFER_FROM_CENTER_ID", condition.TransferFromCenterId);
                        }
                        query.AppendLine(@"                        AND(" + transferFromStoreIdQuery + ")");
                    }

                    // 入荷予定日または入荷実績日(検索・From-To)
                    if (condition.ArriveDateClass == ArriveDateClasses.ArrivePlanDate)
                    {
                        if (condition.DenpyoDateFrom != null)
                        {
                            query.AppendLine(@"                        AND T.SHIP_DATE >= :DENPYO_DATE_FROM ");
                            parameters.Add(":DENPYO_DATE_FROM", condition.DenpyoDateFrom);
                        }
                        if (condition.DenpyoDateTo != null)
                        {
                            query.AppendLine(@"                        AND T.SHIP_DATE <= :DENPYO_DATE_TO ");
                            parameters.Add(":DENPYO_DATE_TO", condition.DenpyoDateTo);
                        }
                    }
                    else if (condition.ArriveDateClass == ArriveDateClasses.TransferResultDate)
                    {
                        if (condition.TransferResultDateFrom != null)
                        {
                            query.AppendLine("                        AND T.TRANSFER_RESULT_DATE >= :TRANSFER_RESULT_DATE_FROM ");
                            parameters.Add(":TRANSFER_RESULT_DATE_FROM", condition.TransferResultDateFrom);
                        }
                        if (condition.TransferResultDateTo != null)
                        {
                            query.AppendLine("                        AND T.TRANSFER_RESULT_DATE <= :TRANSFER_RESULT_DATE_TO ");
                            parameters.Add(":TRANSFER_RESULT_DATE_TO", condition.TransferResultDateTo);
                        }
                    }

                    query.Append(@"
                            GROUP BY
                                    T.SHIPPER_ID
                                ,   T.CENTER_ID
                                ,   T.SLIP_NO
                    )
                    ,   TRANSFER AS (
                            SELECT
                                    SYSTIMESTAMP AS MAKE_DATE
                                ,   :USER_ID AS MAKE_USER_ID
                                ,   :PROGRAM_NAME AS MAKE_PROGRAM_NAME
                                ,   SYSTIMESTAMP AS UPDATE_DATE
                                ,   :USER_ID AS UPDATE_USER_ID
                                ,   :PROGRAM_NAME UPDATE_PROGRAM_NAME
                                ,   0 AS UPDATE_COUNT
                                ,   T_SLIP.SHIPPER_ID
                                ,   :SEQ AS SEQ
                                ,   ROWNUM AS LINE_NO
                                ,   T_SLIP.CENTER_ID
                                ,   T.TRANSFER_CLASS
                                ,   T_SLIP.ARRIVE_PLAN_DATE
                                ,   T.TRANS_FROM_STORE_CLASS
                                ,   T_SLIP.TRANSFER_FROM_STORE_ID
                                ,   VSTS.SHIP_TO_STORE_NAME1 AS TRANSFER_FROM_STORE_NAME
                                ,   T.SLIP_NO
                                ,   T.SLIP_SEQ
                                ,   T.CATEGORY_ID1
                                ,   T.CATEGORY_NAME1
                                ,   T.ITEM_ID
                                ,   T.ITEM_NAME
                                ,   T.ITEM_COLOR_ID
                                ,   T.ITEM_COLOR_NAME
                                ,   T.ITEM_SIZE_ID
                                ,   T.ITEM_SIZE_NAME
                                ,   T.JAN
                                ,   T.ITEM_SKU_ID
                                ,   NVL(T.ARRIVE_PLAN_QTY,0) AS ARRIVE_PLAN_QTY
                                ,   T.RESULT_QTY
                                ,   T.CONFIRM_DATE
                                ,   NVL(T.RESULT_UPDATE_COUNT,-1) AS RESULT_UPDATE_COUNT
                                ,   NVL(T.UNPLANNED_FLAG,0) AS UNPLANNED_FLAG
                                ,   NVL(T.RESULT_QTY,NVL(T.ARRIVE_PLAN_QTY,0)) AS INPUT_RESULT_QTY
                                ,   CASE
                                        WHEN T.STATUS = 0 THEN 1 --1:未入荷
                                        WHEN T.STATUS > 0 AND T.IF_STATE = 2 THEN 4 --4:実績送信済
                                        ELSE 3 --3:検品済
                                    END TRANSFER_STATUS
                                ,   T.TRANSFER_RESULT_DATE
                                ,   T.SHIP_DATE
                                ,   T.BOX_NO
                            FROM
                                    V_TRANSFER T
                            INNER JOIN
                                    TARGET_SLIP_NO T_SLIP
                            ON
                                    T_SLIP.SHIPPER_ID = T.SHIPPER_ID
                                AND T_SLIP.CENTER_ID = T.CENTER_ID
                                AND T_SLIP.SLIP_NO = T.SLIP_NO
                            LEFT OUTER JOIN
                                    V_SHIP_TO_STORES VSTS
                            ON
                                    VSTS.SHIPPER_ID = T.SHIPPER_ID
                                AND VSTS.SHIP_TO_STORE_ID = T.TRANSFER_FROM_STORE_ID
                                AND VSTS.DELETE_FLAG = 0
                            WHERE
                                    1 = 1
                    ");

                    // 事業部
                    if (!string.IsNullOrEmpty(condition.DivisionId))
                    {
                        query.AppendLine("            AND T.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    // ブランド
                    if (!string.IsNullOrEmpty(condition.BrandId))
                    {
                        query.AppendLine("            AND T.BRAND_ID LIKE :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId + "%");
                    }

                    // ブランド名
                    if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                    {
                        query.AppendLine("            AND T.BRAND_NAME LIKE :BRAND_NAME ");
                        parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.AppendLine("            AND T.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.Append("            AND T.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.AppendLine("            AND T.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.AppendLine("            AND T.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.AppendLine("            AND T.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // カラー
                    if (!string.IsNullOrEmpty(condition.ItemColorId))
                    {
                        query.AppendLine("            AND T.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID ");
                        parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId + "%");
                    }

                    // カラー名
                    if (string.IsNullOrEmpty(condition.ItemColorId) && !string.IsNullOrEmpty(condition.ItemColorName))
                    {
                        query.AppendLine("            AND T.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME ");
                        parameters.Add(":ITEM_COLOR_NAME", condition.ItemColorName + "%");
                    }

                    // アイテム
                    if (!string.IsNullOrEmpty(condition.ItemCode))
                    {
                        query.AppendLine(@"                        AND T.ITEM_CODE = :ITEM_CODE ");
                        parameters.Add(":ITEM_CODE", condition.ItemCode);
                    }

                    // サイズ
                    if (!string.IsNullOrEmpty(condition.ItemSizeId))
                    {
                        query.AppendLine("            AND T.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID ");
                        parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId + "%");
                    }

                    // サイズ名
                    if (string.IsNullOrEmpty(condition.ItemSizeId) && !string.IsNullOrEmpty(condition.ItemSizeName))
                    {
                        query.AppendLine("            AND T.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                        parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.AppendLine("            AND T.JAN LIKE :JAN ");
                        parameters.Add(":JAN", condition.Jan + "%");
                    }

                    // 伝票番号
                    if (!string.IsNullOrEmpty(condition.DenpyoNo))
                    {
                        query.AppendLine(@"                         AND T.SLIP_NO LIKE :DENPYO_NO ");
                        parameters.Add(":DENPYO_NO", condition.DenpyoNo + "%");
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.SlipNo))
                    {
                        query.AppendLine(@"                        AND T.BOX_NO LIKE :SLIP_NO ");
                        parameters.Add(":SLIP_NO", condition.SlipNo + "%");
                    }

                    query.Append(@"
                    )
                    SELECT
                            TRANSFER.*
                        ,   TRANSFER_STATUS.GEN_NAME TRANSFER_STATUS_NAME
                    FROM
                            TRANSFER
                    LEFT OUTER JOIN
                            M_GENERALS TRANSFER_STATUS
                    ON
                            TRANSFER_STATUS.SHIPPER_ID = TRANSFER.SHIPPER_ID
                        AND TRANSFER_STATUS.CENTER_ID = '@@@'
                        AND TRANSFER_STATUS.REGISTER_DIVI_CD = '1'
                        AND TRANSFER_STATUS.GEN_DIV_CD = 'TRANSFER_STATUS'
                        AND TRANSFER_STATUS.GEN_CD = TO_CHAR(TRANSFER.TRANSFER_STATUS)
                    ");

                    // Add search condition
                    // 状況
                    if (!string.IsNullOrEmpty(condition.TransferStatus))
                    {
                        query.AppendLine(@"
                            WHERE
                                TO_CHAR(TRANSFER.TRANSFER_STATUS) = :TRANSFER_STATUS ");
                        parameters.Add(":TRANSFER_STATUS", condition.TransferStatus);
                    }
                    else
                    {
                        query.AppendLine(@"
                            WHERE
                                TRANSFER.TRANSFER_STATUS <> 4 ");
                    }

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (Exception ex) //デバッグ用
                {
                    Mvc.Common.AppError.PutLogREF(ex, "InsertArrInputTransfer02");
                    trans.Rollback();
                    return false;
                }
                trans.Commit();
            }
            return true;
        }


        /// <summary>
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<InputTransfer02ResultRow> InputTransfer02GetData(InputTransfer02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            var orderBySql = "";

            // Sort function
            switch (condition.SortKey)
            {
                case InputTransfer02Sort.SkuBoxNo:
                    switch (condition.Sort)
                    {
                        case InputTransfer02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY ITEM_SKU_ID DESC, SLIP_NO DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY ITEM_SKU_ID ASC, SLIP_NO ASC, BOX_NO ASC ";
                            break;
                    }

                    break;

                case InputTransfer02Sort.StoreIdSlipNoBoxNoSku:
                    switch (condition.Sort)
                    {
                        case InputTransfer02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY TRANSFER_FROM_STORE_ID DESC, SLIP_NO DESC, ITEM_SKU_ID DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY TRANSFER_FROM_STORE_ID ASC, SLIP_NO ASC, ITEM_SKU_ID ASC, BOX_NO ASC ";
                            break;
                    }
                    break;

                default:
                    switch (condition.Sort)
                    {
                        case InputTransfer02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY TRANSFER_CLASS DESC, SLIP_NO DESC, ITEM_SKU_ID DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY TRANSFER_CLASS ASC, SLIP_NO ASC, ITEM_SKU_ID ASC, BOX_NO ASC ";
                            break;
                    }

                    break;
            }

            StringBuilder query = new StringBuilder($@"
                WITH
                    WORK_DATA AS (
                        SELECT
                                WW.*    
                            ,   ROW_NUMBER() OVER (PARTITION BY WW.SLIP_NO, WW.ITEM_SKU_ID {orderBySql}) AS SKU_ROW_NUMBER
                        FROM
                                WW_ARR_TRANS_INPUT02 WW
                        WHERE
                                WW.SHIPPER_ID = :SHIPPER_ID
                            AND WW.SEQ = :SEQ
                    )
                SELECT
                        SHIPPER_ID
                    ,   SEQ
                    ,   LINE_NO
                    ,   CENTER_ID
                    ,   TRANSFER_CLASS
                    ,   ARRIVE_PLAN_DATE
                    ,   TRANS_FROM_STORE_CLASS
                    ,   TRANSFER_FROM_STORE_ID
                    ,   TRANSFER_FROM_STORE_NAME
                    ,   SLIP_NO
                    ,   SLIP_SEQ
                    ,   CATEGORY_ID1
                    ,   CATEGORY_NAME1
                    ,   ITEM_ID
                    ,   ITEM_NAME
                    ,   ITEM_COLOR_ID
                    ,   ITEM_COLOR_NAME
                    ,   ITEM_SIZE_ID
                    ,   ITEM_SIZE_NAME
                    ,   JAN
                    ,   ITEM_SKU_ID
                    ,   CASE WHEN SKU_ROW_NUMBER = 1 THEN ARRIVE_PLAN_QTY ELSE NULL END AS ARRIVE_PLAN_QTY
                    ,   RESULT_QTY
                    ,   CONFIRM_DATE
                    ,   TTR_UPDATE_COUNT
                    ,   UNPLANNED_FLAG
                    ,   INPUT_RESULT_QTY
                    ,   TRANSFER_STATUS
                    ,   TRANSFER_RESULT_DATE
                    ,   SLIP_DATE
                    ,   BOX_NO
                    ,   TRANSFER_STATUS_NAME
                FROM
                        WORK_DATA
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            var allRecords = MvcDbContext.Current.Database.Connection.Query<InputTransfer02ResultRow>(query.ToString(), parameters);

            query.AppendLine(orderBySql);
            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var InputTransfer02s = MvcDbContext.Current.Database.Connection.Query<InputTransfer02ResultRow>(query.ToString(), parameters);

            condition.ItemSkuQtySum = allRecords.Select(x => x.ItemSkuId).Distinct().Count();
            condition.ArrivePlanQtySum = allRecords.Select(x => x.ArrivePlanQty).Sum() ?? 0;
            condition.ResultQtySum = allRecords.Select(x => x.ResultQty).Sum() ?? 0;

            // Excute paging
            return new StaticPagedList<InputTransfer02ResultRow>(InputTransfer02s, condition.Page, condition.PageSize, allRecords.Count());
        }

        /// <summary>
        /// ワーク01更新
        /// </summary>
        /// <param name="SelectedInputTransfer01ViewModel"></param>
        /// <returns></returns>
        public bool UpdateArrInputTransfer01(IList<SelectedInputTransfer01ViewModel> InputTransfer01s)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var u in InputTransfer01s)
                {
                    string[] lineNoList = u.LineNo.Split(',');

                    foreach (var l in lineNoList)
                    {
                        long lineNo = long.Parse(l);

                        var InputTransfer01 =
                       dbContext.MovTransInput01s
                       .Where(m => m.ShipperId == u.ShipperId && m.Seq == u.Seq && m.LineNo == lineNo && m.UpdateCount == u.UpdateCount)
                       .SingleOrDefault();
                        if (InputTransfer01 == null)
                        {
                            return false;
                        }

                        InputTransfer01.SetBaseInfoUpdate();
                        InputTransfer01.SelectedFlag = u.IsCheck;
                    }
                }
                //更新
                try
                {
                    //save data to DB
                    dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }

                trans.Commit();
            }
            //return
            return true;
        }

        /// <summary>
        /// ワーク02更新
        /// </summary>
        /// <param name="InputTransfer02s">List record is deleted</param>
        /// <returns>List of rows error </returns>
        public bool UpdateArrInputTransfer02(IList<InputTransfer02ResultRow> InputTransfer02s)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                foreach (var u in InputTransfer02s)
                {
                    var InputTransfer02 =
                   dbContext.MovTransInput02s
                   .Where(m => m.ShipperId == u.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo && m.UpdateCount == u.UpdateCount)
                   .SingleOrDefault();
                    if (InputTransfer02 == null)
                    {
                        return false;
                    }

                    InputTransfer02.SetBaseInfoUpdate();
                    InputTransfer02.InputResultQty = u.InputResultQty;

                    //更新
                    try
                    {
                        //save data to DB
                        dbContext.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        return false;
                    }
                }

                trans.Commit();
            }
            //return
            return true;
        }

        /// <summary>
        /// 実績数入力チェック
        /// </summary>
        /// <param name="InputTransfer02s">List record is deleted</param>
        /// <returns>未入力の入力実績数の件数</returns>
        public int CheckInputResultQty(long seq)
        {
            return MvcDbContext.Current.MovTransInput02s.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == seq && m.InputResultQty == null).Count();
        }

        /// <summary>
        /// 移動入荷検品ワーク作業チェック01
        /// </summary>
        /// <param name="seq"></param>
        /// <returns>データ件数</returns>
        public int CheckWorkingExists01(long seq)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT
                        *
                FROM
                        W_HHT_TRANSFER W
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND WORK_COMPLETE_CLASS = 1 --作業中
                    AND EXISTS (
                            SELECT
                                    'X'
                            FROM
                                    WW_ARR_TRANS_INPUT01 WW
                            WHERE
                                    WW.SHIPPER_ID = W.SHIPPER_ID
                                AND WW.CENTER_ID = W.CENTER_ID
                                AND WW.SLIP_NO = W.SLIP_NO
                                AND WW.SEQ = :SEQ
                        )
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<InputTransfer01ResultRow>(query.ToString(), parameters).Count();

            return totalCount;
        }

        /// <summary>
        /// 移動入荷検品ワーク作業チェック02
        /// </summary>
        /// <param name="seq"></param>
        /// <returns>データ件数</returns>
        public int CheckWorkingExists02(long seq)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                SELECT
                        *
                FROM
                        W_HHT_TRANSFER W
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND WORK_COMPLETE_CLASS = 1 --作業中
                    AND EXISTS (
                            SELECT
                                    'X'
                            FROM
                                    WW_ARR_TRANS_INPUT02 WW
                            WHERE
                                    WW.CENTER_ID = W.CENTER_ID
                                AND WW.SLIP_NO = W.SLIP_NO
                                AND WW.SEQ = :SEQ
                        )
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<InputTransfer02ResultRow>(query.ToString(), parameters).Count();

            return totalCount;
        }

        /// <summary>
        /// 実績登録
        /// </summary>
        public string CreateInputTransfer01(long seq, string centerId)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var param = new DynamicParameters();
                param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
                param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                param.Add("IN_PROGRAM_NAME", "SP_W_MOV_INPUTTRANSFER01", DbType.String, ParameterDirection.Input);
                param.Add("IN_SEQ", seq, DbType.Int32, ParameterDirection.Input);
                param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                var db = MvcDbContext.Current.Database;

                db.Connection.Execute(
                    "SP_W_MOV_INPUTTRANSFER01",
                    param,
                    commandType: CommandType.StoredProcedure);

                if (param.Get<int>("OUT_STATUS") == (int)ProcedureStatus.Success)
                {
                    trans.Commit();
                    return null;
                }
                else
                {
                    trans.Rollback();
                    return param.Get<string>("OUT_MESSAGE");
                }
            }
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public Dictionary<string, string> UpdateInputTransfer02(long seq, string centerId)
        {
            var dbContext = MvcDbContext.Current;
            using (var trans = dbContext.Database.BeginTransaction())
            {
                var param = new DynamicParameters();
                param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
                param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
                param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
                param.Add("IN_SEQ", seq, DbType.Int32, ParameterDirection.Input);
                param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);
                param.Add("OUT_PRINT_NO", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

                var db = MvcDbContext.Current.Database;

                db.Connection.Execute(
                    "PK_W_MOV_INPUTTRANSFER02.UPDATE_RESULT",
                    param,
                    commandType: CommandType.StoredProcedure);

                Dictionary<string, string> outList = new Dictionary<string, string>();
                outList.Add("OUT_STATUS", param.Get<int>("OUT_STATUS").ToString());
                outList.Add("OUT_MESSAGE", param.Get<string>("OUT_MESSAGE"));
                outList.Add("OUT_PRINT_NO", param.Get<string>("OUT_PRINT_NO"));

                if (int.Parse(outList["OUT_STATUS"]) == (int)ProcedureStatus.Success)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }

                return outList;
            }
        }

        /// <summary>
        /// ワーク01 移動入荷実績.更新回数、入荷実績日 更新
        /// </summary>
        /// <param name="InputTransfer01s">List record is deleted</param>
        /// <returns>List of rows error </returns>
        public bool UpdateTtrUpdateCount01(long seq)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    MERGE INTO (
                            SELECT
                                    *
                            FROM
                                    WW_ARR_TRANS_INPUT01 WW
                            WHERE
                                    WW.SHIPPER_ID = :SHIPPER_ID
                                AND WW.SEQ = :SEQ
                    ) WW
                    USING 
                            T_TRANSFER_RESULTS RESULTS
                    ON (
                            WW.SHIPPER_ID = RESULTS.SHIPPER_ID
                        AND WW.CENTER_ID = RESULTS.CENTER_ID
                        AND WW.SLIP_NO = RESULTS.SLIP_NO
                        AND WW.ITEM_SKU_ID = RESULTS.ITEM_SKU_ID
                        AND WW.BOX_NO = RESULTS.BOX_NO
                    )
                    WHEN MATCHED THEN
                        UPDATE SET
                                WW.UPDATE_DATE = SYSTIMESTAMP
                            ,   WW.UPDATE_USER_ID = :UPDATE_USER_ID
                            ,   WW.UPDATE_PROGRAM_NAME = 'UpdateTtrUpdateCount'
                            ,   WW.UPDATE_COUNT = WW.UPDATE_COUNT + 1
                            ,   WW.TRANSFER_RESULT_DATE = RESULTS.TRANSFER_RESULT_DATE
                    ");
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":SEQ", seq);

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
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
        /// ワーク02 移動入荷実績.更新回数、入荷実績日 更新
        /// </summary>
        /// <param name="InputTransfer02s">List record is deleted</param>
        /// <returns>List of rows error </returns>
        public bool UpdateTtrUpdateCount02(long seq)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    MERGE INTO (
                            SELECT
                                    *
                            FROM
                                    WW_ARR_TRANS_INPUT02 WW
                            WHERE
                                    WW.SHIPPER_ID = :SHIPPER_ID
                                AND WW.SEQ = :SEQ
                    ) WW
                    USING 
                            T_TRANSFER_RESULTS RESULTS
                    ON (
                            WW.SHIPPER_ID = RESULTS.SHIPPER_ID
                        AND WW.CENTER_ID = RESULTS.CENTER_ID
                        AND WW.SLIP_NO = RESULTS.SLIP_NO
                        AND WW.ITEM_SKU_ID = RESULTS.ITEM_SKU_ID
                        AND WW.BOX_NO = RESULTS.BOX_NO
                    )
                    WHEN MATCHED THEN
                        UPDATE SET
                                WW.UPDATE_DATE = SYSTIMESTAMP
                            ,   WW.UPDATE_USER_ID = :UPDATE_USER_ID
                            ,   WW.UPDATE_PROGRAM_NAME = 'UpdateTtrUpdateCount'
                            ,   WW.UPDATE_COUNT = WW.UPDATE_COUNT + 1
                            ,   WW.TTR_UPDATE_COUNT = RESULTS.UPDATE_COUNT
                            ,   WW.TRANSFER_RESULT_DATE = RESULTS.TRANSFER_RESULT_DATE
                    ");
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":SEQ", seq);

                    MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
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
        /// 入荷状況取得（「実績送信済」は表示しない）
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListTransferStatus()
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        GEN_CD AS VALUE
                    ,   GEN_NAME AS TEXT
                FROM
                        M_GENERALS
                WHERE
                        SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = '@@@'
                    AND REGISTER_DIVI_CD = '1'
                    AND GEN_DIV_CD  = 'TRANSFER_STATUS'
                    AND GEN_CD <> 4 --4:実績送信済
                ORDER BY
                        ORDER_NO
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }

        /// <summary>
        /// Check Work Table
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        public bool ArrivalAllChange(InputTransfer01SearchConditions conditions, bool check)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in MvcDbContext.Current.MovTransInput01s.Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq))
                {
                    u.SetBaseInfoUpdate();
                    u.SelectedFlag = check;
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

            conditions.SelectedCnt = MvcDbContext.Current.MovTransInput01s
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.SelectedFlag).Count();
            return true;
        }

        /// <summary>
        /// 移動元センター取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCenters()
        {
            return MvcDbContext.Current.Warehouses
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CenterId,
                    Text = m.CenterName1
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// アイテムデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListItems()
        {
            return MvcDbContext.Current.Items
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.ItemCode,
                    Text = m.ItemCode + ":" + m.ItemCodeName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListDivisions()
        {
            return MvcDbContext.Current.Divisions
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.DivisionId,
                    Text = m.DivisionId + ":" + m.DivisionName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 分類1データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListCategorys1()
        {
            return MvcDbContext.Current.ItemCategories4
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId && (categoryId1 == null ? 1 == 1 : m.CategoryId1 == categoryId1))
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId
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
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId
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
    }
}