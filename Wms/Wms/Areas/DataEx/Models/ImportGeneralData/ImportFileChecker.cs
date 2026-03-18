using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.DataEx.ViewModels.ImportGeneralData;

namespace Wms.Areas.DataEx.Models.ImportGeneralData
{
    public class ImportFileChecker
    {
        /// <summary>
        /// 入力ファイルのチェック
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static (ErrorReport report,List<ImportingObject>) Check(ImportGeneralDataViewModel vm)
        {

            return (null,null);
        }

    }
}