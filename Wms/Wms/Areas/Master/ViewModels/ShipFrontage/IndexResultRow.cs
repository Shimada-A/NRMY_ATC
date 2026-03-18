namespace Wms.Areas.Master.ViewModels.ShipFrontage
{
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 出荷レーン間口ワーク
    /// </summary>
    public class IndexResultRow : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// NO (NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Display(Name = nameof(ShipFrontageResource.No), ResourceType = typeof(ShipFrontageResource))]
        public long No { get; set; }

        /// <summary>
        /// レーンNO (LANE_NO)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.LaneNo), ResourceType = typeof(ShipFrontageResource))]
        public string LaneNo { get; set; }

        /// <summary>
        /// 間口NO (FRONTAGE_NO)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.FrontageNo), ResourceType = typeof(ShipFrontageResource))]
        public string FrontageNo { get; set; }

        /// <summary>
        /// 店舗ID (STORE_ID)
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.StoreId), ResourceType = typeof(ShipFrontageResource))]
        public string StoreId { get; set; }

        /// <summary>
        /// 出荷先名称 (STORE_NAME)
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Msg
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.ErrMsg), ResourceType = typeof(ShipFrontageResource))]
        public string ErrMsg { get; set; }

        /// <summary>
        /// ブランドID
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.BrandId),ResourceType = typeof(ShipFrontageResource))]
        public string BrandId { get; set;}

        /// <summary>
        /// ブランド名
        /// </summary>
        [Display(Name = nameof(ShipFrontageResource.BrandName),ResourceType = typeof(ShipFrontageResource))]
        public string BrandName { get; set;}


        #endregion プロパティ
    }
}