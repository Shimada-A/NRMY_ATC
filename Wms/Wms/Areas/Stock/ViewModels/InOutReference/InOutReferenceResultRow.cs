using System;
using System.ComponentModel.DataAnnotations;
using Wms.Areas.Stock.Resources;

namespace Wms.Areas.Stock.ViewModels.InOutReference
{
    /// <summary>
    /// 操作区分
    /// </summary>
    public enum OperationClasses
    {
        [Display(Name = nameof(InOutReferenceResource.Create), ResourceType = typeof(InOutReferenceResource))]
        Create,
        [Display(Name = nameof(InOutReferenceResource.Update), ResourceType = typeof(InOutReferenceResource))]
        Update,
        [Display(Name = nameof(InOutReferenceResource.Delete), ResourceType = typeof(InOutReferenceResource))]
        Delete
    }

    /// <summary>
    /// 処理名称
    /// </summary>
    public enum OperationNames
    {
        [Display(Name = nameof(InOutReferenceResource.Create), ResourceType = typeof(InOutReferenceResource))]
        Create,
        [Display(Name = nameof(InOutReferenceResource.Update), ResourceType = typeof(InOutReferenceResource))]
        Update,
        [Display(Name = nameof(InOutReferenceResource.Delete), ResourceType = typeof(InOutReferenceResource))]
        Delete
    }

    public class InOutReferenceResultRow
    {
        public string CenterId { get; set; }

        public DateTime? MoveDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public long OperationId { get; set; }

        public string OperationName { get; set; }

        public string UpdateProgramName { get; set; }

        public OperationClasses OperationClass { get; set; }

        public string OperationClassName => Share.Helpers.EnumHelper<OperationClasses>.GetDisplayValue(OperationClass);

        public string OperationUserId { get; set; }

        public string OperationUserName { get; set; }

        public string LocationCode { get; set; }

        public string LocationClass { get; set; }

        public string LocationClassName { get; set; }

        public string BoxNo { get; set; }

        public string CategoryId1 { get; set; }

        public string CategoryName1 { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemColorId { get; set; }

        public string ItemColorName { get; set; }

        public string ItemSizeId { get; set; }

        public string ItemSizeName { get; set; }

        public string Jan { get; set; }

        public string GradeId { get; set; }

        public string GradeName { get; set; }

        public string InvoiceNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int? UpdateCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int StockQty { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int AllocQty { get; set; }

        public int? SortStatus { get; set; }
    }
}