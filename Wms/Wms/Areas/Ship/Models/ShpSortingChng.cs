namespace Wms.Areas.Ship.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Ship.Resources;
    using Wms.Models;

    /// <summary>
    /// BtoB出荷梱包進捗照会ワーク
    /// </summary>
    [Table("WW_SHP_SORTING_CHNG")]
    public partial class ShpSortingChng : BaseModel
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
        [Display(Name = nameof(SortSetResource.Seq), ResourceType = typeof(SortSetResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(SortSetResource.LineNo), ResourceType = typeof(SortSetResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        [Display(Name = nameof(SortSetResource.CenterId), ResourceType = typeof(SortSetResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// テーブル種別 (TABLE_CLASS)
        /// </summary>
        [Display(Name = nameof(SortSetResource.TableClass), ResourceType = typeof(SortSetResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? TableClass { get; set; }

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        [Display(Name = nameof(SortSetResource.TransporterId), ResourceType = typeof(SortSetResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 届先都道府県名 (PREF_NAME)
        /// </summary>
        [Display(Name = nameof(SortSetResource.PrefName), ResourceType = typeof(SortSetResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PrefName { get; set; }

        /// <summary>
        /// 届先市区町村名 (CITY_NAME)
        /// </summary>
        [Display(Name = nameof(SortSetResource.CityName), ResourceType = typeof(SortSetResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CityName { get; set; }

        /// <summary>
        /// 区分名 (CLASS)
        /// </summary>
        [Display(Name = nameof(SortSetResource.Class), ResourceType = typeof(SortSetResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Class { get; set; }

        /// <summary>
        /// 出荷予定日 (SHIP_PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(SortSetResource.ShipPlanDate), ResourceType = typeof(SortSetResource))]
        public DateTime? ShipPlanDate { get; set; }

        /// <summary>
        /// 出荷指示ID (SHIP_INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(SortSetResource.ShipInstructId), ResourceType = typeof(SortSetResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipInstructId { get; set; }

        /// <summary>
        /// 出荷先ID (SHIP_TO_STORE_ID)
        /// </summary>
        [Display(Name = nameof(SortSetResource.ShipToStoreId), ResourceType = typeof(SortSetResource))]
        [MaxLength(40, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreId { get; set; }

        /// <summary>
        /// 出荷先名 (SHIP_TO_STORE_NAME)
        /// </summary>
        [Display(Name = nameof(SortSetResource.ShipToStoreName), ResourceType = typeof(SortSetResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToStoreName { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Display(Name = nameof(SortSetResource.TransporterName), ResourceType = typeof(SortSetResource))]
        [MaxLength(200, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 郵便番号 (SHIP_TO_ZIP)
        /// </summary>
        [Display(Name = nameof(SortSetResource.ShipToZip), ResourceType = typeof(SortSetResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToZip { get; set; }

        /// <summary>
        /// 届先住所 (SHIP_TO_ADDRESS)
        /// </summary>
        [Display(Name = nameof(SortSetResource.ShipToAddress), ResourceType = typeof(SortSetResource))]
        [MaxLength(500, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipToAddress { get; set; }

        /// <summary>
        /// 仕分コード (SORTING_CD)
        /// </summary>
        [Display(Name = nameof(SortSetResource.SortingCd), ResourceType = typeof(SortSetResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortingCd { get; set; }

        /// <summary>
        /// 仕分コード（整合） (SORTING_CD_HID)
        /// </summary>
        /// <remarks>
        /// 各配送業者の仕分コードマスタより取得
        /// </remarks>
        [Display(Name = nameof(SortSetResource.SortingCdHid), ResourceType = typeof(SortSetResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortingCdHid { get; set; }

        #endregion
    }
}
