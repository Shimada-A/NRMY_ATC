using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wms.Areas.Ship.ViewModels.TransferReference
{
    public class PickInfoResult
    {
        private string locationCd;
        private string batchNo;
        private string jan;
        private string itemNm;
        private string picQty;

        /// <summary>
        /// ロケ
        /// </summary>
        [Display(Name = nameof(Resources.TransferReferenceResource.LocationCd), ResourceType = typeof(Resources.TransferReferenceResource), Order = 1)]
        public string LocationCd { get { return $"\"{locationCd}\""; } set { locationCd = value; } }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(Resources.TransferReferenceResource.BatchNo), ResourceType = typeof(Resources.TransferReferenceResource), Order = 2)]
        public string BatchNo { get { return $"\"{batchNo}\""; } set { batchNo = value; } }

        /// <summary>
        /// JAN
        /// </summary>
        [Display(Name = nameof(Resources.TransferReferenceResource.JAN), ResourceType = typeof(Resources.TransferReferenceResource), Order = 3)]
        public string Jan { get { return $"\"{jan}\""; } set { jan = value; } }

        /// <summary>
        /// 商品名
        /// </summary>
        [Display(Name = nameof(Resources.TransferReferenceResource.ItemName2), ResourceType = typeof(Resources.TransferReferenceResource), Order = 4)]
        public string ItemName { get { return $"\"{itemNm}\""; } set { itemNm = value; } }

        /// <summary>
        /// ピック数
        /// </summary>
        [Display(Name = nameof(Resources.TransferReferenceResource.PicQty), ResourceType = typeof(Resources.TransferReferenceResource), Order = 5)]
        public string PicQty { get { return $"\"{picQty}\""; } set { picQty = value; } }

    }
}