namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// ロケーション区分
    /// </summary>
    [Table("M_LOCATION_CLASSES")]
    public partial class LocationClasses : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// ロケーション区分 (LOCATION_CLASS)
        /// </summary>
        /// <remarks>
        /// ※「システム処理定義書_在庫」参照
        /// 01:S品 ケースロケ、良品ケースロケ
        /// 02:アソートケース
        /// 03:S品バラ保管ロケ、良品バラ保管ロケ
        /// 11:入荷仮ロケ
        /// 12:返品仮ロケ
        /// 13:仕分ロケ
        /// 14:仕入先返品ロケ
        /// 15:不良品ロケ
        /// 16:調査ロケ
        /// 21:バラ移動中
        /// 22:出荷ロケ
        /// 23:出荷停止？
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationClassesResource.LocationClass), ResourceType = typeof(LocationClassesResource))]
        [MaxLength(2, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationClass { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        /// <remarks>
        /// １ロケ区分には以下のいずれかのみ設定可能
        /// 1:ケース
        /// 2:バラ
        /// 9:指定なし
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationClassesResource.CaseClass), ResourceType = typeof(LocationClassesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public CaseClassEnum CaseClass { get; set; }

        /// <summary>
        /// ロケーション区分名 (LOCATION_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationClassesResource.LocationName), ResourceType = typeof(LocationClassesResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationName { get; set; }

        /// <summary>
        /// 棚卸対象区分 (INVENTORY_CLASS)
        /// </summary>
        /// <remarks>
        /// 0:棚卸対象外ロケ、1:棚卸対象ロケ
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationClassesResource.InventoryClass), ResourceType = typeof(LocationClassesResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int InventoryClass { get; set; }

        /// <summary>
        /// 保管外ロケフラグ (NOT_STOCK_FLAG)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationClassesResource.NotStockFlag), ResourceType = typeof(LocationClassesResource))]
        [Range(0, 1, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int NotStockFlag { get; set; }

        /// <summary>
        /// メンテナンス不可フラグ (MENTE_DISABLE_FLAG)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationClassesResource.MenteDisableFlag), ResourceType = typeof(LocationClassesResource))]
        [Range(0, 1, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int MenteDisableFlag { get; set; }

        #endregion プロパティ
    }
}