namespace Wms.Areas.Ship.ViewModels.BtoBInstructionInput
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class BtoBInstructionInput01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum BtoBInstructionInput01SortKey : byte
        {
            [Display(Name = nameof(Resources.BtoBInstructionInputResource.ShipPlanDateInstructIdSku), ResourceType = typeof(Resources.BtoBInstructionInputResource))]
            ShipPlanDateInstructIdSku,

            [Display(Name = nameof(Resources.BtoBInstructionInputResource.SkuInstructId), ResourceType = typeof(Resources.BtoBInstructionInputResource))]
            SkuInstructId
        }

        /// <summary>
        /// Detail Data to sort
        /// </summary>
        public enum BtoBInstructionInputDetailSortKey : byte
        {
            [Display(Name = nameof(Resources.BtoBInstructionInputResource.ShipPlanDateInstructIdSeq), ResourceType = typeof(Resources.BtoBInstructionInputResource))]
            ShipPlanDateInstructIdSeq,

            [Display(Name = nameof(Resources.BtoBInstructionInputResource.ShipToStoreInstructIdSeq), ResourceType = typeof(Resources.BtoBInstructionInputResource))]
            ShipToStoreInstructIdSeq,

            [Display(Name = nameof(Resources.BtoBInstructionInputResource.SkuInstructIdSeq), ResourceType = typeof(Resources.BtoBInstructionInputResource))]
            SkuInstructIdSeq
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
        /// 受信日From
        /// </summary>
        public DateTime? MakeDateFrom { get; set; }

        /// <summary>
        /// 受信日To
        /// </summary>
        public DateTime? MakeDateTo { get; set; }

        /// <summary>
        /// 受信時From
        /// </summary>
        public string MakeTimeFrom { get; set; }

        /// <summary>
        /// 受信時To
        /// </summary>
        public string MakeTimeTo { get; set; }

        /// <summary>
        /// 出荷予定日From
        /// </summary>
        public DateTime? ShipPlanDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 出荷予定日To
        /// </summary>
        public DateTime? ShipPlanDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 出荷完了日From
        /// </summary>
        public DateTime? CompleteDateFrom { get; set; }

        /// <summary>
        /// 出荷完了日To
        /// </summary>
        public DateTime? CompleteDateTo { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public string ShipAllocStatus { get; set; }

        /// <summary>
        /// 状態(過去分を含む)
        /// </summary>
        public bool ShipAllocStatusOld { get; set; }

        /// <summary>
        /// 指示区分
        /// </summary>
        public string InstructClass { get; set; }

        /// <summary>
        /// 出荷先区分
        /// </summary>
        public string StoreClass { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        public string EmergencyClass { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 欠品登録あり
        /// </summary>
        public bool StockOutRegFlag { get; set; }

        /// <summary>
        /// 欠品確定あり
        /// </summary>
        public bool StockOutFixFlag { get; set; }

        /// <summary>
        /// 実績数不足あり
        /// </summary>
        public bool LackOfResultFlag { get; set; }

        /// <summary>
        /// キャンセル
        /// </summary>
        public bool CancelFlag { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 代表仕入先
        /// </summary>
        public string MainVendorId { get; set; }

        /// <summary>
        /// 代表仕入先
        /// </summary>
        public string MainVendorName { get; set; }

        /// <summary>
        /// 分類1 (CATEGORY_ID1)
        /// </summary>
        public string CategoryId1 { get; set; }

        /// <summary>
        /// 分類2 (CATEGORY_ID2)
        /// </summary>
        public string CategoryId2 { get; set; }

        /// <summary>
        /// 分類3 (CATEGORY_ID3)
        /// </summary>
        public string CategoryId3 { get; set; }

        /// <summary>
        /// 分類4 (CATEGORY_ID4)
        /// </summary>
        public string CategoryId4 { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public BtoBInstructionInput01SortKey SortKey { get; set; } = BtoBInstructionInput01SortKey.ShipPlanDateInstructIdSku;

        /// <summary>
        /// Detail Sort key
        /// </summary>
        public BtoBInstructionInputDetailSortKey DetailSortKey { get; set; } = BtoBInstructionInputDetailSortKey.ShipPlanDateInstructIdSeq;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// SKU数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructQtySum { get; set; }

        /// <summary>
        /// 出荷可能数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQtySum { get; set; }

        /// <summary>
        /// 選択中数
        /// </summary>
        public int? SelectedCnt { get; set; } = 0;

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Result Type
        /// </summary>
        public ResultTypes ResultType { get; set; } = ResultTypes.Sku;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }
        public long OldSeq { get; set; }

        /// <summary>
        /// 出荷区分
        /// </summary>
        public ShipKinds ShipKind { get; set; }
    }

    public enum ResultTypes : byte
    {
        Sku = 0,

        Detail = 1,
    }

}