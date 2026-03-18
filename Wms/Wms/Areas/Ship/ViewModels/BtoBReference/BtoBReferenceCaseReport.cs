using Share.Common.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Ship.Resources;

namespace Wms.Areas.Ship.ViewModels.BtoBReference
{
    /// <summary>
    /// 出荷梱包進捗照会 ダウンロード（ケース単位）
    /// </summary>
    public class BtoBReferenceCaseReport
    {
        [Display(Name = nameof(BtoBReferenceResource.CenterId), ResourceType = typeof(BtoBReferenceResource), Order = 1)]
        public string CenterId { get; set; }

        /// <summary>
        /// ケースNo
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.BoxNo), ResourceType = typeof(BtoBReferenceResource), Order = 2)]
        public string BoxNo { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipToStoreId), ResourceType = typeof(BtoBReferenceResource), Order = 3)]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ShipToStoreName), ResourceType = typeof(BtoBReferenceResource), Order = 4)]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 実績数
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ResultQty), ResourceType = typeof(BtoBReferenceResource), Order = 5)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? ResultQty { get; set; }

        /// <summary>
        /// バッチNo
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.BatchNo), ResourceType = typeof(BtoBReferenceResource), Order = 6)]
        public string BatchNo { get; set; }

        /// <summary>
        /// 納品書No
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NouhinNo), ResourceType = typeof(BtoBReferenceResource), Order = 7)]
        public string NouhinNo { get; set; }

        /// <summary>
        /// 納品書発行日時
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NouhinPrnDate), ResourceType = typeof(BtoBReferenceResource), Order = 8)]
        public DateTime? NouhinPrnDate { get; set; }

        /// <summary>
        /// 納品書発行者ID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NouhinUserId), ResourceType = typeof(BtoBReferenceResource), Order = 9)]
        public string NouhinPrnUserId { get; set; }

        /// <summary>
        /// 納品書発行者名
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.NouhinUserName), ResourceType = typeof(BtoBReferenceResource), Order = 10)]
        public string NouhinPrnUserName { get; set; }

        /// <summary>
        /// 出荷確定日 (KAKU_DATE)
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.KakuDate), ResourceType = typeof(BtoBReferenceResource), Order = 11)]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 配送業者ID
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.TransporterId), ResourceType = typeof(BtoBReferenceResource), Order = 12)]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.TransporterName), ResourceType = typeof(BtoBReferenceResource), Order = 13)]
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送業者顧客コード
        /// </summary>
        [Display(Name = nameof(BtoBReferenceResource.ClientCd), ResourceType = typeof(BtoBReferenceResource), Order = 14)]
        public string ClientCd { get; set; }

        /// <summary>
        /// 状態 (SHIP_STATUS_NAME)
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.ShipStatusName), ResourceType = typeof(ShpBtoBReference01Resource), Order = 15)]
        public string ShipStatusName { get; set; }

        /// <summary>
        /// ブランド
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.BrandName), ResourceType = typeof(ShpBtoBReference01Resource), Order = 16)]
        public string BrandName {  get; set; }

        /// <summary>
        /// 送り状No
        /// </summary>
        [Display(Name = nameof(ShpBtoBReference01Resource.DeliNo), ResourceType = typeof(ShpBtoBReference01Resource), Order = 17)]
        public string DeliNo { get; set; }
    }
}