namespace Wms.Areas.Returns.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Returns.Resources;
    using Wms.Models;

    /// <summary>
    /// EC返品照会ワーク
    /// </summary>
    [Table("WW_RET_EC_REFERENCE01")]
    public partial class RetEcReference : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key, Column(Order = 20)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EcReferenceResource.Seq), ResourceType = typeof(EcReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key, Column(Order = 30)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(EcReferenceResource.LineNo), ResourceType = typeof(EcReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.CenterId), ResourceType = typeof(EcReferenceResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 返品コード (RETURN_ID)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ReturnId), ResourceType = typeof(EcReferenceResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ReturnId { get; set; }

        /// <summary>
        /// 返品登録日 (ARRIVE_DATE)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ArriveDate), ResourceType = typeof(EcReferenceResource))]
        public DateTime? ArriveDate { get; set; }


        /// <summary>
        /// 出荷確定日 (KAKU_DATE)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.KakuDate), ResourceType = typeof(EcReferenceResource))]
        public DateTime? KakuDate { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ShipInstructId), ResourceType = typeof(EcReferenceResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 実績数 (RETURN_QTY_SUM)
        /// </summary>
        [Display(Name = nameof(EcReferenceResource.ReturnQtySum), ResourceType = typeof(EcReferenceResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? ReturnQtySum { get; set; }
        #endregion
    }
}