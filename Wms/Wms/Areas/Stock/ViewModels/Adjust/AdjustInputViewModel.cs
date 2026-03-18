namespace Wms.Areas.Stock.ViewModels.Adjust
{
    using PagedList;
    using Share.Common.Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Stock.Resources;

    public class AdjustInputViewModel
    {

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }

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
        /// ロケーション
        /// </summary>
        /// <remarks>
        public string LocationCd { get; set; }

        /// <summary>
        /// ロケーション区分
        /// </summary>
        /// <remarks>
        public string LocationClass { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        public string Jan { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        public string GradeId { get; set; }

        /// <summary>
        /// 格付
        /// </summary>
        /// <remarks>
        public string GradeName { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? BeforeStockQty { get; set; }

        /// <summary>
        /// 引当数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 未引当数
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? UnAllocQty { get { return this.BeforeStockQty - this.AllocQty; } }

        /// <summary>
        /// 在庫数合計
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQtySum { get; set; }

        /// <summary>
        /// 在庫訂正数合計
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AdjustQtyToSum { get; set; }

        /// <summary>
        /// 訂正理由
        /// </summary>
        /// <remarks>
        public string AdjustReasonCd { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.Note), ResourceType = typeof(AdjustResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Note { get; set; }

        /// <summary>
        /// 在庫調整No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.SlipNo), ResourceType = typeof(AdjustResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// List record
        /// </summary>
        public IList<Models.StkAdjust02> AdjustInputs { get; set; }
    }
}