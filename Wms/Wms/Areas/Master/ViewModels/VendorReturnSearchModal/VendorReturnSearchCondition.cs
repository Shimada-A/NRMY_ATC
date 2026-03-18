namespace Wms.Areas.Master.ViewModels.VendorReturnSearchModal
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Areas.Master.Resources;

    public class VendorReturnSearchCondition
    {
        /// <summary>
        /// 並び順
        /// </summary>
        public enum SortKey1
        {
            [Display(Name = nameof(SharedResource.VendorId), ResourceType = typeof(SharedResource))]
            VendorReturnId
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

        public string CenterId { get; set; }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        public string VendorReturnId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        public string VendorReturnName { get; set; }

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
        public IPagedList<VendorReturnViewModel> VendorReturnViewModel { get; set; }

        public IList<VendorReturnViewModel> vendorReturnViewModel { get; set; }

        public int? totalCnt { get; set; }
    }
}