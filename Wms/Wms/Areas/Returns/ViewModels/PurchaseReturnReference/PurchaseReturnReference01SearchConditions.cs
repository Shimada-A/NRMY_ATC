namespace Wms.Areas.Returns.ViewModels.PurchaseReturnReference
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class PurchaseReturnReference01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum PurchaseReturnReference01SortKey : byte
        {
            [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.ReturnId), ResourceType = typeof(Resources.PurchaseReturnReferenceResource))]
            ReturnId,

            [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.VendorIdReturnId), ResourceType = typeof(Resources.PurchaseReturnReferenceResource))]
            VendorIdReturnId
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
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// 訂正区分 (RETUEN_CLASS)
        /// </summary>
        public enum ReturnClasses
        {
            [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.Return), ResourceType = typeof(Resources.PurchaseReturnReferenceResource))]
            Return = 0,

            [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.Correction), ResourceType = typeof(Resources.PurchaseReturnReferenceResource))]
            Correction = 1,

            [Display(Name = nameof(Resources.PurchaseReturnReferenceResource.NotSelect), ResourceType = typeof(Resources.PurchaseReturnReferenceResource))]
            NotSelect = 2
        }
        /// <remarks>
        /// 0:仕入先返品, 1:仕入訂正　
        /// </remarks>
        public ReturnClasses RetuenClass { get; set; } = ReturnClasses.NotSelect;

        /// <summary>
        /// /// 実績登録（＝確定）日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArriveDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// /// 実績登録（＝確定）日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArriveDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionCd { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

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
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }



        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        public string ItemSizeId { get; set; }

 
        /// <summary>
        /// 入力担当者ID (INPUT_USER_ID)
        /// </summary>
        public string InputUserId { get; set; }

        /// <summary>
        /// 入力担当者名 (INPUT_USER_NAME)
        /// </summary>
        public string InputUserName { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public PurchaseReturnReference01SortKey SortKey { get; set; } = PurchaseReturnReference01SortKey.ReturnId;

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
        /// 返品伝票ID (RETURN_ID)
        /// </summary>
        /// <remarks>
        /// システム内で仕入返品伝票ID採番(上位への連携時はセンターがキーにないので、センターコードがはいっているNOとする）
        /// </remarks>
        public string ReturnId { get; set; }

        public string HidReturnId { get; set; }

        public ReturnClasses HidReturnClass { get; set; }

        /// <summary>
        /// 返品伝票明細ID (RETURN_SEQ)
        /// </summary>
        public int ReturnSeq { get; set; }

        /// <summary>
        /// 返品実績数 (RETURN_QTY)
        /// </summary>
        public int ReturnQty { get; set; }

        /// <summary>
        /// 標準下代 (NORMAL_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// 下代　(商品原価)
        /// 返品：ユーザー画面入力値
        /// 訂正：T_ARRIVE_PLANSの該当納品書番号、SKUの標準下代
        /// </remarks>
        public decimal NormalBuyingPrice { get; set; }

        /// <summary>
        /// 仕入下代 (PURCHASE_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// W下代　(商品原価＋輸入コスト)
        /// 返品：下代の値をセット
        /// 訂正：T_ARRIVE_PLANSの該当納品書番号、SKUの仕入下代
        /// </remarks>
        public decimal PurchaseBuyingPrice { get; set; }

        /// <summary>
        /// 標準上代(税抜) (NORMAL_SELLING_PRICE_EX_TAX)
        /// </summary>
        /// <remarks>
        /// 上代
        /// 返品：0固定
        /// 訂正：T_ARRIVE_PLANSの該当納品書番号、SKUの標準上代(税抜)
        /// </remarks>
        public int NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 梱包番号 (BOX_NO)
        /// </summary>
        /// <remarks>
        /// ケースNo
        /// </remarks>
        public string BoxNo { get; set; }





    }
}