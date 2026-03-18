namespace Wms.Areas.Master.ViewModels.LocTransporter
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Resources;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class LocTransporterSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum LocTransporterSortKey : byte
        {
            /// <summary>
            /// 出荷元→出荷先→適用開始日
            /// </summary>
            [Display(Name = nameof(Resources.LocTransporterResource.SKey1), ResourceType = typeof(Resources.LocTransporterResource))]
            SKey1,

            /// <summary>
            /// 出荷元→適用開始日→出荷先
            /// </summary>
            [Display(Name = nameof(Resources.LocTransporterResource.SKey2), ResourceType = typeof(Resources.LocTransporterResource))]
            SKey2,

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
        /// 出荷先所在ID
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;


        /// <summary>
        /// 出荷先所在名称
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public LocTransporterSortKey SortKey { get; set; }

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
        /// 出荷先所在区分
        /// </summary>
        public string ShipToStoreClass { get; set; }

        //public enum LocClasses : int
        //{
        //    [Display(Name = nameof(CommonResource.None), ResourceType = typeof(CommonResource))]
        //    None = 0,

        //    [Display(Name = nameof(CommonResource.StoreNormal), ResourceType = typeof(CommonResource))]
        //    Normal = 1,

        //    [Display(Name = nameof(CommonResource.StoreOutlets), ResourceType = typeof(CommonResource))]
        //    Outlets = 2,

        //    [Display(Name = nameof(CommonResource.StoreEC), ResourceType = typeof(CommonResource))]
        //    EC = 3,

        //    [Display(Name = nameof(CommonResource.StoreStocktake), ResourceType = typeof(CommonResource))]
        //    Stocktake = 4,

        //    [Display(Name = nameof(CommonResource.StoreWarehouse), ResourceType = typeof(CommonResource))]
        //    Warehouse = 8,

        //    [Display(Name = nameof(CommonResource.StoreHome), ResourceType = typeof(CommonResource))]
        //    Home = 9
        //}

        public List<SyukkasakiSearchModal.AreaItem> AreaItem { get; set; }

        public bool IsNewDate { get; set; }

        public List<string> RowId { get; set; }
    }

    public class LocTransporterResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<SearchItem> LocTransporters { get; set; }
    }

    public class Index
    {
        public LocTransporterSearchCondition SearchConditions { get; set; }

        public LocTransporterResult LocTransporterResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocTransporterViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new LocTransporterSearchCondition();
            this.LocTransporterResult = new LocTransporterResult();
        }
    }
}