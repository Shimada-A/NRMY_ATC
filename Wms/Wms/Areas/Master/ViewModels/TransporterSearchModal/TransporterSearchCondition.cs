namespace Wms.Areas.Master.ViewModels.TransporterSearchModal
{
    using System.ComponentModel.DataAnnotations;
    using PagedList;
    using Wms.Areas.Master.Resources;

    public class TransporterSearchCondition
    {
        /// <summary>
        /// 並び順
        /// </summary>
        public enum SortKey1
        {
            [Display(Name = nameof(TransporterResource.TransporterId), ResourceType = typeof(TransporterResource))]
            TransporterId,

            [Display(Name = nameof(TransporterResource.TransporterName), ResourceType = typeof(TransporterResource))]
            TransporterName
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
        public string TransporterId { get; set; }

        /// <summary>
        /// 仕入先名
        /// </summary>
        public string TransporterName { get; set; }

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
        public IPagedList<TransporterViewModel> TransporterViewModel { get; set; }

        /// <summary>
        /// 仕入先ID
        /// </summary>
        public string ParameterTransporterId { get; set; }
    }
}