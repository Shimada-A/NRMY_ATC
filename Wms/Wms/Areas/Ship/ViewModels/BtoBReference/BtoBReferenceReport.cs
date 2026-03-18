namespace Wms.Areas.Ship.ViewModels.BtoBReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;

    public class BtoBReferenceReport
    {
        /// <summary>
        /// センター
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.CenterId), ResourceType = typeof(BtoBReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipToStoreId), ResourceType = typeof(BtoBReferenceResource), Order = 2)]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipToStoreName), ResourceType = typeof(BtoBReferenceResource), Order = 3)]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.BoxNo), ResourceType = typeof(BtoBReferenceResource), Order = 4)]
        public string BoxNo { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.BatchNo), ResourceType = typeof(BtoBReferenceResource), Order = 5)]
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチ名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.BatchName), ResourceType = typeof(BtoBReferenceResource), Order = 6)]
        public string BatchName { get; set; }

        /// <summary>
        /// 出荷指示ID SHIP_INSTRUCT_ID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipInstructId), ResourceType = typeof(BtoBReferenceResource), Order = 7)]
        public string ShipInstructId { get; set; }


        /// <summary>
        /// 出荷指示明細
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipInstructSeq), ResourceType = typeof(BtoBReferenceResource), Order = 8)]
        public string ShipInstructSeq { get; set; }

        ///// <summary>
        ///// SKU
        ///// </summary>
        //[Display(Name = nameof(BtoBReferenceResource.ItemSkuId), ResourceType = typeof(BtoBReferenceResource), Order = 9)]
        //public string ItemSkuId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.Item), ResourceType = typeof(BtoBReferenceResource), Order = 10)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemName), ResourceType = typeof(BtoBReferenceResource), Order = 11)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemColor), ResourceType = typeof(BtoBReferenceResource), Order = 12)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemColorName), ResourceType = typeof(BtoBReferenceResource), Order = 13)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemSize), ResourceType = typeof(BtoBReferenceResource), Order = 14)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ItemSizeName), ResourceType = typeof(BtoBReferenceResource), Order = 15)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.Jan), ResourceType = typeof(BtoBReferenceResource), Order = 16)]
        public string Jan { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ResultQty), ResourceType = typeof(BtoBReferenceResource), Order = 17)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 状態 (SHIP_STATUS_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ShipStatusName), ResourceType = typeof(ShpBtoBReference01Resource), Order = 18)]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipStatusName { get; set; }

        /// <summary>
        /// 出荷確定日 (KAKU_DATE)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.KakuDate), ResourceType = typeof(ShpBtoBReference01Resource), Order = 19)]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 送り状No
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.DeliNo), ResourceType = typeof(BtoBReferenceResource), Order = 20)]
        public string DeliNo { get; set; }

        /// <summary>
        /// カートン確定日時
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.CaseKakuDate), ResourceType = typeof(BtoBReferenceResource), Order = 21)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? CaseKakuDate { get; set; }

        /// <summary>
        /// カートン担当者
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.CaseKakuUser), ResourceType = typeof(BtoBReferenceResource), Order = 22)]
        public string CaseKakuUser { get; set; }

        /// <summary>
        /// 検品日時
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.KenDate), ResourceType = typeof(BtoBReferenceResource), Order = 23)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? KenDate { get; set; }

        /// <summary>
        /// 検品担当者
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.KenUser), ResourceType = typeof(BtoBReferenceResource), Order = 24)]
        public string KenUser { get; set; }

        /// <summary>
        /// 納品書発行日時
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NouhinPrnDate), ResourceType = typeof(BtoBReferenceResource), Order = 25)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? NouhinPrnDate { get; set; }

        /// <summary>
        /// 納品書発行担当者
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NouhinUser), ResourceType = typeof(BtoBReferenceResource), Order = 26)]
        public string NouhinUser { get; set; }

        ///// <summary>
        ///// 浪速送り状親No
        ///// </summary>
        //[Display(Name = nameof(BtoBReferenceResource.DeliNo2), ResourceType = typeof(BtoBReferenceResource), Order = 27)]
        //public string DeliNo2 { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.Transporter), ResourceType = typeof(BtoBReferenceResource), Order = 28)]
        public string TransporterName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NouhinNo), ResourceType = typeof(BtoBReferenceResource), Order = 29)]
        public string NouhinNo { get; set; }

        /// <summary>
        /// 上代
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NormalSellingPriceExTax), ResourceType = typeof(BtoBReferenceResource), Order = 30)]
        public int? NormalSellingPriceExTax { get; set; }
    }
}