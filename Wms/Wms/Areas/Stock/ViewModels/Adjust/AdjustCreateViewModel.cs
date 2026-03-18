using Share.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Wms.Areas.Stock.Resources;

namespace Wms.Areas.Stock.ViewModels.Adjust
{
    public class AdjustCreateViewModel
    {
        /// <summary>
        /// センターコード
        /// </summary>
        [Display(Name = nameof(AdjustResource.CenterId), ResourceType = typeof(AdjustResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AdjustResource.LocationCd), ResourceType = typeof(AdjustResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [MaxLength(36, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AdjustResource.BoxNo), ResourceType = typeof(AdjustResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AdjustResource.ItemSkuId), ResourceType = typeof(AdjustResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        [MaxLength(13, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AdjustResource.Jan), ResourceType = typeof(AdjustResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Range(0, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AdjustResource.StockQty), ResourceType = typeof(AdjustResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQty { get; set; }

        [Display(Name = nameof(AdjustResource.AdjustReasonCd), ResourceType = typeof(AdjustResource))]
        /// <summary>
        /// 訂正理由
        /// </summary>
        public string AdjustReasonCd { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(AdjustResource.Note), ResourceType = typeof(AdjustResource))]
        public string Note { get; set; }

        /// <summary>
        /// 在庫調整No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.SlipNo), ResourceType = typeof(AdjustResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SlipNo { get; set; }

        /// <summary>
        /// 納品書No
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(AdjustResource.InvoiceNoForCreate), ResourceType = typeof(AdjustResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InvoiceNo { get; set; }
    }
}