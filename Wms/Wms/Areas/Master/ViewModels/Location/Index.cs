namespace Wms.Areas.Master.ViewModels.Location
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class LocationSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum LocationSortKey : byte
        {
            [Display(Name = nameof(Resources.LocationResource.LocationCd), ResourceType = typeof(Resources.LocationResource))]
            LocationCd,
            [Display(Name = nameof(Resources.LocationResource.LocationClassToCd), ResourceType = typeof(Resources.LocationResource))]
            LocationClass
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
        /// List センターコード
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// センター名
        /// </summary>
        public string CenterName { get; set; }

        /// <summary>
        /// List ロケーション区分
        /// </summary>
        public string LocationClass { get; set; }

        /// <summary>
        /// ロケーション区分名
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// List エリア
        /// </summary>
        public string Locsec1 { get; set; }

        /// <summary>
        /// List 棚列
        /// </summary>
        public string Locsec2 { get; set; }

        /// <summary>
        /// List 棚番
        /// </summary>
        public string Locsec3 { get; set; }

        /// <summary>
        /// List 段
        /// </summary>
        public string Locsec4 { get; set; }

        /// <summary>
        /// List 間口
        /// </summary>
        public string Locsec5 { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public LocationSortKey SortKey { get; set; }

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
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
    }

    public class LocationResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<LocationList> Locations { get; set; }
    }

    public class Index
    {
        public LocationSearchCondition SearchConditions { get; set; }

        public LocationResult LocationResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new LocationSearchCondition();
            this.LocationResult = new LocationResult();
        }
    }
}