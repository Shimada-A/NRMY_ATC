namespace Wms.Areas.Master.ViewModels.LocTransporter
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// センター別店舗別配送業者ワーク
    /// </summary>
    public partial class IndexResultRow : BaseModel
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
        public long Seq { get; set; }

        /// <summary>
        /// NO (NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(MasBoxSettingResource.No), ResourceType = typeof(MasBoxSettingResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long No { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// （センターコード）
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.CenterId), ResourceType = typeof(LocTransporterResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// センター名称
        /// </summary>
        public string CenterName { get; set; }

        /// <summary>
        /// 出荷先所在区分 (SHIP_TO_LOC_CLASS)
        /// </summary>
        /// <remarks>
        /// 1：自社倉庫　3：自社店舗　８：EC倉庫
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.ShipToStoreClass), ResourceType = typeof(LocTransporterResource))]
        public string ShipToStoreClass { get; set; }

        /// <summary>
        /// 出荷先所在ID (SHIP_TO_STORE_ID)
        /// </summary>
        /// <remarks>
        /// （センターコード、店舗コード）
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.ShipToStoreId), ResourceType = typeof(LocTransporterResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先所在名称
        /// </summary>
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 適用開始日 (START_DATE)
        /// </summary>
        /// <remarks>
        /// YYYY/MM/DD
        /// 受信した出荷指示の出荷日がこの日以降について有効とする。
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.StartDate), ResourceType = typeof(LocTransporterResource))]
        public string StartDate { get; set; }

        /// <summary>
        /// メイン配送業者ID (TRANSPORTER_ID)
        /// </summary>
        /// <remarks>
        /// 01：ヤマト運輸
        /// 02：佐川急便
        /// 03：浪速運送
        /// 04：東京納品代行
        /// 0501：ハイエス　埼玉
        /// 0502：ハイエス　東京・・・
        /// 06：ルート便
        /// 07：あんしん
        /// </remarks>
        [Display(Name = nameof(LocTransporterResource.TransporterId), ResourceType = typeof(LocTransporterResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// メイン配送業者名称
        /// </summary>
        public string TransporterName { get; set; }

        /// <summary>
        /// メインリードタイム (LEAD_TIMES)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimes), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimes { get; set; }

        
        /// <summary>
        /// エリア
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 都道府県コード
        /// </summary>
        public string PrefId { get; set; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        public string PrefName { get; set; }

        /// <summary>
        /// 配送業者ID(月) (TRANSPORTER_ID_MON)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdMon), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdMon { get; set; }

        /// <summary>
        /// 配送業者ID(月)名称
        /// </summary>
        public string TransporterNameMon { get; set; }

        /// <summary>
        /// リードタイム(月) (LEAD_TIMES_MON)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesMon), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesMon { get; set; }

        /// <summary>
        /// 配送業者ID(火) (TRANSPORTER_ID_TUE)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdTue), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdTue { get; set; }

        /// <summary>
        /// 配送業者ID(火)名称
        /// </summary>
        public string TransporterNameTue { get; set; }

        /// <summary>
        /// リードタイム(火) (LEAD_TIMES_TUE)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesTue), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesTue { get; set; }

        /// <summary>
        /// 配送業者ID(水) (TRANSPORTER_ID_WED)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdWed), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdWed { get; set; }

        /// <summary>
        /// 配送業者ID(水)名称
        /// </summary>
        public string TransporterNameWed { get; set; }

        /// <summary>
        /// リードタイム(水) (LEAD_TIMES_WED)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesWed), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesWed { get; set; }

        /// <summary>
        /// 配送業者ID(木) (TRANSPORTER_ID_THU)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdThu), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdThu { get; set; }

        /// <summary>
        /// 配送業者ID(木)名称
        /// </summary>
        public string TransporterNameThu { get; set; }

        /// <summary>
        /// リードタイム(木) (LEAD_TIMES_THU)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesThu), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesThu { get; set; }

        /// <summary>
        /// 配送業者ID(金) (TRANSPORTER_ID_FRI)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdFri), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdFri { get; set; }

        /// <summary>
        /// 配送業者ID(金)名称
        /// </summary>
        public string TransporterNameFri { get; set; }

        /// <summary>
        /// リードタイム(金) (LEAD_TIMES_FRI)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesFri), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesFri { get; set; }

        /// <summary>
        /// 配送業者ID(土) (TRANSPORTER_ID_SAT)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdSat), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdSat { get; set; }

        /// <summary>
        /// 配送業者ID(土)名称
        /// </summary>
        public string TransporterNameSat { get; set; }

        /// <summary>
        /// リードタイム(土) (LEAD_TIMES_SAT)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesSat), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesSat { get; set; }

        /// <summary>
        /// 配送業者ID(日) (TRANSPORTER_ID_SUN)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdSun), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdSun { get; set; }

        /// <summary>
        /// 配送業者ID(日)名称
        /// </summary>
        public string TransporterNameSun { get; set; }

        /// <summary>
        /// リードタイム(日) (LEAD_TIMES_SUN)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesSun), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesSun { get; set; }

        /// <summary>
        /// 配送業者ID(祝) (TRANSPORTER_ID_HOL)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.TransporterIdHol), ResourceType = typeof(LocTransporterResource))]
        public string TransporterIdHol { get; set; }

        /// <summary>
        /// 配送業者ID(祝)名称
        /// </summary>
        public string TransporterNameHol { get; set; }

        /// <summary>
        /// リードタイム(祝) (LEAD_TIMES_HOL)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.LeadTimesHol), ResourceType = typeof(LocTransporterResource))]
        public string LeadTimesHol { get; set; }

        /// <summary>
        /// 顧客コード(佐川) (CLIENT_CD)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ClientCd), ResourceType = typeof(LocTransporterResource))]
        public string ClientCd { get; set; }


        /// <summary>
        /// 顧客コード(浪速) (CONTROL_ID)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ControlId), ResourceType = typeof(LocTransporterResource))]
        public string ControlId { get; set; }

        /// <summary>
        /// 顧客コード(WS) (CONSIGNOR_ID)
        /// </summary>
        [Display(Name = nameof(LocTransporterResource.ConsignorId), ResourceType = typeof(LocTransporterResource))]
        public string ConsignorId { get; set; }

        /// <summary>
        /// エラー情報 (ERR_MSG)
        /// </summary>
        [Display(Name = nameof(MasBoxSettingResource.ErrMsg), ResourceType = typeof(MasBoxSettingResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ErrMsg { get; set; }

        #endregion
    }
}
