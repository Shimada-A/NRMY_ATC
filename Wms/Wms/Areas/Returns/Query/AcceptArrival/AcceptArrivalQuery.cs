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
using Wms.Areas.Returns.Models;
using Wms.Areas.Returns.Resources;
using Wms.Common;
using Wms.Models;
using Wms.Query;
using Wms.Areas.Returns.ViewModels.AcceptArrival;
using static Wms.Areas.Returns.ViewModels.AcceptArrival.AcceptArrival02SearchConditions;
namespace Wms.Areas.Returns.Query.AcceptArrival
{
    public class AcceptArrivalQuery
    {
        /// <summary>
        /// 商品情報取得
        /// </summary>
        /// <param name="Jan"></param>
        /// <returns></returns>
        public List<ItemInfo> GetItemInfo(string Jan)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT MIS.JAN
                      ,MIS.ITEM_ID
                      ,MIS.ITEM_NAME
                      ,MIS.ITEM_COLOR_ID
                      ,MC.ITEM_COLOR_NAME
                      ,MIS.ITEM_SIZE_ID
                      ,MS.ITEM_SIZE_NAME
                  FROM M_ITEM_SKU MIS
                  LEFT JOIN M_COLORS MC
                    ON MIS.SHIPPER_ID = MC.SHIPPER_ID
                   AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
                  LEFT JOIN M_SIZES MS
                    ON MIS.SHIPPER_ID = MS.SHIPPER_ID
                   AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
                 WHERE MIS.SHIPPER_ID = :SHIPPER_ID
                   AND MIS.JAN = :JAN
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":JAN", Jan);
            return MvcDbContext.Current.Database.Connection.Query<ItemInfo>(query.ToString(), parameters).ToList();
        }
        /// <summary>
        /// 出荷情報取得
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public List<AcceptArrival02ResultRow> GetShipInfo(AcceptArrival02SearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT A_ECSHIPS.SHIP_INSTRUCT_ID
                      ,MAX(A_ECSHIPS.KAKU_DATE) AS KAKU_DATE
                      ,MAX(A_ECSHIPS.DEST_PREF_NAME) AS DEST_PREF_NAME
                      ,MAX(A_ECSHIPS.DEST_ZIP) AS DEST_ZIP
                      ,MAX(A_ECSHIPS.DEST_FAMILY_NAME)||'  '||MAX(A_ECSHIPS.DEST_FIRST_NAME) AS DEST_NAME
                  FROM A_ECSHIPS A_ECSHIPS
            ");
            if (condition.ShpScanClass == ShpScanClasses.JanScan)
            {
                string[] arrJan = (condition.JanCombine).Split(',');
                string[] arrScanQty = (condition.ScanQtyCombine).Split(',');
                for (int i = 0; i < arrJan.Length; i++)
                {
                    if (arrScanQty[i] != "0")
                    {
                        query.Append(" INNER JOIN A_ECSHIPS A_ECSHIPS_S" + (i + 1) + "" +
                                     "    ON A_ECSHIPS.SHIPPER_ID =   A_ECSHIPS_S" + (i + 1) + ".SHIPPER_ID" +
                                     "   AND A_ECSHIPS.CENTER_ID =   A_ECSHIPS_S" + (i + 1) + ".CENTER_ID" +
                                     "   AND A_ECSHIPS.SHIP_INSTRUCT_ID =   A_ECSHIPS_S" + (i + 1) + ".SHIP_INSTRUCT_ID" +
                                     "   AND A_ECSHIPS_S" + (i + 1) + ".JAN = :JAN" + (i + 1) +
                                     "   AND A_ECSHIPS_S" + (i + 1) + ".RESULT_QTY >= :RESULT_QTY" + (i + 1));
                        parameters.Add(":JAN" + (i + 1), arrJan[i]);
                        parameters.Add(":RESULT_QTY" + (i + 1), arrScanQty[i]);
                    }
                }
            }
            query.Append(@"
                 WHERE A_ECSHIPS.SHIPPER_ID = :SHIPPER_ID
                   AND A_ECSHIPS.CENTER_ID = :CENTER_ID
            ");
            if (condition.ShpScanClass == ShpScanClasses.ShpScan)
            {
                query.Append(" AND A_ECSHIPS.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID ");
                if (!string.IsNullOrEmpty(condition.DestZip))
                {
                    parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId.ToString());
                }
                else
                {
                    parameters.Add(":SHIP_INSTRUCT_ID", "");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(condition.DestZip))
                {
                    query.Append(" AND A_ECSHIPS.DEST_ZIP = :DEST_ZIP ");
                    parameters.Add(":DEST_ZIP", condition.DestZip);
                }

                if (!string.IsNullOrEmpty(condition.DestName))
                {
                    query.Append(" AND (A_ECSHIPS.DEST_FAMILY_NAME || A_ECSHIPS.DEST_FIRST_NAME) LIKE :DEST_NAME ");
                    parameters.Add(":DEST_NAME", condition.DestName + "%");
                }
                if (!string.IsNullOrEmpty(condition.DestAddress))
                {
                    query.Append(" AND (A_ECSHIPS.DEST_PREF_NAME || A_ECSHIPS.DEST_CITY_NAME || A_ECSHIPS.DEST_ADDRESS1 || A_ECSHIPS.DEST_ADDRESS2 || A_ECSHIPS.DEST_ADDRESS3) LIKE :DEST_ADDRESS ");
                    parameters.Add(":DEST_ADDRESS", condition.DestAddress + "%");
                }
                if (!string.IsNullOrEmpty(condition.DestTel))
                {
                    query.Append(" AND REPLACE(A_ECSHIPS.DEST_TEL,'-') = :DEST_TEL ");
                    parameters.Add(":DEST_TEL", condition.DestTel);
                }

            }
            query.Append(@"
                GROUP BY A_ECSHIPS.SHIP_INSTRUCT_ID
            ");

            // Sort function
            switch (condition.SortKey)
            {
                case DataSortKey.KakuShpIns:
                    switch (condition.Sort)
                    {
                        case AcceptArrival02SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@" ORDER BY KAKU_DATE DESC, SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY KAKU_DATE, SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;
                case DataSortKey.ZipKaku:
                    switch (condition.Sort)
                    {
                        case AcceptArrival02SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@" ORDER BY DEST_ZIP DESC, KAKU_DATE DESC, SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY DEST_ZIP ASC, KAKU_DATE ASC, SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;
                default:
                    switch (condition.Sort)
                    {
                        case AcceptArrival02SearchConditions.AscDescSort.Desc:
                            query.AppendLine(@" ORDER BY SHIP_INSTRUCT_ID DESC ");
                            break;

                        default:
                            query.AppendLine(@" ORDER BY SHIP_INSTRUCT_ID ASC ");
                            break;
                    }

                    break;
            }
            if (condition.ShipInstructId != null)
            {
                parameters.Add(":SHIP_INSTRUCT_ID", condition.ShipInstructId.ToString());
            }
            else
            {
                if (!string.IsNullOrEmpty(condition.DestZip))
                {

                }

            }
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", condition.CenterId);
            // Fill data to memory
            var AcceptArrival02s = MvcDbContext.Current.Database.Connection.Query<AcceptArrival02ResultRow>(query.ToString(), parameters);
            // Excute paging
            return new List<AcceptArrival02ResultRow>(AcceptArrival02s);
        }
        /// <summary>
        /// 返品情報取得
        /// </summary>
        /// <param name="ShipInstructId"></param>
        /// <returns></returns>
        public List<AcceptArrival03ResultRow> GetReturnInfo(string CenterId, string ShipInstructId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
            SELECT A_ECSHP.SHIP_INSTRUCT_ID
                  ,A_ECSHP.SHIP_INSTRUCT_SEQ
                  ,MAX(A_ECSHP.EC_CLASS) EC_CLASS
                  ,MAX(A_ECSHP.DEST_PREF_NAME) AS DEST_PREF_NAME
                  ,MAX(A_ECSHP.DEST_FAMILY_NAME)|| '  ' ||MAX(A_ECSHP.DEST_FIRST_NAME) AS DEST_NAME
                  ,MAX(A_ECSHP.ITEM_SKU_ID) AS ITEM_SKU_ID
                  ,MAX(MIS.ITEM_NAME) AS ITEM_NAME
                  ,MAX(A_ECSHP.JAN) AS JAN
                  ,MAX(A_ECSHP.ITEM_ID) AS ITEM_ID
                  ,MAX(A_ECSHP.ITEM_COLOR_ID) AS ITEM_COLOR_ID
                  ,MAX(MC.ITEM_COLOR_NAME) AS ITEM_COLOR_NAME
                  ,MAX(A_ECSHP.ITEM_SIZE_ID) AS ITEM_SIZE_ID
                  ,MAX(MS.ITEM_SIZE_NAME) AS ITEM_SIZE_NAME
                  ,MAX(T_ECRET.ARRIVE_DATE) AS ARRIVE_DATE
                  ,MAX(A_ECSHP.RELATED_ORDER_NO) AS RELATED_ORDER_NO
                  ,MAX(A_ECSHP.KAKU_DATE) AS KAKU_DATE
                  ,MAX(A_ECSHP.RESULT_QTY) AS RESULT_QTY
                  ,NVL(SUM(T_ECRET.RETURN_QTY),0) AS RETURN_QTY_BEFORE
              FROM A_ECSHIPS A_ECSHP
              LEFT JOIN T_ECRETURN_RESULTS T_ECRET
                ON A_ECSHP.SHIPPER_ID = T_ECRET.SHIPPER_ID
               AND A_ECSHP.CENTER_ID = T_ECRET.CENTER_ID
               AND A_ECSHP.SHIP_INSTRUCT_ID = T_ECRET.SHIP_INSTRUCT_ID
               AND A_ECSHP.ITEM_SKU_ID = T_ECRET.ITEM_SKU_ID
              LEFT JOIN M_ITEM_SKU MIS
                ON A_ECSHP.SHIPPER_ID = MIS.SHIPPER_ID
               AND A_ECSHP.ITEM_SKU_ID = MIS.ITEM_SKU_ID
              LEFT JOIN M_COLORS MC
                ON MIS.SHIPPER_ID = MC.SHIPPER_ID
               AND MIS.ITEM_COLOR_ID = MC.ITEM_COLOR_ID
              LEFT JOIN M_SIZES MS
                ON MIS.SHIPPER_ID = MS.SHIPPER_ID
               AND MIS.ITEM_SIZE_ID = MS.ITEM_SIZE_ID
             WHERE A_ECSHP.CENTER_ID = :CENTER_ID
               AND A_ECSHP.SHIPPER_ID = :SHIPPER_ID
               AND A_ECSHP.SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID
             GROUP BY
                   A_ECSHP.CENTER_ID
                  ,A_ECSHP.SHIP_INSTRUCT_ID
                  ,A_ECSHP.SHIP_INSTRUCT_SEQ
             ORDER BY
                   A_ECSHP.SHIP_INSTRUCT_ID
                  ,A_ECSHP.SHIP_INSTRUCT_SEQ

            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", CenterId);
            parameters.Add(":SHIP_INSTRUCT_ID", ShipInstructId);
            try
            {
                return MvcDbContext.Current.Database.Connection.Query<AcceptArrival03ResultRow>(query.ToString(), parameters).ToList();
            }
            catch (System.Exception ex)
            {
                Mvc.Common.AppError.PutLogREF(ex, "GetReturnInfo");
                throw;
            }

        }
        /// <summary>
        /// 返品情報登録
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public string InputReturn(string CenterId, List<AcceptArrival03ResultRow> inputData)
        {
            SearchInput searchInput = new SearchInput();
            //返品IDを取得
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
            SELECT SF_GET_SEQ(
                    :IN_SHIPPER_ID
                   ,:IN_CENTER_ID
                   , 23
                   )
              FROM DUAL
            ");
            parameters.Add(":IN_SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":IN_CENTER_ID", CenterId);
            var returnId = MvcDbContext.Current.Database.Connection.Query<string>(query.ToString(), parameters).FirstOrDefault();
            int retC = 0;
            string message = null;
            using (System.Data.Common.DbTransaction tran = MvcDbContext.Current.Database.Connection.BeginTransaction())
            {
                try
                {
                    int insertCnt = 0; //返品明細ID用カウント
                    //返品実績テーブルへ値を登録
                    for (int i = 0; i < inputData.Count; i++)
                    {
                        if (inputData[i].ReturnQtyNow != null && inputData[i].ReturnQtyNow != 0)
                        {

                            DynamicParameters parametersInsert = new DynamicParameters();
                            StringBuilder queryInsert = new StringBuilder();
                            queryInsert.Append(@"
                            INSERT INTO T_ECRETURN_RESULTS(
                                   MAKE_DATE
                                  ,MAKE_USER_ID
                                  ,MAKE_PROGRAM_NAME
                                  ,UPDATE_DATE
                                  ,UPDATE_USER_ID
                                  ,UPDATE_PROGRAM_NAME
                                  ,UPDATE_COUNT
                                  ,SHIPPER_ID
                                  ,CENTER_ID
                                  ,RETURN_ID
                                  ,RETURN_SEQ
                                  ,SHIP_INSTRUCT_ID
                                  ,SHIP_INSTRUCT_SEQ
                                  ,EC_CLASS
                                  ,RELATED_ORDER_NO
                                  ,ARRIVE_DATE
                                  ,ITEM_SKU_ID
                                  ,JAN
                                  ,ITEM_ID
                                  ,ITEM_COLOR_ID
                                  ,ITEM_SIZE_ID
                                  ,RETURN_QTY
                                  ,INPUT_USER_ID
                                  ,INPUT_USER_NAME
                                  ) VALUES (
                                   SYSTIMESTAMP
                                  ,:MAKE_USER_ID
                                  ,:MAKE_PROGRAM_NAME
                                  ,SYSTIMESTAMP
                                  ,:UPDATE_USER_ID
                                  ,:UPDATE_PROGRAM_NAME
                                  ,0
                                  ,:SHIPPER_ID
                                  ,:CENTER_ID
                                  ,:RETURN_ID
                                  ,:RETURN_SEQ
                                  ,:SHIP_INSTRUCT_ID
                                  ,:SHIP_INSTRUCT_SEQ
                                  ,:EC_CLASS
                                  ,:RELATED_ORDER_NO
                                  ,SYSDATE
                                  ,:ITEM_SKU_ID
                                  ,:JAN
                                  ,:ITEM_ID
                                  ,:ITEM_COLOR_ID
                                  ,:ITEM_SIZE_ID
                                  ,:RETURN_QTY
                                  ,:INPUT_USER_ID
                                  ,:INPUT_USER_NAME
                                  )
                             ");
                            parametersInsert.Add(":MAKE_USER_ID", Common.Profile.User.UserId);
                            parametersInsert.Add(":MAKE_PROGRAM_NAME", "EcAcceptArrival01");
                            parametersInsert.Add(":UPDATE_USER_ID", Common.Profile.User.UserId);
                            parametersInsert.Add(":UPDATE_PROGRAM_NAME", "EcAcceptArrival01");
                            parametersInsert.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
                            parametersInsert.Add(":CENTER_ID", CenterId);
                            parametersInsert.Add(":RETURN_ID", returnId);
                            parametersInsert.Add(":RETURN_SEQ", insertCnt + 1);
                            parametersInsert.Add(":SHIP_INSTRUCT_ID", inputData[i].ShipInstructId);
                            parametersInsert.Add(":SHIP_INSTRUCT_SEQ", inputData[i].ShipInstructSeq);
                            parametersInsert.Add(":EC_CLASS", inputData[i].EcClass);
                            parametersInsert.Add(":RELATED_ORDER_NO", inputData[i].RelatedOrderNo);
                            parametersInsert.Add(":ITEM_SKU_ID", inputData[i].ItemSkuId);
                            parametersInsert.Add(":JAN", inputData[i].Jan);
                            parametersInsert.Add(":ITEM_ID", inputData[i].ItemId);
                            parametersInsert.Add(":ITEM_COLOR_ID", inputData[i].ItemColorId);
                            parametersInsert.Add(":ITEM_SIZE_ID", inputData[i].ItemSizeId);
                            parametersInsert.Add(":RETURN_QTY", inputData[i].ReturnQtyNow);
                            parametersInsert.Add(":INPUT_USER_ID", Common.Profile.User.UserId);
                            parametersInsert.Add(":INPUT_USER_NAME", Common.Profile.User.UserName);
                            retC = MvcDbContext.Current.Database.Connection.Execute(queryInsert.ToString(), parametersInsert);
                            insertCnt += 1;
                        }

                    }

                    if (retC > 0)
                    {
                        // 在庫登録
                        message = InputZaiko(returnId, CenterId);
                        if (!string.IsNullOrEmpty(message))
                        {
                            tran.Rollback();
                            return message;
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    tran.Rollback();
                    return "返品情報登録エラーです。";
                }
                tran.Commit();
            }
            return null;
        }

        /// <summary>
        /// 在庫登録
        /// </summary>
        public string InputZaiko(string returnId, string CenterId)
        {
            //ストアド実行
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            parameters.Add("IN_CENTER_ID", CenterId, DbType.String, ParameterDirection.Input);
            parameters.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            parameters.Add("IN_RETURN_ID", returnId, DbType.String, ParameterDirection.Input);
            parameters.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute("SP_W_RTN_STOCK_REG", parameters, commandType: CommandType.StoredProcedure);
            if (parameters.Get<ProcedureStatus>("OUT_STATUS") != ProcedureStatus.Success)
            {
                return parameters.Get<string>("OUT_MESSAGE");
            }
            return null;
        }

        /// <summary>
        /// Jan存在チェック
        /// </summary>
        /// <param name="jan"></param>
        /// <returns></returns>
        public bool ExistJan(string jan)
        {
            bool result = false;

            StringBuilder query = new StringBuilder(@"
                                SELECT COUNT(1)
                                  FROM M_ITEM_SKU MIS
                                 WHERE MIS.SHIPPER_ID = :SHIPPER_ID
                                   AND MIS.JAN = :JAN");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":JAN", jan);

            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);

            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    result = true;
                }
            }

            return result;
        }
        /// <summary>
        /// 出荷指示ID存在チェック
        /// </summary>
        /// <param name="shipInstructId"></param>
        /// <returns></returns>
        public bool ExistId(string shipInstructId, string centerId)
        {
            bool result = false;

            StringBuilder query = new StringBuilder(@"
                                SELECT COUNT(1)
                                  FROM A_ECSHIPS
                                 WHERE SHIPPER_ID = :SHIPPER_ID
                                   AND CENTER_ID = :CENTER_ID
                                   AND SHIP_INSTRUCT_ID = :SHIP_INSTRUCT_ID");

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", centerId);
            parameters.Add(":SHIP_INSTRUCT_ID", shipInstructId);

            object outResult = MvcDbContext.Current.Database.Connection.ExecuteScalar(query.ToString(), parameters);

            if (outResult != null)
            {
                if (int.Parse(outResult.ToString()) != 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}