namespace Wms.Areas.Master.ViewModels.Location
{
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// Store list country which is posted from view
    /// </summary>
    public class LocationList : BaseModel
    {
        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationCd), ResourceType = typeof(LocationResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// エリア (LOCSEC_1)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec1), ResourceType = typeof(LocationResource))]
        public string Locsec1 { get; set; }

        /// <summary>
        /// 棚列 (LOCSEC_2)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec2), ResourceType = typeof(LocationResource))]
        public string Locsec2 { get; set; }

        /// <summary>
        /// 棚番 (LOCSEC_3)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec3), ResourceType = typeof(LocationResource))]
        public string Locsec3 { get; set; }

        /// <summary>
        /// 段 (LOCSEC_4)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec4), ResourceType = typeof(LocationResource))]
        public string Locsec4 { get; set; }

        /// <summary>
        /// 間口 (LOCSEC_5)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec5), ResourceType = typeof(LocationResource))]
        public string Locsec5 { get; set; }

        /// <summary>
        /// ロケーション区分 (LOCATION_CLASS)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationClass), ResourceType = typeof(LocationResource))]
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
        public string LocationName { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource))]
        public string CaseClass { get; set; }

        /// <summary>
        /// 荷姿区分名称
        /// </summary>
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource))]
        public string CaseClassName { get; set; }

        /// <summary>
        /// 格付 (GRADE_ID)
        /// </summary>
        [Display(Name = nameof(LocationResource.GradeId), ResourceType = typeof(LocationResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付名称 (GRADE_NAME)
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 引当優先順位 (ALLOC_PRIORITY)
        /// </summary>
        [Display(Name = nameof(LocationResource.AllocPriority), ResourceType = typeof(LocationResource))]
        public string AllocPriority { get; set; }

        /// <summary>
        /// ピッキンググループNO (PICKING_GROUP_NO)
        /// </summary>
        /// <remarks>
        /// 1エリア内のピッキンググループNOハンディピック時、ピッキングリスト出力時のエリア内のピックグループとする。
        /// </remarks>
        [Display(Name = nameof(LocationResource.PickingGroupNo), ResourceType = typeof(LocationResource))]
        public string PickingGroupNo { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// （センターコード）
        /// </remarks>
        [Display(Name = nameof(LocationResource.CenterId), ResourceType = typeof(LocationResource))]
        public string CenterId { get; set; }

        public string Rid { get; set; }

        /// <summary>
        /// メンテナンス不可フラグ (MENTE_DISABLE_FLAG)
        /// </summary>
        /// <remarks>
        /// メンテナンス不可フラグ=1のロケ、または在庫レコードあるロケ
        /// </remarks>
        [Display(Name = nameof(LocationResource.MenteDisableFlag), ResourceType = typeof(LocationResource))]
        public int MenteDisableFlag { get; set; }
    }
}