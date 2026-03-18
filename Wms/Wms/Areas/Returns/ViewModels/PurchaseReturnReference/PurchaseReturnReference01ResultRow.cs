namespace Wms.Areas.Returns.ViewModels.PurchaseReturnReference
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 移動入荷進捗照会(移動元別)
    /// </summary>
    public class PurchaseReturnReference01ResultRow
    {
        #region プロパティ

        public string ShipperId { get; set; }

        public string CenterId { get; set; }


        /// <summary>
        /// 返品伝票ID (RETURN_ID)
        /// </summary>
        /// <remarks>
        /// システム内で仕入返品伝票ID採番(上位への連携時はセンターがキーにないので、センターコードがはいっているNOとする）
        /// </remarks>
        public string ReturnId { get; set; }

        /// <remarks>
        /// 0:仕入先返品, 1:仕入訂正　
        /// </remarks>
        public byte ReturnClass { get; set; }

        public string ReturnClassName { get; set; }

        /// <summary>
        /// 仕入先ID (VENDOR_ID)
        /// </summary>
        public string VendorId { get; set; }

        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号 (INVOICE_NO)
        /// </summary>
        /// <remarks>
        /// 返品：セットしない
        /// 訂正：画面入力値
        /// </remarks>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 返品日 (ARRIVE_DATE)
        /// </summary>
        /// <remarks>
        /// 実績登録（＝確定）日
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 入力担当者ID (INPUT_USER_ID)
        /// </summary>
        public string InputUserId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

        #endregion プロパティ
    }
}