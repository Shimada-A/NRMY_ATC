namespace Wms.Areas.Returns.ViewModels.AcceptArrival
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Wms.Areas.Returns.Resources;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class AcceptArrival03ResultRow
    {

        /// <summary>
        /// 注文番号
        /// </summary>
        public String ShipInstructId { get; set; }

        /// <summary>
        /// 出荷指示明細ID
        /// </summary>
        public String ShipInstructSeq { get; set; }

        /// <summary>
        /// ECサイト区分
        /// </summary>
        public String EcClass { get; set; }

        /// <summary>
        /// 返品日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 関連注文番号
        /// </summary>
        public String RelatedOrderNo { get; set; }

        /// <summary>
        /// 出荷確定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public String DestPrefName { get; set; }

        /// <summary>
        /// 出荷先名称
        /// </summary>
        public String DestName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }
        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// 出荷実績数
        /// </summary>
        public string ResultQty { get; set; }

        /// <summary>
        /// 過去返品数
        /// </summary>
        public string ReturnQtyBefore { get; set; }

        /// <summary>
        /// 今回返品数
        /// </summary>
        [Display(Name = nameof(AcceptArrivalResource.ReturnQtyNow), ResourceType = typeof(AcceptArrivalResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ReturnQtyNow { get; set; }

    }
}