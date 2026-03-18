namespace Wms.Areas.Ship.Models
{
    using Mvc.Common;
    using Wms.Models;
    using Wms.Extensions.Attributes;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;


    /// <summary>
    /// ケース出荷指示取込ワーク
    /// </summary>
    [Table("WW_SHP_CASE_INSTRUCTION")]
    public partial class ShpCaseInstruction : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UploadCaseInstructionResource.Seq), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UploadCaseInstructionResource.LineNo), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.CenterId), ResourceType = typeof(UploadCaseInstructionResource))]
        [MaxLength(40, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷種別 (SHIP_KIND)
        /// </summary>
        /// <remarks>
        /// 4：ケース出荷　5：ケース出荷JAN抜取
        /// </remarks>
        [Display(Name = nameof(UploadCaseInstructionResource.ShipKind), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-99, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? ShipKind { get; set; }

        /// <summary>
        /// ケース出荷指示名称 (SHIP_INSTRUCT_NAME)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.ShipInstructName), ResourceType = typeof(UploadCaseInstructionResource))]
        [MaxLength(200, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructName { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        /// <remarks>
        /// YYYY/MM/DD
        /// </remarks>
        [Display(Name = nameof(UploadCaseInstructionResource.ShipPlanDate), ResourceType = typeof(UploadCaseInstructionResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 梱包番号 (BOX_NO)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.BoxNo), ResourceType = typeof(UploadCaseInstructionResource))]
        [MaxLength(36, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string BoxNo { get; set; }

        /// <summary>
        /// 店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.ShipToStoreId), ResourceType = typeof(UploadCaseInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 優先順 (PRIORIT_ORDER)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.PrioritOrder), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? PrioritOrder { get; set; }

        /// <summary>
        /// 抜き取りJAN (JAN)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.Jan), ResourceType = typeof(UploadCaseInstructionResource))]
        [MaxLength(13, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string Jan { get; set; }

        /// <summary>
        /// 数量 (INSTRUCT_QTY)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.InstructQty), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? InstructQty { get; set; }

        /// <summary>
        /// エラー区分 (ERR_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:エラーなし　それ以外エラー発生時エラーコード
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(UploadCaseInstructionResource.ErrClass), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int ErrClass { get; set; }

        /// <summary>
        /// エラーメッセージ (ERR_MESSAGE)
        /// </summary>
        /// <remarks>
        /// エラーメッセージ内容(日本語)
        /// </remarks>
        [Display(Name = nameof(UploadCaseInstructionResource.ErrMessage), ResourceType = typeof(UploadCaseInstructionResource))]
        [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string ErrMessage { get; set; }

        #endregion
    }
}
