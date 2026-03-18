namespace Wms.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common.Resources;
    using Wms.Resources;

    /// <summary>
    /// 連携データ
    /// </summary>
    [Table("M_IF_DATA")]
    public partial class IfData : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 連携データID (IF_DATA_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfDataResource.IfDataId), ResourceType = typeof(IfDataResource))]
        public string IfDataId { get; set; }

        /// <summary>
        /// 連携データ名 (IF_DATA_NAME)
        /// </summary>
        /// <remarks>
        /// お知らせメッセージに使用する
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfDataResource.IfDataName), ResourceType = typeof(IfDataResource))]
        public string IfDataName { get; set; }

        /// <summary>
        /// 送受信区分 (IF_CLASS)
        /// </summary>
        /// <remarks>
        /// 1:受信  2:送信
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfDataResource.IfClass), ResourceType = typeof(IfDataResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte IfClass { get; set; }

        /// <summary>
        /// 作成元テーブル物理名 (TABLE_NAME)
        /// </summary>
        /// <remarks>
        /// 受信時：お知らせメッセージに使用する。
        /// 送信時：連携状況設定に使用する。
        /// ※実際にDataExchangeで使用するのはﾏｯﾋﾟﾝｸﾞﾃｰﾌﾞﾙのほう
        /// </remarks>
        [Display(Name = nameof(IfDataResource.TableName), ResourceType = typeof(IfDataResource))]
        public string TableName { get; set; }

        /// <summary>
        /// 作成元テーブルテーブルコメント (TABLE_COMMENT)
        /// </summary>
        /// <remarks>
        /// 開発者識別用
        /// </remarks>
        [Display(Name = nameof(IfDataResource.TableComment), ResourceType = typeof(IfDataResource))]
        public string TableComment { get; set; }

        /// <summary>
        /// システム区分(モノのFrom) (FROM_SYSTEM_CLASS)
        /// </summary>
        /// <remarks>
        /// 送信時のみ使用
        /// 1:基幹 2:店舗BO 3:WMS 4:生産管理 5:自社EC 6:他社EC 7:画面
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfDataResource.FromSystemClass), ResourceType = typeof(IfDataResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte FromSystemClass { get; set; }

        /// <summary>
        /// システム区分(モノのTo) (TO_SYSTEM_CLASS)
        /// </summary>
        /// <remarks>
        /// 送信時のみ使用
        /// 1:基幹 2:店舗BO 3:WMS 4:生産管理 5:自社EC 6:他社EC 7:画面
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(IfDataResource.ToSystemClass), ResourceType = typeof(IfDataResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte ToSystemClass { get; set; }

        #endregion
    }
}
