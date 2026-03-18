namespace Wms.Areas.Ship.ViewModels.PrintInvoice
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Arrival.Resources;
    using Wms.Common;
    using Wms.Resources;

    /// <summary>
    /// 店舗出荷分残一覧
    /// </summary>
    public class ZanShipStore
    {
        public List<StoreInfo> ZanShipStores { get; set; }

        public class StoreInfo {
            /// <summary>
            /// 店舗ID
            /// </summary>
            public string ShipToStoreId { get; set; }

            /// <summary>
            /// 店舗名
            /// </summary>
            public string StoreName { get; set; }
        }
    }
}