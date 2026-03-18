namespace Wms.Areas.Master.ViewModels.Location
{
    using System.ComponentModel.DataAnnotations;
    using Share.Common.Resources;
    using Share.Extensions.Attributes;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// ロケーションマスタ設定
    /// </summary>
    public class IndexResultRow : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// センターコード (CENTER_ID)
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        /// <remarks>
        /// SF_GET_WORK_ID　より取得
        /// </remarks>
        public long Seq { get; set; }

        /// <summary>
        /// NO (NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Display(Name = nameof(LocationResource.No), ResourceType = typeof(LocationResource))]
        public long No { get; set; }

        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationCd), ResourceType = typeof(LocationResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// エリア (LOCSEC_1)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec1), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// 棚列 (LOCSEC_2)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec2), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_2 { get; set; }

        /// <summary>
        /// 棚番 (LOCSEC_3)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec3), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_3 { get; set; }

        /// <summary>
        /// 段 (LOCSEC_4)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec4), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_4 { get; set; }

        /// <summary>
        /// 間口 (LOCSEC_5)
        /// </summary>
        [Display(Name = nameof(LocationResource.Locsec5), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_5 { get; set; }

        /// <summary>
        /// ロケーション区分 (LOCATION_CLASS)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationClass), ResourceType = typeof(LocationResource))]
        public string LocationClass { get; set; }

        /// <summary>
        /// ロケーション区分 (LOCATION_CLASS)
        /// </summary>
        [Display(Name = nameof(LocationResource.LocationClass), ResourceType = typeof(LocationResource))]
        [MaxLength(2, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationName { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource))]
        public CaseClassEnum CaseClass { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_NAME)
        /// </summary>
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource))]
        public string CaseName { get; set; }

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        /// <remarks>
        /// S：S品　A：A品　B：B品　C：C品
        /// ロケ区分マスタで該当区分にセット格付け（複数あり）のみセット可能
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.GradeId), ResourceType = typeof(LocationResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 格付ID (GRADE_NAME)
        /// </summary>
        [Display(Name = nameof(LocationResource.GradeId), ResourceType = typeof(LocationResource))]
        public string GradeName { get; set; }

        /// <summary>
        /// 引当優先順位 (ALLOC_PRIORITY)
        /// </summary>
        /// <remarks>
        /// デフォルトは最大値
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.AllocPriority), ResourceType = typeof(LocationResource))]
        public string AllocPriority { get; set; }

        /// <summary>
        /// ピッキンググループNo (PICKING_GROUP_NO)
        /// </summary>
        /// <remarks>
        /// 1エリア内のピッキンググループNo
        /// ハンディピック時、ピッキングリスト出力時のエリア内のピックグループとする。
        /// </remarks>
        [Display(Name = nameof(LocationResource.PickingGroupNo), ResourceType = typeof(LocationResource))]
        public string PickingGroupNo { get; set; }

        /// <summary>
        /// Msg
        /// </summary>
        [Display(Name = nameof(LocationResource.ErrMsg), ResourceType = typeof(LocationResource))]
        public string ErrMsg { get; set; }

        #endregion プロパティ
    }
}