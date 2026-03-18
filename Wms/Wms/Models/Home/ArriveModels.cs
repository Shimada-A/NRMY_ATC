namespace Wms.Models.Home
{

    public class ArriveModels
    {
        /// <summary>
        /// センターコード
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 入荷予定日(ARRIVE_PLAN_DATE)
        /// </summary>
        public string ArrivePlanDate { get; set; }

        /// <summary>
        /// 入荷予定数(ARRIVE_PLAN_QTY)
        /// </summary>
        public int ArrivePlanQty { get; set; }

        /// <summary>
        /// 入荷実績数(ARRIVE_RESULT_QTY)
        /// </summary>
        public int ArriveResultQty { get; set; }

        /// <summary>
        /// TC入荷予定数(ARRIVE_TC_PLAN_QTY)
        /// </summary>
        public int ArriveTcPlanQty { get; set; }

        /// <summary>
        /// TC入荷実績数(ARRIVE_TC_RESULT_QTY)
        /// </summary>
        public int ArriveTcResultQty { get; set; }

        /// <summary>
        /// DC入荷予定数(ARRIVE_DC_PLAN_QTY)
        /// </summary>
        public int ArriveDcPlanQty { get; set; }

        /// <summary>
        /// TC入荷実績数(ARRIVE_DC_RESULT_QTY)
        /// </summary>
        public int ArriveDcResultQty { get; set; }
    }
}