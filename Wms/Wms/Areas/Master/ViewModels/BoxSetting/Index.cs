namespace Wms.Areas.Master.ViewModels.BoxSetting
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class BoxSettingSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum BoxSettingSortKey : byte
        {
            [Display(Name = nameof(Resources.BoxSettingResource.Category), ResourceType = typeof(Resources.BoxSettingResource))]
            Category,

            [Display(Name = nameof(Resources.BoxSettingResource.ItemId), ResourceType = typeof(Resources.BoxSettingResource))]
            ItemId
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
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public BoxSettingSortKey SortKey { get; set; }

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

    public class BoxSettingResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<BoxSettingList> BoxSettings { get; set; }
    }

    public class Index
    {
        public BoxSettingSearchCondition SearchConditions { get; set; }

        public BoxSettingResult BoxSettingResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxSettingViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new BoxSettingSearchCondition();
            this.BoxSettingResult = new BoxSettingResult();
        }
    }
}