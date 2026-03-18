namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Models;

    /// <summary>
    /// ロケーションマスタワーク
    /// </summary>
    [Table("WW_MAS_LOCATIONS")]
    public partial class MasLocation : BaseModel
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
        [Display(Name = nameof(LocationResource.No), ResourceType = typeof(LocationResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public long No { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Display(Name = nameof(LocationResource.CenterId), ResourceType = typeof(LocationResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        /// <remarks>
        /// ハイフンいり　C2A-XXX-XXX-XXX-XXX　とする。ロケーションラベルの印字もハイフン入りで印字を行う。
        /// 既存ラベルは　C2A---XXX-XXX-XXX-XXX　となっているので変換必要
        /// </remarks>
        [Display(Name = nameof(LocationResource.LocationCd), ResourceType = typeof(LocationResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// エリア (LOCSEC_1)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Display(Name = nameof(LocationResource.Locsec1), ResourceType = typeof(LocationResource))]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// 棚列 (LOCSEC_2)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Display(Name = nameof(LocationResource.Locsec2), ResourceType = typeof(LocationResource))]
        public string Locsec_2 { get; set; }

        /// <summary>
        /// 棚番 (LOCSEC_3)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Display(Name = nameof(LocationResource.Locsec3), ResourceType = typeof(LocationResource))]
        public string Locsec_3 { get; set; }

        /// <summary>
        /// 段 (LOCSEC_4)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Display(Name = nameof(LocationResource.Locsec4), ResourceType = typeof(LocationResource))]
        public string Locsec_4 { get; set; }

        /// <summary>
        /// 間口 (LOCSEC_5)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Display(Name = nameof(LocationResource.Locsec5), ResourceType = typeof(LocationResource))]
        public string Locsec_5 { get; set; }

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
        [Display(Name = nameof(LocationResource.LocationClass), ResourceType = typeof(LocationResource))]
        public string LocationClass { get; set; }

        /// <summary>
        /// 荷姿区分 (CASE_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:ケース
        /// 2:バラ
        /// 9:指定なし
        /// ロケ区分がきまれば決まる。ロケ区分マスタ01,02のときはケースのみ。03のときはバラのみ。それ以外は9：指定なし、とする。引当対象のロケ（ケース管理ロケ、バラ管理ロケ）は、ケース、またはバラのどちらかとなる。
        /// </remarks>
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource))]
        public string CaseClass { get; set; }

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        /// <remarks>
        /// S：S品　A：A品　B：B品　C：C品
        /// ロケ区分マスタで該当区分にセット格付け（複数あり）のみセット可能
        /// </remarks>
        [Display(Name = nameof(LocationResource.GradeId), ResourceType = typeof(LocationResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 引当優先順位 (ALLOC_PRIORITY)
        /// </summary>
        /// <remarks>
        /// デフォルトは最大値
        /// </remarks>
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
        /// エラー情報 (ERR_MSG)
        /// </summary>
        [Display(Name = nameof(LocationResource.ErrMsg), ResourceType = typeof(LocationResource))]
        public string ErrMsg { get; set; }

        #endregion プロパティ
    }
}