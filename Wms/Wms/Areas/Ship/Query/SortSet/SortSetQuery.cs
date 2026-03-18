namespace Wms.Areas.Ship.Query.SortSet
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
    using Wms.Areas.Ship.Models;
    using Wms.Areas.Ship.Resources;
    using Wms.Areas.Ship.ViewModels.SortSet;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;
    using static Wms.Areas.Ship.ViewModels.SortSet.SortSetSearchConditions;

    public class SortSetQuery
    {
        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="condition">Search Information</param>
        /// <returns></returns>
        public bool InsertWorkData(SortSetSearchConditions condition)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    StringBuilder query = new StringBuilder();

                    // 1.ワークID採番
                    condition.Seq = new BaseQuery().GetWorkId();
                    condition.Page = 1;

                    query.Append(@"
                        INSERT INTO WW_SHP_SORTING_CHNG (
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
                            ,   TABLE_CLASS
                            ,   TRANSPORTER_ID
                            ,   PREF_NAME
                            ,   CITY_NAME
                            ,   CLASS
                            ,   SHIP_PLAN_DATE
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_NAME
                            ,   SHIP_TO_ZIP
                            ,   SHIP_TO_ADDRESS
                            ,   SORTING_CD
                            ,   SORTING_CD_HID
                            ,   ADDRESS1
                        )
                        WITH
                            SELECTED_SHIP_DATA AS (
                                SELECT
                                        1 AS TABLE_CLASS
                                    ,   TS.TRANSPORTER_ID
                                    ,   MAX(VSTS.SHIP_TO_PREF_NAME) AS PREF_NAME
                                    ,   MAX(VSTS.SHIP_TO_CITY_NAME) AS CITY_NAME
                                    ,   MAX(VSTS.SHIP_TO_ADDRESS1) AS ADDRESS1
                                    ,   'BtoB' CLASS
                                    ,   MAX(TS.SHIP_PLAN_DATE) AS SHIP_PLAN_DATE
                                    ,   TS.SHIP_INSTRUCT_ID
                                    ,   TS.SHIP_TO_STORE_ID
                                    ,   MAX(VSTS.SHIP_TO_STORE_NAME1) AS SHIP_TO_STORE_NAME
                                    ,   MAX(MT.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                                    ,   MAX(VSTS.SHIP_TO_ZIP) AS SHIP_TO_ZIP
                                    ,   MAX(VSTS.SHIP_TO_PREF_NAME || VSTS.SHIP_TO_CITY_NAME || VSTS.SHIP_TO_ADDRESS1 || VSTS.SHIP_TO_ADDRESS2 || VSTS.SHIP_TO_ADDRESS3) AS SHIP_TO_ADDRESS
                                    ,   TS.CENTER_ID
                                    ,   '' AS DELI_SHIWAKE_CD
                                    ,   SF_GET_SORTING_CD_TRANSPORTER(:SHIPPER_ID, TS.SHIP_TO_STORE_ID, TS.TRANSPORTER_ID) AS SORTING_CD_HID
                                FROM
                                        T_SHIPS TS
                                INNER JOIN
                                        V_SHIP_TO_STORES VSTS
                                ON
                                        TS.SHIP_TO_STORE_ID = VSTS.SHIP_TO_STORE_ID
                                    AND TS.SHIPPER_ID   = VSTS.SHIPPER_ID
                                INNER JOIN
                                        M_TRANSPORTERS MT
                                ON
                                        TS.SHIPPER_ID   = MT.SHIPPER_ID
                                    AND TS.TRANSPORTER_ID = MT.TRANSPORTER_ID
                                WHERE
                                        TS.SHIPPER_ID = :SHIPPER_ID
                                    AND TS.CENTER_ID  = :CENTER_ID
                                    AND TS.DELI_SHIWAKE_CD = ' '
                                GROUP BY
                                        TS.CENTER_ID
                                    ,   TS.SHIP_INSTRUCT_ID
                                    ,   TS.SHIP_TO_STORE_ID
                                    ,   TS.TRANSPORTER_ID
                        )
                        ,   SELECTED_ECSHIP_DATA AS (
                                SELECT
                                        2 AS TABLE_CLASS
                                    ,   MAX(TE.TRANSPORTER_ID) AS TRANSPORTER_ID
                                    ,   MAX(TE.DEST_PREF_NAME) AS PREF_NAME
                                    ,   MAX(TE.DEST_CITY_NAME) AS CITY_NAME
                                    ,   MAX(TE.DEST_ADDRESS1) AS ADDRESS1
                                    ,   'EC' CLASS
                                    ,   MAX(TE.ORDER_DATE) AS SHIP_PLAN_DATE
                                    ,   TE.SHIP_INSTRUCT_ID
                                    ,   N'' SHIP_TO_STORE_ID
                                    ,   N'' SHIP_TO_STORE_NAME
                                    ,   MAX(MT.TRANSPORTER_NAME) AS TRANSPORTER_NAME
                                    ,   MAX(TE.DEST_ZIP) AS SHIP_TO_ZIP
                                    ,   MAX(TE.DEST_PREF_NAME || TE.DEST_CITY_NAME || TE.DEST_ADDRESS1 || TE.DEST_ADDRESS2 || TE.DEST_ADDRESS3) AS SHIP_TO_ADDRESS
                                    ,   TE.CENTER_ID
                                    ,   '' AS DELI_SHIWAKE_CD
                                    ,   SF_GET_SORTING_CD_EC(
                                                :SHIPPER_ID
                                            ,   TE.CENTER_ID
                                            ,   MAX(TE.TRANSPORTER_ID)
                                            ,   MAX(TE.DEST_ZIP)
                                            ,   MAX(TE.DEST_PREF_NAME)
                                            ,   MAX(TE.DEST_CITY_NAME)
                                            ,   MAX(TE.DEST_ADDRESS1)
                                        ) AS SORTING_CD_HID
                                FROM
                                        T_ECSHIPS TE
                                INNER JOIN
                                        M_TRANSPORTERS MT
                                ON
                                        TE.SHIPPER_ID   = MT.SHIPPER_ID
                                    AND TE.TRANSPORTER_ID = MT.TRANSPORTER_ID
                                WHERE
                                        TE.SHIPPER_ID = :SHIPPER_ID
                                    AND TE.CENTER_ID  = :CENTER_ID
                                    AND TE.DELI_SHIWAKE_CD = ' ' 
                                GROUP BY
                                        TE.CENTER_ID
                                    ,   TE.SHIP_INSTRUCT_ID
                        )
                        ,   TARGET_DATA AS (
                                SELECT * FROM SELECTED_SHIP_DATA
                                UNION
                                SELECT * FROM SELECTED_ECSHIP_DATA
                        )
                        SELECT
                                :MAKE_DATE
                            ,   :MAKE_USER_ID
                            ,   'SortSet'
                            ,   :MAKE_DATE
                            ,   :MAKE_USER_ID
                            ,   'SortSet'
                            ,   0
                            ,   :SHIPPER_ID
                            ,   :SEQ
                            ,   ROWNUM
                            ,   CENTER_ID
                            ,   TABLE_CLASS
                            ,   TRANSPORTER_ID
                            ,   PREF_NAME
                            ,   CITY_NAME
                            ,   CLASS
                            ,   SHIP_PLAN_DATE
                            ,   SHIP_INSTRUCT_ID
                            ,   SHIP_TO_STORE_ID
                            ,   SHIP_TO_STORE_NAME
                            ,   TRANSPORTER_NAME
                            ,   SHIP_TO_ZIP
                            ,   SHIP_TO_ADDRESS
                            ,   DELI_SHIWAKE_CD
                            ,   SORTING_CD_HID
                            ,   ADDRESS1
                        FROM
                                TARGET_DATA
                    ");
                    parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
                    parameters.Add(":CENTER_ID", condition.CenterId);
                    parameters.Add(":MAKE_DATE", DateTimeOffset.Now);
                    parameters.Add(":MAKE_USER_ID", Profile.User.UserId);
                    parameters.Add(":SEQ", condition.Seq);

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
        /// 一覧データ取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IPagedList<SortSetResultRow> GetData(SortSetSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder(@"
                SELECT
                        *
                FROM
                        WW_SHP_SORTING_CHNG
                WHERE
                        SEQ = :SEQ
                    AND SHIPPER_ID = :SHIPPER_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":SEQ", condition.Seq);

            // 全レコード数を取得
            int totalCount = MvcDbContext.Current.Database.Connection.Query<SortSetResultRow>(query.ToString(), parameters).Count();

            condition.TotalCount = totalCount;

            // Sort function
            switch (condition.SortKey)
            {
                case SortSetSortKey.SortId:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID DESC, SHIP_PLAN_DATE DESC, SHIP_TO_STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_INSTRUCT_ID ASC, SHIP_PLAN_DATE ASC, SHIP_TO_STORE_ID ASC");
                            break;
                    }

                    break;

                default:
                    switch (condition.Sort)
                    {
                        case AscDescSort.Desc:
                            query.AppendLine(@"ORDER BY SHIP_PLAN_DATE DESC, SHIP_INSTRUCT_ID DESC, SHIP_TO_STORE_ID DESC");
                            break;

                        default:
                            query.AppendLine(@"ORDER BY SHIP_PLAN_DATE ASC, SHIP_INSTRUCT_ID ASC, SHIP_TO_STORE_ID ASC");
                            break;
                    }

                    break;
            }

            query.AppendLine(" OFFSET :OFFSET ROWS FETCH NEXT :PAGE_SIZE ROWS ONLY");
            parameters.Add(":PAGE_SIZE", condition.PageSize);

            // Choose data corresponding on each page
            parameters.AddDynamicParams(new { OFFSET = (condition.Page - 1) * condition.PageSize });

            // Fill data to memory
            var SortSets = MvcDbContext.Current.Database.Connection.Query<SortSetResultRow>(query.ToString(), parameters);
            // Excute paging
            return new StaticPagedList<SortSetResultRow>(SortSets, condition.Page, condition.PageSize, totalCount);
        }

        /// <summary>
        /// ワークデータ更新(入力値保持)
        /// </summary>
        /// <param name="SortSets"></param>
        /// <returns></returns>
        public bool UpdateWorkData(IList<SortSetResultRow> SortSets)
        {
            using (var trans = MvcDbContext.Current.Database.BeginTransaction())
            {
                foreach (var cModel in SortSets
                                            .Where(x => !string.IsNullOrWhiteSpace(x.SortingCd) || (x.TableClass == 2 && !string.IsNullOrWhiteSpace(x.ShipToZip)))
                                            .Select((v, i) => new { v, i }))
                {
                    var sort_set_wk = MvcDbContext.Current.ShpSortingChngs
                                  .Where(m => m.ShipperId == Common.Profile.User.ShipperId && m.Seq == cModel.v.Seq && m.LineNo == cModel.v.LineNo)
                                  .SingleOrDefault();
                    if (sort_set_wk == null)
                    {
                        return false;
                    }

                    sort_set_wk.SetBaseInfoUpdate();
                    sort_set_wk.ShipToZip = cModel.v.ShipToZip;
                    sort_set_wk.SortingCd = cModel.v.SortingCd;
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
        /// 実績更新
        /// </summary>
        public void SortSet(long seq, string centerId, out ProcedureStatus status, out string message)
        {
            var tempMsg = string.Empty;
            var tempStatus = ProcedureStatus.Success;

            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", centerId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_WORK_ID", seq, DbType.Int32, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "SP_W_SHP_SORTING_CD_SETL",
                param,
                commandType: CommandType.StoredProcedure);
            tempStatus = param.Get<ProcedureStatus>("OUT_STATUS");
            if (tempStatus != ProcedureStatus.Success)
            {
                tempMsg = param.Get<string>("OUT_MESSAGE");
            }

            status = tempStatus;
            message = tempMsg;
        }

        /// <summary>
        /// 「郵便番号」入力
        /// </summary>
        public string GetSortingCd(string zip, string prefName, string cityName, string address1, string centerId, string transporterId)
        {
            return MvcDbContext.Current.Database.SqlQuery<string>($@"SELECT SF_GET_SORTING_CD_EC('{Profile.User.ShipperId}'
                                                                                               ,'{centerId}'
                                                                                               ,'{transporterId}'
                                                                                               ,'{zip}'
                                                                                               ,'{prefName}'
                                                                                               ,'{cityName}'
                                                                                               ,'{address1}'
                                                                                            ) 
                                                                      FROM DUAL").SingleOrDefault();
        }
    }
}