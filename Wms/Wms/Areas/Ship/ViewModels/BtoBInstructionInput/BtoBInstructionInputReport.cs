namespace Wms.Areas.Ship.ViewModels.BtoBInstructionInput
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    public class BtoBInstructionInputReport
    {
        #region プロパティ

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = nameof(BtoBInstructionInputResource.ShipPlanDate), ResourceType = typeof(BtoBInstructionInputResource), Order = 1)]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 指示区分
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.InstructClassName), ResourceType = typeof(BtoBInstructionInputResource), Order = 2)]
        public string InstructClassName { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.EmergencyClassName), ResourceType = typeof(BtoBInstructionInputResource), Order = 3)]
        public string EmergencyClassName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ShipInstructId), ResourceType = typeof(BtoBInstructionInputResource), Order = 4)]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemId), ResourceType = typeof(BtoBInstructionInputResource), Order = 5)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemName), ResourceType = typeof(BtoBInstructionInputResource), Order = 6)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemColorId), ResourceType = typeof(BtoBInstructionInputResource), Order = 7)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemColorName), ResourceType = typeof(BtoBInstructionInputResource), Order = 8)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemSizeId), ResourceType = typeof(BtoBInstructionInputResource), Order = 9)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionInputResource.ItemSizeName), ResourceType = typeof(BtoBInstructionInputResource), Order = 10)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(BtoBInstructionInputResource.Jan), ResourceType = typeof(BtoBInstructionInputResource), Order = 11)]
        public string Jan { get; set; }

        /// <summary>
        /// 出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionInputResource.ShipToQty), ResourceType = typeof(BtoBInstructionInputResource), Order = 12)]
        public int? ShipToQty { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionInputResource.InstructQty), ResourceType = typeof(BtoBInstructionInputResource), Order = 13)]
        public int? InstructQty { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.MakeDate), ResourceType = typeof(BtoBInstructionInputResource), Order = 14)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? MakeDate { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        [Display(Name = nameof(BtoBInstructionInputResource.AllocQty), ResourceType = typeof(BtoBInstructionInputResource), Order = 15)]
        public string AllocQty { get; set; }

        #endregion プロパティ
    }
}