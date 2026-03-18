namespace Wms.Areas.Ship.ViewModels.BtoBInstructDelete
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Ship.Resources;
    using Wms.Common;
    using Wms.ViewModels.Shared;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class BtoBInstructDeleteSearchConditions : IndicateViewModel
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum BtoBInstructDeleteSortKey : byte
        {
            [Display(Name = nameof(Resources.BtoBInstructDeleteResource.ShipPlanDateInstructId), ResourceType = typeof(Resources.BtoBInstructDeleteResource))]
            ShipPlanDateInstructId,

            [Display(Name = nameof(Resources.BtoBInstructDeleteResource.InstructId), ResourceType = typeof(Resources.BtoBInstructDeleteResource))]
            InstructId
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
        /// 引当名称
        /// </summary>
        public string AllocationName { get; set; }

        /// <summary>
        /// 事業部
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// 受信日From
        /// </summary>
        public DateTime? AllocDateFrom { get; set; }

        /// <summary>
        /// 受信日To
        /// </summary>
        public DateTime? AllocDateTo { get; set; }

        /// <summary>
        /// 受信時From
        /// </summary>
        public string AllocTimeFrom { get; set; }

        /// <summary>
        /// 受信時To
        /// </summary>
        public string AllocTimeTo { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        public string BrandName { get; set; }

        /// <summary>
        /// 出荷予定日From
        /// </summary>
        public DateTime? ShipPlanDateFrom { get; set; }

        /// <summary>
        /// 出荷予定日To
        /// </summary>
        public DateTime? ShipPlanDateTo { get; set; }

        /// <summary>
        /// 代表仕入先
        /// </summary>
        public string MainVendorId { get; set; }

        /// <summary>
        /// 代表仕入先
        /// </summary>
        public string MainVendorName { get; set; }

        /// <summary>
        /// 納品日
        /// </summary>
        public DateTime? DeliveryDate { get; set; }

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
        /// アイテムコード
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// 出荷先区分
        /// </summary>
        public string ShipToClass { get; set; }

        /// <summary>
        /// JAN (JAN)
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToId { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public string ShipToName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterName { get; set; }

        /// <summary>
        /// 緊急
        /// </summary>
        public string EmergencyClass { get; set; }

        /// <summary>
        /// 配送エリア
        /// </summary>
        public string TransporterArea { get; set; }

        /// <summary>
        /// 配送エリア名
        /// </summary>
        public string TransporterAreaName { get; set; }

        /// <summary>
        /// 出荷指示ID
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 指示区分
        /// </summary>
        public InstructClasses InstructClass { get; set; } = InstructClasses.FirstDb;

        /// <summary>
        /// Sort key
        /// </summary>
        public BtoBInstructDeleteSortKey SortKey { get; set; } = BtoBInstructDeleteSortKey.ShipPlanDateInstructId;

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
        /// 出荷指示ID数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? InstructIdSum { get; set; }

        /// <summary>
        /// SKU数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuSum { get; set; }

        /// <summary>
        /// 明細数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? DetailSum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? PlanQtySum { get; set; }

        /// <summary>
        /// 選択中数
        /// </summary>
        public int? SelectedCnt { get; set; } = 0;

        /// <summary>
        /// Result Type
        /// </summary>
        public ResultTypes ResultType { get; set; } = ResultTypes.Sku;

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 明細表示出荷指示ID
        /// </summary>
        public string TargetShipInstructId { get; set; }

        public IList<SelectedBtoBInstructDeleteViewModel> BtoBInstructDeletes { get; set; }
    }

    /// <summary>
    /// 明細種別 0：在庫明細 1：ケース明細
    /// </summary>
    public enum ResultTypes : byte
    {
        Sku = 0,

        Detail = 1,
    }

    /// <summary>
    /// 検索種別 0：検索 1：並べ替え/改ページ
    /// </summary>
    public enum SearchTypes : byte
    {
        Search = 0,

        SortPage = 1,
    }
}