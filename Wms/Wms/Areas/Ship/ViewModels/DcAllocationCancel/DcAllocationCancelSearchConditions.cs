namespace Wms.Areas.Ship.ViewModels.DcAllocationCancel
{
    using Share.Common.Resources;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class DcAllocationCancelSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum DcAllocationCancelSortKey : byte
        {
            [Display(Name = nameof(Resources.DcAllocationCancelResource.ShipPlanDateInstructSkuDetailId), ResourceType = typeof(Resources.DcAllocationCancelResource))]
            ShipPlanDateInstructSkuDetailId,

            [Display(Name = nameof(Resources.DcAllocationCancelResource.SkuInstructIdDetailId), ResourceType = typeof(Resources.DcAllocationCancelResource))]
            SkuInstructIdDetailId,

            [Display(Name = nameof(Resources.DcAllocationCancelResource.ShipIdSkuInstructIdDetailId), ResourceType = typeof(Resources.DcAllocationCancelResource))]
            ShipIdSkuInstructIdDetailId
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// バッチNo
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(DcAllocationCancelResource.BatchNoD), ResourceType = typeof(DcAllocationCancelResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public DcAllocationCancelSortKey SortKey { get; set; } = DcAllocationCancelSortKey.ShipPlanDateInstructSkuDetailId;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// SKU数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 明細数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DetailSum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PlanQtySum { get; set; }
    }
}