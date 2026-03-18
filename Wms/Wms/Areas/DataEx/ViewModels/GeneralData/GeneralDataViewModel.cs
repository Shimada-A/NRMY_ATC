using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.Master.ViewModels.EditLayout;

namespace Wms.Areas.DataEx.ViewModels.GeneralData
{
    public class GeneralDataViewModel : EditCondition
    {
        /// <summary>
        /// テンプレートID(汎用データ出力、取込では表示上空となるデータもあるため用意)
        /// </summary>
        public long? GeneralDataTemplateId { get; set; }

        /// <summary>
        /// テンプレートID データと紐づいている物
        /// </summary>
        public long? GeneralDataTemplateIdRelatingData { get; set; }

        /// <summary>
        /// オブジェクト名(対象テーブル名)
        /// </summary>
        public string ObjectName { get; set; }

        /// <summary>
        /// 表示用レイアウトリスト
        /// </summary>
        public SelectList LayoutList { get; set; }
    }
}