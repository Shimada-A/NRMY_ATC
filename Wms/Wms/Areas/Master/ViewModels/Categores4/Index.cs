namespace Wms.Areas.Master.ViewModels.Categores4
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Common;

    public class Categores4SearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum Categores4SortKey : byte
        {
            [Display(Name = nameof(Resources.Categores4Resource.CategoryIdSort), ResourceType = typeof(Resources.Categores4Resource))]
            CategoryId1,

            [Display(Name = nameof(Resources.Categores4Resource.CategoryNameSort), ResourceType = typeof(Resources.Categores4Resource))]
            CategoryName1
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
        /// 分類ID1 (CATEGORY_ID1)
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類ID2 (CATEGORY_ID2)
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類ID3 (CATEGORY_ID3)
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類ID4 (CATEGORY_ID4)
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 分類名 (CATEGORY_NAME1)
        /// </summary>
        public string CategoryName1 { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public Categores4SortKey SortKey { get; set; }

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

    public class Categores4Result
    {
        /// <summary>
        /// List record
        /// </summary>
        public IPagedList<Categores4ViewModel> Categores4s { get; set; }
    }

    public class Index
    {
        public Categores4SearchCondition SearchConditions { get; set; }

        public Categores4Result Categores4Result { get; set; }

        public int Page { get; set; }


        public Index()
        {
            this.SearchConditions = new Categores4SearchCondition();
            this.Categores4Result = new Categores4Result();
        }
    }
}