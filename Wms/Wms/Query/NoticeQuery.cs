namespace Wms.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Wms.Common;
    using Wms.Models;
    using Wms.ViewModels;
    using Wms.ViewModels.Notice;

    public class NoticeQuery
    {
        /// <summary>
        /// お知らせ一覧を取得します。
        /// </summary>
        /// <param name="shipperId">荷主ID</param>
        /// <param name="userId">ユーザーID</param>
        /// <returns></returns>
        public IPagedList<NoticeResultRow> Listing(NoticeSearchConditions conditions)
        {
            var parameters = new DynamicParameters();
            var sql = @"
                WITH
                    GET_MESSAGE_CLASS_NAME AS (
                        SELECT
                                GEN_CD
                            ,   GEN_NAME
                            ,   SHIPPER_ID
                        FROM
                                M_GENERALS
                        WHERE
                                REGISTER_DIVI_CD = '1'
                            AND GEN_DIV_CD = 'MESSAGE_CLASS'
                            AND CENTER_ID = '@@@'
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        GET_NAME.GEN_NAME AS MESSAGE_CLASS_NM
                    ,   HEAD.NOTICE_HEADER_ID
                    ,   HEAD.OCCURRENCE_DATE
                    ,   HEAD.NOTICE_IF_MESSAGE_ID
                    ,   HEAD.IF_RUN_ID
                    ,   HEAD.LOC_CLASS
                    ,   HEAD.LOC_ID
                    ,   HEAD.SUBJECT
                    ,   HEAD.MESSAGE_PARAMETER
                    ,   HEAD.IF_DATA_ID
                    ,   HEAD.IF_SYSTEM_ID
                    ,   M_NOTICE.MESSAGE_CLASS
                    ,   M_NOTICE.VIEW_FLAG
                    ,   M_NOTICE.MESSAGE
                    ,   T_RUN.IF_RUN_STATE
                    ,   T_RUN.START_TIME
                    ,   T_RUN.END_TIME
                    ,   UNIT_NAME.NAME
                    ,   M_DATA.IF_DATA_NAME
                    ,   M_DATA.IF_CLASS
                    ,   M_DATA.TABLE_NAME
                    ,   M_DATA.TABLE_COMMENT
                    ,   M_SYSTEM.IF_SYSTEM_NAME
                FROM
                        T_NOTICE_HEADERS HEAD
                INNER JOIN
                        M_NOTICE_IF_MESSAGES M_NOTICE
                ON
                        HEAD.NOTICE_IF_MESSAGE_ID = M_NOTICE.NOTICE_IF_MESSAGE_ID
                    AND HEAD.SHIPPER_ID = M_NOTICE.SHIPPER_ID
                LEFT OUTER JOIN
                        T_IF_RUNS T_RUN
                ON
                        HEAD.IF_RUN_ID = T_RUN.IF_RUN_ID
                    AND HEAD.SHIPPER_ID = T_RUN.SHIPPER_ID
                LEFT OUTER JOIN
                        M_IF_RUN_UNIT_NAMES UNIT_NAME
                ON
                        T_RUN.IF_RUN_UNIT_ID = UNIT_NAME.IF_RUN_UNIT_ID
                    AND T_RUN.SHIPPER_ID = UNIT_NAME.SHIPPER_ID
                LEFT OUTER JOIN
                        M_IF_DATA M_DATA
                ON
                        HEAD.IF_DATA_ID = M_DATA.IF_DATA_ID
                    AND HEAD.SHIPPER_ID = M_DATA.SHIPPER_ID
                LEFT OUTER JOIN
                        M_IF_SYSTEMS M_SYSTEM
                ON
                        HEAD.IF_SYSTEM_ID = M_SYSTEM.IF_SYSTEM_ID
                    AND HEAD.SHIPPER_ID = M_SYSTEM.SHIPPER_ID
                LEFT OUTER JOIN
                        GET_MESSAGE_CLASS_NAME GET_NAME
                ON
                        TO_CHAR(M_NOTICE.MESSAGE_CLASS) = GET_NAME.GEN_CD
                    AND M_NOTICE.SHIPPER_ID = GET_NAME.SHIPPER_ID
                WHERE
                        HEAD.SHIPPER_ID = :SHIPPER_ID
                    AND HEAD.LOC_ID  IN (:SHIPPER_ID, :LOC_ID)
            ";

            //種別
            if (!string.IsNullOrEmpty(conditions.MessageClassValue) )
            {
                sql = sql + @"
                    AND M_NOTICE.MESSAGE_CLASS = :MESSAGE_CLASS
                ";
                parameters.AddDynamicParams(new { MESSAGE_CLASS = Convert.ToByte(conditions.MessageClassValue) });
            }
            //発生期間
            if (!string.IsNullOrEmpty(conditions.OccurrenceDateFrom.ToString()) )
            {
                sql = sql + @"
                    AND TO_CHAR(HEAD.OCCURRENCE_DATE, 'YYYY/MM/DD') >= :OCCURRENCE_DATE_FROM
                ";
                parameters.AddDynamicParams(new { OCCURRENCE_DATE_FROM = $"{conditions.OccurrenceDateFrom:yyyy/MM/dd}" });
            }
            if (!string.IsNullOrEmpty(conditions.OccurrenceDateTo.ToString()) )
            {
                sql = sql + @"
                    AND TO_CHAR(HEAD.OCCURRENCE_DATE, 'YYYY/MM/DD') <= :OCCURRENCE_DATE_TO
                ";
                parameters.AddDynamicParams(new { OCCURRENCE_DATE_TO = $"{conditions.OccurrenceDateTo:yyyy/MM/dd}" });
            }

            parameters.AddDynamicParams(new{ LOC_ID = conditions.CenterId });
            parameters.AddDynamicParams(new{ SHIPPER_ID = Common.Profile.User.ShipperId });

            //全レコード数を取得
            var totalCount = MvcDbContext.Current.Database.Connection.Query<NoticeResultRow>(sql.ToString(), parameters).Count();

            //並び順
            if (conditions.SortKey == NoticeSearchConditions.NoticeSortKey.OccurrenceDate)
            {
                switch (conditions.Sort)
                {
                    case NoticeSearchConditions.AscDescSort.Asc:
                        sql = sql + @"
                                ORDER BY
                                        HEAD.OCCURRENCE_DATE
                                    ,   HEAD.NOTICE_HEADER_ID
                             ";
                        break;
                    case NoticeSearchConditions.AscDescSort.Desc:
                        sql = sql + @"
                                ORDER BY
                                        HEAD.OCCURRENCE_DATE DESC
                                    ,   HEAD.NOTICE_HEADER_ID DESC
                             ";
                        break;
                    default:
                        break;
                }
            }

            //ページ
            sql = sql + @" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY ";
            parameters.AddDynamicParams(new { OFFSET = (conditions.Page - 1) * conditions.PageSize });
            parameters.AddDynamicParams(new { PAGE_SIZE = conditions.PageSize });

            var notices = MvcDbContext.Current.Database.Connection.Query<NoticeResultRow
                , NoticeHeader, NoticeIfMessage, IfRun, IfRunUnitName, IfData, IfSystem, NoticeResultRow>(
                sql,
                (noticeResultRow, noticeHeader, noticeIfMessage, ifRun, ifRunUnitName, ifData, ifSystem) =>
                {
                    noticeResultRow.NoticeHeader = noticeHeader;
                    noticeResultRow.NoticeIfMessage = noticeIfMessage;
                    noticeResultRow.IfRun = ifRun;
                    noticeResultRow.IfRunUnitName = ifRunUnitName;
                    noticeResultRow.IfData = ifData;
                    noticeResultRow.IfSystem = ifSystem;
                    return noticeResultRow;
                },
                parameters,
                splitOn: "NOTICE_HEADER_ID, MESSAGE_CLASS, IF_RUN_STATE, NAME, IF_DATA_NAME, IF_SYSTEM_NAME"
            );

            return new StaticPagedList<NoticeResultRow>(notices, conditions.Page, conditions.PageSize, totalCount);
        }

        /// <summary>
        /// 受信系エラー詳細情報取得
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public List<NoticeDetailReceiveRow> ReceiveListing(NoticeDetailHeader results)
        {
            var parameters = new DynamicParameters();

            var targetTable = results.TableName;
            //該当データがない場合空のリストを返す
            if (targetTable == null)
            {
                return new List<NoticeDetailReceiveRow>();
            }

            var sql = @"
                WITH
                    GET_EC_CLASS_NAME AS (
                        SELECT
                                GEN_CD
                            ,   GEN_NAME
                            ,   SHIPPER_ID
                        FROM
                                M_GENERALS
                        WHERE
                                REGISTER_DIVI_CD = '1'
                            AND GEN_DIV_CD = 'IF_ERR_CLASS'
                            AND CENTER_ID = '@@@'
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        ROW_NUMBER()OVER(ORDER BY R_DATA.IF_FILE_NAME, R_DATA.IF_SEQ) AS ROW_NO
                    ,   R_DATA.IF_FILE_NAME
                    ,   R_DATA.IF_SEQ
                    ,   R_DATA.IF_ERR_COLUMN_COMMENT
                    ,   R_DATA.IF_ERR_CLASS
                    ,   GET_NAME.GEN_NAME AS IF_ERR_CLASS_NM
            ";
            //発注の場合
            if (targetTable == "R_PO")
            {
                sql = sql + @"
                    ,   R_DATA.PO_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //納伝の場合
            if (targetTable == "R_ARRIVE_PLANS")
            {
                sql = sql + @"
                    ,   R_DATA.PO_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.BRANCH_NO
                    ,   R_DATA.ARRIVE_CENTER_ID
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ARRIVE_PLAN_DATE
                 ";
            }
            //初回半分の場合
            if (targetTable == "R_FIRST_DB")
            {
                sql = sql + @"
                    ,   R_DATA.SHIP_INSTRUCT_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.SHIP_TO_STORE_ID
                    ,   R_DATA.DISTRIBUTE_DATE
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //フォローの場合
            if (targetTable == "R_FOLLOW_DB")
            {
                sql = sql + @"
                    ,   R_DATA.SHIP_INSTRUCT_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.SHIP_TO_STORE_ID
                    ,   R_DATA.INSTRUCT_DATE
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //入庫予定
            if (targetTable == "R_TRANSFER_PLANS")
            {
                sql = sql + @"
                    ,   R_DATA.SLIP_NO
                    ,   R_DATA.TRANSFER_FROM_STORE_ID
                    ,   R_DATA.TRANSFER_TO_CENTER_ID
                    ,   R_DATA.SLIP_DATE
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //納品伝票
            if (targetTable == "R_DELIVERY_SLIPS")
            {
                sql = sql + @"
                    ,   R_DATA.SLIP_NO
                    ,   R_DATA.SLIP_DATE
                    ,   R_DATA.PO_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //ASN
            if (targetTable == "R_ASN")
            {
                sql = sql + @"
                    ,   R_DATA.SLIP_NO
                    ,   R_DATA.SLIP_DATE
                    ,   R_DATA.PO_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.CARTON_ID
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //入荷実績未受信
            if (targetTable == "R_ARRIVE_RESULT_UNRCV")
            {
                sql = sql + @"
                    ,   R_DATA.SLIP_NO
                    ,   R_DATA.SLIP_DATE
                    ,   R_DATA.PO_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.ITEM_ID
                 ";
            }
            //入荷実績
            if (targetTable == "R_ARRIVE_RESULTS")
            {
                sql = sql + @"
                    ,   R_DATA.PO_ID
                    ,   R_DATA.SLIP_DATE
                    ,   R_DATA.PO_ID
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.BRANCH_NO
                    ,   R_DATA.PARTNER_SLIP_NO
                    ,   R_DATA.CARTON_ID
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //移動入荷実績
            if (targetTable == "R_TRANSFER_RESULTS")
            {
                sql = sql + @"
                    ,   R_DATA.SLIP_NO
                    ,   R_DATA.TRANSFER_FROM_STORE_ID
                    ,   R_DATA.TRANSFER_TO_CENTER_ID
                    ,   R_DATA.BOX_NO
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //出荷梱包実績
            if (targetTable == "R_SHIP_PACKING_INFO")
            {
                sql = sql + @"
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.BOX_NO
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //棚卸スキャン実績
            if (targetTable == "R_INVENTORY_SCAN")
            {
                sql = sql + @"
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.INVENTORY_NO
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                    ,   R_DATA.LOCATION_CD
                    ,   R_DATA.BOX_NO
                 ";
            }
            //仕入梱包実績
            if (targetTable == "R_PACKING_ARRIVE_PLANS")
            {
                sql = sql + @"
                    ,   R_DATA.BOX_NO
                    ,   R_DATA.BOX_SEQ
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                    ,   R_DATA.INVOICE_NO
                    ,   R_DATA.INVOICE_SEQ
                    ,   R_DATA.CENTER_ID
                 ";
            }
            //注文
            if (targetTable == "R_ECSHIPS")
            {
                sql = sql + @"
                    ,   R_DATA.SHIP_INSTRUCT_ID
                    ,   R_DATA.SHIP_INSTRUCT_SEQ
                    ,   R_DATA.CENTER_ID
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                 ";
            }
            //GAS
            if (targetTable == "R_GAS")
            {
                sql = sql + @"
                    ,   R_DATA.GAS_BATCH_NO
                    ,   R_DATA.ITEM_SKU_ID
                    ,   R_DATA.BOX_NO
                 ";
            }
            //商品マスタ
            if(targetTable == "R_ITEM_SKU")
            {
                sql = sql + @"
                    ,   R_DATA.ITEM_ID
                    ,   R_DATA.ITEM_COLOR_ID
                    ,   R_DATA.ITEM_SIZE_ID
                    ,   R_DATA.MAIN_VENDOR_ID AS VENDOR_ID
                    ,   R_DATA.JAN
                 ";
            }
            sql = sql + @"
                FROM
            ";
            sql = sql + targetTable + @" R_DATA";
            sql = sql + @"
                LEFT OUTER JOIN
                        GET_EC_CLASS_NAME GET_NAME
                ON
                        TO_CHAR(R_DATA.IF_ERR_CLASS) = GET_NAME.GEN_CD
                    AND R_DATA.SHIPPER_ID = GET_NAME.SHIPPER_ID
                WHERE
                        R_DATA.IF_RUN_ID = :IF_RUN_ID
                    AND R_DATA.SHIPPER_ID = :SHIPPER_ID
                    AND R_DATA.IF_DB_INSERT_CLASS = 99
                ORDER BY
                        R_DATA.IF_FILE_NAME
                    ,   R_DATA.IF_SEQ
            ";
            parameters.AddDynamicParams(new { IF_RUN_ID = results.IfRunId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId });

            var detailes = MvcDbContext.Current.Database.Connection.Query<NoticeDetailReceiveRow>(
                sql,
                parameters
            ).ToList();

            return detailes;
        }

        /// <summary>
        /// お知らせ連携エラー明細一覧の取得
        /// </summary>
        /// <param name="noticeHeaderId"></param>
        /// <returns></returns>
        public List<NoticeDetailRow> NoticeListing(string noticeHeaderId)
        {
            var parameters = new DynamicParameters();
            var sql = @"
                WITH
                    GET_EC_CLASS_NAME AS (
                        SELECT
                                GEN_CD
                            ,   GEN_NAME
                            ,   SHIPPER_ID
                        FROM
                                M_GENERALS
                        WHERE
                                REGISTER_DIVI_CD = '1'
                            AND GEN_DIV_CD = 'EC_CLASS'
                            AND CENTER_ID = '@@@'
                            AND SHIPPER_ID = :SHIPPER_ID
                )
                SELECT
                        ROW_NUMBER()OVER(ORDER BY T_NOTICE.SEQ) AS ROW_NO
                    ,   GET_NAME.GEN_NAME AS EC_CLASS_NM
                    ,   T_NOTICE.NOTICE_HEADER_ID
                    ,   T_NOTICE.SHIP_ARRIVE_INSTRUCT_ID
                    ,   T_NOTICE.SHIP_FROM_LOC_ID
                    ,   T_NOTICE.ARRIVE_TO_LOC_ID
                    ,   T_NOTICE.ORDER_NO
                    ,   T_NOTICE.PLAN_DATE
                    ,   T_NOTICE.EC_CLASS
                    ,   T_NOTICE.ITEM_SKU_ID
                    ,   T_NOTICE.PLAN_QTY
                    ,   T_NOTICE.ALLOC_QTY
                    ,   T_NOTICE.JET_STOCK_QTY
                    ,   T_NOTICE.ITEM_NAME
                FROM
                        T_NOTICE_DETAILS T_NOTICE
                LEFT OUTER JOIN
                        GET_EC_CLASS_NAME GET_NAME
                ON
                        TO_CHAR(T_NOTICE.EC_CLASS) = GET_NAME.GEN_CD
                    AND T_NOTICE.SHIPPER_ID = GET_NAME.SHIPPER_ID
                WHERE
                        T_NOTICE.NOTICE_HEADER_ID = :NOTICE_HEADER_ID
                    AND T_NOTICE.SHIPPER_ID = :SHIPPER_ID
                ORDER BY
                        T_NOTICE.SEQ
            ";

            parameters.AddDynamicParams(new { NOTICE_HEADER_ID = noticeHeaderId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId });

            var detailes = MvcDbContext.Current.Database.Connection.Query<NoticeDetailRow, NoticeDetail, NoticeDetailRow>(
                sql,
                (noticeDetailRow, noticeDetail) =>
                {
                    noticeDetailRow.NoticeDetail = noticeDetail;
                    return noticeDetailRow;
                },
                parameters,
                splitOn: "NOTICE_HEADER_ID"
            ).ToList();

            return detailes;
        }

        /// <summary>
        /// 日次在庫引き落としマイナス発生エラー時の明細
        /// </summary>
        /// <param name="noticeHeaderId"></param>
        /// <returns></returns>
        public List<NoticeDetailRow> StockNoticeListing(string noticeHeaderId)
        {
            var parameters = new DynamicParameters();
            var sql = @"
                SELECT
                        ROW_NUMBER()OVER(ORDER BY T_NOTICE.SEQ) AS ROW_NO
                    ,   T_NOTICE.NOTICE_HEADER_ID
                    ,   T_NOTICE.SHIP_ARRIVE_INSTRUCT_ID
                    ,   T_NOTICE.SHIP_FROM_LOC_ID
                    ,   T_NOTICE.ORDER_NO
                    ,   T_NOTICE.ITEM_SKU_ID
                    ,   STOCK.STOCK_QTY AS DIFFERENCE_QTY
                FROM
                        T_NOTICE_DETAILS T_NOTICE
                LEFT OUTER JOIN
                        T_STOCKS STOCK
                ON
                        STOCK.ITEM_SKU_ID = T_NOTICE.ITEM_SKU_ID
                    AND STOCK.LOCATION_CD = T_NOTICE.ORDER_NO
                    AND STOCK.CENTER_ID = T_NOTICE.SHIP_FROM_LOC_ID
                    AND STOCK.SHIPPER_ID = T_NOTICE.SHIPPER_ID
                WHERE
                        T_NOTICE.NOTICE_HEADER_ID = :NOTICE_HEADER_ID
                    AND T_NOTICE.SHIPPER_ID = :SHIPPER_ID
                ORDER BY
                        T_NOTICE.SEQ
            ";

            parameters.AddDynamicParams(new { NOTICE_HEADER_ID = noticeHeaderId });
            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId });

            var detailes = MvcDbContext.Current.Database.Connection.Query<NoticeDetailRow, NoticeDetail, NoticeDetailRow>(
                sql,
                (noticeDetailRow, noticeDetail) =>
                {
                    noticeDetailRow.NoticeDetail = noticeDetail;
                    return noticeDetailRow;
                },
                parameters,
                splitOn: "NOTICE_HEADER_ID"
            ).ToList();

            return detailes;
        }

        /// <summary>
        /// 連携実行状態区分名称取得
        /// </summary>
        /// <param name="ifRunState"></param>
        /// <returns></returns>
        public string GetIfRunStateNm(int ifRunState)
        {
            string IfRunState  = ifRunState.ToString();
            return MvcDbContext.Current.Generals
                .Where(m => m.RegisterDiviCd == "1" 
                    && m.GenDivCd == "IF_RUN_STATE" 
                    && m.GenCd == IfRunState
                    && m.CenterId == "@@@"
                    && m.ShipperId == Profile.User.ShipperId)
                .Select(m => m.GenName)
                .SingleOrDefault();
        }

        /// <summary>
        /// お知らせポップアップデータ取得
        /// </summary>
        /// <returns></returns>
        public List<NoticePopUpRow> GetPopUpList()
        {
            var parameters = new DynamicParameters();
            var sql = @"
                SELECT
                        M_NOTICE.MESSAGE_CLASS
                    ,   HEAD.OCCURRENCE_DATE
                    ,   HEAD.SUBJECT
                FROM
                        T_NOTICE_HEADERS HEAD
                INNER JOIN
                        M_NOTICE_IF_MESSAGES M_NOTICE
                ON
                        HEAD.NOTICE_IF_MESSAGE_ID = M_NOTICE.NOTICE_IF_MESSAGE_ID
                    AND HEAD.SHIPPER_ID = M_NOTICE.SHIPPER_ID
                WHERE
                        HEAD.SHIPPER_ID = :SHIPPER_ID
                    AND HEAD.LOC_ID  IN (:SHIPPER_ID, :LOC_ID)
                ORDER BY
                        HEAD.OCCURRENCE_DATE DESC
                    ,   HEAD.NOTICE_HEADER_ID DESC
                OFFSET 0 ROWS FETCH NEXT :MAX_COUNT ROWS ONLY
            ";

            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId });
            parameters.AddDynamicParams(new { LOC_ID = Common.Profile.User.CenterId });
            var maxCount = 0;
            maxCount = Convert.ToInt32(MvcDbContext.Current.Generals
                .Where(m => m.RegisterDiviCd == "1"
                    && m.GenDivCd == "NOTICE_POPUP_MAXROW"
                    && m.GenCd == "1"
                    && m.CenterId == "@@@"
                    && m.ShipperId == Profile.User.ShipperId)
                .Select(m => m.GenName)
                .SingleOrDefault());

            parameters.AddDynamicParams(new { MAX_COUNT = maxCount });

            var notices = MvcDbContext.Current.Database.Connection.Query<NoticePopUpRow>(
                sql,
                parameters
            ).ToList();

            return notices;
        }
    }
}