namespace Wms.Areas.Stock.ViewModels.ImportInstruction
{
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Stock.Resources;
    using Wms.Common;

    /// <summary>
    /// Data to bind to view
    /// </summary>
    public class ImportInstructionSearchConditions
    {
        /// <summary>
        /// Data to sort
        /// </summary>
        public enum SortKey : byte
        {
            [Display(Name = nameof(ImportInstructionResource.SortInstructNo), ResourceType = typeof(ImportInstructionResource))]
            SortInstructNo,
            [Display(Name = nameof(ImportInstructionResource.SortInstructName_No), ResourceType = typeof(ImportInstructionResource))]
            SortInstructNameNo,
        }

        /// <summary>
        /// 昇順降順リスト
        /// </summary>
        public enum AscDescSort
        {
            [Display(Name = nameof(Share.Common.Resources.FormsResource.ASC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Asc,

            [Display(Name = nameof(Share.Common.Resources.FormsResource.DESC), ResourceType = typeof(Share.Common.Resources.FormsResource))]
            Desc
        }

        /// <summary>
        /// 仕分指示名称
        /// </summary>
        [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(ImportInstructionResource.SortInstructName), ResourceType = typeof(ImportInstructionResource))]
        public string SortInstructName { get; set; }

        /// <summary>
        /// Detail Sort key
        /// </summary>
        public SortKey Key { get; set; } = SortKey.SortInstructNo;

        /// <summary>
        /// Sort
        /// </summary>
        public AscDescSort Sort { get; set; }

        /// <summary>
        /// Search Type
        /// </summary>
        public SearchTypes SearchType { get; set; } = SearchTypes.Search;

        /// <summary>
        /// Page number
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Row on page
        /// </summary>
        public int PageSize { get; set; } = 1;

        /// <summary>
        /// センター
        /// </summary>
        public string CenterId { get; set; }

        /// <summary>
        /// ワークID (SEQ)
        /// </summary>
        public long Seq { get; set; }

        /// <summary>
        /// 取込ファイル名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 選択中数
        /// </summary>
        public int? SelectedCnt { get; set; } = 0;

        /// <summary>
        /// 選択中実績済数
        /// </summary>
        public int? ResultCnt { get; set; } = 0;

        public IList<SelectedImportInstructionViewModel> InstructionViewModels { get; set; }

        public string Ret { get; set; }

        public string Print { get; set; }
    }
}