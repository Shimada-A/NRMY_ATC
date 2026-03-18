namespace Wms.Areas.Ship.Query.TransporterChngEc
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Common;
    using Share.Extensions.Classes;
    using Share.Models;
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.TransporterChngEc;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.TransporterChngEc.TransporterChngEcSearchConditions;

    public class TransporterChngEcQuery
    {
        /// <summary>
        /// Insert Work Table
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertShpTransporterChngEc(TransporterChngEcSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder(@"
                        INSERT INTO WW_SHP_TRANSPORTER_CHNG_EC (
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
                            ,   SHIP_PLAN_DATE
                            ,   SHIP_INSTRUCT_ID
                            ,   ORDER_DATE
                            ,   ARRIVE_REQUEST_DATE
                            ,   TRANSPORTER_ID
                            ,   TRANSPORTER_NAME
                            ,   DEST_ZIP
                            ,   ALLOC_FLAG
                            ,   BATCH_NO
                            ,   BOX_NO
                            ,   UPDATE_COUNT_SHIP
                            ,   UPDATE_COUNT_PACKAGE
                            ,   PROCESS_CLASS
                        )
                        SELECT 
                              " + "SYSTIMESTAMP " + " AS MAKE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS MAKE_USER_ID" +
                              ",'TransporterChngEc'" + " AS MAKE_PROGRAM_NAME" +
                              ",SYSTIMESTAMP " + "AS UPDATE_DATE" +
                              ", '" + Common.Profile.User.UserId + "' AS UPDATE_USER_ID" +
                              ",'TransporterChngEc'" + " AS UPDATE_PROGRAM_NAME" +
                              ",0" + " AS UPDATE_COUNT" +
                              "," + "'" + Common.Profile.User.ShipperId + "'" + " AS SHIPPER_ID" +
                              "," + condition.Seq + " AS SEQ");
                    query.Append(@"
                            ,   ROW_NUMBER()  OVER (ORDER BY TS.CENTER_ID,TS.SHIPPER_ID ,TS.SHIP_INSTRUCT_ID ) AS LINE_NO
                            ,   0                           AS IS_CHECK
                            ,   TS.CENTER_ID
                            ,   MAX(TS.SHIP_REQUEST_DATE)   AS SHIP_REQUEST_DATE
                            ,   TS.SHIP_INSTRUCT_ID
                            ,   MAX(TS.ORDER_DATE)          AS ORDER_DATE
                           
                            ,   MAX(TS.ARRIVE_REQUEST_DATE) AS ARRIVE_REQUEST_DATE ");

                    if (condition.AllocStatus == AllocStatusEnum.UnAlloc)
                    {
                        query.Append(@" 
                            ,   MAX(TS.TRANSPORTER_ID)   AS TRANSPORTER_ID
                            ,   MAX(MT.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                            ,   MAX(TS.DEST_ZIP)            AS DEST_ZIP
                            ,   MAX(TS.ALLOC_FLAG)       AS ALLOC_FLAG
                            ,   NULL                     AS BATCH_NO
                            ,   NULL                     AS BOX_NO
                            ,   MAX(TS.UPDATE_COUNT)     AS UPDATE_COUNT_SHIP
                            ,   NULL                     AS UPDATE_COUNT_PACKAGE
                            ,   1                        AS PROCESS_CLASS
                        ");
                    }
                    else
                    {
                        query.Append(@" 
                            ,   MAX(TSPI.TRANSPORTER_ID)   AS TRANSPORTER_ID
                            ,   MAX(NVL(MT.TRANSPORTER_NAME, MT_SHP.TRANSPORTER_NAME)) AS TRANSPORTER_NAME
                            ,   MAX(TS.DEST_ZIP)           AS DEST_ZIP
                            ,   MAX(TS.ALLOC_FLAG)         AS ALLOC_FLAG
                            ,   MAX(TS.BATCH_NO)           AS BATCH_NO
                            ,   MAX(TSPI.BOX_NO)           AS BOX_NO
                            ,   MAX(TS.UPDATE_COUNT)       AS UPDATE_COUNT_SHIP
                            ,   MAX(TSPI.UPDATE_COUNT)     AS UPDATE_COUNT_PACKAGE
                            ,   2                          AS PROCESS_CLASS
                        ");
                    }
                    query.Append(" FROM T_ECSHIPS TS ");

                    if (condition.AllocStatus == AllocStatusEnum.Alloced)
                    {
                        query.Append(@" 
                            LEFT JOIN
                                    T_SHIP_PACKING_INFO TSPI
                            ON
                                    TS.SHIPPER_ID        = TSPI.SHIPPER_ID
                                AND TS.CENTER_ID         = TSPI.CENTER_ID
                                AND TS.SHIP_INSTRUCT_ID  = TSPI.SHIP_INSTRUCT_ID
                                AND TS.SHIP_INSTRUCT_SEQ = TSPI.SHIP_INSTRUCT_SEQ
                        ");
                    }
       
                    if (condition.AllocStatus == AllocStatusEnum.UnAlloc)
                    {
                        query.Append(@" 
                            LEFT JOIN
                                    M_TRANSPORTERS MT
                            ON
                                    TS.SHIPPER_ID   = MT.SHIPPER_ID
                                AND TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                        ");
                    }
                    else
                    {
                        query.Append(@" 
                            LEFT JOIN
                                    M_TRANSPORTERS MT
                            ON
                                    TSPI.SHIPPER_ID = MT.SHIPPER_ID
                                AND TSPI.TRANSPORTER_ID = MT.TRANSPORTER_ID
                            LEFT JOIN
                                    M_TRANSPORTERS MT_SHP
                            ON
                                    TS.SHIPPER_ID   = MT_SHP.SHIPPER_ID
                                AND TS.TRANSPORTER_ID = MT_SHP.TRANSPORTER_ID
                        ");
                    }
                    query.Append(@"
                         WHERE TS.SHIPPER_ID = :SHIPPER_ID
                           AND TS.CENTER_ID  = :CENTER_ID
                           AND TS.TRANSPORTER_ID  IN ('01','09') ");

                    if (condition.AllocStatus == AllocStatusEnum.UnAlloc)
                    {
                        query.Append(" AND TS.ALLOC_FLAG = 0 ");
                    }
                    else
                    {
                        query.Append(@" AND TS.ALLOC_FLAG = 1");
                        query.Append(@" AND TS.KAKU_FLAG = 0");
                        query.Append(@" AND NVL(TSPI.DELI_PRN_FLAG, 0) = 0 ");
                    }
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);


                    // Add search condition
                    // 受信日時(From-To)
                    if (condition.AllocDateFrom != null)
                    {
                        query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') >= :ALLOC_DATE_TIME_FROM ");
                        parameters.Add(":ALLOC_DATE_TIME_FROM", condition.AllocTimeFrom == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " 00:00:00" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateFrom) + " " + condition.AllocTimeFrom);
                    }

                    if (condition.AllocDateTo != null)
                    {
                        query.Append(" AND TO_CHAR(TS.MAKE_DATE,'yyyy/MM/dd hh24:mi:ss') <= :ALLOC_DATE_TIME_TO ");
                        parameters.Add(":ALLOC_DATE_TIME_TO", condition.AllocTimeTo == null ?
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " 23:59:59" :
                                                                string.Format("{0:yyyy/MM/dd}", condition.AllocDateTo) + " " + condition.AllocTimeTo);
                    }

                    // 出荷予定日(From-To)
                    if (condition.ShipPlanDateFrom != null)
                    {
                        query.Append(" AND TS.SHIP_REQUEST_DATE >= :SHIP_PLAN_DATE_FROM ");
                        parameters.Add(":SHIP_PLAN_DATE_FROM", condition.ShipPlanDateFrom);
                    }

                    if (condition.ShipPlanDateTo != null)
                    {
                        query.Append(" AND TS.SHIP_REQUEST_DATE <= :SHIP_PLAN_DATE_TO ");
                        parameters.Add(":SHIP_PLAN_DATE_TO", condition.ShipPlanDateTo);
                    }

                    // 納品日
                    if (condition.ArriveRequestDate != null)
                    {
                        query.Append(" AND TS.ARRIVE_REQUEST_DATE = :ARRIVE_REQUEST_DATE ");
                        parameters.Add(":ARRIVE_REQUEST_DATE", condition.ArriveRequestDate);
                    }

                    //変更前の配送者コード
                    if (condition.SearchTransporterId != null)
                    {
                        query.Append(" AND TS.TRANSPORTER_ID = :TRANSPORTER_ID ");
                        parameters.Add(":TRANSPORTER_ID", condition.SearchTransporterId);
                    }

                    // 配送エリア
                    if (!string.IsNullOrEmpty(condition.TransporterArea))
                    {
                        string[] transporterAreas = (condition.TransporterArea ?? condition.TransporterArea).Split(',');
                        query.Append(" AND TS.PREF_ID IN :PREF_ID ");
                        parameters.Add(":PREF_ID", transporterAreas);
                    }

                    // 出荷指示ID
                    if (!string.IsNullOrEmpty(condition.ShipInstructId))
                    {
                        query.Append(" AND TS.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                        parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId);
                    }

                    // ケースNo
                    if (!string.IsNullOrEmpty(condition.BoxNo) && condition.AllocStatus == AllocStatusEnum.Alloced)
                    {
                        query.Append(" AND TSPI.BOX_NO = :BOX_NO ");
                        parameters.Add(":BOX_NO", condition.BoxNo);
                    }
                    query.Append(@" 
                        GROUP BY 
                                 TS.CENTER_ID
                                ,TS.SHIPPER_ID 
                                ,TS.SHIP_INSTRUCT_ID
                    ");

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
        /// Get StockInquiry List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public IPagedList<TransporterChngEcResultRow> GetData(TransporterChngEcSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT *
                  FROM WW_SHP_TRANSPORTER_CHNG_EC
                 WHERE SHIPPER_ID = :SHIPPER_ID
                   AND SEQ = :SEQ
                 ORDER BY
                        SHIP_INSTRUCT_ID ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<TransporterChngEcResultRow>(query.ToString(), parameters).Count();

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var TransporterChngEcs = MvcDbContext.Current.Database.Connection.Query<TransporterChngEcResultRow>(query.ToString(), parameters);
            condition.SelectedCnt = MvcDbContext.Current.ShpTransporterChngEcs.Where(x => x.Seq == condition.Seq && x.ShipperId == Profile.User.ShipperId && x.IsCheck).Count();

            // Excute paging
            return new StaticPagedList<TransporterChngEcResultRow>(TransporterChngEcs, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// Update Work Table
        /// </summary>
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool UpdateShpTransporterChng(IList<SelectedTransporterChngEcViewModel> ShpTransporterChngEcs)
        {
            DynamicParameters parameters = new DynamicParameters();
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var u in ShpTransporterChngEcs)
                {
                    // 在庫明細
                    var shpTransporterChng = MvcDbContext.Current.ShpTransporterChngEcs
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == u.Seq && m.LineNo == u.LineNo)
                                  .SingleOrDefault();

                    if (shpTransporterChng == null)
                    {
                        return false;
                    }

                    shpTransporterChng.SetBaseInfoUpdate();
                    shpTransporterChng.IsCheck = u.IsCheck;
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
        /// <param name="StockInquirys"></param>
        /// <returns></returns>
        public bool ShpTransporterChngAllChange(TransporterChngEcSearchConditions conditions, bool check)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query;
                    query = new StringBuilder(@"
                        UPDATE  WW_SHP_TRANSPORTER_CHNG_EC
                        SET
                                UPDATE_DATE = :UPDATE_DATE
                            ,   UPDATE_USER_ID = :UPDATE_USER_ID
                            ,   UPDATE_PROGRAM_NAME = 'TransporterChng'
                            ,   UPDATE_COUNT = UPDATE_COUNT + 1
                            ,   IS_CHECK = :IS_CHECK
                        WHERE
                                SEQ = :SEQ
                            AND SHIPPER_ID = :SHIPPER_ID
                    ");
                    parameters.Add(":UPDATE_DATE", DateTimeOffset.Now);
                    parameters.Add(":UPDATE_USER_ID", Profile.User.UserId);
                    parameters.Add(":IS_CHECK", (check == true) ? 1 : 0);
                    parameters.Add(":SEQ", conditions.Seq);
                    parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                    var result = MvcDbContext.Current.Database.Connection.Execute(query.ToString(), parameters);
                }
                catch (DbUpdateException ex) when (OracleExtention.DuplicateValueOnIndexException(ex))
                {
                    return false;
                }
                trans.Commit();
            }

            conditions.SelectedCnt = MvcDbContext.Current.ShpTransporterChngs
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == conditions.Seq && m.IsCheck).Count();
            return true;
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void TransporterChange(TransporterChngEcSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", searchConditions.Seq, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_PROCESS_CLASS", searchConditions.AllocStatus, DbType.Int32, ParameterDirection.Input);
            param.Add("IN_CHANGED_TRANSPORTER_ID", searchConditions.ChangedTransporterId, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_TRANSPORTER_CHNG1_EC",
                param,
                commandType: CommandType.StoredProcedure); 
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 実績更新
        /// </summary>
        public void TransporterChange2(TransporterChngEcSearchConditions searchConditions, out ProcedureStatus status, out string message)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", searchConditions.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", searchConditions.CaseNo, DbType.String, ParameterDirection.Input);
            param.Add("IN_CHANGED_TRANSPORTER_ID", searchConditions.ChangedTransporterIdPrint, DbType.String, ParameterDirection.Input);
            param.Add("IN_SIZE", searchConditions.Size, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_TRANSPORTER_CHNG2_EC",
                param,
                commandType: CommandType.StoredProcedure);
            status = param.Get<ProcedureStatus>("OUT_STATUS");
            message = param.Get<string>("OUT_MESSAGE");
        }

        /// <summary>
        /// 事業部データ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListTransporters()
        {
            return MvcDbContext.Current.Transporters
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.TransporterId.ToString(),
                    Text = m.TransporterName
                })
                .OrderBy(m => m.Value);
        }

        /// <summary>
        /// サイズチェック
        /// </summary>
        public int CheckSize(string size)
        {
            return MvcDbContext.Current.Generals
                .Where(m => m.ShipperId == Profile.User.ShipperId 
                         && m.RegisterDiviCd == "1" 
                         && m.CenterId == "@@@" 
                         && m.GenDivCd == "YAMATO_BOX_SIZE" 
                         && m.GenCd == size)
                .Count();
        }
    }
}