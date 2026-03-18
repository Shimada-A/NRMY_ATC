namespace Wms.Areas.Master.ViewModels.EcPrefTransporter
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using PagedList;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class EcPrefTransporterSearchCondition
    {
        /// <summary>
        /// 表示順
        /// </summary>
        public enum EcPrefTransporterSortKey : byte
        {
            /// <summary>
            /// 都道府県コード
            /// </summary>
            [Display(Name = nameof(Resources.EcPrefTransporterResource.PrefId), ResourceType = typeof(Resources.EcPrefTransporterResource))]
            PrefId,
            /// <summary>
            /// 配送業者ID→都道府県コード
            /// </summary>
            [Display(Name = nameof(Resources.EcPrefTransporterResource.TransporterIdPrefId), ResourceType = typeof(Resources.EcPrefTransporterResource))]
            TransporterIdPrefId
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
        /// センターコード(倉庫ID)
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        ///配送業者ID
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 表示順
        /// </summary>
        public EcPrefTransporterSortKey SortKey { get; set; }

        /// <summary>
        /// 昇順降順
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
    }

    public class EcPrefTransporterResult
    {
        /// <summary>
        /// 一覧
        /// </summary>
        public IPagedList<EcPrefTransporterList> EcPrefTransporters { get; set; }
    }

    public class Index
    {
        public EcPrefTransporterSearchCondition SearchConditions { get; set; }

        public EcPrefTransporterResult EcPrefTransporterResult { get; set; }

        public int Page { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcPrefTransporterViewModel"/> class.
        /// </summary>
        public Index()
        {
            this.SearchConditions = new EcPrefTransporterSearchCondition();
            this.EcPrefTransporterResult = new EcPrefTransporterResult();
        }
    }
}