namespace Wms.Areas.Master.ViewModels.SyukkasakiSearchModal
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Resources;

    public class SyukkasakiSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum StoreSortKey : byte
        {
            /// <summary>
            /// 出荷先所在ID
            /// </summary>
            [Display(Name = nameof(Resources.LocTransporterResource.ShipToStoreId), ResourceType = typeof(Resources.LocTransporterResource))]
            ShipToStoreId,

            /// <summary>
            /// 出荷先所在ID
            /// </summary>
            [Display(Name = nameof(Resources.LocTransporterResource.Area), ResourceType = typeof(Resources.LocTransporterResource))]
            AeraId,
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

        public List<AreaItem> AreaItem { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        public string StoreName { get; set; }

        public StoreSortKey SortKey { get; set; }

        public AscDescSort Sort { get; set; }

        public string StoreClass { get; set; } 

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
        public IPagedList<SyukkasakiViewModel> SyukkasakiViewModel { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        public string ParameterStoreId { get; set; }
        public string TempStoreId { get; set; }
    }

    public class AreaItem
    {
        public bool IsCheck { get; set; }

        public string AreaId { get; set; }
    }
}