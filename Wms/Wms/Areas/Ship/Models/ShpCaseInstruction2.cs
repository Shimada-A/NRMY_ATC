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
    /// ケース出荷指示取込画面ワーク
    /// </summary>
    [Table("WW_SHP_CASE_INSTRUCTION2")]
    public partial class ShpCaseInstruction2 : BaseModel
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
        /// ケース出荷指示取込日時 (SHIP_INSTRUCT_DATE)
        /// </summary>
        /// <remarks>
        /// YYYY/MM/DD
        /// </remarks>
        [Display(Name = nameof(UploadCaseInstructionResource.ShipInstructDate), ResourceType = typeof(UploadCaseInstructionResource))]
        public DateTime? ShipInstructDate { get; set; }

        /// <summary>
        /// バッチNo (BATCH_NO)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.BatchNo), ResourceType = typeof(UploadCaseInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string BatchNo { get; set; }

        /// <summary>
        /// 明細行数 (DETAIL_ROW_QTY)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.DetailRowQty), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DetailRowQty { get; set; }

        /// <summary>
        /// ケース数 (CASE_QTY)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.CaseQty), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? CaseQty { get; set; }

        /// <summary>
        /// 店舗数 (STORE_QTY)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.StoreQty), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? StoreQty { get; set; }

        /// <summary>
        /// 抜き取りJAN数 (PIC_JAN_QTY)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.PicJanQty), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? PicJanQty { get; set; }

        /// <summary>
        /// 抜き取り指示数 (PIC_INS_QTY)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.PicInsQty), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? PicInsQty { get; set; }

        /// <summary>
        /// 引当エラー数 (HIKI_ERR_QTY)
        /// </summary>
        [Display(Name = nameof(UploadCaseInstructionResource.HikiErrQty), ResourceType = typeof(UploadCaseInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? HikiErrQty { get; set; }

        #endregion
    }
}
