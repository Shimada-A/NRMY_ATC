namespace Wms.Areas.Master.ViewModels.VendorSearchModal
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Areas.Master.Resources;

    public class VendorSearchCondition
    {
        /// <summary>
        /// 並び順
        /// </summary>
        public enum SortKey1
        {
            [Display(Name = nameof(SharedResource.VendorId), ResourceType = typeof(SharedResource))]
            VendorId,

            [Display(Name = nameof(SharedResource.Vendor), ResourceType = typeof(SharedResource))]
            Vendor
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum SortKey2
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 並び順
        /// </summary>
        public SortKey1 SortKey { get; set; }

        /// <summary>
        /// 降順or昇順
        /// </summary>
        public SortKey2 OrderKey { get; set; }

        /// <summary>
        /// ページ番号
        /// </summary>
        private int page;

        /// <summary>
        /// ページ番号の設定
        /// </summary>
        public int? Page
        {
            get
            {
                if (page == 0)
                {
                    return 1;
                }

                return page;
            }

            set
            {
                page = value ?? 1;
            }
        }

        /// <summary>
        /// 検索結果一覧リスト
        /// </summary>
        public IPagedList<VendorViewModel> VendorViewModel { get; set; }
    }
}