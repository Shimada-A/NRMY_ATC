namespace Wms.Areas.Ship.ViewModels.EcAllocationCancel
{
    using Share.Common.Resources;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class EcAllocationCancelSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum EcAllocationCancelSortKey : byte
        {
            [Display(Name = nameof(Resources.EcAllocationCancelResource.ShipRequestDateOrderDateInstructId), ResourceType = typeof(Resources.EcAllocationCancelResource))]
            ShipRequestDateOrderDateInstructId,

            [Display(Name = nameof(Resources.EcAllocationCancelResource.ShipInstructId), ResourceType = typeof(Resources.EcAllocationCancelResource))]
            ShipInstructId
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
        [Display(Name = nameof(EcAllocationCancelResource.AllocGroupNoH), ResourceType = typeof(EcAllocationCancelResource))]
        public string AllocGroupNo { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public EcAllocationCancelSortKey SortKey { get; set; } = EcAllocationCancelSortKey.ShipRequestDateOrderDateInstructId;

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
        /// オーダー数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderQtySum { get; set; }

        /// <summary>
        /// SKU数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQtySum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PlanQtySum { get; set; }
    }
}