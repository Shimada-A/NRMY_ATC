namespace Wms.Areas.Returns.ViewModels.EcReference
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class EcReference02SearchConditions
    {
        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; } = Common.Profile.User.CenterId;

        /// <summary>
        /// EC注文番号
        /// </summary>
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        public string ReturnId { get; set; }

        /// <summary>
        /// 返品登録日
        /// </summary>
        public DateTime? ArriveDate { get; set; }

        /// <summary>
        /// 返品伝票ID
        /// </summary>
        public string RelatedOrderNo { get; set; }

        /// <summary>
        /// 返品日
        /// </summary>
        public string KakuDate { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public long LineNo { get; set; }
    }
}