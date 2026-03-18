namespace Wms.Areas.Returns.ViewModels.PurchaseReturnReference
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Returns.Resources;

    /// <summary>
    /// 仕入入荷進捗照会(明細別)
    /// </summary>
    public class PurchaseReturnReference02ResultRow
    {
        #region プロパティ

        public string ShipperId { get; set; }

        public string CenterId { get; set; }


        /// <summary>
        /// 返品伝票ID (RETURN_ID)
        /// </summary>
        /// <remarks>
        /// システム内で仕入返品伝票ID採番(上位への連携時はセンターがキーにないので、センターコードがはいっているNOとする）
        /// </remarks>
        public string ReturnId { get; set; }

        /// <summary>
        /// 返品伝票明細ID (RETURN_SEQ)
        /// </summary>
        public int ReturnSeq { get; set; }

        /// <remarks>
        /// 0:仕入先返品, 1:仕入訂正　
        /// </remarks>
        public byte RetuenClass { get; set; }

        public string ReturnClassName { get; set; }

        /// <summary>
        /// 仕入先ID (VENDOR_ID)
        /// </summary>
        public string VendorId { get; set; }

        /// <summary>
        /// 納品書番号 (INVOICE_NO)
        /// </summary>
        /// <remarks>
        /// 返品：セットしない
        /// 訂正：画面入力値
        /// </remarks>
        public string InvoiceNo { get; set; }

        /// <summary>
        /// 返品日 (ARRIVE_DATE)
        /// </summary>
        /// <remarks>
        /// 実績登録（＝確定）日
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 商品ID(品番) (ITEM_ID)
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// カラーID (ITEM_COLOR_ID)
        /// </summary>
        public string ItemColorId { get; set; }

        /// <summary>
        /// サイズID (ITEM_SIZE_ID)
        /// </summary>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// 返品実績数 (RETURN_QTY)
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ReturnQty { get; set; }

        /// <summary>
        /// 標準下代 (NORMAL_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// 下代　(商品原価)
        /// 返品：ユーザー画面入力値
        /// 訂正：T_ARRIVE_PLANSの該当納品書番号、SKUの標準下代
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? NormalBuyingPrice { get; set; }

        /// <summary>
        /// 仕入下代 (PURCHASE_BUYING_PRICE)
        /// </summary>
        /// <remarks>
        /// W下代　(商品原価＋輸入コスト)
        /// 返品：下代の値をセット
        /// 訂正：T_ARRIVE_PLANSの該当納品書番号、SKUの仕入下代
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? PurchaseBuyingPrice { get; set; }

        /// <summary>
        /// 標準上代(税抜) (NORMAL_SELLING_PRICE_EX_TAX)
        /// </summary>
        /// <remarks>
        /// 上代
        /// 返品：0固定
        /// 訂正：T_ARRIVE_PLANSの該当納品書番号、SKUの標準上代(税抜)
        /// </remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? NormalSellingPriceExTax { get; set; }

        /// <summary>
        /// 入力担当者ID (INPUT_USER_ID)
        /// </summary>
        public string InputUserId { get; set; }

        /// <summary>
        /// 入力担当者名 (INPUT_USER_NAME)
        /// </summary>
        public string InputUserName { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }
        
        /// <summary>
        /// Page
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        public long LineNo { get; set; }

        public string ItemName { get; set; }

        public string ItemColorName { get; set; }

        public string ItemSizeName { get; set; }

        public string VendorName { get; set; }

        public string CategoryName { get; set; }

        public int TotalItemCount { get; set; }

        #endregion プロパティ
    }
}