using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wms.Areas.Ship.ViewModels.EcAllocation
{
    public class EcAllocationDetailHead
    {
        /// <summary>
        /// 出荷予定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ShipRequestDate { get; set; }

        /// <summary>
        /// データ受信日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? DataDate { get; set; }

        /// <summary>
        /// 注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 注文日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 配送先都道府県
        /// </summary>
        /// <remarks>
        public string DestPrefName { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        /// <remarks>
        public string TransporterName { get; set; }

        /// <summary>
        /// 仕分番号
        /// </summary>
        /// <remarks>
        public string DeliShiwakeCd { get; set; }

        /// <summary>
        /// EC出荷形態
        /// </summary>
        /// <remarks>
        public string EcShipClassName { get; set; }

        /// <summary>
        /// 配送指定日
        /// </summary>
        /// <remarks>
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ArriveRequestDate { get; set; }

        /// <summary>
        /// 配送時間帯
        /// </summary>
        /// <remarks>]
        public string ArriveRequestTime { get; set; }

        /// <summary>
        /// SKU数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ItemSkuQtySum { get; set; }

        /// <summary>
        /// 予定数合計
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int OrderQtySum { get; set; }
    }

    public class EcAllocationDetailResultRow
    {
        public long ShipInstructSeq { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemColorId { get; set; }

        public string ItemColorName { get; set; }

        public string ItemSizeId { get; set; }

        public string ItemSizeName { get; set; }

        public string Jan { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? OrderQty { get; set; }
    }

    public class EcAllocationDetailViewModel
    {
        public EcAllocationDetailHead DetailHead { get; set; }

        /// <summary>
        /// List record
        /// </summary>
        public IEnumerable<EcAllocationDetailResultRow> EcAllocationDetails { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EcAllocationDetailViewModel"/> class.
        /// </summary>
        public EcAllocationDetailViewModel()
        {
            this.DetailHead = new EcAllocationDetailHead();
        }
    }
}