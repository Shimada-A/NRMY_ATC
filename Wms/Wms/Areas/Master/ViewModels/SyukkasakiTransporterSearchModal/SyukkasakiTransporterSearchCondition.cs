namespace Wms.Areas.Master.ViewModels.SyukkasakiTransporterSearchModal
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Resources;

    public class SyukkasakiTransporterSearchCondition
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum StoreSortKey : byte
        {
            /// <summary>
            /// 出荷先ID
            /// </summary>
            [Display(Name = nameof(Resources.SyukkasakiTransporterResource.ShipToStoreId), ResourceType = typeof(Resources.SyukkasakiTransporterResource))]
            ShipToStoreId,

            /// <summary>
            /// 出荷先NAME
            /// </summary>
            [Display(Name = nameof(Resources.SyukkasakiTransporterResource.ShipToStoreName), ResourceType = typeof(Resources.SyukkasakiTransporterResource))]
            ShipToStoreName,
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
        /// 催事店舗区分
        /// </summary>
        public enum StoreOutletsClasses : byte
        {
            [Display(Name = nameof(Resources.SyukkasakiTransporterResource.InStoreOutlets), ResourceType = typeof(Resources.SyukkasakiTransporterResource))]
            Outlets,

            [Display(Name = nameof(Resources.SyukkasakiTransporterResource.NotStoreOutlets), ResourceType = typeof(Resources.SyukkasakiTransporterResource))]
            NotOutlets,

            [Display(Name = nameof(Resources.SyukkasakiTransporterResource.OnlyStoreOutlets), ResourceType = typeof(Resources.SyukkasakiTransporterResource))]
            OnlyOutlets
        }

        /// <summary>
        /// センターID
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// センター名
        /// </summary>
        public string CenterName { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 出荷先区分
        /// </summary>
        public string StoreClass { get; set; }

        /// <summary>
        /// 店ランク
        /// </summary>
        public string StoreRanks { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        public List<AreaItem> AreaItem { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        public string ShipToStoreId { get; set; }


        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// 催事店舗区分
        /// </summary>
        public StoreOutletsClasses StoreOutletsClass { get; set; } = StoreOutletsClasses.NotOutlets;

        /// <summary>
        /// 並び順
        /// </summary>
        public StoreSortKey SortKey { get; set; }

        /// <summary>
        /// 降順or昇順
        /// </summary>
        public AscDescSort OrderKey { get; set; }

        /// <summary>
        /// 出荷先区分が倉庫のみ
        /// </summary>
        public bool IsCenterOnly { get; set; } = false;

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
        /// メイン／月～祝
        /// </summary>
        public string DayClass { get; set; } = "0";

        /// <summary>
        /// 検索結果一覧リスト
        /// </summary>
        public IPagedList<SyukkasakiTransporterViewModel> SyukkasakiTransporterViewModel { get; set; }
    }

    public class AreaItem
    {
        public bool IsCheck { get; set; }

        public string AreaId { get; set; }
    }

}