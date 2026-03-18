namespace Wms.Areas.Master.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;
    using Wms.Resources;

    /// <summary>
    /// センター別店舗別配送業者ワーク
    /// </summary>
    [Table("WW_MAS_LOC_TRANSPORTERS")]
    public partial class MasLocTransporter : BaseModel
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
        [Display(Name = nameof(MasBoxSettingResource.Seq), ResourceType = typeof(MasBoxSettingResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long Seq { get; set; }

        /// <summary>
        /// NO連番 (NO)
        /// </summary>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasBoxSettingResource.No), ResourceType = typeof(MasBoxSettingResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long No { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// （センターコード）
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.CenterId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        /// <remarks>
        /// （センターコード、店舗コード）
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.ShipToStoreId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先店舗区分 (SHIP_TO_STORE_CLASS)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ShipToStoreClass), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreClass { get; set; }

        /// <summary>
        /// 適用開始日 (START_DATE)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.StartDate), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StartDate { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// リードタイム (LEAD_TIMES)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimes), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimes { get; set; }

        /// <summary>
        /// 配送業者ID(月) (TRANSPORTER_ID_MON)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdMon), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdMon { get; set; }

        /// <summary>
        /// リードタイム(月) (LEAD_TIMES_MON)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesMon), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesMon { get; set; }

        /// <summary>
        /// 配送業者ID(火) (TRANSPORTER_ID_TUE)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdTue), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdTue { get; set; }

        /// <summary>
        /// リードタイム(火) (LEAD_TIMES_TUE)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesTue), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesTue { get; set; }

        /// <summary>
        /// 配送業者ID(水) (TRANSPORTER_ID_WED)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdWed), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdWed { get; set; }

        /// <summary>
        /// リードタイム(水) (LEAD_TIMES_WED)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesWed), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesWed { get; set; }

        /// <summary>
        /// 配送業者ID(木) (TRANSPORTER_ID_THU)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdThu), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdThu { get; set; }

        /// <summary>
        /// リードタイム(木) (LEAD_TIMES_THU)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesThu), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesThu { get; set; }

        /// <summary>
        /// 配送業者ID(金) (TRANSPORTER_ID_FRI)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdFri), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdFri { get; set; }

        /// <summary>
        /// リードタイム(金) (LEAD_TIMES_FRI)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesFri), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesFri { get; set; }

        /// <summary>
        /// 配送業者ID(土) (TRANSPORTER_ID_SAT)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdSat), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdSat { get; set; }

        /// <summary>
        /// リードタイム(土) (LEAD_TIMES_SAT)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesSat), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesSat { get; set; }

        /// <summary>
        /// 配送業者ID(日) (TRANSPORTER_ID_SUN)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdSun), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdSun { get; set; }

        /// <summary>
        /// リードタイム(日) (LEAD_TIMES_SUN)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesSun), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesSun { get; set; }

        /// <summary>
        /// 配送業者ID(祝) (TRANSPORTER_ID_HOL)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdHol), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdHol { get; set; }

        /// <summary>
        /// リードタイム(祝) (LEAD_TIMES_HOL)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesHol), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LeadTimesHol { get; set; }

        /// <summary>
        /// エラー情報 (ERR_MSG)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ErrMsg), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ErrMsg { get; set; }

        /// <summary>
        /// 佐川顧客コード (CLIENT_CD)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ClientCd), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientCd { get; set; }

        /// <summary>
        /// 浪速管理コード (CONTROL_ID)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ControlId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ControlId { get; set; }

        /// <summary>
        /// 荷送人コード ワールドサプライ用　お客様コード (CONTROL_ID)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ConsignorId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorId { get; set; }

        #endregion
    }
}
