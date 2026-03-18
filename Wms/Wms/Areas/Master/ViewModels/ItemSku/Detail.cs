namespace Wms.Areas.Master.ViewModels.ItemSku
{
    using Share.Common.Resources;
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 品番SKU
    /// </summary>
    public class Detail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 品名略称 (ITEM_SHORT_NAME)
        /// </summary>
        public string ItemShortName { get; set; }

        /// <summary>
        /// ブランドID (BRAND_ID)
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド名 (BRAND_NAME)
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類名1 (CATEGORY_NAME1)
        /// </summary>
        public string CategoryName1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類名2 (CATEGORY_NAME2)
        /// </summary>
        public string CategoryName2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類名3 (CATEGORY_NAME3)
        /// </summary>
        public string CategoryName3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 標準上代(税込) (NORMAL_SELLING_PRICE)
        /// </summary>
        /// <remarks>
        /// 売価
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int NormalSellingPrice { get; set; }

        /// <summary>
        /// 分類名4 (CATEGORY_NAME4)
        /// </summary>
        public string CategoryName4 { get; set; }

        /// <summary>
        /// 標準上代(税抜) (NORMAL_SELLING_PRICE_EX_TAX)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 標準下代 (NORMAL_BUYING_PRICE)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal NormalBuyingPrice { get; set; }

        /// <summary>
        /// 仕入下代 (PURCHASE_BUYING_PRICE)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal PurchaseBuyingPrice { get; set; }

        /// <summary>
        /// 元上代(税抜) (PROPER_PRICE_EX_TAX)
        /// </summary>
        /// <remarks>
        /// ※品番マスタ新規作成時の標準上代(税抜)
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ProperPriceExTax { get; set; }

        /// <summary>
        /// シーズン年 (SEASON_YEAR)
        /// </summary>
        public int SeasonYear { get; set; }

        /// <summary>
        /// シーズンID (ITEM_SEASON_ID)
        /// </summary>
        public ItemSeasons ItemSeasonId { get; set; }

        /// <summary>
        /// シーズン名 (ITEM_SEASON_NAME)
        /// </summary>
        public string ItemSeasonName { get; set; }

        /// <summary>
        /// 販売開始日 (SALE_START_DATE)
        /// </summary>
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SaleStartDate { get; set; }

        /// <summary>
        /// 販売終了日 (SALE_END_DATE)
        /// </summary>
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? SaleEndDate { get; set; }

        /// <summary>
        /// 回収日 (WITHDRAWAL_DATE)
        /// </summary>
        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? WithdrawalDate { get; set; }

        /// <summary>
        /// 原産国 (ORIGIN_COUNTRY)
        /// </summary>
        public string OriginCountry { get; set; }

        /// <summary>
        /// 顧客タイプID (ITEM_CUSTOMER_TYPE_ID)
        /// </summary>
        public string ItemCustomerTypeId { get; set; }

        /// <summary>
        /// 顧客タイプ
        /// </summary>
        public string ItemCustomerTypeName { get; set; }

        /// <summary>
        /// 商品ランクID (ITEM_RANK_ID)
        /// </summary>
        public string ItemRankId { get; set; }

        /// <summary>
        /// 商品ランク
        /// </summary>
        public string ItemRankName { get; set; }

        /// <summary>
        /// 代表仕入先所在ID (MAIN_VENDOR_ID)
        /// </summary>
        public string MainVendorId { get; set; }

        /// <summary>
        /// 代表仕入先所在名 (MAIN_VENDOR_NAME)
        /// </summary>
        public string MainVendorName { get; set; }

        /// <summary>
        /// ケース入数 (CASE_IRISU)
        /// </summary>
        public int CaseIrisu { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名 (ITEM_COLOR_NAME)
        /// </summary>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名 (ITEM_SIZE_NAME)
        /// </summary>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 単品容積 (PIECE_VOL)
        /// </summary>
        /// <remarks>
        /// 単品容積　単位はCBM(立法メートル）
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? PieceVol { get; set; }

        /// <summary>
        /// 事業部CD (DIVISION_ID)
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// 事業部 (DIVISION_NAME)
        /// </summary>
        public string DivisionName { get; set; }

        /// <summary>
        /// 削除フラグ 0:未削除 1:削除済み
        /// </summary>
        [Display(Name = nameof(Resources.StoreResource.DeleteFlag), ResourceType = typeof(Resources.StoreResource))]
        public bool DeleteFlag { get; set; }


        /// <summary>
        /// 付属品名 (NOVELTY_NAME)
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.NoveltyName), ResourceType = typeof(ItemSkuResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NoveltyName { get; set; }

        /// <summary>
        /// アイテムコード
        /// </summary>
        [Display(Name = nameof(ItemSkuResource.ItemCode),ResourceType = typeof(ItemSkuResource))]
        public string ItemCode { get; set; }

        /// <summary>
        /// アイテムコード名
        /// </summary>
        public string ItemCodeName { get; set; }

        #endregion プロパティ
    }
}