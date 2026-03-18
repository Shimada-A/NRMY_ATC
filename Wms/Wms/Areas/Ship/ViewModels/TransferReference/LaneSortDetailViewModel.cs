namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using PagedList;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Common;
    using static Wms.Areas.Ship.ViewModels.TransferReference.TransferReferenceSearchConditions;

    public class LaneSortDetailHead
    {
        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// バッチ名称
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// 出荷区分
        /// </summary>
        public ShipKinds ShipKind { get; set; }

        /// <summary>
        /// 出荷予定日
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 総出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipToStoreQty { get; set; }

        /// <summary>
        /// 総SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 総指示行数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipInstructSeqQty { get; set; }

        /// <summary>
        /// 総指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }
    }

    public class LaneSortDetailRow
    {
        /// <summary>
        /// ブランドID
        /// </summary>
        public string BrandId { get; set; }
        
        /// <summary>
        /// ブランド略称
        /// </summary>
        public string BrandShortName { get; set; }

        /// <summary>
        /// レーンNO
        /// </summary>
        public string LaneNo { get; set; }

        /// <summary>
        /// 出荷先数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipToStoreQty { get; set; }

        /// <summary>
        /// SKU数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ItemSkuQty { get; set; }

        /// <summary>
        /// 指示数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? AllocQty { get; set; }

        /// <summary>
        /// 完了数量
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? SortQty { get; set; }

        /// <summary>
        /// 進捗率
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? Percent { get; set; }
    }

    public class LaneResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<LaneSortDetailRow> LaneResults { get; set; }
    }

    public class LaneSortDetailViewModel
    {
        public LaneSortDetailHead Head { get; set; }

        public LaneResult LaneResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceViewModel"/> class.
        /// </summary>
        public LaneSortDetailViewModel()
        {
            this.Head = new LaneSortDetailHead();
            this.LaneResults = new LaneResult();
        }
    }
}