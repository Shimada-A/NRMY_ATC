namespace Wms.Areas.Master.ViewModels.Color
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Common;

    public class ColorSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum ColorSortKey : byte
        {
            [Display(Name = nameof(Resources.ColorResource.ItemColorId), ResourceType = typeof(Resources.ColorResource))]
            ItemColorId,

            [Display(Name = nameof(Resources.ColorResource.ItemColorName), ResourceType = typeof(Resources.ColorResource))]
            ItemColorName
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
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名 (ITEM_COLOR_NAME)
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public ColorSortKey SortKey { get; set; }

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
        /// 削除フラグ
        /// </summary>
        public bool DeleteFlag { get; set; }
    }

    public class ColorResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Models.Color> Colors { get; set; }
    }

    public class Index
    {
        public ColorSearchCondition SearchConditions { get; set; }

        public ColorResult ColorResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoreViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new ColorSearchCondition();
            this.ColorResult = new ColorResult();
        }
    }
}