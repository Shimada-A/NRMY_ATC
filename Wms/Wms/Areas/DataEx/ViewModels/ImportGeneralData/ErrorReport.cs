using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wms.Areas.DataEx.ViewModels.ImportGeneralData
{
    /// <summary>
    /// 汎用データ取込でのエラー情報
    /// </summary>
    public class ErrorReport
    {

        /// <summary>
        /// 行数
        /// </summary>
        public int RowNo { get; set; }

        /// <summary>
        /// エラー項目
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// エラー詳細
        /// </summary>
        public string Detail { get; set; }
    }
}