using Mvc.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Wms.Models;
using Wms.Areas.Stock.Resources;
using Share.Common.Resources;

namespace Wms.Areas.Stock.Models
{
    /// <summary>
    /// 在庫仕分指示ワーク01
    /// </summary>
    [Table("WW_SORT_STOCK_INSTRUCTS01")]
    public partial class SortStockInstructs01 : BaseModel
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
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "Seq", ResourceType = typeof(ImportInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long Seq { get; set; }

        /// <summary>
        /// 連番 (LINE_NO)
        /// </summary>
        [Key]
        [Column(Order = 100)]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "LineNo", ResourceType = typeof(ImportInstructionResource))]
        [Range(-999999999999999999, 999999999999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public long LineNo { get; set; }

        /// <summary>
        /// 行選択フラグ (IS_CHECK)
        /// </summary>
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = "IsCheck", ResourceType = typeof(ImportInstructionResource))]
        public bool IsCheck { get; set; }

        /// <summary>
        /// 倉庫ID (CENTER_ID)
        /// </summary>
        [Display(Name = "CenterId", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(40, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string CenterId { get; set; }

        /// <summary>
        /// 仕分指示No (SORT_INSTRUCT_NO)
        /// </summary>
        [Display(Name = "SortInstructNo", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortInstructNo { get; set; }

        /// <summary>
        /// 仕分指示名称 (SORT_INSTRUCT_NAME)
        /// </summary>
        [Display(Name = "SortInstructName", ResourceType = typeof(ImportInstructionResource))]
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        public string SortInstructName { get; set; }

        /// <summary>
        /// 仕分指示取込日時 (SORT_IMPORT_DATE)
        /// </summary>
        [Display(Name = "SortImportDate", ResourceType = typeof(ImportInstructionResource))]
        public DateTime? SortImportDate { get; set; }

        /// <summary>
        /// カテゴリー仕分振分No数 (CATEGORY_SORT_QTY)
        /// </summary>
        [Display(Name = "CategorySortQty", ResourceType = typeof(ImportInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? CategorySortQty { get; set; }

        /// <summary>
        /// SKU仕分振分No数 (SKU_SORT_QTY)
        /// </summary>
        [Display(Name = "SkuSortQty", ResourceType = typeof(ImportInstructionResource))]
        [Range(-999999999, 999999999, ErrorMessageResourceName = "Range", ErrorMessageResourceType = typeof(MessagesResource))]
        public int? SkuSortQty { get; set; }

        #endregion
    }
}
