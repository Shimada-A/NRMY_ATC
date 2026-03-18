namespace Wms.Areas.Inventory.ViewModels.Input
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Inventory.Resources;
    using Wms.Common;

    /// <summary>
    /// 棚卸実績入力
    /// </summary>
    public class InputResultRow
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 倉庫ID
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// 棚卸No
        /// </summary>
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸行No (INVENTORY_SEQ)
        /// </summary>
        /// <remarks>
        /// 棚卸番号内の識別、実績と結合するためのシーケンス　ソートでは使用しない
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.InventorySeq), ResourceType = typeof(InvInput01Resource))]
        public int InventorySeq { get; set; }

        /// <summary>
        /// 棚卸開始日時 (INVENTORY_START_DATE)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.InventoryStartDate), ResourceType = typeof(InvInput01Resource))]
        public DateTime? InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸区分 (INVENTORY_CLASS)
        /// </summary>
        /// <remarks>
        /// 1：全件棚卸、2：循環棚卸
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.InventoryClass), ResourceType = typeof(InvInput01Resource))]
        public int InventoryClass { get; set; }

        /// <summary>
        /// 棚卸名称 (INVENTORY_NAME)
        /// </summary>
        /// <remarks>
        /// 取込ファイル名を登録する
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.InventoryName), ResourceType = typeof(InvInput01Resource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.ItemSkuId), ResourceType = typeof(InvInput01Resource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// ロケーション
        /// </summary>
        public string LocationCd { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:ケース、2:バラ、9:指定なし　ロケMよりセット
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.CaseClass), ResourceType = typeof(InvInput01Resource))]
        public int CaseClass { get; set; }

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.GradeId), ResourceType = typeof(InvInput01Resource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 荷姿
        /// </summary>
        public string CaseClassName { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        /// <remarks>
        public string ItemId { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        /// <remarks>
        public string ItemName { get; set; }

        /// <summary>
        /// カラー
        /// </summary>
        /// <remarks>
        public string ItemColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        /// <remarks>
        public string ItemColorName { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        /// <remarks>
        public string ItemSizeId { get; set; }

        /// <summary>
        /// サイズ名
        /// </summary>
        /// <remarks>
        public string ItemSizeName { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        /// <remarks>
        public string Jan { get; set; }

        /// <summary>
        /// 帳簿在庫数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputResource.StockQtyStart), ResourceType = typeof(InputResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockQtyStart { get; set; }

        /// <summary>
        /// 在庫有無フラグ (STOCK_FLAG)
        /// </summary>
        /// <remarks>
        /// 0：在庫無、1：在庫有
        /// 棚卸開始時に在庫がある場合は「1：在庫有」
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.StockFlag), ResourceType = typeof(InvInput01Resource))]
        public bool? StockFlag { get; set; }

        /// <summary>
        /// 外装棚卸許可フラグ (SIMPLE_INVENTORY_FLAG)
        /// </summary>
        /// <remarks>
        /// 外装での棚卸を許可するか判断
        /// 0：許可しない、1：許可する
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.SimpleInventoryFlag), ResourceType = typeof(InvInput01Resource))]
        public bool? SimpleInventoryFlag { get; set; }

        /// <summary>
        /// 修正前実績数 (RESULT_QTY_BEFORE)
        /// </summary>
        [Display(Name = nameof(InvInput01Resource.ResultQtyBefore), ResourceType = typeof(InvInput01Resource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtyBefore { get; set; }

        /// <summary>
        /// 実棚数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputResource.ResultQty), ResourceType = typeof(InputResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        [RegularExpression(
          "^[0-9]+$",
          ErrorMessage = "0以上の整数を入力してください")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// 差異数
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(InputResource.DifferenceQty), ResourceType = typeof(InputResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// 新規追加フラグ (ADD_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:既存、1:新規追加
        /// </remarks>
        [Display(Name = nameof(InvInput01Resource.AddFlag), ResourceType = typeof(InvInput01Resource))]
        public bool AddFlag { get; set; }

        /// <summary>
        /// 実棚数Hid
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQtyHid { get; set; }

        public string ChangeModel { get; set; }

        #endregion プロパティ
    }
}