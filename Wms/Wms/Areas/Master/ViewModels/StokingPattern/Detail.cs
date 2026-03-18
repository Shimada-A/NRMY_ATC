namespace Wms.Areas.Master.ViewModels.StockingPattern
{
    using Microsoft.Ajax.Utilities;
    using PagedList;
    using Share.Common.Resources;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Wms.Areas.Master.Resources;
    using Wms.Common;
    using Wms.Models;

    /// <summary>
    /// 仕分けパターン
    /// </summary>
    public class Detail : BaseModel
    {
        #region プロパティ

        /// <summary>
        /// 仕分けパターンID (PATTERN_ID)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StockingPatternResource.PatternId), ResourceType = typeof(StockingPatternResource))]
        [MaxLength(3, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PatternId { get; set; }

        /// <summary>
        /// 仕分けパターン名 (PATTERN_NAME)
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(MessagesResource.Required), ErrorMessageResourceType = typeof(MessagesResource))]
        [Display(Name = nameof(StockingPatternResource.PatternName), ResourceType = typeof(StockingPatternResource))]
        [MaxLength(100, ErrorMessageResourceName = nameof(MessagesResource.MaxLength), ErrorMessageResourceType = typeof(MessagesResource))]
        public string PatternName { get; set; }

        /// <summary>
        /// Checkbox Delete Checked
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 更新回数（排他制御用）
        /// </summary>
        public int UpdateCount { get; set; }

        /// <summary>
        /// 検索済み判断フラグ
        /// </summary>
        public bool SearchFlag { get; set; }
        public List<CategoryTableItem> Categories { get; set; }
        public Detail()
        {
            this.Categories = new List<CategoryTableItem>();
        }
        public string CategoryName1 { get; set; }
        #endregion プロパティ
    }
    /// <summary>
    /// 詳細画面のテーブルのカテゴリーID、カテゴリー名を取得
    /// </summary>
    public class CategoryTableItem
    {
        public string CategoryId1 { get; set; }
        public string CategoryName1 { get; set; }
        public byte StockingClass { get; set; }
        public int UpdateCount { get; set; }

    }

}