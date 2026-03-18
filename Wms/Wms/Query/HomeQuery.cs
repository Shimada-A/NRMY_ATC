namespace Wms.Query
{
    using Dapper;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Wms.Common;
    using Wms.Models;
    using Wms.ViewModels.Home;
    using Wms.Models.Home;

    public class HomeQuery
    {
        /// <summary>
        /// センターデータ取得
        /// </summary>
        /// <returns>セレクトボックスデータ</returns>
        public IEnumerable<SelectListItem> GetSelectListWarehouses()
        {
            return MvcDbContext.Current.Warehouses
                .Where(m => m.ShipperId == Profile.User.ShipperId)
                .Select(m => new SelectListItem
                {
                    Value = m.CenterId.ToString(),
                    Text = m.CenterName1,
                })
                .OrderBy(m => m.Text);
        }


        public List<Message> GetDataMessage(string CenterId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                          SELECT TDM.MESSAGE_CLASS
                                ,TDM.DISP_MESSAGE
                          FROM T_DASH_MESSAGE TDM
                          WHERE TDM.SHIPPER_ID =:SHIPPER_ID
                            AND TDM.CENTER_ID =:CENTER_ID
                          ORDER BY TDM.MESSAGE_CLASS
                                   ,TDM.ORDER_NO 
                                   ,TDM.DASH_NOTICE_SEQ
                        ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", CenterId);
            var cnt = MvcDbContext.Current.Database.Connection.Query<Message>(query.ToString(), parameters).Count();
            if (cnt > 0)
            {
                return MvcDbContext.Current.Database.Connection.Query<Message>(query.ToString(), parameters).ToList();
            }
            else
            {
                return null;
            }
            
        }

        public ArriveModels GetDashArrives(string CenterId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                           SELECT  TDA.CENTER_ID
                                  ,TO_CHAR(TDA.ARRIVE_PLAN_DATE,'YYYY/MM/DD') || '（' ||TO_CHAR(TDA.ARRIVE_PLAN_DATE,'DY') || '）' AS ARRIVE_PLAN_DATE
                                  ,TDA.ARRIVE_PLAN_QTY
                                  ,TDA.ARRIVE_RESULT_QTY
                                  ,TDA.ARRIVE_TC_PLAN_QTY
                                  ,TDA.ARRIVE_TC_RESULT_QTY
                                  ,TDA.ARRIVE_DC_PLAN_QTY
                                  ,TDA.ARRIVE_DC_RESULT_QTY
                           FROM T_DASH_ARRIVES TDA
                           WHERE TDA.SHIPPER_ID = :SHIPPER_ID
                             AND TDA.CENTER_ID = :CENTER_ID
                         ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", CenterId);

            try
            {

                var cnt = MvcDbContext.Current.Database.Connection.Query<ArriveModels>(query.ToString(), parameters).Count();
                if (cnt > 0)
                {
                    return MvcDbContext.Current.Database.Connection.Query<ArriveModels>(query.ToString(), parameters).First();
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public ShipModels GetDataShip(string CenterId)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                           SELECT  TDS.CENTER_ID
                                  ,TO_CHAR(TDS.SHIP_PLAN_DATE,'YYYY/MM/DD') || '（' ||TO_CHAR(SHIP_PLAN_DATE,'DY') || '）' AS SHIP_PLAN_DATE
                                  ,TDS.SHIP_EC_PLAN_QTY
                                  ,TDS.SHIP_EC_RESULT_QTY
                                  ,TDS.SHIP_EC_ORDER_QTY
                                  ,TDS.SHIP_TC_PLAN_QTY
                                  ,TDS.SHIP_TC_RESULT_QTY
                                  ,TDS.SHIP_DC_PLAN_QTY
                                  ,TDS.SHIP_DC_RESULT_QTY
                                  ,TDS.PICK_EC_PLAN_QTY
                                  ,TDS.PICK_EC_RESULT_QTY
                                  ,TDS.PICK_DC_PLAN_QTY
                                  ,TDS.PICK_DC_RESULT_QTY
                                  ,TDS.STORE_TC_PLAN_QTY
                                  ,TDS.STORE_TC_RESULT_QTY
                                  ,TDS.STORE_DC_PLAN_QTY
                                  ,TDS.STORE_DC_RESULT_QTY
                                  ,TDS.INVOICE_EC_PLAN_QTY
                                  ,TDS.INVOICE_EC_RESULT_QTY
                                  ,TDS.INVOICE_EC_ORDER_QTY
                                  ,TDS.INVOICE_TC_PLAN_QTY
                                  ,TDS.INVOICE_TC_RESULT_QTY
                                  ,TDS.INVOICE_DC_PLAN_QTY
                                  ,TDS.INVOICE_DC_RESULT_QTY
                           FROM T_DASH_SHIPS TDS
                           WHERE TDS.SHIPPER_ID = :SHIPPER_ID
                             AND TDS.CENTER_ID = :CENTER_ID
            ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", CenterId);

            try
            {
                var cnt = MvcDbContext.Current.Database.Connection.Query<ShipModels>(query.ToString(), parameters).Count();
                if (cnt > 0)
                {
                    return MvcDbContext.Current.Database.Connection.Query<ShipModels>(query.ToString(), parameters).First();
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Get システム間隔時間
        /// </summary>
        /// <returns></returns>
        public int GetIntervalTime()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@"
               SELECT MG.GEN_NAME *1000
               FROM M_GENERALS MG
               WHERE MG.GEN_DIV_CD = 'DASHBOARD_RELOAD_INTERVAL'
                 AND REGISTER_DIVI_CD = '1'
            ");

            int updInterval = MvcDbContext.Current.Database.Connection.ExecuteScalar<int>(query.ToString());

            return updInterval;
        }
    }
}