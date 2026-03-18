namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// 店舗
    /// </summary>
    [Table("M_STORES")]
    public partial class Store : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.DeleteFlag), ResourceType = typeof(StoreResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        /// <remarks>
        /// 店舗コード 
        /// IF得意先マスタ.得意先コード[文字(8)]
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.StoreId), ResourceType = typeof(StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 店舗区分 (STORE_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:直営、2:消化、3:委託、4:FC、5:買取、6:ライセンス、　
        /// （8:倉庫）
        /// IF得意先マスタ.店舗区分[文字(1)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.StoreClass), ResourceType = typeof(StoreResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreClass { get; set; }

        /// <summary>
        /// 店舗名1 (STORE_NAME1)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.得意先名[文字(60)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.StoreName1), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreName1 { get; set; }

        /// <summary>
        /// 店舗名2 (STORE_NAME2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreName2), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreName2 { get; set; }

        /// <summary>
        /// 店舗名(略称) (STORE_SHORT_NAME)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.略称[文字(20)]
        /// ハンディ、ラベル印字で使用する。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.StoreShortName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreShortName { get; set; }

        /// <summary>
        /// 店舗名1(カナ) (STORE_KANA_NAME1)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreKanaName1), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreKanaName1 { get; set; }

        /// <summary>
        /// 店舗名2(カナ) (STORE_KANA_NAME2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreKanaName2), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreKanaName2 { get; set; }

        /// <summary>
        /// 店舗名(略称カナ) (STORE_KANA_SHORT_NAME)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreKanaShortName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreKanaShortName { get; set; }

        /// <summary>
        /// 店舗国 (STORE_COUNTRY)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreCountry), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreCountry { get; set; }

        /// <summary>
        /// 店舗郵便番号 (STORE_ZIP)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.郵便番号[文字(10)]　
        /// マスタ受信時、ハイフン削除する
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreZip), ResourceType = typeof(StoreResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreZip { get; set; }

        /// <summary>
        /// 店舗都道府県 (STORE_PREF_NAME)
        /// </summary>
        /// <remarks>
        /// 受信時、住所１から切り出して設定
        /// </remarks>
        [Display(Name = nameof(StoreResource.StorePrefName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StorePrefName { get; set; }

        /// <summary>
        /// 店舗都道府県(カナ) (STORE_PREF_KANA_NAME)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StorePrefKanaName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StorePrefKanaName { get; set; }

        /// <summary>
        /// 店舗市区町村 (STORE_CITY_NAME)
        /// </summary>
        [Display(Name = nameof(StoreResource.StoreCityName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreCityName { get; set; }

        /// <summary>
        /// 店舗市区町村(カナ) (STORE_CITY_KANA_NAME)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreCityKanaName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreCityKanaName { get; set; }

        /// <summary>
        /// 店舗それ以降の住所1 (STORE_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.住所１[文字(100)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreAddress1), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreAddress1 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所1(カナ) (STORE_KANA_ADDRESS1)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreKanaAddress1), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreKanaAddress1 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所2 (STORE_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.住所２[文字(100)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreAddress2), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreAddress2 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所2(カナ) (STORE_KANA_ADDRESS2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreKanaAddress2), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreKanaAddress2 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所3 (STORE_ADDRESS3)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.住所３[文字(100)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreAddress3), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreAddress3 { get; set; }

        /// <summary>
        /// 店舗それ以降の住所3(カナ) (STORE_KANA_ADDRESS3)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreKanaAddress3), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreKanaAddress3 { get; set; }

        /// <summary>
        /// 店舗TEL (STORE_TEL)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.電話番号[文字(60)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreTel), ResourceType = typeof(StoreResource))]
        [MaxLength(60, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreTel { get; set; }

        /// <summary>
        /// 店舗FAX (STORE_FAX)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.FAX番号[文字(60)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreFax), ResourceType = typeof(StoreResource))]
        [MaxLength(60, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreFax { get; set; }

        /// <summary>
        /// 店舗Mail1 (STORE_MAIL1)
        /// </summary>
        [Display(Name = nameof(StoreResource.StoreMail1), ResourceType = typeof(StoreResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreMail1 { get; set; }

        /// <summary>
        /// 店舗Mail2 (STORE_MAIL2)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreMail2), ResourceType = typeof(StoreResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreMail2 { get; set; }

        /// <summary>
        /// ECサイト区分 (EC_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:Amazon, 2:Yahoo, 3:Rakuten, 4:Zozo, 5:SHOPLIST
        /// </remarks>
        [Display(Name = nameof(StoreResource.EcClass), ResourceType = typeof(StoreResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string EcClass { get; set; }

        /// <summary>
        /// 店舗ランクID (STORE_RANK_ID)
        /// </summary>
        /// <remarks>
        /// 1:S, 2:A, 3:B, 4:C, 5:D, 6:E
        /// </remarks>
        [Display(Name = nameof(StoreResource.StoreRankId), ResourceType = typeof(StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StoreRankId { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.TransporterId), ResourceType = typeof(StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 開店日 (OPEN_DATE)
        /// </summary>
        [Display(Name = nameof(StoreResource.OpenDate), ResourceType = typeof(StoreResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? OpenDate { get; set; }

        /// <summary>
        /// 閉店日 (CLOSE_DATE)
        /// </summary>
        [Display(Name = nameof(StoreResource.CloseDate), ResourceType = typeof(StoreResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// 坪数 (SITEAREA_QTY)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.SiteareaQty), ResourceType = typeof(StoreResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SiteareaQty { get; set; }

        /// <summary>
        /// エリアマネージャーID (AREA_MANAGER_ID)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.AreaManagerId), ResourceType = typeof(StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AreaManagerId { get; set; }

        /// <summary>
        /// 会社区分 (COMPANY_CLASS)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.CompanyClass), ResourceType = typeof(StoreResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CompanyClass { get; set; }

        /// <summary>
        /// ブランド区分 (BRAND_CLASS)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.BrandClass), ResourceType = typeof(StoreResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandClass { get; set; }

        /// <summary>
        /// 所属事業所ID (BELONGS_OFFICE_ID)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.BelongsOfficeId), ResourceType = typeof(StoreResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BelongsOfficeId { get; set; }

        /// <summary>
        /// 坪タイプID (SITEAREA_TYPE_ID)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(StoreResource.SiteareaTypeId), ResourceType = typeof(StoreResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SiteareaTypeId { get; set; }

        /// <summary>
        /// エリアID (AREA_ID)
        /// </summary>
        [Display(Name = nameof(StoreResource.AreaId), ResourceType = typeof(StoreResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AreaId { get; set; }

        /// <summary>
        /// チャネルID (CHANNEL_ID)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.チャネルコード[文字(2)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.ChannelId), ResourceType = typeof(StoreResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ChannelId { get; set; }

        /// <summary>
        /// 都道府県コード (PREF_ID)
        /// </summary>
        /// <remarks>
        /// 受信時、都道府県マスタからセットする
        /// </remarks>
        [Display(Name = nameof(StoreResource.PrefId), ResourceType = typeof(StoreResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefId { get; set; }

        /// <summary>
        /// 欠品不可フラグ (STOCK_OUT_DISABLE_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:引当時欠品不可
        /// メンテナンス画面からメンテされる。
        /// 受信時新規追加する場合、初期値0
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.StockOutDisableFlag), ResourceType = typeof(StoreResource))]
        public bool StockOutDisableFlag { get; set; }

        /// <summary>
        /// 検品必須フラグ (INSPECTION_MUST_FLAG)
        /// </summary>
        /// <remarks>
        /// 1:検品必須
        /// メンテナンス画面からメンテされる。
        /// 受信時新規追加する場合、初期値0
        /// 
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StoreResource.InspectionMustFlag), ResourceType = typeof(StoreResource))]
        public bool InspectionMustFlag { get; set; }

        /// <summary>
        /// 事業部ID (DIVISION_ID)
        /// </summary>
        [Display(Name = nameof(StoreResource.DivisionId), ResourceType = typeof(StoreResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// チャネル名 (CHANNEL_NAME)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.チャネル名[文字(60)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.ChannelName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ChannelName { get; set; }

        /// <summary>
        /// 売上基準区分 (SALE_BASE_CLASS)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.売上基準区分[数字(1)]
        /// 1:消化ベース、2:出荷ベース
        /// </remarks>
        [Display(Name = nameof(StoreResource.SaleBaseClass), ResourceType = typeof(StoreResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SaleBaseClass { get; set; }

        /// <summary>
        /// 仕分パターンID (PATTERN_ID)
        /// </summary>
        /// <remarks>
        /// メンテナンス画面からメンテされる
        /// </remarks>
        [Display(Name = nameof(StoreResource.PatternId), ResourceType = typeof(StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PatternId { get; set; }

        /// <summary>
        /// 管轄倉庫コード (CONTROL_CENTER_ID)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.管轄倉庫コード[文字(3)]
        /// </remarks>
        [Display(Name = nameof(StoreResource.ControlCenterId), ResourceType = typeof(StoreResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ControlCenterId { get; set; }

        /// <summary>
        /// 送り状用店舗名 (INVOICE_STORE_NAME)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.得意先名[文字(60)]
        /// メンテナンス画面からメンテされる
        /// </remarks>
        [Display(Name = nameof(StoreResource.InvoiceStoreName), ResourceType = typeof(StoreResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InvoiceStoreName { get; set; }

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
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string RoundClass { get; set; }

        /// <summary>
        /// 仮店舗区分 (TEMP_STORE_CLASS)
        /// </summary>
        /// <remarks>
        /// IF得意先マスタ.仮店舗区分[文字(1)]
        /// 0：通常、1：仮店舗、2：営業発注
        /// </remarks>
        [Display(Name = nameof(StoreResource.TempStoreClass), ResourceType = typeof(StoreResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TempStoreClass { get; set; }

        #endregion
    }
}
