namespace Wms.Areas.Others.ViewModels.WorkingReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Others.Resources;
    using Wms.Common;

    /// <summary>
    /// 在庫仕分指示明細
    /// </summary>
    public class WorkingReferenceResultRow
    {
        #region プロパティ

        public bool IsCheck { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Display(Name = "Seq", ResourceType = typeof(WorkingReferenceResource))]
        public long Seq { get; set; }

        public long? HaitaSeq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Display(Name = "LineNo", ResourceType = typeof(WorkingReferenceResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = "CenterId", ResourceType = typeof(WorkingReferenceResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 納品書番号 (INVOICE_NO)
        /// </summary>
        [Display(Name = "InvoiceNo", ResourceType = typeof(WorkingReferenceResource))]
        public string InvoiceNo { get; set; }

        public string ItemSkuId { get; set; }
        
        /// <summary>
        /// 作業開始日時 (START_DATE)
        /// </summary>
        [Display(Name = "StartDate", ResourceType = typeof(WorkingReferenceResource))]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 作業者ID (WORK_USER_ID)
        /// </summary>
        [Display(Name = "WorkUserId", ResourceType = typeof(WorkingReferenceResource))]
        public string WorkUserId { get; set; }

        /// <summary>
        /// 作業者名 (WORK_USER_NAME)
        /// </summary>
        [Display(Name = "WorkUserName", ResourceType = typeof(WorkingReferenceResource))]
        public string WorkUserName { get; set; }

        /// <summary>
        /// 端末番号 (TERMINAL_INFO)
        /// </summary>
        [Display(Name = "TerminalInfo", ResourceType = typeof(WorkingReferenceResource))]
        public string TerminalInfo { get; set; }

        /// <summary>
        /// 品番 (ITEM_ID)
        /// </summary>
        [Display(Name = "ItemId", ResourceType = typeof(WorkingReferenceResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Display(Name = "ItemName", ResourceType = typeof(WorkingReferenceResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        [Display(Name = "ItemColorId", ResourceType = typeof(WorkingReferenceResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名 (ITEM_COLOR_NAME)
        /// </summary>
        [Display(Name = "ItemColorName", ResourceType = typeof(WorkingReferenceResource))]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        [Display(Name = "ItemSizeId", ResourceType = typeof(WorkingReferenceResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名 (ITEM_SIZE_NAME)
        /// </summary>
        [Display(Name = "ItemSizeName", ResourceType = typeof(WorkingReferenceResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Display(Name = "Jan", ResourceType = typeof(WorkingReferenceResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 移動伝票番号 (SLIP_NO)
        /// </summary>
        [Display(Name = "SlipNo", ResourceType = typeof(WorkingReferenceResource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 伝票連番 (SLIP_SEQ)
        /// </summary>
        [Display(Name = "SlipSeq", ResourceType = typeof(WorkingReferenceResource))]
        public int? SlipSeq { get; set; }

        /// <summary>
        /// 移動区分 (TRANSFER_CLASS)
        /// </summary>
        [Display(Name = "TransferClass", ResourceType = typeof(WorkingReferenceResource))]
        public byte? TransferClass { get; set; }

        /// <summary>
        /// 移動元店舗区分 (TRANS_FROM_STORE_CLASS)
        /// </summary>
        [Display(Name = "TransFromStoreClass", ResourceType = typeof(WorkingReferenceResource))]
        public string TransFromStoreClass { get; set; }

        /// <summary>
        /// 移動元店舗 (TRANSFER_FROM_STORE_NAME)
        /// </summary>
        [Display(Name = "TransferFromStoreName", ResourceType = typeof(WorkingReferenceResource))]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// 移動先店舗 (TRANSFER_TO_STORE_NAME)
        /// </summary>
        [Display(Name = "TransferToStoreName", ResourceType = typeof(WorkingReferenceResource))]
        public string TransferToStoreName { get; set; }

        /// <summary>
        /// ロケーション (LOCATION_CD)
        /// </summary>
        [Display(Name = "LocationCd", ResourceType = typeof(WorkingReferenceResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// 予定数 (ARRIVE_PLAN_QTY)
        /// </summary>
        [Display(Name = "ArrivePlanQty", ResourceType = typeof(WorkingReferenceResource))]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 引当数 (HIKI_QTY)
        /// </summary>
        [Display(Name = "HikiQty", ResourceType = typeof(WorkingReferenceResource))]
        public int? HikiQty { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        [Display(Name = "ResultQty", ResourceType = typeof(WorkingReferenceResource))]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 梱包番号
        /// </summary>
        [Display(Name = "BoxNo", ResourceType = typeof(WorkingReferenceResource))]
        public string BoxNo { get; set; }

        //SKU種類数
        public int? SkuQty { get; set; }

        //予定数
        public int? SortPlanQty { get; set; }

        //実績数
        public int? SortResultQty { get; set; }

        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }

        public string PicKind { get; set; }

        public string BatchNo { get; set; }

        public string LaneNo { get; set; }

        public string FrontageNo { get; set; }

        public string ShipToStoreId { get; set; }

        public string ShipToStoreName { get; set; }

        #endregion プロパティ
    }
}