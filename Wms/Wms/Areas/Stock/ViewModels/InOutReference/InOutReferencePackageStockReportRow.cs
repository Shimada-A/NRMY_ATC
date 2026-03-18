using System;

namespace Wms.Areas.Stock.ViewModels.InOutReference
{
    public class InOutReferencePackageStockReportRow
    {
        private OperationClasses _operationClasses = OperationClasses.Create;

        public string CenterId { get; set; }

        public DateTime? MoveDate { get; set; }

        public long OperationId { get; set; }

        public string OperationName { get; set; }

        public string OperationClass
        {
            get
            {
                // 名称で返す
                return Share.Helpers.EnumHelper<OperationClasses>.GetDisplayValue(_operationClasses);
            }
            set => _operationClasses = (OperationClasses)Enum.ToObject(typeof(OperationClasses), short.Parse(value));
        }

        public string OperationUserId { get; set; }

        public string OperationUserName { get; set; }

        public int? UpdateCount { get; set; }

        public string LocationCode { get; set; }

        public string LocationClassName { get; set; }

        public string BoxNo { get; set; }

        public string CategoryName1 { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string ItemColorId { get; set; }

        public string ItemColorName { get; set; }

        public string ItemSizeId { get; set; }

        public string ItemSizeName { get; set; }

        public string Jan { get; set; }

        public string GradeName { get; set; }

        public string InvoiceNo { get; set; }

        public int StockQty { get; set; }

        public int AllocQty { get; set; }

        public string SortStatus { get; set; }

    }
}