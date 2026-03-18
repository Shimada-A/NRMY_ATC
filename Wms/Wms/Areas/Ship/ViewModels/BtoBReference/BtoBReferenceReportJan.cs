namespace Wms.Areas.Ship.ViewModels.BtoBReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;

    public class BtoBReferenceReportJan
    {
        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.Center), ResourceType = typeof(BtoBReferenceResource), Order = 1)]
        public string Center { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipToStore), ResourceType = typeof(BtoBReferenceResource), Order = 2)]
        public string ShipToStore { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.BoxNo), ResourceType = typeof(BtoBReferenceResource), Order = 2)]
        public string BoxNo { get; set; }

        /// <summary>
        /// ユーザーID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.UserId), ResourceType = typeof(BtoBReferenceResource), Order = 3)]
        public string UserId { get; set; }

        /// <summary>
        /// ユーザー
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.User), ResourceType = typeof(BtoBReferenceResource), Order = 4)]
        public string UserName { get; set; }

        /// <summary>
        /// 出荷指示ID SHIP_INSTRUCT_ID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipInstructId), ResourceType = typeof(BtoBReferenceResource), Order = 5)]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipInstructSeq), ResourceType = typeof(BtoBReferenceResource), Order = 6)]
        public string ShipInstructSeq { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipPlanDate), ResourceType = typeof(BtoBReferenceResource), Order = 7)]
        public string ShipPlanDate { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.BatchNo), ResourceType = typeof(BtoBReferenceResource), Order = 8)]
        public string BatchNo { get; set; }

        /// <summary>
        /// 分類
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.Category1), ResourceType = typeof(BtoBReferenceResource), Order = 9)]
        public string Category1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.Item), ResourceType = typeof(BtoBReferenceResource), Order = 10)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemNm), ResourceType = typeof(BtoBReferenceResource), Order = 11)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemColor), ResourceType = typeof(BtoBReferenceResource), Order = 12)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemColorNm), ResourceType = typeof(BtoBReferenceResource), Order = 13)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemSize), ResourceType = typeof(BtoBReferenceResource), Order = 14)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemSizeNm), ResourceType = typeof(BtoBReferenceResource), Order = 15)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.Jan), ResourceType = typeof(BtoBReferenceResource), Order = 18)]
        public string Jan { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.AllocQty), ResourceType = typeof(BtoBReferenceResource), Order = 19)]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ResultQty), ResourceType = typeof(BtoBReferenceResource), Order = 20)]
        public int? ResultQty { get; set; }

    }
}