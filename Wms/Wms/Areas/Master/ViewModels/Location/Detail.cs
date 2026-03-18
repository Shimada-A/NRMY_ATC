namespace Wms.Areas.Master.ViewModels.Location
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common;
    using Share.Common.Resources;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    public class Detail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Display(Name = nameof(LocationResource.CenterId), ResourceType = typeof(LocationResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// ロケーションコード (LOCATION_CD)
        /// </summary>
        /// <remarks>
        /// ハイフンいり　C2A-XXX-XXX-XXX-XXX　とする。ロケーションラベルの印字もハイフン入りで印字を行う。
        /// 既存ラベルは　C2A---XXX-XXX-XXX-XXX　となっているので変換必要
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Display(Name = nameof(LocationResource.LocationCd), ResourceType = typeof(LocationResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocationCd { get; set; }

        /// <summary>
        /// エリア (LOCSEC_1)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.Locsec1), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [MinLength(3, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(RegularExpressionPattern.Alphanumeric, ErrorMessageResourceName = nameof(MessagesResource.Alphanumeric), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_1 { get; set; }

        /// <summary>
        /// 棚列 (LOCSEC_2)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.Locsec2), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [MinLength(3, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(RegularExpressionPattern.Alphanumeric, ErrorMessageResourceName = nameof(MessagesResource.Alphanumeric), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_2 { get; set; }

        /// <summary>
        /// 棚番 (LOCSEC_3)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.Locsec3), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [MinLength(3, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(RegularExpressionPattern.Alphanumeric, ErrorMessageResourceName = nameof(MessagesResource.Alphanumeric), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_3 { get; set; }

        /// <summary>
        /// 段 (LOCSEC_4)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.Locsec4), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [MinLength(3, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(RegularExpressionPattern.Alphanumeric, ErrorMessageResourceName = nameof(MessagesResource.Alphanumeric), ErrorMessageResourceType = typeof(MessagesResource))]
        public string Locsec_4 { get; set; }

        /// <summary>
        /// 間口 (LOCSEC_5)
        /// </summary>
        /// <remarks>
        /// 3桁必須
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.Locsec5), ResourceType = typeof(LocationResource))]
        [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [MinLength(3, ErrorMessageResourceName = "MinLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(RegularExpressionPattern.Alphanumeric, ErrorMessageResourceName = nameof(MessagesResource.Alphanumeric), ErrorMessageResourceType = typeof(MessagesResource))]
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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.LocationClass), ResourceType = typeof(LocationResource))]
        [MaxLength(2, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.CaseClass), ResourceType = typeof(LocationResource))]
        [Range(-99, 99, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public CaseClassEnum CaseClass { get; set; }

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
        /// 引当優先順位 (ALLOC_PRIORITY)
        /// </summary>
        /// <remarks>
        /// デフォルトは最大値
        /// </remarks>
        [Display(Name = nameof(LocationResource.AllocPriority), ResourceType = typeof(LocationResource))]
        [Range(0, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? AllocPriority { get; set; }

        /// <summary>
        /// ピッキンググループNo (PICKING_GROUP_NO)
        /// </summary>
        /// <remarks>
        /// 1エリア内のピッキンググループNo
        /// ハンディピック時、ピッキングリスト出力時のエリア内のピックグループとする。
        /// </remarks>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(LocationResource.PickingGroupNo), ResourceType = typeof(LocationResource))]
        [Range(0, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int PickingGroupNo { get; set; }

        /// <summary>
        /// 在庫数
        /// </summary>
        public int? StockQty { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
        #endregion プロパティ
    }
}