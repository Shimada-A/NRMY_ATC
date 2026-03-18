namespace Wms.Areas.Master.ViewModels.Location
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;

    public class Report
    {
        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationCd), ResourceType = typeof(LocationResource), Order = 1)]
        public string LocationCd { get; set; }

        /// <summary>
        /// エリア
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec1), ResourceType = typeof(Resources.LocationResource), Order = 2)]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// 棚列
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec2), ResourceType = typeof(Resources.LocationResource), Order = 3)]
        public string Locsec_2 { get; set; }

        /// <summary>
        /// 棚番
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec3), ResourceType = typeof(Resources.LocationResource), Order = 4)]
        public string Locsec_3 { get; set; }

        /// <summary>
        /// 段
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec4), ResourceType = typeof(Resources.LocationResource), Order = 5)]
        public string Locsec_4 { get; set; }

        /// <summary>
        /// 間口
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.Locsec5), ResourceType = typeof(Resources.LocationResource), Order = 6)]
        public string Locsec_5 { get; set; }

        /// <summary>
        /// ロケーション区分
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.LocationClass), ResourceType = typeof(Resources.LocationResource), Order = 7)]
        public string LocationClass { get; set; }

        /// <summary>
        /// ロケーション区分名 (LOCATION_NAME)
        /// </summary>
        /// <remarks>
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
        [Display(Name = nameof(Resources.LocationResource.LocationName), ResourceType = typeof(Resources.LocationResource), Order = 8)]
        public string LocationName { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource), Order = 9)]
        public string CaseClass { get; set; }

        /// <summary>
        /// 荷姿区分名称
        /// </summary>
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource), Order = 10)]
        public string CaseClassName { get; set; }

        /// <summary>
        /// 格付ID
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.GradeId), ResourceType = typeof(Resources.LocationResource), Order = 11)]
        public string GradeId { get; set; }

        /// <summary>
        /// 引当優先順位
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.AllocPriority), ResourceType = typeof(Resources.LocationResource), Order = 12)]
        public string AllocPriority { get; set; }

        /// <summary>
        /// ピッキンググループNo
        /// </summary>
        /// <remarks>
        [Display(Name = nameof(Resources.LocationResource.PickingGroupNo), ResourceType = typeof(Resources.LocationResource), Order = 13)]
        public string PickingGroupNo { get; set; }

        /// <summary>
        /// 棚卸No (INVENTORY_NO)
        /// </summary>
        /// <remarks>
        /// 棚卸中の場合、設定されている。棚卸開始でセットする。
        /// 棚卸中のロケは２重に棚卸指定できない。
        /// </remarks>
        [Display(Name = nameof(LocationResource.InventoryNo), ResourceType = typeof(LocationResource))]
        public string InventoryNo { get; set; }

        /// <summary>
        /// 棚卸名称 (INVENTORY_NAME)
        /// </summary>
        [Display(Name = nameof(LocationResource.InventoryName), ResourceType = typeof(LocationResource))]
        public string InventoryName { get; set; }

        /// <summary>
        /// 棚卸確定フラグ (INVENTORY_CONFIRM_FLAG)
        /// </summary>
        /// <remarks>
        /// 1：未確定、2：仮確定、3：本確定
        /// ※棚卸No ＜＞NULL、かつ、棚卸確定フラグ「1:未確定」の場合は、引当不可
        /// </remarks>
        [Display(Name = nameof(LocationResource.InventoryConfirmFlag), ResourceType = typeof(LocationResource))]
        public string InventoryConfirmFlag { get; set; }

        /// <summary>
        /// 棚卸開始日時 (INVENTORY_START_DATE)
        /// </summary>
        [Display(Name = nameof(LocationResource.InventoryStartDate), ResourceType = typeof(LocationResource))]
        public string InventoryStartDate { get; set; }

        /// <summary>
        /// 棚卸確定日時 (INVENTORY_CONFIRM_DATE)
        /// </summary>
        [Display(Name = nameof(LocationResource.InventoryConfirmDate), ResourceType = typeof(LocationResource))]
        public string InventoryConfirmDate { get; set; }
    }
}