namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using PagedList;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EcunitReferenceHead
    {
        /// <summary>
        /// バッチNo
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 総オーダー数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipInstructQty { get; set; }

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

    public class DetailRow
    {
        /// <summary>
        /// EC出荷形態
        /// </summary>
        public string EcShipClassName { get; set; }

        /// <summary>
        /// 指示ｵｰﾀﾞｰ数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ShipInstructQty { get; set; }

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

    public class OrderResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<DetailRow> OrderResults { get; set; }
    }

    public class GasResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<DetailRow> GasResults { get; set; }
    }

    public class EcunitReferenceViewModel
    {
        public EcunitReferenceHead Head { get; set; }

        public DetailRow SinglePickResults { get; set; }

        public OrderResult OrderResults { get; set; }

        public GasResult GasResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceViewModel"/> class.
        /// </summary>
        public EcunitReferenceViewModel()
        {
            this.Head = new EcunitReferenceHead();
            this.SinglePickResults = new DetailRow();
            this.OrderResults = new OrderResult();
            this.GasResults = new GasResult();
        }
    }
}