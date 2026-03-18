namespace Wms.Models.Home
{
    public class ShipModels
    {
        /// <summary>
        /// センターコード
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 引当済出荷日(SHIP_PLAN_DATE)
        /// </summary>
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// 引当済出荷日当日 EC予定数量(SHIP_EC_PLAN_QTY)
        /// </summary>
        public int ShipEcPlanQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 EC実績数量(SHIP_EC_RESULT_QTY)
        /// </summary>
        public int ShipEcResultQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 ECオーダー数(SHIP_EC_ORDER_QTY)
        /// </summary>
        public int ShipEcOrderQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 TC予定数量(SHIP_TC_PLAN_QTY)
        /// </summary>
        public int ShipTcPlanQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 TC実績数量(SHIP_TC_RESULT_QTY)
        /// </summary>
        public int ShipTcResultQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 DC予定数量(SHIP_DC_PLAN_QTY)
        /// </summary>
        public int ShipDcPlanQty { get; set; }

        /// <summary>
        /// 引当済出荷日当日 TC実績数量(SHIP_TC_RESULT_QTY)
        /// </summary>
        public int ShipDcResultQty { get; set; }

        /// <summary>
        /// ピック作業 EC予定数量(PICK_EC_PLAN_QTY)
        /// </summary>
        public int PickEcPlanQty { get; set; }

        /// <summary>
        /// ピック作業 EC実績数量(PICK_EC_RESULT_QTY)
        /// </summary>
        public int PickEcResultQty { get; set; }

        /// <summary>
        /// ピック作業 DC予定数量(PICK_DC_PLAN_QTY)
        /// </summary>
        public int PickDcPlanQty { get; set; }

        /// <summary>
        /// ピック作業 DC実績数量(PICK_DC_RESULT_QTY)
        /// </summary>
        public int PickDcResultQty { get; set; }

        /// <summary>
        /// 店別仕分/摘取 TC予定数量(STORE_TC_PLAN_QTY)
        /// </summary>
        public int StoreTcPlanQty { get; set; }

        /// <summary>
        /// 店別仕分/摘取 TC実績数量(STORE_TC_RESULT_QTY)
        /// </summary>
        public int StoreTcResultQty { get; set; }

        /// <summary>
        /// 店別仕分/摘取 DC予定数量(STORE_DC_PLAN_QTY)
        /// </summary>
        public int StoreDcPlanQty { get; set; }

        /// <summary>
        /// 店別仕分/摘取 DC実績数量(STORE_DC_RESULT_QTY)
        /// </summary>
        public int StoreDcResultQty { get; set; }

        /// <summary>
        /// 納品書発行 EC予定数量(INVOICE_EC_PLAN_QTY)
        /// </summary>
        public int InvoiceEcPlanQty { get; set; }

        /// <summary>
        /// 納品書発行 EC実績数量(INVOICE_EC_RESULT_QTY)
        /// </summary>
        public int InvoiceEcResultQty { get; set; }

        /// <summary>
        /// 納品書発行 ECオーダー数(INVOICE_EC_ORDER_QTY)
        /// </summary>
        public int InvoiceEcOrderQty { get; set; }

        /// <summary>
        /// 納品書発行 TC予定数量(INVOICE_TC_PLAN_QTY)
        /// </summary>
        public int InvoiceTcPlanQty { get; set; }

        /// <summary>
        /// 納品書発行 TC実績数量(INVOICE_TC_RESULT_QTY)
        /// </summary>
        public int InvoiceTcResultQty { get; set; }

        /// <summary>
        /// 納品書発行 DC予定数量(INVOICE_DC_PLAN_QTY)
        /// </summary>
        public int InvoiceDcPlanQty { get; set; }

        /// <summary>
        /// 納品書発行 DC実績数量(INVOICE_DC_RESULT_QTY)
        /// </summary>
        public int InvoiceDcResultQty { get; set; }

    }
}