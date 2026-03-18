namespace Wms.Areas.Others.Query.WorkReference
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using Wms.Areas.Others.Models;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Others.Resources;
    using Wms.Areas.Others.ViewModels.WorkReference;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Others.ViewModels.WorkReference.WorkReferenceSearchConditions;
    using Wms.Areas.Master.Models;
    using System;

    public class WorkReferenceQuery : BaseQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertArrWorkReference(WorkReferenceSearchConditions condition)
        {
            condition.Seq = new BaseQuery().GetWorkId();
            condition.Page = 1;

            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                    INSERT INTO WW_OTH_WORK_PERF (
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
                        ,   PROCESSING_ID
                        ,   PROCESSING_TYPE
                        ,   PROCESSING_NAME
                        ,   WORK_USER_ID
                        ,   WORK_USER_NAME
                        ,   WORK_START_DATE
                        ,   WORK_END_DATE
                        ,   WORK_BREAK_TIME
                        ,   WORK_TIME
                        ,   WORK_QTY
                        ,   WORK_STATUS
                        ,   WORK_STATUS_NAME
                        ,   PROCESSING_MESSAGE
                    )
                    WITH
                        WORK_TBL01 AS ( -- 各処理IDの作業開始日時を抽出
                           SELECT
                                   SHIPPER_ID
                               ,   CENTER_ID
                               ,   PROCESSING_ID
                               ,   MAX(MAKE_DATE) AS WORK_START_DATE
                           FROM
                                   L_INSPECTION_LOG
                           WHERE
                                   PROCESSING_DETAIL = '01'
                               AND SHIPPER_ID = :SHIPPER_ID
                           GROUP BY
                                   SHIPPER_ID
                               ,   CENTER_ID
                               ,   PROCESSING_ID
                    )
                    ,   WORK_TBL02 AS ( -- 各処理IDの作業完了日時を抽出
                           SELECT
                                   SHIPPER_ID
                               ,   CENTER_ID
                               ,   PROCESSING_ID
                               ,   MAX(MAKE_DATE) AS WORK_END_DATE
                               ,   MAX(PROCESSING_MESSAGE) AS PROCESSING_MESSAGE
                           FROM
                                   L_INSPECTION_LOG
                           WHERE
                                   PROCESSING_DETAIL = '02'
                               AND SHIPPER_ID = :SHIPPER_ID
                           GROUP BY
                                   SHIPPER_ID
                               ,   CENTER_ID
                               ,   PROCESSING_ID
                    )
                    ,   WORK_TBL03_TMP AS ( -- 各処理IDの中断時間を算出する際に使用
                            SELECT
                                    SHIPPER_ID
                                ,   CENTER_ID
                                ,   MAKE_DATE
                                ,   PROCESSING_ID
                                ,   PROCESSING_DETAIL
                                ,   ROW_NUMBER() OVER(ORDER BY SHIPPER_ID ASC,CENTER_ID ASC,PROCESSING_ID ASC,MAKE_DATE ASC) NUM     
                            FROM
                                    L_INSPECTION_LOG
                            WHERE
                                    SHIPPER_ID = :SHIPPER_ID
                    )
                    ,   WORK_SECOND AS (  -- 中断時間を秒数で換算
                            SELECT
                                    T04.SHIPPER_ID
                                ,   T04.CENTER_ID
                                ,   T04.PROCESSING_ID
                                ,   SUM((EXTRACT(DAY FROM (T04.MAKE_DATE - T03.MAKE_DATE)) * 24 * 60 * 60)
                                        + (EXTRACT(HOUR FROM (T04.MAKE_DATE - T03.MAKE_DATE)) * 60 * 60)
                                        + (EXTRACT(MINUTE FROM (T04.MAKE_DATE - T03.MAKE_DATE)) * 60)
                                        + EXTRACT(SECOND FROM (T04.MAKE_DATE - T03.MAKE_DATE))) AS SUM_BREAK_SECOND
                            FROM (
                                    SELECT * FROM WORK_TBL03_TMP WHERE PROCESSING_DETAIL = '04'
                            )  T04
                            INNER JOIN (
                                    SELECT * FROM WORK_TBL03_TMP WHERE PROCESSING_DETAIL = '03'
                            )  T03
                            ON  
                                    T03.PROCESSING_ID = T04.PROCESSING_ID
                                AND T03.NUM = T04.NUM - 1
                            WHERE
                                    T04.SHIPPER_ID = :SHIPPER_ID
                            GROUP BY
                                    T04.SHIPPER_ID
                                ,   T04.CENTER_ID
                                ,   T04.PROCESSING_ID
                        )
                     ,   WORK_TBL03 AS ( -- 各処理IDの中断時間を算出
                            SELECT
                                    SHIPPER_ID
                                ,   CENTER_ID
                                ,   PROCESSING_ID
                                ,   TO_CHAR(TRUNC(SUM_BREAK_SECOND / (60 * 60)), 'FM9999')
                                    || ':' || 
                                    TO_CHAR(TRUNC(MOD(SUM_BREAK_SECOND, (60 * 60)) / 60), 'FM09')
                                    || ':' || 
                                    TO_CHAR(MOD(MOD(SUM_BREAK_SECOND, (60 * 60)), 60), 'FM09') AS WORK_BREAK_TIME
                                ,   TRUNC(SUM_BREAK_SECOND / (60 * 60)) AS BREAK_HOUR
                                ,   TRUNC(MOD(SUM_BREAK_SECOND, (60 * 60)) / 60) AS BREAK_MINUTE
                                ,   MOD(MOD(SUM_BREAK_SECOND, (60 * 60)), 60) AS BREAK_SECOND
                            FROM
                                    WORK_SECOND
                        )
                    ,   WORK_TBL04 AS (
                            SELECT
                                    LIL.SHIPPER_ID
                                ,   LIL.CENTER_ID
                                ,   LIL.PROCESSING_ID
                                ,   CASE 
                                        WHEN LIL.PROCESSING_DETAIL = '01' THEN '1'
                                        WHEN LIL.PROCESSING_DETAIL = '02' THEN '3'
                                        WHEN LIL.PROCESSING_DETAIL = '03' THEN '2'
                                        WHEN LIL.PROCESSING_DETAIL = '04' THEN '1'
                                    END AS WORK_STATUS
                            FROM 
                                    L_INSPECTION_LOG LIL
                            INNER JOIN (
                                    SELECT
                                            SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   PROCESSING_ID
                                        ,   MAX(MAKE_DATE) AS MAX_MAKE_DATE
                                    FROM
                                        L_INSPECTION_LOG
                                    GROUP BY
                                            SHIPPER_ID
                                        ,   CENTER_ID
                                        ,   PROCESSING_ID
                            ) TMP
                            ON
                                    TMP.SHIPPER_ID = LIL.SHIPPER_ID
                                AND TMP.CENTER_ID = LIL.CENTER_ID
                                AND TMP.PROCESSING_ID = LIL.PROCESSING_ID
                                AND TMP.MAX_MAKE_DATE = LIL.MAKE_DATE
                            WHERE
                                    LIL.SHIPPER_ID = :SHIPPER_ID
                                   
                    )
                    ,   WORK_TBL05 AS (
                            SELECT
                                    LIL.CENTER_ID AS CENTER_ID
                                ,   LIL.PROCESSING_ID AS PROCESSING_ID
                                ,   MAX(LIL.PROCESSING_TYPE) AS PROCESSING_TYPE
                                ,   MAX(MO.OPERATION_NAME) AS PROCESSING_NAME
                                ,   MAX(LIL.MAKE_USER_ID) AS WORK_USER_ID
                                ,   MAX(MU.USER_NAME) AS WORK_USER_NAME
                                ,   MAX(W01.WORK_START_DATE) AS WORK_START_DATE
                                ,   MAX(W02.WORK_END_DATE) AS WORK_END_DATE
                                ,   MAX(W03.WORK_BREAK_TIME) AS WORK_BREAK_TIME
                                ,   CASE WHEN MAX(W02.WORK_END_DATE) IS NOT NULL
                                        THEN (MAX(W02.WORK_END_DATE) - NUMTODSINTERVAL(NVL2(MAX(W03.BREAK_HOUR),MAX(W03.BREAK_HOUR),'0'),'HOUR')
                                                                     - NUMTODSINTERVAL(NVL2(MAX(W03.BREAK_MINUTE),MAX(W03.BREAK_MINUTE),'0'),'MINUTE')
                                                                     - NUMTODSINTERVAL(NVL2(MAX(W03.BREAK_SECOND),MAX(W03.BREAK_SECOND),'0'),'SECOND')
                                             ) - MAX(W01.WORK_START_DATE)
                                        ELSE NULL
                                    END AS WORK_TIME
                                ,   SUM(LIL.RESULT_QTY) AS WORK_QTY
                                ,   MAX(W04.WORK_STATUS) AS WORK_STATUS
                                ,   MAX(WORK_STATUS.GEN_NAME) AS WORK_STATUS_NAME
                                ,   MAX(W02.PROCESSING_MESSAGE) AS PROCESSING_MESSAGE

                            FROM
                                    L_INSPECTION_LOG LIL
                            LEFT OUTER JOIN
                                    WORK_TBL01 W01
                            ON 
                                    W01.SHIPPER_ID = LIL.SHIPPER_ID
                                AND W01.CENTER_ID = LIL.CENTER_ID
                                AND W01.PROCESSING_ID = LIL.PROCESSING_ID
                            LEFT OUTER JOIN
                                    WORK_TBL02 W02
                            ON 
                                    W02.SHIPPER_ID = LIL.SHIPPER_ID
                                AND W02.CENTER_ID = LIL.CENTER_ID
                                AND W02.PROCESSING_ID = LIL.PROCESSING_ID
                            LEFT OUTER JOIN
                                    WORK_TBL03 W03
                            ON 
                                    W03.SHIPPER_ID = LIL.SHIPPER_ID
                                AND W03.CENTER_ID = LIL.CENTER_ID
                                AND W03.PROCESSING_ID = LIL.PROCESSING_ID
                            LEFT OUTER JOIN
                                    WORK_TBL04 W04
                            ON 
                                    W04.SHIPPER_ID = LIL.SHIPPER_ID
                                AND W04.CENTER_ID = LIL.CENTER_ID
                                AND W04.PROCESSING_ID = LIL.PROCESSING_ID
                            LEFT OUTER JOIN
                                    M_OPERATIONS MO
                            ON 
                                    MO.SHIPPER_ID = LIL.SHIPPER_ID
                                AND MO.OPERATION_ID = LIL.PROCESSING_TYPE
                            LEFT OUTER JOIN
                                    M_USERS MU
                            ON 
                                    MU.SHIPPER_ID = LIL.SHIPPER_ID
                                AND MU.USER_ID = LIL.MAKE_USER_ID
                            LEFT OUTER JOIN
                                    M_GENERALS WORK_STATUS
                            ON 
                                    WORK_STATUS.SHIPPER_ID = LIL.SHIPPER_ID
                                AND WORK_STATUS.CENTER_ID = '@@@'
                                AND WORK_STATUS.REGISTER_DIVI_CD = '1'
                                AND WORK_STATUS.GEN_DIV_CD = 'OPERATION_STATUS'
                                AND WORK_STATUS.GEN_CD = TO_CHAR(W04.WORK_STATUS)
                    WHERE
                            LIL.SHIPPER_ID = :SHIPPER_ID
                    ");
                
                    // センター
                    if (!string.IsNullOrEmpty(condition.CenterId))
                    {
                        query.AppendLine("    AND LIL.CENTER_ID = :CENTER_ID ");
                        parameters.Add(":CENTER_ID", condition.CenterId);
                    }

                    query.Append(@"
                    GROUP BY
                            LIL.SHIPPER_ID
                          , LIL.CENTER_ID
                          , LIL.PROCESSING_ID
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
                        ,   ROWNUM
                        ,   W05.CENTER_ID
                        ,   W05.PROCESSING_ID
                        ,   W05.PROCESSING_TYPE
                        ,   W05.PROCESSING_NAME
                        ,   W05.WORK_USER_ID
                        ,   W05.WORK_USER_NAME
                        ,   W05.WORK_START_DATE
                        ,   W05.WORK_END_DATE
                        ,   W05.WORK_BREAK_TIME
                        ,   CASE WHEN W05.WORK_TIME IS NOT NULL
                                THEN TO_CHAR(EXTRACT(DAY FROM W05.WORK_TIME) * 24 
                                     + EXTRACT(HOUR FROM W05.WORK_TIME),'FM09')
                                     || ':'
                                     ||TO_CHAR(EXTRACT(MINUTE FROM W05.WORK_TIME),'FM09') 
                                     || ':'
                                     ||TO_CHAR(EXTRACT(SECOND FROM W05.WORK_TIME),'FM09')
                                ELSE NULL
                            END
                        ,   W05.WORK_QTY
                        ,   W05.WORK_STATUS
                        ,   W05.WORK_STATUS_NAME
                        ,   W05.PROCESSING_MESSAGE
                    FROM
                       WORK_TBL05 W05
                    WHERE
                       1 = 1
                    ");

                    parameters.Add(":USER_ID", Common.Profile.User.UserId);
                    parameters.Add(":PROGRAM_NAME", "InsertArrWorkReference");
                    parameters.Add(":SEQ", condition.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

                    // 受信日時(From-To)
                    if (condition.WorkStartDateFrom != null)
                    {
                        query.Append(" AND TO_CHAR(W05.WORK_START_DATE,'yyyy/MM/dd hh24:mi:ss') >= :WORK_START_DATE_TIME_FROM ");
                        parameters.Add(":WORK_START_DATE_TIME_FROM", condition.WorkStartTimeFrom == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.WorkStartDateFrom) + " 00:00:00" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.WorkStartDateFrom) + " " + condition.WorkStartTimeFrom);
                    }

                    if (condition.WorkStartDateTo != null)
                    {
                        query.Append(" AND TO_CHAR(W05.WORK_START_DATE,'yyyy/MM/dd hh24:mi:ss') <= :WORK_START_DATE_TIME_TO ");
                        parameters.Add(":WORK_START_DATE_TIME_TO", condition.WorkStartTimeTo == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.WorkStartDateTo) + " 23:59:59" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.WorkStartDateTo) + " " + condition.WorkStartTimeTo);
                    }

                    // 作業種別
                    if (!string.IsNullOrEmpty(condition.ProcessingType))
                    {
                        query.AppendLine("    AND W05.PROCESSING_TYPE = :PROCESSING_TYPE ");
                        parameters.Add(":PROCESSING_TYPE", condition.ProcessingType);
                    }

                    // 作業者
                    if (!string.IsNullOrEmpty(condition.WorkUserId))
                    {
                        query.Append(" AND W05.WORK_USER_ID LIKE :WORK_USER_ID ");
                        parameters.Add(":WORK_USER_ID", condition.WorkUserId + "%");
                    }

                    if (string.IsNullOrEmpty(condition.WorkUserId) && !string.IsNullOrEmpty(condition.WorkUserName))
                    {
                        query.Append(@" AND W05.WORK_USER_NAME LIKE :WORK_USER_NAME");
                        parameters.Add(":WORK_USER_NAME", "%" + condition.WorkUserName + "%");
                    }

                    // 作業状況
                    if (!string.IsNullOrEmpty(condition.WorkStatus))
                    {
                        query.AppendLine("    AND W05.WORK_STATUS = :WORK_STATUS ");
                        parameters.Add(":WORK_STATUS", condition.WorkStatus);
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
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<WorkReferenceResultRow> WorkReferenceGetData(WorkReferenceSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
            SELECT
                    *
            FROM
                    WW_OTH_WORK_PERF WW
            WHERE
                    WW.SHIPPER_ID = :SHIPPER_ID
                AND WW.SEQ = :SEQ
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<WorkReferenceResultRow>(query.ToString(), parameters).Count();

            // Sort function
            switch (condition.SortKey)
            {
                case WorkReferenceSortKey.ProcessingTypeWorkStartDate:
                    switch (condition.Sort)
                    {
                        case WorkReferenceSearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY PROCESSING_TYPE DESC,WORK_START_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY PROCESSING_TYPE ASC,WORK_START_DATE ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case WorkReferenceSearchConditions.AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY WORK_START_DATE DESC ");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY WORK_START_DATE ASC ");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var workReferences = MvcDbContext.Current.Database.Connection.Query<WorkReferenceResultRow>(query.ToString(), parameters);

            // Excute paging
            return new StaticPagedList<WorkReferenceResultRow>(workReferences, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// 処理種別取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListProcessingType()
        {
            return MvcDbContext.Current.Operations
                .Where(m => m.ShipperId == Common.Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.OperationId,
                    Text = m.OperationName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// 作業種別取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListWorkStatus()
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
                    AND GEN_DIV_CD  = 'OPERATION_STATUS'
                ORDER BY
                        ORDER_NO
            ");

            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);

            // 全レコード数を取得
            return MvcDbContext.Current.Database.Connection.Query<SelectListItem>(query.ToString(), parameters);
        }

        /// <summary>
        /// センターリスト取得
        /// </summary>
        /// <returns>ドロップダウンリストデータ</returns>
        public IEnumerable<SelectListItem> GetCenterListItem()
        {
            return MvcDbContext.Current.Warehouses.Where(m => m.ShipperId == Common.Profile.User.ShipperId && !m.DeleteFlag)
                .Select(m => new SelectListItem
                {
                    Value = m.CenterId,
                    Text = m.CenterId + ":" + m.CenterName1
                }).OrderBy(m => m.Value);
        }
    }
}