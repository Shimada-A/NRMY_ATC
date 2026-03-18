namespace Wms.Areas.Move.ViewModels.TransferReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Move.Resources;

    public class TransferReference02Report
    {

        /// <summary>
        /// 伝票日付
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.DenpyoDate), ResourceType = typeof(TransferReferenceResource), Order = 1)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SlipDate { get; set; }

        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreId), ResourceType = typeof(Resources.TransferReferenceResource), Order = 2)]
        public string TransferFromStoreId { get; set; }

        /// <summary>
        /// 移動元
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.TransferFromStoreName), ResourceType = typeof(Resources.TransferReferenceResource), Order = 3)]
        public string TransferFromStoreName { get; set; }

        /// <summary>
        /// 伝票番号
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.SlipNo), ResourceType = typeof(TransferReferenceResource), Order = 4)]
        public string SlipNo { get; set; }

        ///// <summary>
        ///// 行No
        ///// </summary>
        ///// <remarks>
        //[Display(Name = nameof(TransferReferenceResource.SlipSeq), ResourceType = typeof(TransferReferenceResource), Order = 4)]
        //public long SlipSeq { get; set; }

        /// <summary>
        /// 分類1
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.CategoryName1), ResourceType = typeof(TransferReferenceResource), Order = 5)]
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Item), ResourceType = typeof(TransferReferenceResource), Order = 6)]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Item), ResourceType = typeof(TransferReferenceResource), Order = 7)]
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemColor), ResourceType = typeof(TransferReferenceResource), Order = 8)]
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemColor), ResourceType = typeof(TransferReferenceResource), Order = 9)]
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemSize), ResourceType = typeof(TransferReferenceResource), Order = 10)]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ItemSize), ResourceType = typeof(TransferReferenceResource), Order = 11)]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.Jan), ResourceType = typeof(TransferReferenceResource), Order = 12)]
        public string Jan { get; set; }

        /// <summary>
        /// 予定数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ArrivePlanQty), ResourceType = typeof(TransferReferenceResource), Order = 13)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQty { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.ResultQty), ResourceType = typeof(TransferReferenceResource), Order = 14)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.DifferenceQty), ResourceType = typeof(TransferReferenceResource), Order = 15)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// 状況名
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.TransferStatus), ResourceType = typeof(TransferReferenceResource), Order = 16)]
        public string TransferStatusName { get; set; }

        /// <summary>
        /// 実績確定日
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.TransferReferenceResource.ConfirmDate), ResourceType = typeof(Resources.TransferReferenceResource), Order = 17)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ConfirmDate { get; set; }

        /// <summary>
        /// 予定外
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.UnplannedFlag), ResourceType = typeof(TransferReferenceResource), Order = 18)]
        public string UnplannedFlag { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(TransferReferenceResource.BoxNo), ResourceType = typeof(TransferReferenceResource), Order = 19)]
        public string BoxNo { get; set; }
    }
}