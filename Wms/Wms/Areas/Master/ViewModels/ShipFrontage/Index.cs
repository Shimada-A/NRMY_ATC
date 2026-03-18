namespace Wms.Areas.Master.ViewModels.ShipFrontage
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class ShipFrontageSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ShipFrontageSortKey 
        {
            [Display(Name = nameof(Resources.ShipFrontageResource.LaneFrontage), ResourceType = typeof(Resources.ShipFrontageResource))]
            LaneFrontage,

            [Display(Name = nameof(Resources.ShipFrontageResource.StoreId), ResourceType = typeof(Resources.ShipFrontageResource))]
            StoreId
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
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// レーンNo (LANE_NO)
        /// </summary>
        public int? LaneNo { get; set; }

        /// <summary>
        /// 間口No (FRONTAGE_NO)
        /// </summary>
        public int? FrontageNo { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public ShipFrontageSortKey SortKey { get; set; }

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
        /// ブランドID
        /// </summary>
        public string BrandId { get; set; } 

        /// <summary>
        /// ブランド名
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
    }

    public class ShipFrontageResultRow : BaseModel
    {
        /// <summary>
        /// 倉庫ID (SHIPPER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.CenterId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// センター
        /// </summary>
        public string CenterName { get; set; }

        /// <summary>
        /// レーンNo (LANE_NO)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.LaneNo), ResourceType = typeof(ShipFrontageResource))]
        [Range(1, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte LaneNo { get; set; }

        /// <summary>
        /// 間口No (FRONTAGE_NO)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.FrontageNo), ResourceType = typeof(ShipFrontageResource))]
        [Range(1, 999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int FrontageNo { get; set; }

        /// <summary>
        /// 出荷先所在ID (STORE_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.StoreId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ShipFrontageResource.BrandId), ResourceType = typeof(ShipFrontageResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 出荷先所在名
        /// </summary>
        public string StoreName { get; set; }

        public string Rid { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
    }

    public class ShipFrontageResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<ShipFrontageResultRow> ShipFrontages { get; set; }
    }

    public class Index
    {
        public ShipFrontageSearchCondition SearchConditions { get; set; }

        public ShipFrontageResult ShipFrontageResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipFrontageViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new ShipFrontageSearchCondition();
            this.ShipFrontageResult = new ShipFrontageResult();
        }
    }
}