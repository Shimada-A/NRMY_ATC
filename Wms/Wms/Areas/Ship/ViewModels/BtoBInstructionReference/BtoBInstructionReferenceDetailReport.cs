namespace Wms.Areas.Ship.ViewModels.BtoBInstructionReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// DC List
    /// </summary>
    public class BtoBInstructionReferenceDetailReport
    {
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
        /// 明細ID
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ShipInstructSeq), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 5)]
        public int? ShipInstructSeq { get; set; }

        /// <summary>
        /// 分類1(名称)
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.CategoryName1), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 6)]
        public string CategoryName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.SKU), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 7)]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemColorId), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 8)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemColorName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 9)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemSizeId), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 10)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ItemSizeName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 11)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.JAN), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 12)]

        public string Jan { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ShipToStoreId), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 13)]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.ShipToStoreName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 14)]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.TransporterName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 15)]
        public string TransporterName { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.InstructQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 16)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.AllocQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 17)]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 欠品登録
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.StockOutRegQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 18)]
        public int? StockOutRegQty { get; set; }

        /// <summary>
        /// 欠品確定
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.StockOutFixQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 19)]
        public int? StockOutFixQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = nameof(BtoBInstructionReferenceResource.ResultQty), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 20)]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.CompleteFlagName), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 21)]
        public string CompleteFlagName { get; set; }

        /// <summary>
        /// 完了日
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.CompleteDateT), ResourceType = typeof(BtoBInstructionReferenceResource), Order = 22)]
        public string CompleteDate { get; set; }

        /// <summary>
        /// 伝票日付
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.SlipDate), ResourceType = typeof(BtoBInstructionReferenceResource), Order =23)]
        public string SlipDate { get; set; }

        /// <summary>
        /// 売上区分
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.SalesClass), ResourceType = typeof(BtoBInstructionReferenceResource),Order = 24)]
        public string SalesClass { get; set;}

        /// <summary>
        /// オフ率
        /// </summary>
        [Display(Name = nameof(BtoBInstructionReferenceResource.OffRate), ResourceType = typeof(BtoBInstructionReferenceResource),Order = 25)]
        public string OffRate { get; set;}
    }
}