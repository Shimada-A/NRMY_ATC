namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    using PagedList;
    using System;
    using Wms.Common;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Wms.Areas.Ship.ViewModels.TransferReference.TransferReferenceSearchConditions;

    public class StoreSortOrderPicDetailHead
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

    public class StoreSortOrderPicDetailRow
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

        /// <summary>
        /// 欠品登録数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutRegQty { get; set; }


        /// <summary>
        /// 欠品確定数
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? StockOutFixQty { get; set; }
    }

    public class StoreSortResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<StoreSortOrderPicDetailRow> StoreSortResults { get; set; }
    }

    public class OrderPicResult
    {
        /// <summary>
        /// List record
        /// </summary>
        public IList<StoreSortOrderPicDetailRow> OrderPicResults { get; set; }
    }

    public class StoreSortOrderPicDetailViewModel
    {
        public StoreSortOrderPicDetailHead Head { get; set; }

        public StoreSortResult StoreSortResults { get; set; }

        public OrderPicResult OrderPicResults { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReferenceViewModel"/> class.
        /// </summary>
        public StoreSortOrderPicDetailViewModel()
        {
            this.Head = new StoreSortOrderPicDetailHead();
            this.StoreSortResults = new StoreSortResult();
            this.OrderPicResults = new OrderPicResult();
        }
    }
}