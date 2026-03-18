namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// お知らせ連携エラー明細
    /// </summary>
    [Table("T_NOTICE_DETAILS")]
    public partial class NoticeDetail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// お知らせエラーヘッダID (NOTICE_HEADER_ID)
        /// </summary>
        /// <remarks>
        /// お知らせ連携エラーヘッダID
        /// </remarks>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.NoticeHeaderId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string NoticeHeaderId { get; set; }

        /// <summary>
        /// 明細番号 (SEQ)
        /// </summary>
        /// <remarks>
        /// IDごとの連番
        /// </remarks>
        [Key]
        [Column(Order = 12)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.Seq), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int Seq { get; set; }

        /// <summary>
        /// 指示管理ID (INSTRUCT_ID)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.InstructId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string InstructId { get; set; }

        /// <summary>
        /// 指示区分 (INSTRUCT_CLASS)
        /// </summary>
        /// <remarks>
        /// コード一覧「指示区分」参照
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.InstructClass), ResourceType = typeof(NoticeDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte InstructClass { get; set; }

        /// <summary>
        /// 指示ID (SHIP_ARRIVE_INSTRUCT_ID)
        /// </summary>
        /// <remarks>
        /// 出荷指示IDまたは入荷予定ID
        /// </remarks>
        [Display(Name = nameof(NoticeDetailResource.ShipArriveInstructId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipArriveInstructId { get; set; }

        /// <summary>
        /// 出荷元所在区分 (SHIP_FROM_LOC_CLASS)
        /// </summary>
        /// <remarks>
        /// コード一覧「所在区分」参照
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.ShipFromLocClass), ResourceType = typeof(NoticeDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte ShipFromLocClass { get; set; }

        /// <summary>
        /// 出荷元所在ID (SHIP_FROM_LOC_ID)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.ShipFromLocId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipFromLocId { get; set; }

        /// <summary>
        /// 入荷先所在区分 (ARRIVE_TO_LOC_CLASS)
        /// </summary>
        /// <remarks>
        /// コード一覧「所在区分」参照
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.ArriveToLocClass), ResourceType = typeof(NoticeDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte ArriveToLocClass { get; set; }

        /// <summary>
        /// 入荷先所在ID (ARRIVE_TO_LOC_ID)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.ArriveToLocId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ArriveToLocId { get; set; }

        /// <summary>
        /// 在庫所在区分 (STOCK_LOC_CLASS)
        /// </summary>
        /// <remarks>
        /// コード一覧「所在区分」参照
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.StockLocClass), ResourceType = typeof(NoticeDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte StockLocClass { get; set; }

        /// <summary>
        /// 在庫所在ID (STOCK_LOC_ID)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.StockLocId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string StockLocId { get; set; }

        /// <summary>
        /// 注文番号 (ORDER_NO)
        /// </summary>
        /// <remarks>
        /// EC受注の場合は出荷指示IDをセット
        /// </remarks>
        [Display(Name = nameof(NoticeDetailResource.OrderNo), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string OrderNo { get; set; }

        /// <summary>
        /// SKU (ITEM_SKU_ID)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.ItemSkuId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(30, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemSkuId { get; set; }

        /// <summary>
        /// 品名 (ITEM_NAME)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.ItemName), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ItemName { get; set; }

        /// <summary>
        /// 格付ID (GRADE_ID)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.GradeId), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string GradeId { get; set; }

        /// <summary>
        /// 予定数 (PLAN_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.PlanQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int PlanQty { get; set; }

        /// <summary>
        /// 欠品数 (STOCKOUT_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.StockoutQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int StockoutQty { get; set; }

        /// <summary>
        /// 出荷可能数 (SHIPPABLE_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.ShippableQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ShippableQty { get; set; }

        /// <summary>
        /// 引当数 (ALLOC_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.AllocQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int AllocQty { get; set; }

        /// <summary>
        /// 実績数 (RESULT_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.ResultQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ResultQty { get; set; }

        /// <summary>
        /// JET在庫数 (JET_STOCK_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.JetStockQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int JetStockQty { get; set; }

        /// <summary>
        /// 受信在庫数 (RECEIVE_STOCK_QTY)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.ReceiveStockQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ReceiveStockQty { get; set; }

        /// <summary>
        /// 差異数 (DIFFERENCE_QTY)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.DifferenceQty), ResourceType = typeof(NoticeDetailResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DifferenceQty { get; set; }

        /// <summary>
        /// 予定日 (PLAN_DATE)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.PlanDate), ResourceType = typeof(NoticeDetailResource))]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? PlanDate { get; set; }

        /// <summary>
        /// 実績日 (RESULT_DATE)
        /// </summary>
        /// <remarks>
        /// 指定日または実績日
        /// </remarks>
        [Display(Name = nameof(NoticeDetailResource.ResultDate), ResourceType = typeof(NoticeDetailResource))]
        public DateTime? ResultDate { get; set; }

        /// <summary>
        /// 所在郵便番号 (LOC_ZIP)
        /// </summary>
        [Display(Name = nameof(NoticeDetailResource.LocZip), ResourceType = typeof(NoticeDetailResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string LocZip { get; set; }

        /// <summary>
        /// ECサイト区分 (EC_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:Amazon 2:Yahoo 3:Rakuten 4:Zozo 5:SHOPLIST 6:ecbeing 7:楽天 8:Qoo10
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(NoticeDetailResource.EcClass), ResourceType = typeof(NoticeDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte EcClass { get; set; }

        #endregion
    }
}
