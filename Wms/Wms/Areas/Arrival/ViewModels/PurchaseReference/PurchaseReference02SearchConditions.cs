namespace Wms.Areas.Arrival.ViewModels.PurchaseReference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using System.Runtime.Serialization;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    [Serializable]
    public class PurchaseReference02SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum PurchaseReference02DetailSort : byte
        {
            [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDateSkuItem), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            ArrivePlanDateSkuItem,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDateVendorIdInvoice), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            ArrivePlanDateVendorIdInvoice,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDateVendorIdSku), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            ArrivePlanDateVendorIdSku,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.VendorIdInvoiceNoSku), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            VendorIdInvoiceNoSku,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.VendorIdSkuInvoiceNo), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            VendorIdSkuInvoiceNo,
        }
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum PurchaseReference02BoxSort : byte
        {
            [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDateSkuItemVendorId), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            ArrivePlanDateSkuItemVendorId,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDateVendorIdInvoiceBoxNo), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            ArrivePlanDateVendorIdInvoiceBoxNo,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.ArrivePlanDateVendorIdSkuBoxNo), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            ArrivePlanDateVendorIdSkuBoxNo,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.VendorIdInvoiceNoSkuBoxNo), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            VendorIdInvoiceNoSkuBoxNo,

            [Display(Name = nameof(Resources.PurchaseReferenceResource.VendorIdSkuInvoiceNoBoxNo), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            VendorIdSkuInvoiceNoBoxNo,

            //[Display(Name = nameof(Resources.PurchaseReferenceResource.VendorNmBoxNoInvoiceNoSku), ResourceType = typeof(Resources.PurchaseReferenceResource))]
            //VendorNmBoxNoInvoiceNoSku
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
        /// 明細種別 0：明細 1：ケース
        /// </summary>
        public enum ResultTypes : byte
        {
            Detail = 0,
            BoxNo = 1,
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 入荷予定日From
        /// </summary>
        public DateTime? ArrivePlanDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 入荷予定日To
        /// </summary>
        public DateTime? ArrivePlanDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 実績確定日From
        /// </summary>
        public DateTime? ConfirmDateFrom { get; set; }

        /// <summary>
        /// 実績確定日To
        /// </summary>
        public DateTime? ConfirmDateTo { get; set; }

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
        /// 分類1
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// アイテムコード
        /// </summary>
        public string ItemCode { get; set; }


        /// <summary>
        /// 状況
        /// </summary>
        public string ArrivalStatus { get; set; }

        /// <summary>
        /// 納品書番号
        /// </summary>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 発注番号
        /// </summary>
        public string PoId { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; } = 0;

        /// <summary>
        /// Sort key
        /// </summary>
        public PurchaseReference02DetailSort DetailSort { get; set; } = PurchaseReference02DetailSort.ArrivePlanDateSkuItem;

        /// <summary>
        /// Sort key
        /// </summary>
        public PurchaseReference02BoxSort BoxSort { get; set; } = PurchaseReference02BoxSort.ArrivePlanDateSkuItemVendorId;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Result Type
        /// </summary>
        public ResultTypes ResultType { get; set; } = ResultTypes.Detail;

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

        public bool FromMenu { get; set; } = true;

        /// <summary>
        /// 遷移元画面(戻る対象画面)
        /// </summary>
        public string ReturnDisp { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PurchaseReference02SearchConditions()
        {
            BrandId = new List<string>();
        }
    }
}