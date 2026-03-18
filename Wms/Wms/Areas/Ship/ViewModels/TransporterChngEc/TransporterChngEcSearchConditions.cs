namespace Wms.Areas.Ship.ViewModels.TransporterChngEc
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
    public class TransporterChngEcSearchConditions
    {
        /// <summary>
        /// 引当状況
        /// </summary>
        public enum AllocStatusEnum
        {
            [Display(Name = nameof(TransporterChngEcResource.UnAlloc), ResourceType = typeof(TransporterChngEcResource))]
            UnAlloc = 1,

            [Display(Name = nameof(TransporterChngEcResource.Alloced), ResourceType = typeof(TransporterChngEcResource))]
            Alloced = 2,

            [Display(Name = nameof(TransporterChngEcResource.Printed), ResourceType = typeof(TransporterChngEcResource))]
            Printed = 3
        }

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// 引当状況
        /// </summary>
        public AllocStatusEnum AllocStatus { get; set; } = AllocStatusEnum.UnAlloc;

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
        /// 出荷予定日From
        /// </summary>
        public DateTime? ShipPlanDateFrom { get; set; } = DateTime.Now;

        /// <summary>
        /// 出荷予定日To
        /// </summary>
        public DateTime? ShipPlanDateTo { get; set; } = DateTime.Now;

        /// <summary>
        /// 配送指定日
        /// </summary>
        public DateTime? ArriveRequestDate { get; set; }

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
        /// ケースNo
        /// </summary>
        public string BoxNo { get; set; }


        /// <summary>
        /// 変更前配送業者
        /// </summary>
        public string SearchTransporterId { get; set; }

        /// <summary>
        /// 変更後配送業者印刷用
        /// </summary>
        public string ChangedTransporterIdPrint { get; set; }

        /// <summary>
        /// 変更後配送業者
        /// </summary>
        public string ChangedTransporterId { get; set; }

        /// <summary>
        /// サイズ
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 対象ケースNo
        /// </summary>
        public string CaseNo { get; set; }

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

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
        public IList<SelectedTransporterChngEcViewModel> TransporterChngEcs { get; set; }
    }
}