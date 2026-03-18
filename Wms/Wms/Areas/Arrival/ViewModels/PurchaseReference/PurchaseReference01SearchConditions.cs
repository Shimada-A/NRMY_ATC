namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PurchaseReference01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum PurchaseReference01SortKey : byte
        {
            [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDateVendorId), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            ArrivePlanDateVendorId,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.BrandIdArrivePlanDate), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            BrandIdArrivePlanDate

            //[Display(Name = nameof(Resources.PurchaseReferenceResource.VendorIdArrivePlanDate), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            //VendorIdArrivePlanDate,

            //[Display(Name = nameof(Resources.PurchaseReferenceResource.VendorNmArrivePlanDate), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            //VendorNmArrivePlanDate
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
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 入荷予定日From
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArrivePlanDateFrom { get; set; } = DateTime.Now;
        public string strArrivePlanDateFrom { get {
                return DateTime.TryParse(ArrivePlanDateFrom.ToString(), out DateTime dt) ? dt.ToString("yyyy/MM/dd") : string.Empty; } }
        /// <summary>
        /// 入荷予定日To
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? ArrivePlanDateTo { get; set; } = DateTime.Now;
        public string strArrivePlanDateTo
        {
            get
            {
                return DateTime.TryParse(ArrivePlanDateTo.ToString(), out DateTime dt) ? dt.ToString("yyyy/MM/dd") : string.Empty;
            }
        }

        /// <summary>
        /// 実績確定日From
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ConfirmDateFrom { get; set; } = null;
        public string strConfirmDateFrom
        {
            get
            {
                return DateTime.TryParse(ConfirmDateFrom.ToString(), out DateTime dt) ? dt.ToString("yyyy/MM/dd") : string.Empty;
            }
        }
        /// <summary>
        /// 実績確定日To
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]

        public DateTime? ConfirmDateTo { get; set; } = null;
        public string strConfirmDateTo
        {
            get
            {
                return DateTime.TryParse(ConfirmDateTo.ToString(), out DateTime dt) ? dt.ToString("yyyy/MM/dd") : string.Empty;
            }
        }

        /// <summary>
        /// 過去分を含む
        /// </summary>
        public bool ContainsArchive { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public List<string> BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 仕入先
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public PurchaseReference01SortKey SortKey { get; set; } = PurchaseReference01SortKey.ArrivePlanDateVendorId;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQtySum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ArrivePlanQtySum { get; set; }

        /// <summary>
        /// 実績数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtySum { get; set; }

        /// <summary>
        /// 予定伝票数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PlanSlipNoSum { get; set; }

        /// <summary>
        /// 実績伝票数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultSlipNoSum { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PurchaseReference01SearchConditions()
        {
            BrandId = new List<string>();
        }
    }
}