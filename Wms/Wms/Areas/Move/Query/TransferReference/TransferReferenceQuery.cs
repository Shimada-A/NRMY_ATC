namespace Wms.Areas.Move.Query.TransferReference
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Wms.Areas.Move.Models;
    using Wms.Areas.Move.Resources;
    using Wms.Areas.Move.ViewModels.TransferReference;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Move.ViewModels.TransferReference.TransferReference01SearchConditions;
    using static Wms.Areas.Move.ViewModels.TransferReference.TransferReference02SearchConditions;

    public class TransferReferenceQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrTransferReference01(TransferReference01SearchConditions condition)
        {
            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            // 2.ワーク作成
            // 検索結果取得
            var result = AddMovTransRef01(condition, false);
            // 過去分を含む場合
            if (result && condition.ContainsArchive)
            {
                result = AddMovTransRef01(condition, true);
            }
            return result;
        }

        /// <summary>
        /// 移動入荷進捗照会ワーク01にデータを登録する
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <param name="archiveTable">累積テーブルを取得するかどうか</param>
        /// <returns></returns>
        private bool AddMovTransRef01(TransferReference01SearchConditions condition, bool archiveTable)
        {
            string transferTableName;
            List<int> lineNoList = MvcDbContext.Current.MovTransRef01s
                .Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId)
                .Select(x => (int)x.LineNo).ToList();

            if (archiveTable)
            {
                transferTableName = "V_A_TRANSFER T";
            }
            else
            {
                transferTableName = "V_TRANSFER T";
            }

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_ARR_TRANS_REF01 (
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
                        ,   TRANSFER_FROM_STORE_ID
                        ,   TRANSFER_FROM_STORE_NAME
                        ,   SLIP_NO
                        ,   PLAN_SLIP_NO
                        ,   RESULT_SLIP_NO
                        ,   ITEM_SKU_ID
                        ,   ARRIVE_PLAN_QTY
                        ,   RESULT_QTY
                        ,   STATUS
                        ,   CONFIRM_DATE
                        ,   IF_STATE
                        ,   UNPLANNED_FLAG
                        ,   SLIP_DATE
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
                    FROM ");
                    query.AppendLine(@"                            " + transferTableName);
                    query.AppendLine(@"                    WHERE
                            T.SHIPPER_ID = :SHIPPER_ID ");

                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "AddMovTransRef01");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":MAX_LINE_NO", lineNoList.Count > 0 ? lineNoList.Max() : 0);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.AppendLine(@"            AND T.CENTER_ID = :CENTER_ID ");
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

                    if (condition.StoreReturnFlag || condition.BaseMoveNoWmsCenterFlag)
                    {
                        // 移動元店舗
                        if (!string.IsNullOrEmpty(condition.TransferFromStoreId))
                        {
                            string[] shipToIds = (condition.TransferFromStoreId ?? condition.TransferFromStoreId).Split(',');
                            query.AppendLine(@"                        AND T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_STORE_ID ");
                            parameters.Add(":TRANSFER_FROM_STORE_ID", shipToIds);
                        }
                    }
                    if(condition.BaseMoveFlag)
                    {
                        // 移動元センター
                        if (!string.IsNullOrEmpty(condition.TransferFromCenterId))
                        {
                            query.AppendLine(@"                        AND T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_STORE_ID ");
                            parameters.Add(":TRANSFER_FROM_STORE_ID", condition.TransferFromCenterId);
                        }
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
                            query.AppendLine(@"                        AND T.TRANSFER_RESULT_DATE >= :TRANSFER_RESULT_DATE_FROM ");
                            parameters.Add(":TRANSFER_RESULT_DATE_FROM", condition.TransferResultDateFrom);
                        }
                        if (condition.TransferResultDateTo != null)
                        {
                            query.AppendLine(@"                        AND T.TRANSFER_RESULT_DATE <= :TRANSFER_RESULT_DATE_TO ");
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
                        ,   ROWNUM + :MAX_LINE_NO
                        ,   T_SLIP.CENTER_ID
                        ,   T_SLIP.TRANSFER_CLASS
                        ,   T_SLIP.ARRIVE_PLAN_DATE
                        ,   T_SLIP.TRANSFER_FROM_STORE_ID
                        ,   VSTS.SHIP_TO_STORE_NAME1 AS TRANSFER_FROM_STORE_NAME
                        ,   T_SLIP.SLIP_NO
                        ,   T.PLAN_SLIP_NO
                        ,   T.RESULT_SLIP_NO
                        ,   T.ITEM_SKU_ID
                        ,   T.ARRIVE_PLAN_QTY
                        ,   T.RESULT_QTY
                        ,   T.STATUS
                        ,   TRUNC(T.CONFIRM_DATE)
                        ,   T.IF_STATE
                        ,   T.UNPLANNED_FLAG
                        ,   T_SLIP.SHIP_DATE
                    FROM ");
                    query.AppendLine(@"                            " + transferTableName);
                    query.AppendLine(@"            INNER JOIN
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
                        query.AppendLine(@"            AND T.DIVISION_ID = :DIVISION_ID ");
                        parameters.Add(":DIVISION_ID", condition.DivisionId);
                    }

                    // ブランド
                    if (!string.IsNullOrEmpty(condition.BrandId))
                    {
                        query.AppendLine(@"                        AND T.BRAND_ID LIKE :BRAND_ID ");
                        parameters.Add(":BRAND_ID", condition.BrandId + "%");
                    }

                    // ブランド名
                    if (string.IsNullOrEmpty(condition.BrandId) && !string.IsNullOrEmpty(condition.BrandName))
                    {
                        query.AppendLine(@"                        AND T.BRAND_NAME LIKE :BRAND_NAME ");
                        parameters.Add(":BRAND_NAME", condition.BrandName + "%");
                    }

                    // 分類
                    if (!string.IsNullOrEmpty(condition.CategoryId1))
                    {
                        query.AppendLine(@"                        AND T.CATEGORY_ID1 = :CATEGORY_ID1 ");
                        parameters.Add(":CATEGORY_ID1", condition.CategoryId1);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId2))
                    {
                        query.AppendLine(@"                        AND T.CATEGORY_ID2 = :CATEGORY_ID2 ");
                        parameters.Add(":CATEGORY_ID2", condition.CategoryId2);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId3))
                    {
                        query.AppendLine(@"                        AND T.CATEGORY_ID3 = :CATEGORY_ID3 ");
                        parameters.Add(":CATEGORY_ID3", condition.CategoryId3);
                    }

                    if (!string.IsNullOrEmpty(condition.CategoryId4))
                    {
                        query.AppendLine(@"                        AND T.CATEGORY_ID4 = :CATEGORY_ID4 ");
                        parameters.Add(":CATEGORY_ID4", condition.CategoryId4);
                    }

                    // 品番
                    if (!string.IsNullOrEmpty(condition.ItemId))
                    {
                        query.AppendLine(@"                        AND T.ITEM_ID LIKE :ITEM_ID ");
                        parameters.Add(":ITEM_ID", condition.ItemId + "%");
                    }

                    // カラー
                    if (!string.IsNullOrEmpty(condition.ItemColorId))
                    {
                        query.AppendLine(@"                        AND T.ITEM_COLOR_ID LIKE :ITEM_COLOR_ID ");
                        parameters.Add(":ITEM_COLOR_ID", condition.ItemColorId + "%");
                    }

                    // カラー名
                    if (string.IsNullOrEmpty(condition.ItemColorId) && !string.IsNullOrEmpty(condition.ItemColorName))
                    {
                        query.AppendLine(@"                        AND T.ITEM_COLOR_NAME LIKE :ITEM_COLOR_NAME ");
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
                        query.AppendLine(@"                        AND T.ITEM_SIZE_ID LIKE :ITEM_SIZE_ID ");
                        parameters.Add(":ITEM_SIZE_ID", condition.ItemSizeId + "%");
                    }

                    // サイズ名
                    if (string.IsNullOrEmpty(condition.ItemSizeId) && !string.IsNullOrEmpty(condition.ItemSizeName))
                    {
                        query.AppendLine(@"                        AND T.ITEM_SIZE_NAME LIKE :ITEM_SIZE_NAME ");
                        parameters.Add(":ITEM_SIZE_NAME", condition.ItemSizeName + "%");
                    }

                    // JAN
                    if (!string.IsNullOrEmpty(condition.Jan))
                    {
                        query.AppendLine(@"                        AND T.JAN LIKE :JAN ");
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
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<TransferReference01ResultRow> TransferReference01GetData(TransferReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            WITH
                WORK_DATA AS (
                    SELECT
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   ITEM_SKU_ID
                        ,   MAX(LINE_NO) AS LINE_NO
                        ,   MAX(SEQ) AS SEQ
                        ,   MAX(TRANSFER_FROM_STORE_ID) AS TRANSFER_FROM_STORE_ID
                        ,   MAX(TRANSFER_FROM_STORE_NAME) AS TRANSFER_FROM_STORE_NAME
                        ,   MAX(PLAN_SLIP_NO) AS PLAN_SLIP_NO
                        ,   MAX(RESULT_SLIP_NO) AS RESULT_SLIP_NO
                        ,   MAX(ARRIVE_PLAN_QTY) AS ARRIVE_PLAN_QTY
                        ,   SUM(RESULT_QTY) AS RESULT_QTY
                        ,   MAX(STATUS) AS STATUS
                        ,   MAX(IF_STATE) AS IF_STATE
                        ,   MAX(CONFIRM_DATE) AS CONFIRM_DATE
                        ,   MAX(UNPLANNED_FLAG) AS UNPLANNED_FLAG
                        ,   MAX(SLIP_DATE) AS SLIP_DATE
                        ,   MAX(TRANSFER_CLASS) AS TRANSFER_CLASS
                    FROM
                            WW_ARR_TRANS_REF01
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                    GROUP BY
                            SHIPPER_ID
                        ,   CENTER_ID
                        ,   SLIP_NO
                        ,   ITEM_SKU_ID
                )
                ,WORK_TBL AS (
                    SELECT
                            MAX(WW.LINE_NO) AS LINE_NO
                        ,   MAX(WW.SEQ) AS SEQ
                        ,   WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   WW.TRANSFER_FROM_STORE_ID
                        ,   MAX(WW.TRANSFER_FROM_STORE_NAME) AS TRANSFER_FROM_STORE_NAME
                        ,   NVL(COUNT(DISTINCT(WW.PLAN_SLIP_NO)),0) AS SLIP_NO_PLAN_QTY
                        ,   CASE COUNT(DISTINCT(WW.RESULT_SLIP_NO))
                                WHEN 0 THEN NULL
                                ELSE COUNT(DISTINCT(WW.RESULT_SLIP_NO))
                            END AS SLIP_NO_RESULT_QTY
                        ,   COUNT(DISTINCT(WW.ITEM_SKU_ID)) AS ITEM_SKU_QTY
                        ,   NVL(SUM(WW.ARRIVE_PLAN_QTY),0) AS ARRIVE_PLAN_QTY
                        ,   SUM(WW.RESULT_QTY) AS RESULT_QTY
                        ,   CASE
                                WHEN SUM(WW.STATUS) = 0 THEN 1 --1:未入荷
                                WHEN MAX(WW.STATUS) > 0 AND MIN(WW.STATUS) = 0 THEN 2 --2:一部検品済
                                WHEN MIN(WW.STATUS) > 0 AND MIN(WW.IF_STATE) = 2 THEN 4 --4:実績送信済
                                ELSE 3 --3:検品済
                            END AS TRANSFER_STATUS
                        ,   WW.CONFIRM_DATE
                        ,   NVL(MIN(WW.UNPLANNED_FLAG),0) AS UNPLANNED_FLAG
                        ,   WW.SLIP_DATE AS DENPYO_DATE
                        ,   WW.TRANSFER_CLASS AS TRANSFER_CLASS
                    FROM
                            WORK_DATA WW
                    GROUP BY
                            WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   WW.TRANSFER_CLASS
                        ,   WW.SLIP_DATE
                        ,   WW.TRANSFER_FROM_STORE_ID
                        ,   WW.CONFIRM_DATE
            )
            SELECT
                    WORK_TBL.*
                ,   TRANSFER_STATUS.GEN_NAME AS TRANSFER_STATUS_NAME 
            FROM
                    WORK_TBL
            LEFT OUTER JOIN
                    M_GENERALS TRANSFER_STATUS
            ON
                    TRANSFER_STATUS.SHIPPER_ID = WORK_TBL.SHIPPER_ID
                AND TRANSFER_STATUS.CENTER_ID = '@@@'
                AND TRANSFER_STATUS.REGISTER_DIVI_CD = '1'
                AND TRANSFER_STATUS.GEN_DIV_CD = 'TRANSFER_STATUS'
                AND TRANSFER_STATUS.GEN_CD = TO_CHAR(WORK_TBL.TRANSFER_STATUS)
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 状況
            if (!string.IsNullOrEmpty(condition.TransferStatus))
            {
                query.AppendLine(@"WHERE TO_CHAR(WORK_TBL.TRANSFER_STATUS) = :TRANSFER_STATUS ");
                parameters.Add(":TRANSFER_STATUS", condition.TransferStatus);
            }

            // 全レコード数を取得
            var allRecords = MvcDbContext.Current.Database.Connection.Query<TransferReference01ResultRow>(query.ToString(), parameters);

            // Sort function
            switch (condition.SortKey)
            {
                case TransferReference01SortKey.DenpyoDateTransferClassStoreId:
                    switch (condition.Sort)
                    {
                        case TransferReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY DENPYO_DATE DESC,TRANSFER_CLASS DESC,TRANSFER_FROM_STORE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY DENPYO_DATE ASC,TRANSFER_CLASS ASC,TRANSFER_FROM_STORE_ID ASC ");
                            break;
                    }

                    break;
                case TransferReference01SortKey.StoreIdDenpyoDate:
                    switch (condition.Sort)
                    {
                        case TransferReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TRANSFER_FROM_STORE_ID DESC,DENPYO_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TRANSFER_FROM_STORE_ID ASC,DENPYO_DATE ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case TransferReference01SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY TRANSFER_CLASS DESC,DENPYO_DATE DESC,TRANSFER_FROM_STORE_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY TRANSFER_CLASS ASC,DENPYO_DATE ASC,TRANSFER_FROM_STORE_ID ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var TransferReference01s = MvcDbContext.Current.Database.Connection.Query<TransferReference01ResultRow>(query.ToString(), parameters);

            condition.ArrivePlanQtySum = allRecords.Sum(x => x.ArrivePlanQty) ?? 0;
            condition.ResultQtySum = allRecords.Sum(x => x.ResultQty) ?? 0;

            var movTransRef01s = GetDetailData01(condition);
            condition.ItemSkuQtySum = movTransRef01s.ItemSkuQtySum;
            condition.SlipNoPlanQtySum = movTransRef01s.SlipNoPlanQtySum;
            condition.SlipNoResultQtySum = movTransRef01s.SlipNoResultQtySum;

            // Excute paging
            return new StaticPagedList<TransferReference01ResultRow>(TransferReference01s, condition.Page, condition.PageSize, allRecords.Count());
        }

        /// <summary>
        /// 明細ヘッダ取得
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public TransferReference01SearchConditions GetDetailData01(TransferReference01SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            WITH
                WORK_DATA AS (
                    SELECT
                            *
                    FROM
                            WW_ARR_TRANS_REF01
                    WHERE
                            SHIPPER_ID = :SHIPPER_ID
                        AND SEQ = :SEQ
                )
                ,WORK_TBL AS (
                    SELECT
                            WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   WW.TRANSFER_CLASS
                        ,   WW.SLIP_DATE
                        ,   WW.TRANSFER_FROM_STORE_ID
                        ,   CASE
                                WHEN SUM(WW.STATUS) = 0 THEN 1 --1:未入荷
                                WHEN MAX(WW.STATUS) > 0 AND MIN(WW.STATUS) = 0 THEN 2 --2:一部検品済
                                WHEN MIN(WW.STATUS) > 0 AND MIN(WW.IF_STATE) = 2 THEN 4 --4:実績送信済
                                ELSE 3 --3:検品済
                            END AS TRANSFER_STATUS
                    FROM
                            WORK_DATA WW
                    GROUP BY
                            WW.SHIPPER_ID
                        ,   WW.CENTER_ID
                        ,   WW.TRANSFER_CLASS
                        ,   WW.SLIP_DATE
                        ,   WW.TRANSFER_FROM_STORE_ID
            )
            SELECT
                    COUNT(DISTINCT(WW.ITEM_SKU_ID)) AS ITEM_SKU_QTY_SUM
                ,   COUNT(DISTINCT(WW.PLAN_SLIP_NO)) AS SLIP_NO_PLAN_QTY_SUM
                ,   COUNT(DISTINCT(WW.RESULT_SLIP_NO)) AS SLIP_NO_RESULT_QTY_SUM
            FROM
                    WORK_DATA WW
            LEFT OUTER JOIN
                    WORK_TBL
            ON
                    WW.SHIPPER_ID = WORK_TBL.SHIPPER_ID
                AND WW.CENTER_ID = WORK_TBL.CENTER_ID
                AND WW.TRANSFER_CLASS = WORK_TBL.TRANSFER_CLASS
                AND WW.SLIP_DATE = WORK_TBL.SLIP_DATE
                AND WW.TRANSFER_FROM_STORE_ID = WORK_TBL.TRANSFER_FROM_STORE_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 状況
            if (!string.IsNullOrEmpty(condition.TransferStatus))
            {
                query.AppendLine(@"WHERE TO_CHAR(WORK_TBL.TRANSFER_STATUS) = :TRANSFER_STATUS ");
                parameters.Add(":TRANSFER_STATUS", condition.TransferStatus);
            }

            return MvcDbContext.Current.Database.Connection.Query<TransferReference01SearchConditions>(query.ToString(), parameters).FirstOrDefault();
        }

        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrTransferReference02(TransferReference02SearchConditions condition)
        {
            // 1.ワークID採番
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            // 2.ワーク作成
            // 検索結果取得
            var result = AddMovTransRef02(condition, false);
            // 過去分を含む場合
            if (condition.ContainsArchive)
            {
                result = AddMovTransRef02(condition, true);
            }
            return result;
        }

        /// <summary>
        /// 移動入荷進捗照会ワーク02にデータを登録する
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <param name="archiveTable">累積テーブルを取得するかどうか</param>
        /// <returns>移動入荷進捗照会ワーク02に登録するデータ</returns>
        private bool AddMovTransRef02(TransferReference02SearchConditions condition, bool archiveTable)
        {
            string transferTableName;
            List<int> lineNoList = MvcDbContext.Current.MovTransRef02s
                .Where(x => x.Seq == condition.Seq && x.ShipperId == Common.Profile.User.ShipperId)
                .Select(x => (int)x.LineNo).ToList();

            if (archiveTable)
            {
                transferTableName = "V_A_TRANSFER T";
            }
            else
            {
                transferTableName = "V_TRANSFER T";
            }

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    var query = new StringBuilder();

                    query.Append($@"
                        INSERT INTO WW_ARR_TRANS_REF02 (
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
                            ,   TRANSFER_FROM_STORE_CLASS
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
                            ,   TRANSFER_STATUS
                            ,   CONFIRM_DATE
                            ,   UNPLANNED_FLAG
                            ,   SLIP_DATE
                            ,   BOX_NO
                            ,   BRAND_ID
                            ,   BRAND_SHORT_NAME
                            ,   DIFFERENCE_QTY
                            ,   TRANSFER_STATUS_NAME
                        )
                        WITH
                            TARGET_SLIP_NO AS (
                                --検索条件に該当する移動伝票番号を絞り込む
                                SELECT
                                        T.SHIPPER_ID
                                    ,   T.CENTER_ID
                                    ,   T.SLIP_NO
                                    ,   NVL(MAX(T.ARRIVE_PLAN_DATE), MAX(T.TRANSFER_RESULT_DATE)) AS ARRIVE_PLAN_DATE
                                    ,   MAX(T.TRANSFER_CLASS) AS TRANSFER_CLASS
                                    ,   MAX(T.SHIP_DATE) AS SHIP_DATE
                                    ,   MAX(T.TRANS_FROM_STORE_CLASS) AS TRANS_FROM_STORE_CLASS
                                    ,   MAX(T.TRANSFER_FROM_STORE_ID) AS TRANSFER_FROM_STORE_ID
                                FROM
                                        {transferTableName}
                                WHERE
                                        T.SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "AddMovTransRef02");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":MAX_LINE_NO", lineNoList.Count > 0 ? lineNoList.Max() : 0);
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

                    if (condition.StoreReturnFlag || condition.BaseMoveNoWmsCenterFlag)
                    {
                        // 移動元店舗
                        if (!string.IsNullOrEmpty(condition.TransferFromStoreId))
                        {
                            string[] shipToIds = (condition.TransferFromStoreId ?? condition.TransferFromStoreId).Split(',');
                            query.AppendLine(@"                        AND T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_STORE_ID ");
                            parameters.Add(":TRANSFER_FROM_STORE_ID", shipToIds);
                        }
                    }
                    if(condition.BaseMoveFlag)
                    {
                        // 移動元センター
                        if (!string.IsNullOrEmpty(condition.TransferFromCenterId))
                        {
                            query.AppendLine(@"                        AND T.TRANSFER_FROM_STORE_ID IN :TRANSFER_FROM_STORE_ID ");
                            parameters.Add(":TRANSFER_FROM_STORE_ID", condition.TransferFromCenterId);
                        }
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

                    // 伝票番号
                    if (!string.IsNullOrEmpty(condition.DenpyoNo))
                    {
                        query.AppendLine(@"                         AND T.SLIP_NO LIKE :DENPYO_NO ");
                        parameters.Add(":DENPYO_NO", condition.DenpyoNo + "%");
                    }

                    query.Append($@"
                            GROUP BY
                                    T.SHIPPER_ID
                                ,   T.CENTER_ID
                                ,   T.SLIP_NO
                        )
                    ,   TRANSFER_SKU AS (
                            --検索条件のSKUを絞り込む
                            SELECT
                                    T_SLIP.SHIPPER_ID
                                ,   T_SLIP.CENTER_ID
                                ,   T_SLIP.TRANSFER_CLASS
                                ,   T_SLIP.TRANS_FROM_STORE_CLASS
                                ,   T_SLIP.TRANSFER_FROM_STORE_ID
                                ,   T_SLIP.SLIP_NO
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
                                ,   NVL(T.ARRIVE_PLAN_QTY, 0) AS ARRIVE_PLAN_QTY
                                ,   T.RESULT_QTY
                                ,   T.CONFIRM_DATE
                                ,   NVL(T.UNPLANNED_FLAG, 0) AS UNPLANNED_FLAG
                                ,   T_SLIP.SHIP_DATE
                                ,   T.BOX_NO
                                ,   T.BRAND_ID
                                ,   T.BRAND_SHORT_NAME
                                ,   SUM(T.RESULT_QTY) OVER (PARTITION BY T.SLIP_NO, T.ITEM_SKU_ID) AS SKU_RESULT_QTY
                                ,   T.IF_STATE
                                ,   T.STATUS
                            FROM 
                                    {transferTableName}
                            INNER JOIN
                                    TARGET_SLIP_NO T_SLIP
                            ON
                                    T_SLIP.SHIPPER_ID = T.SHIPPER_ID
                                AND T_SLIP.CENTER_ID = T.CENTER_ID
                                AND T_SLIP.SLIP_NO = T.SLIP_NO
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
                        query.AppendLine("            AND T.BRAND_SHORT_NAME LIKE :BRAND_NAME ");
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
                        query.AppendLine("            AND T.CATEGORY_ID2 = :CATEGORY_ID2 ");
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
                        query.AppendLine(@"           AND T.ITEM_CODE = :ITEM_CODE ");
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

                    query.Append($@"
                        )
                    ,   TRANSFER AS (
                            SELECT
                                    SYSTIMESTAMP AS MAKE_DATE
                                ,   :USER_ID AS MAKE_USER_ID
                                ,   :PROGRAM_NAME AS MAKE_PROGRAM_NAME
                                ,   SYSTIMESTAMP AS UPDATE_DATE
                                ,   :USER_ID AS UPDATE_USER_ID
                                ,   :PROGRAM_NAME AS UPDATE_PROGRAM_NAME
                                ,   0 AS UPDATE_COUNT
                                ,   T.SHIPPER_ID
                                ,   :SEQ AS SEQ
                                ,   ROWNUM + :MAX_LINE_NO AS LINE_NO
                                ,   T.CENTER_ID
                                ,   T.TRANSFER_CLASS
                                ,   T.TRANS_FROM_STORE_CLASS
                                ,   T.TRANSFER_FROM_STORE_ID
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
                                ,   NVL(T.ARRIVE_PLAN_QTY, 0) AS ARRIVE_PLAN_QTY
                                ,   T.RESULT_QTY
                                ,   CASE
                                        WHEN T.STATUS = 0 THEN 1 --1:未入荷
                                        WHEN T.STATUS > 0 AND T.IF_STATE = 2 THEN 4 --4:実績送信済
                                        ELSE 3 --3:検品済
                                    END TRANSFER_STATUS
                                ,   T.CONFIRM_DATE
                                ,   NVL(T.UNPLANNED_FLAG, 0) AS UNPLANNED_FLAG
                                ,   T.SHIP_DATE
                                ,   T.BOX_NO
                                ,   T.BRAND_ID
                                ,   T.BRAND_SHORT_NAME
                                ,   T.SKU_RESULT_QTY - NVL(T.ARRIVE_PLAN_QTY, 0) AS DIFFERENCE_QTY
                            FROM 
                                    TRANSFER_SKU T
                            LEFT OUTER JOIN
                                    V_SHIP_TO_STORES VSTS
                            ON
                                    VSTS.SHIPPER_ID = T.SHIPPER_ID
                                AND VSTS.SHIP_TO_STORE_ID = T.TRANSFER_FROM_STORE_ID
                                AND VSTS.DELETE_FLAG = 0
                            WHERE
                                    1 = 1
                    ");

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

                    // 状況
                    if (!string.IsNullOrEmpty(condition.TransferStatus))
                    {
                        query.AppendLine(@"WHERE TO_CHAR(TRANSFER.TRANSFER_STATUS) = :TRANSFER_STATUS ");
                        parameters.Add(":TRANSFER_STATUS", condition.TransferStatus);
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
        /// 移動入荷進捗照会ワーク02
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<TransferReference02ResultRow> TransferReference02GetData(TransferReference02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            var orderBySql = "";

            // Sort function
            switch (condition.SortKey)
            {
                case TransferReference02SortKey.TransferClassDenpyoDateStoreIdSkuDenpyoNo:
                    switch (condition.Sort)
                    {
                        case TransferReference02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY TRANSFER_CLASS DESC, SLIP_DATE DESC, TRANSFER_FROM_STORE_ID DESC, ITEM_SKU_ID DESC, SLIP_NO DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY TRANSFER_CLASS ASC, SLIP_DATE ASC, TRANSFER_FROM_STORE_ID ASC, ITEM_SKU_ID ASC, SLIP_NO ASC, BOX_NO ASC ";
                            break;
                    }

                    break;
                case TransferReference02SortKey.SkuStoreIdDenpyoDate:
                    switch (condition.Sort)
                    {
                        case TransferReference02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY ITEM_SKU_ID DESC, TRANSFER_FROM_STORE_ID DESC, SLIP_DATE DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY ITEM_SKU_ID ASC, TRANSFER_FROM_STORE_ID ASC, SLIP_DATE ASC, BOX_NO ASC ";
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case TransferReference02SearchConditions.AscDescSort.Desc:
                            orderBySql = @"ORDER BY TRANSFER_CLASS DESC, SLIP_DATE DESC, TRANSFER_FROM_STORE_ID DESC, SLIP_NO DESC, ITEM_SKU_ID DESC, BOX_NO DESC ";
                            break;

                        default:
                            orderBySql = @"ORDER BY TRANSFER_CLASS ASC, SLIP_DATE ASC, TRANSFER_FROM_STORE_ID ASC, SLIP_NO ASC, ITEM_SKU_ID ASC, BOX_NO ASC ";
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
                                WW_ARR_TRANS_REF02 WW
                        WHERE
                                WW.SHIPPER_ID = :SHIPPER_ID
                            AND WW.SEQ = :SEQ
                    )
                SELECT
                        WW.SHIPPER_ID
                    ,   WW.SEQ
                    ,   WW.LINE_NO
                    ,   WW.CENTER_ID
                    ,   WW.TRANSFER_CLASS
                    ,   WW.TRANSFER_FROM_STORE_CLASS
                    ,   WW.TRANSFER_FROM_STORE_ID
                    ,   WW.TRANSFER_FROM_STORE_NAME
                    ,   WW.SLIP_NO
                    ,   WW.SLIP_SEQ
                    ,   WW.CATEGORY_ID1
                    ,   WW.CATEGORY_NAME1
                    ,   WW.ITEM_ID
                    ,   WW.ITEM_NAME
                    ,   WW.ITEM_COLOR_ID
                    ,   WW.ITEM_COLOR_NAME
                    ,   WW.ITEM_SIZE_ID
                    ,   WW.ITEM_SIZE_NAME
                    ,   WW.JAN
                    ,   WW.ITEM_SKU_ID
                    ,   CASE WHEN WW.SKU_ROW_NUMBER = 1 THEN WW.ARRIVE_PLAN_QTY ELSE NULL END AS ARRIVE_PLAN_QTY
                    ,   WW.RESULT_QTY
                    ,   WW.TRANSFER_STATUS
                    ,   WW.CONFIRM_DATE
                    ,   WW.UNPLANNED_FLAG
                    ,   WW.SLIP_DATE
                    ,   WW.BOX_NO
                    ,   WW.BRAND_ID
                    ,   WW.BRAND_SHORT_NAME
                    ,   CASE WHEN WW.SKU_ROW_NUMBER = 1 THEN WW.DIFFERENCE_QTY ELSE NULL END AS DIFFERENCE_QTY
                    ,   WW.TRANSFER_STATUS_NAME
                FROM
                        WORK_DATA WW
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            var allRecords = MvcDbContext.Current.Database.Connection.Query<TransferReference02ResultRow>(query.ToString(), parameters);

            query.AppendLine(orderBySql);
            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");

            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var TransferReference02s = MvcDbContext.Current.Database.Connection.Query<TransferReference02ResultRow>(query.ToString(), parameters);

            condition.ItemSkuQtySum = allRecords.Select(x => x.ItemSkuId).Distinct().Count();
            condition.ArrivePlanQtySum = allRecords.Select(x => x.ArrivePlanQty).Sum() ?? 0;
            condition.ResultQtySum = allRecords.Select(x => x.ResultQty).Sum() ?? 0;
            condition.SlipNoPlanQtySum = allRecords.Where(x => x.ArrivePlanQty != null).Select(x => x.SlipNo).Distinct().Count();
            condition.SlipNoResultQtySum = allRecords.Where(x => x.ResultQty != null).Select(x => x.SlipNo).Distinct().Count();

            // Excute paging
            return new StaticPagedList<TransferReference02ResultRow>(TransferReference02s, condition.Page, condition.PageSize, allRecords.Count());
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