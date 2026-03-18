namespace Wms.Areas.Ship.ViewModels.EcConfirmProgress
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
    public class EcConfirmProgress01SearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum EcConfirmProgressSortKey : byte
        {
            [Display(Name = nameof(EcConfirmProgressResource.ShipPlanDateShipInstructId), ResourceType = typeof(EcConfirmProgressResource))]
            ShipPlanDateShipInstructId,

            [Display(Name = nameof(EcConfirmProgressResource.ShipInstructIdShipPlanDate), ResourceType = typeof(EcConfirmProgressResource))]
            ShipInstructIdShipPlanDate
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(FormsResource.ASC), ResourceType = typeof(FormsResource))]
            Asc,

            [Display(Name = nameof(FormsResource.DESC), ResourceType = typeof(FormsResource))]
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
        /// 出荷確定日From
        /// </summary>
        public DateTime? KakuDateFrom { get; set; }

        /// <summary>
        /// 出荷確定日To
        /// </summary>
        public DateTime? KakuDateTo { get; set; }

        /// <summary>
        /// 状態
        /// </summary>
        public string ShipStatus { get; set; }

        /// <summary>
        /// 状態(過去分を含む)
        /// </summary>
        public bool ShipStatusOld { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送指定日From
        /// </summary>
        public DateTime? ArriveRequestDateFrom { get; set; }

        /// <summary>
        /// 配送指定日To
        /// </summary>
        public DateTime? ArriveRequestDateTo { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// GASバッチNo
        /// </summary>
        public string GASBatchNo { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        public string EcShipClass { get; set; }

        /// <summary>
        /// キャンセル
        /// </summary>
        public bool CancelFlag { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

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
        /// EC出荷形態：シングル
        /// </summary>
        public bool SingleFlag { get; set; }

        /// <summary>
        /// EC出荷形態：オーダー
        /// </summary>
        public bool OrderFlag { get; set; }

        /// <summary>
        /// EC出荷形態：GAS
        /// </summary>
        public bool GasFlag { get; set; }

        /// <summary>
        /// 送り状番号
        /// </summary>
        public string DeliNo { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// Sort key
        /// </summary>
        public EcConfirmProgressSortKey SortKey { get; set; } = EcConfirmProgressSortKey.ShipPlanDateShipInstructId;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Move
        /// </summary>
        public int Move { get; set; } = 0;

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
        public int? ShipInstructSum { get; set; }

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
        public int? SelectedCnt { get; set; } = 0;

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }
        public IList<SelectedEcConfirmProgress01ViewModel> EcConfirmProgress01s { get; set; }
    }

}