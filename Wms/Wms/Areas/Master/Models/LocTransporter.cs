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
    /// センター別店舗別配送業者
    /// </summary>
    [Table("M_LOC_TRANSPORTERS")]
    public partial class LocTransporter : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.CenterId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 出荷先店舗ID (SHIP_TO_STORE_ID)
        /// </summary>
        /// <remarks>
        /// 出荷先店舗ID、倉庫ID
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.ShipToStoreId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先店舗区分 (SHIP_TO_STORE_CLASS)
        /// </summary>
        /// <remarks>
        /// 出荷先店舗区分、倉庫区分　・・・未使用とする（出荷先ビューをみること）
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.ShipToStoreClass), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte ShipToStoreClass { get; set; }

        /// <summary>
        /// 適用開始日 (START_DATE)
        /// </summary>
        /// <remarks>
        /// YYYY/MM/DD
        /// 受信した出荷指示の出荷日がこの日以降について有効とする。
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.StartDate), ResourceType = typeof(LocTransporterResource))]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        /// <remarks>
        /// 01：ヤマト運輸
        /// 02：佐川急便
        /// 03：浪速運送
        /// 04：東京納品代行
        /// 06：ルート便 
        /// 07：あんしん　
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者ID(月) (TRANSPORTER_ID_MON)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdMon), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdMon { get; set; }

        /// <summary>
        /// リードタイム(月) (LEAD_TIMES_MON)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesMon), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesMon { get; set; }

        /// <summary>
        /// 配送業者ID(火) (TRANSPORTER_ID_TUE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdTue), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdTue { get; set; }

        /// <summary>
        /// リードタイム(火) (LEAD_TIMES_TUE)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesTue), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesTue { get; set; }

        /// <summary>
        /// 配送業者ID(水) (TRANSPORTER_ID_WED)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdWed), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdWed { get; set; }

        /// <summary>
        /// リードタイム(水) (LEAD_TIMES_WED)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesWed), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesWed { get; set; }

        /// <summary>
        /// 配送業者ID(木) (TRANSPORTER_ID_THU)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdThu), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdThu { get; set; }

        /// <summary>
        /// リードタイム(木) (LEAD_TIMES_THU)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesThu), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesThu { get; set; }

        /// <summary>
        /// 配送業者ID(金) (TRANSPORTER_ID_FRI)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdFri), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdFri { get; set; }

        /// <summary>
        /// リードタイム(金) (LEAD_TIMES_FRI)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesFri), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesFri { get; set; }

        /// <summary>
        /// 配送業者ID(土) (TRANSPORTER_ID_SAT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdSat), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdSat { get; set; }

        /// <summary>
        /// リードタイム(土) (LEAD_TIMES_SAT)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesSat), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesSat { get; set; }

        /// <summary>
        /// 配送業者ID(日) (TRANSPORTER_ID_SUN)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdSun), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdSun { get; set; }

        /// <summary>
        /// リードタイム(日) (LEAD_TIMES_SUN)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesSun), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesSun { get; set; }

        /// <summary>
        /// 配送業者ID(祝) (TRANSPORTER_ID_HOL)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.TransporterIdHol), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterIdHol { get; set; }

        /// <summary>
        /// リードタイム(祝) (LEAD_TIMES_HOL)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimesHol), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimesHol { get; set; }

        /// <summary>
        /// リードタイム (LEAD_TIMES)
        /// </summary>
        /// <remarks>
        /// メインのリードタイム
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocTransporterResource.LeadTimes), ResourceType = typeof(LocTransporterResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public byte LeadTimes { get; set; }

        /// <summary>
        /// お客様コード (CLIENT_CD)
        /// </summary>
        /// <remarks>
        /// 佐川顧客コード
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.ClientCd), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(12, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ClientCd { get; set; }

        /// <summary>
        /// [管理番号(浪速)]浪速管理ID
        /// </summary>
        /// <remarks>
        /// 浪速管理ID
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.ClientCd), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(6, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ControlId { get; set; }

        /// <summary>
        /// [荷送人コード]ワールドサプライ用　お客様コード
        /// </summary>
        /// <remarks>
        /// ワールドサプライ用　お客様コード
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.ConsignorId), ResourceType = typeof(LocTransporterResource))]
        [MaxLength(8, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ConsignorId { get; set; }

        #endregion
    }
}
