namespace Wms.Areas.Master.ViewModels.Store
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Foolproof;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 店舗
    /// </summary>
    public partial class Detail : BaseModel
    {
        /// <summary>
        /// 店舗区分 (STORE_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:通常店舗, 2:催事店舗, 3:EC, 4:卸, 8:倉庫, 9:本部
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StoreClass), ResourceType = typeof(Resources.StoreResource))]
        public string StoreClass { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        /// <remarks>
        /// （店舗コード）
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StoreId), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗名1 (STORE_NAME1)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StoreName1), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreName1 { get; set; }

        /// <summary>
        /// 店舗名2 (STORE_NAME2)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreName2), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreName2 { get; set; }

        /// <summary>
        /// 店舗名(略称) (STORE_SHORT_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StoreShortName), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreShortName { get; set; }

        /// <summary>
        /// 店舗郵便番号 (STORE_ZIP)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StoreZip), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreZip { get; set; }

        /// <summary>
        /// 店舗都道府県 (STORE_PREF_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StorePrefName), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StorePrefName { get; set; }

        /// <summary>
        /// 店舗市区町村 (STORE_CITY_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreCityName), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreCityName { get; set; }

        /// <summary>
        /// 店舗それ以降の住所1 (STORE_ADDRESS1)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StoreAddress1), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreAddress1 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所2 (STORE_ADDRESS2)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreAddress2), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreAddress2 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所3 (STORE_ADDRESS3)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreAddress3), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreAddress3 { get; set; }

        /// <summary>
        /// 店舗TEL (STORE_TEL)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreTel), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(60, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreTel { get; set; }

        /// <summary>
        /// 店舗FAX (STORE_FAX)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreFax), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(60, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreFax { get; set; }

        /// <summary>
        /// 店舗Mail1 (STORE_MAIL1)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreMail1), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreMail1 { get; set; }

        /// <summary>
        /// ECサイト区分 (EC_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:Amazon, 2:Yahoo, 3:Rakuten, 4:Zozo, 5:SHOPLIST
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.EcClass), ResourceType = typeof(Resources.StoreResource))]
        public string EcClass { get; set; }

        /// <summary>
        /// 店舗ランクID (STORE_RANK_ID)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.StoreRankId), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreRankId { get; set; }

        /// <summary>
        /// 開店日 (OPEN_DATE)
        /// </summary>
        /// <remarks>
        /// EC倉庫は未設定
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.OpenDate), ResourceType = typeof(Resources.StoreResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string OpenDate { get; set; }

        /// <summary>
        /// 閉店日 (CLOSE_DATE)
        /// </summary>
        /// <remarks>
        /// EC倉庫は未設定
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.CloseDate), ResourceType = typeof(Resources.StoreResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public string CloseDate { get; set; }

        /// <summary>
        /// エリアID (AREA_ID)
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.AreaId), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AreaId { get; set; }

        /// <summary>
        /// 都道府県コード (PREF_ID)
        /// </summary>
        /// <remarks>
        /// 受信時都道府県からセットする
        /// </remarks>
        [Display(Name = nameof(Resources.StoreResource.PrefId), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefId { get; set; }

        /// <summary>
        /// 欠品不可フラグ (STOCK_OUT_DISABLE_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:引当時欠品不可
        /// メンテナンス画面からメンテされる。
        /// 受信時新規追加する場合、店舗区分が卸なら1を自動でセットする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.StockOutDisableFlag), ResourceType = typeof(Resources.StoreResource))]
        public bool StockOutDisableFlag { get; set; }

        /// <summary>
        /// 検品必須フラグ (INSPECTION_MUST_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:検品必須
        /// メンテナンス画面からメンテされる。
        /// 受信時新規追加する場合、店舗区分が卸なら1を自動でセットする
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.InspectionMustFlag), ResourceType = typeof(Resources.StoreResource))]
        public bool InspectionMustFlag { get; set; }

        /// <summary>
        /// 事業部ID
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.DivisionId), ResourceType = typeof(Resources.StoreResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// 事業部名
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// 削除フラグ 0:未削除 1:削除済み
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.StoreResource.DeleteFlag), ResourceType = typeof(Resources.StoreResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 送り状用店舗名 (INVOICE_STORE_NAME)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.得意先名[文字(60)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.InvoiceStoreName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InvoiceStoreName { get; set; }

        /// <summary>
        /// 仕分パターンID (PATTERN_ID)
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(StoreResource.PatternId), ResourceType = typeof(StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PatternId { get; set; }

        /// <summary>
        /// 売上基準区分 (SALE_BASE_CLASS)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.売上基準区分[数字(1)]

        /// 1:消化ベース、2:出荷ベース
        /// </remarks>
        [Display(Name = nameof(StoreResource.SaleBaseClass), ResourceType = typeof(StoreResource))]
        public string SaleBaseClass { get; set; }

        /// <summary>
        /// 管轄倉庫コード (CONTROL_CENTER_ID)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.管轄倉庫コード[文字(3)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.ControlCenterId), ResourceType = typeof(StoreResource))]
        public string ControlCenterId { get; set; }


        /// <summary>
        /// 金額丸目区分 (ROUND_CLASS)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.金額丸目[数字(1)]

        /// 0：1円未満四捨五入

        /// 2：1円の位切上

        /// 5：1円未満切捨
        /// </remarks>
        [Display(Name = nameof(StoreResource.RoundClass), ResourceType = typeof(StoreResource))]
        public string RoundClass { get; set; }


        /// <summary>
        /// 仮店舗区分 (TEMP_STORE_CLASS)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.仮店舗区分[文字(1)]

        /// 0：通常、1：仮店舗、2：営業発注
        /// </remarks>
        [Display(Name = nameof(StoreResource.TempStoreClass), ResourceType = typeof(StoreResource))]
        public string TempStoreClass { get; set; }

        /// <summary>
        /// 閉店区分 (CLOSE_CLASS)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.閉店区分[文字(1)]

        /// 0：営業中、1：閉店
        /// </remarks>
        [Display(Name = nameof(StoreResource.CloseClass), ResourceType = typeof(StoreResource))]
        public string CloseClass { get; set; }

    }
}