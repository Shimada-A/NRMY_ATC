namespace Wms.Areas.Ship.ViewModels.ModifyTcInstruction
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class ModifyTcInstructionResultRow
    {
        #region プロパティ

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
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        /// <remarks>
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        /// <remarks>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 明細ID
        /// </summary>
        /// <remarks>
        public string ShipInstructSeq { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        /// <remarks>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        /// <remarks>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 出荷優先順
        /// </summary>
        /// <remarks>
        public int? PriorityOrder { get; set; }

        /// <summary>
        /// 欠品不可店
        /// </summary>
        /// <remarks>
        public string StockOutStore { get; set; }

        /// <summary>
        /// レーンNo
        /// </summary>
        /// <remarks>
        public int? LaneNo { get; set; }

        /// <summary>
        /// 間口No
        /// </summary>
        /// <remarks>
        public int? FrontageNo { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? AllocUpDate { get; set; }

        /// <summary>
        /// 出荷指示数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructQty { get; set; }

        /// <summary>
        /// 最小出荷指示
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? MinInstructQty { get; set; }

        /// <summary>
        /// 修正後指示数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(ModifyTcInstructionResource.WmsInstructQty), ResourceType = typeof(ModifyTcInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? WmsInstructQty { get; set; }
        public long Seq { get; set; }
        public long LineNo { get; set; }

        #endregion プロパティ
    }
}