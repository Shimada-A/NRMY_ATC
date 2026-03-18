namespace Wms.Areas.Others.ViewModels.Inspection
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;

    public class InspectionResultRow
    {
        /// <summary>
        /// 荷主ID
        /// </summary>
        /// <remarks>
        public string ShipperId { get; set; }

        /// <summary>
        /// センター
        /// </summary>
        /// <remarks>
        public string CenterId { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? MakeDate { get; set; }

        /// <summary>
        /// 作成ユーザーID
        /// </summary>
        /// <remarks>
        public DateTime? MakeUserId { get; set; }

        /// <summary>
        /// 作業内容(WORKCONTENT)
        /// </summary>
        /// <remarks>
        public string WorkContent { get; set; }

        /// <summary>
        /// 自動連番(SYSTEM_LOG_ID)
        /// </summary>
        /// <remarks>
        public int SystemLogId { get; set; }

        /// <summary>
        /// 倉庫ID(LOC_ID)
        /// </summary>
        /// <remarks>
        public string LocId { get; set; }

        /// <summary>
        /// 処理ID(PROCESSING_ID)
        /// </summary>
        /// <remarks>
        public string ProCessingId { get; set; }

        /// <summary>
        /// 処理詳細(PROCESSING_DETAIL)
        /// </summary>
        /// <remarks>
        public string ProCessingDetail { get; set; }

        /// <summary>
        /// 処理内容(PROCESSING_MESSAGE)
        /// </summary>
        /// <remarks>
        public string ProCessingMessage { get; set; }

        /// <summary>
        /// バッチNo(BATCH_NO)
        /// </summary>
        /// <remarks>
        public string BatchNo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        public string ItemName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        /// <remarks>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        /// <remarks>
        public string ColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        public string ColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        /// <remarks>
        public string SizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        public string SizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        public string Jan { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        /// <remarks>
        public string LoccationCd { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        public int? ResultQty { get; set; }

        /// <summary>
        /// 出荷先店舗ID(SHIP_TO_STORE_ID)
        /// </summary>
        /// <remarks>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 梱包番号
        /// </summary>
        /// <remarks>
        public string BoxNo { get; set; }

        /// <summary>
        /// 梱包番号2
        /// </summary>
        /// <remarks>
        public string BoxNo2 { get; set; }
    }
}