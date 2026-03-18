namespace Wms.Areas.Master.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Share.Common;
    using Share.Common.Resources;
    using Wms.Models;

    /// <summary>
    /// 配送業者
    /// </summary>
    [Table("M_TRANSPORTERS")]
    public partial class Transporter : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 配送業者ID (TRANSPORTER_ID)
        /// </summary>
        [Key]
        [Column(Order = 11)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.TransporterResource.TransporterId), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        [RegularExpression(RegularExpressionPattern.Id, ErrorMessageResourceName = nameof(MessagesResource.Id), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterId { get; set; }

        /// <summary>
        /// 配送業者名 (TRANSPORTER_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.TransporterResource.TransporterName), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterName { get; set; }

        /// <summary>
        /// 配送業者(略称) (TRANSPORTER_SHORT_NAME)
        /// </summary>
        [Display(Name = nameof(Resources.TransporterResource.TransporterShortName), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterShortName { get; set; }

        /// <summary>
        /// TEL (TRANSPORTER_TEL)
        /// </summary>
        [Display(Name = nameof(Resources.TransporterResource.TransporterTel), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterTel { get; set; }

        /// <summary>
        /// FAX (TRANSPORTER_FAX)
        /// </summary>
        [Display(Name = nameof(Resources.TransporterResource.TransporterFax), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterFax { get; set; }

        /// <summary>
        /// Mail (TRANSPORTER_MAIL)
        /// </summary>
        [Display(Name = nameof(Resources.TransporterResource.TransporterMail), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(300, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterMail { get; set; }

        /// <summary>
        /// 郵便番号 (TRANSPORTER_ZIP)
        /// </summary>
        [Display(Name = nameof(Resources.TransporterResource.TransporterZip), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(10, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterZip { get; set; }

        /// <summary>
        /// 住所1 (TRANSPORTER_ADDRESS1)
        /// </summary>
        [Display(Name = nameof(Resources.TransporterResource.TransporterAddress1), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterAddress1 { get; set; }

        /// <summary>
        /// 住所2 (TRANSPORTER_ADDRESS2)
        /// </summary>
        [Display(Name = nameof(Resources.TransporterResource.TransporterAddress2), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransporterAddress2 { get; set; }

        /// <summary>
        /// 配送曜日 (TRANSPORT_WEEK_FLAGS)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.TransporterResource.TransportWeekFlags), ResourceType = typeof(Resources.TransporterResource))]
        [MaxLength(7, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string TransportWeekFlags { get; set; }

        /// <summary>
        /// 配送業者区分 (TRANSPORTER_CLASS)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.TransporterResource.TransporterClass), ResourceType = typeof(Resources.TransporterResource))]
        //[Range(1, 2, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int TransporterClass { get; set; }

        /// <summary>
        /// 送り状発行フラグ (INVOICE_PRINT_FLAG)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(Resources.TransporterResource.InvoicePrintFlag), ResourceType = typeof(Resources.TransporterResource))]
        public bool InvoicePrintFlag { get; set; }

        #endregion プロパティ
    }
}