namespace Wms.Areas.Ship.ViewModels.EcAllocation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using Wms.ViewModels.Shared;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class EcAllocationSearchConditions : IndicateViewModel
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum EcAllocationSortKey : byte
        {
            //出荷予定日→注文日→注文番号
            [Display(Name = nameof(Resources.EcAllocationResource.ShipPlanDateOrderDateOrderNo), ResourceType = typeof(Resources.EcAllocationResource))]
            ShipPlanDateOrderDateOrderNo,

            [Display(Name = nameof(Resources.EcAllocationResource.OrderNo), ResourceType = typeof(Resources.EcAllocationResource))]
            OrderNo,

            [Display(Name = nameof(Resources.EcAllocationResource.OrderQtySort), ResourceType = typeof(Resources.EcAllocationResource))]
            OrderQty
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
        /// 引当状況
        /// </summary>
        public string AllocationState { get; set; }

        /// <summary>
        /// 引当名称
        /// </summary>
        public string AllocationName { get; set; }

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
        /// 当日以前出荷日分
        /// </summary>
        public bool ShipDateBeforeToday { get; set; } = true;

        /// <summary>
        /// 翌日以降出荷日分
        /// </summary>
        public bool ShipDateAfterToday { get; set; }

        /// <summary>
        /// 出荷予定日From
        /// </summary>
        public DateTime? ShipPlanDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 出荷予定日To
        /// </summary>
        public DateTime? ShipPlanDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// シングル
        /// </summary>
        public bool Single { get; set; } = true;

        /// <summary>
        /// オーダー
        /// </summary>
        public bool Order { get; set; } = true;

        /// <summary>
        /// GAS
        /// </summary>
        public bool Gas { get; set; } = true;

        /// <summary>
        /// １オーダー１バッチNoとする
        /// </summary>
        public bool OrderBatchNo { get; set; }

        /// <summary>
        /// GAS１ユニットで１バッチNoとする
        /// </summary>
        public bool BatchNoInUnit { get; set; }

        /// <summary>
        /// オーダーにする
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        /// １オーダー１バッチNoとする
        /// </summary>
        public bool AllOrderBatchNo { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        public TransporterDateClasses TransporterDateClass { get; set; } = TransporterDateClasses.All;

        /// <summary>
        /// 配送指定日
        /// </summary>
        public DateTime? TransporterDate { get; set; }

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
        /// 品番
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// JAN
        /// </summary>
        public string Jan { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string ItemSkuId { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public EcAllocationSortKey SortKey { get; set; } = EcAllocationSortKey.ShipPlanDateOrderDateOrderNo;

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
        /// オーダー数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderSum { get; set; }

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
        /// 選択中数
        /// </summary>
        public int SelectedCnt { get; set; } = 0;

        /// <summary>
        /// 選択中引当エラー数
        /// </summary>
        public int SelectedErrCnt { get; set; } = 0;

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// add conditions
        /// </summary>
        public bool AddConditions { get; set; } = false;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }
        public IList<SelectedEcAllocationViewModel> EcAllocations { get; set; }

        /// <summary>
        /// add conditions
        /// </summary>
        public string Ret { get; set; }

        public string Print { get; set; }
    }
}