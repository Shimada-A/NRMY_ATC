using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.DataEx.ViewModels.GeneralData;

namespace Wms.Areas.DataEx.ViewModels.ImportGeneralData
{
    /// <summary>
    /// 汎用データ取込ViewModel
    /// </summary>
    public class ImportGeneralDataViewModel : GeneralDataViewModel
    {
        /// <summary>
        /// 処理件数
        /// </summary>
        public int ImportCount { get; set; }

        /// <summary>
        /// 異常終了件数
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        ///  正常終了件数
        /// </summary>
        public int SuccessCount { get { return ImportCount - ErrorCount; } }

        /// <summary>
        /// エラーの情報リスト
        /// </summary>
        public List<ErrorReport> ErrorReports { get; set; }

#pragma warning disable CA1819 // プロパティは配列を返すことはできません
        public HttpPostedFileBase[] Files { get; set; }
#pragma warning restore CA1819 // プロパティは配列を返すことはできません
    }
}