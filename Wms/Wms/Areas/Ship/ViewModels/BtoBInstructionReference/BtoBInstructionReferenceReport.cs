namespace Wms.Areas.Ship.ViewModels.BtoBInstructionReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class BtoBInstructionReferenceReport
    {
        #region プロパティ

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.ShipPlanDate), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 1)]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 指示区分
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.InstructClassName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 2)]
        public string InstructClassName { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.EmergencyClassName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 3)]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ShipInstructId), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 4)]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemId), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 5)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 6)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemColorId), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 7)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemColorName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 8)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemSizeId), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 9)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemSizeName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 10)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionReferenceResource.JAN), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 11)]
        public string Jan { get; set; }

        /// <summary>
        /// 出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.ShipToQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 12)]
        public int? ShipToQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.InstructQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 13)]
        public int? InstructQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.ResultQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 14)]
        public int? ResultQty { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.AllocDate), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 15)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? AllocDate { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.CompleteFlagName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 16)]
        public string CompleteFlagName { get; set; }

        #endregion プロパティ
    }
}