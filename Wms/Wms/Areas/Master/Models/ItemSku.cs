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
    /// 商品
    /// </summary>
    [Table("M_ITEM_SKU")]
    public partial class ItemSku : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 削除フラグ (DELETE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:未削除 1:削除済み
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.DeleteFlag), ResourceType = typeof(ItemSkuResource))]
        public bool DeleteFlag { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.品番[文字(7)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.ItemId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.品名[文字(60)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.ItemName), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// 品名略称 (ITEM_SHORT_NAME)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.品名略称[文字(30)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.ItemShortName), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemShortName { get; set; }

        /// <summary>
        /// 品名カナ (ITEM_KANA_NAME)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.ItemKanaName), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemKanaName { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.ブランドコード[文字(3)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.BrandId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string BrandId { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.部門コード[文字(2)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.CategoryId1), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.品種コード[文字(2)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.CategoryId2), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.CategoryId3), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.CategoryId4), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 分類5 (CATEGORY_ID5)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.CategoryId5), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CategoryId5 { get; set; }

        /// <summary>
        /// 標準上代(税込) (NORMAL_SELLING_PRICE)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.NormalSellingPrice), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NormalSellingPrice { get; set; }

        /// <summary>
        /// 標準上代(税抜) (NORMAL_SELLING_PRICE_EX_TAX)
        /// </summary>
        /// <remarks>
        /// 売価
        /// IF商品マスタ.上代[数値(8,0)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.NormalSellingPriceExTax), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 標準下代 (NORMAL_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.確定原価[数値(8,0)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.NormalBuyingPrice), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999.99, 999999999.99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal NormalBuyingPrice { get; set; }

        /// <summary>
        /// 仕入下代 (PURCHASE_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.確定原価[数値(8,0)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.PurchaseBuyingPrice), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999.99, 999999999.99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal PurchaseBuyingPrice { get; set; }

        /// <summary>
        /// 元上代(税込) (PROPER_PRICE)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.ProperPrice), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ProperPrice { get; set; }

        /// <summary>
        /// 元上代(税抜) (PROPER_PRICE_EX_TAX)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.上代[数値(8,0)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.ProperPriceExTax), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ProperPriceExTax { get; set; }

        /// <summary>
        /// シーズン年 (SEASON_YEAR)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.年度[数値(4,0)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.SeasonYear), ResourceType = typeof(ItemSkuResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int SeasonYear { get; set; }

        /// <summary>
        /// シーズンID (ITEM_SEASON_ID)
        /// </summary>
        /// <remarks>
        /// 1:春, 2:春夏, 3:夏, 5:秋, 6:秋冬, 7:冬, 9:通年
        /// IF商品マスタ.シーズンコード[文字(1)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.ItemSeasonId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSeasonId { get; set; }

        /// <summary>
        /// 販売開始日 (SALE_START_DATE)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.SaleStartDate), ResourceType = typeof(ItemSkuResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SaleStartDate { get; set; }

        /// <summary>
        /// 販売終了日 (SALE_END_DATE)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.SaleEndDate), ResourceType = typeof(ItemSkuResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SaleEndDate { get; set; }

        /// <summary>
        /// 回収日 (WITHDRAWAL_DATE)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.WithdrawalDate), ResourceType = typeof(ItemSkuResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? WithdrawalDate { get; set; }

        /// <summary>
        /// 原産国 (ORIGIN_COUNTRY)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.原産国[文字(4)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.OriginCountry), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OriginCountry { get; set; }

        /// <summary>
        /// 顧客タイプID (ITEM_CUSTOMER_TYPE_ID)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.ItemCustomerTypeId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemCustomerTypeId { get; set; }

        /// <summary>
        /// 商品ランクID (ITEM_RANK_ID)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.ItemRankId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemRankId { get; set; }

        /// <summary>
        /// 代表仕入先ID (MAIN_VENDOR_ID)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.仕入先コード[文字(8)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.MainVendorId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string MainVendorId { get; set; }

        /// <summary>
        /// サイズパターンID (ITEM_SIZE_PATTERN_ID)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.サイズパターンコード[文字(3)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.ItemSizePatternId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizePatternId { get; set; }

        /// <summary>
        /// ケース入数 (CASE_IRISU)
        /// </summary>
        /// <remarks>
        /// 実際未使用
        /// ケース閾値チェックでは、ケース閾値マスタと、荷姿別在庫の初期在庫数を使用する。
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.CaseIrisu), ResourceType = typeof(ItemSkuResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int CaseIrisu { get; set; }

        /// <summary>
        /// ボール入数 (BOWL_IRISU)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.BowlIrisu), ResourceType = typeof(ItemSkuResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? BowlIrisu { get; set; }

        /// <summary>
        /// サイズ展開ID (SIZE_EXPANSION_ID)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.SizeExpansionId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(4, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SizeExpansionId { get; set; }

        /// <summary>
        /// 想定売価 (NOMINAL_PRICE)
        /// </summary>
        /// <remarks>
        /// 未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.NominalPrice), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NominalPrice { get; set; }

        /// <summary>
        /// 事業部ID (DIVISION_ID)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.DivisionId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(4, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string DivisionId { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        /// <remarks>
        /// 新規受信時　品番＋カラーID＋サイズID　で設定する
        /// 以降SKU　明細項目
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.ItemSkuId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.カラーコード[文字(2)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.ItemColorId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.サイズコード[文字(2)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.ItemSizeId), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(5, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.JANコード[文字(13)]
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ItemSkuResource.Jan), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(13, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 単品容積 (PIECE_VOL)
        /// </summary>
        /// <remarks>
        /// 単品容積　単位はCBM(立法メートル）仕入梱包実績受信時、WMSで設定する。
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.PieceVol), ResourceType = typeof(ItemSkuResource))]
        [Range(-99999999999.99999999, 99999999999.99999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? PieceVol { get; set; }

        /// <summary>
        /// サイズ名 (ITEM_SIZE_NAME)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.サイズ名[文字(10)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.ItemSizeName), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSizeName { get; set; }

        /// <summary>
        /// サイズ表示順 (ITEM_SIZE_DISPLAY_ORDER)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.サイズ表示順[数字(2)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.ItemSizeDisplayOrder), ResourceType = typeof(ItemSkuResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSizeDisplayOrder { get; set; }

        /// <summary>
        /// ICタグ有無区分 (IC_TAG_CLASS)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.ICタグ有無区分[数字(1)]
        /// 0:なし、1:あり
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.IcTagClass), ResourceType = typeof(ItemSkuResource))]
        public bool? IcTagClass { get; set; }

        /// <summary>
        /// 略号コード (ABB_CODE)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.略号コード[文字(8)]
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.AbbCode), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string AbbCode { get; set; }

        /// <summary>
        /// 付属品名 (NOVELTY_NAME)
        /// </summary>
        /// <remarks>
        /// メンテナンス画面からメンテされる
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.NoveltyName), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NoveltyName { get; set; }

        /// <summary>
        /// アイテムコード (ITEM_CODE)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.アイテムコード[文字(3)]
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.ItemCode), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemCode { get; set; }

        /// <summary>
        /// 在庫管理区分 (STOCK_MNG_CLASS)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.在庫管理区分[文字(1)]
        /// 1:在庫管理対象、2:在庫管理対象外
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.StockMngClass), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StockMngClass { get; set; }

        /// <summary>
        /// MD区分 (MD_CLASS)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.MD区分[文字(2)]
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.MdClass), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string MdClass { get; set; }

        /// <summary>
        /// 国内海外区分 (INTERNAL_CLASS)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.国内海外区分[文字(1)]
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.InternalClass), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InternalClass { get; set; }

        /// <summary>
        /// 世代 (GENERATION_CLASS)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.世代[文字(1)]
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.GenerationClass), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GenerationClass { get; set; }

        /// <summary>
        /// 福袋予約品区分 (LUCKY_BAG_CLASS)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.福袋予約品区分[文字(1)]
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.LuckyBagClass), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LuckyBagClass { get; set; }

        /// <summary>
        /// 男女区分 (GENDER_CLASS)
        /// </summary>
        /// <remarks>
        /// IF商品マスタ.男女区分[文字(1)]
        /// ※WMS未使用
        /// </remarks>
        [Display(Name = nameof(ItemSkuResource.GenderClass), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(1, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GenderClass { get; set; }

        #endregion
    }
}
