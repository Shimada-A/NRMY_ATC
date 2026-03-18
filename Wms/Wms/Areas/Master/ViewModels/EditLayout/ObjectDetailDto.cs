using Share.Common.Resources;
using Share.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Resources;
using Wms.Common;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public partial class ObjectDetailDTO
    {
        public ObjectDetailDTO()
        {

        }

        public ObjectDetailDTO(LayoutCondition condition)
        {
            IsNew = false;
            ShipperId = condition.ShipperId;
            CenterId = Common.Profile.User.CenterId;
            TemplateId = condition.TemplateId;
            ObjectId = condition.ObjectId;
            ColumnId = condition.ColumnId;
            ConditionClass = (ConditionClass)condition.ConditionClass;
            ConditionValueFrom = condition.ConditionValueFrom;
            ConditionValueTo = condition.ConditionValueTo;
            SortOrder = condition.SortOrder;
            SortDirection = (SortDirection)condition.SortDirection;
        }
        public ObjectDetailDTO(LayoutDetail detail)
        {
            IsNew = false;
            ShipperId = detail.ShipperId;
            CenterId = Common.Profile.User.CenterId;
            TemplateId = detail.TemplateId;
            ObjectId = detail.ObjectId;
            ColumnId = detail.ColumnId;
            UpdateClass = (UpdateClass)detail.UpdateClass.GetValueOrDefault();
            ImportClass = (ImportClass)detail.ImportClass.GetValueOrDefault();
            FixedValue = detail.FixedValue;
        }

        public bool IsNew { get; set; }

        public long TemplateId { get; set; }

        /// <summary>
        /// 荷主ID (SHIPPER_ID)
        /// </summary>
        [Key]
        [Column(Order = 102)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ShipperId), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ShipperId { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        /// <remarks>
        /// センターコード　0905,0924,0942
        /// </remarks>
        [Key]
        [Column(Order = 13)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(DeliareaGroupResource.CenterId), ResourceType = typeof(DeliareaGroupResource))]
        [MaxLength(20, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// オブジェクトID (OBJECT_ID)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ObjectId), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ObjectId { get; set; }

        /// <summary>
        /// カラムID (COLUMN_ID)
        /// </summary>
        [Key]
        [Column(Order = 101)]
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ColumnId), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ColumnId { get; set; }

        /// <summary>
        /// カラム名 (COLUMN_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ColumnName), ResourceType = typeof(ObjectDetailResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string ColumnName { get; set; }

        /// <summary>
        /// カラム№ (COLUMN_NO)
        /// </summary>
        /// <remarks>
        /// 連番
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.ColumnNo), ResourceType = typeof(ObjectDetailResource))]
        [Range(-9999, 9999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int ColumnNo { get; set; }

        private Common.DataType _dataType;

        /// <summary>
        /// データ型 (DATA_TYPE)
        /// </summary>
        /// <remarks>
        /// 0:文字列　1:数値
        /// </remarks>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ObjectDetailResource.DataType), ResourceType = typeof(ObjectDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public Common.DataType DataType {
            get {
                return _dataType;
            }
            set
            {
                _dataType = value;
                DataTypeName = EnumHelperEx.GetDisplayValue(_dataType);
            }
        }

        /// <summary>
        /// 桁数 (DIGIT_INT)
        /// </summary>
        [Display(Name = nameof(ObjectDetailResource.DigitInt), ResourceType = typeof(ObjectDetailResource))]
        [Range(-99999, 99999, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public int? DigitInt { get; set; }

        /// <summary>
        /// 小数点以下桁数 (DIGIT_DEC)
        /// </summary>
        [Display(Name = nameof(ObjectDetailResource.DigitDec), ResourceType = typeof(ObjectDetailResource))]
        [Range(-99, 99, ErrorMessageResourceName = nameof(MessagesResource.Range), ErrorMessageResourceType = typeof(MessagesResource))]
        public byte? DigitDec { get; set; }

        /// <summary>
        /// 使用フラグ (USE_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:利用不可　1:利用可
        /// </remarks>
        [Display(Name = nameof(ObjectDetailResource.UseFlag), ResourceType = typeof(ObjectDetailResource))]
        public bool? UseFlag { get; set; }

        /// <summary>
        /// プライマリキーフラグ (PRIMARY_FLAG)
        /// </summary>
        [Display(Name = nameof(ObjectDetailResource.PrimaryFlag), ResourceType = typeof(ObjectDetailResource))]
        public bool? PrimaryFlag { get; set; }

        /// <summary>
        /// 必須フラグ (REQUIRED_FLAG)
        /// </summary>
        /// <remarks>
        /// 0:任意項目　1:必須項目
        /// </remarks>
        [Display(Name = nameof(ObjectDetailResource.RequiredFlag), ResourceType = typeof(ObjectDetailResource))]
        public bool? RequiredFlag { get; set; }

    }
}