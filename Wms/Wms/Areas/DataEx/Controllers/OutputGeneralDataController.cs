using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Wms.Areas.DataEx.Models.OutputGeneralData;
using Wms.Areas.DataEx.ViewModels.GeneralData;
using Wms.Areas.DataEx.ViewModels.OutputGeneralData;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Query.EditLayout;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Controllers;

namespace Wms.Areas.DataEx.Controllers
{
    public partial class OutputGeneralDataController : BaseController
    {
        // GET: DataEx/OuyputGeneralData
        public ActionResult Index(OutputGeneralDataViewModel vm)
        {
            return GetView("Index",vm);
        }

        /// <summary>
        /// OutputGeneralDataのビュー取得
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        private ActionResult GetView(string viewName, OutputGeneralDataViewModel vm)
        {
            ModelState.Clear();
            if (Session[nameof(OutputGeneralDataViewModel)] == null)
            {
                Session[nameof(OutputGeneralDataViewModel)] = vm;
            }

            UpdateViewModel(vm);

            // データとヘッダーのテンプレートIdが一致していない時
            if (vm.GeneralDataTemplateIdRelatingData != vm.GeneralDataTemplateId)
            {
                vm.GeneralDataTemplateId = vm.GeneralDataTemplateIdRelatingData;
            }

            var layouts = Layout.GetLayouts(Common.Profile.User.ShipperId, IoClass.Output, "");
            vm.LayoutList = new SelectList(layouts, "TemplateId", "TemplateName");

            return View(viewName, vm);
        }

        /// <summary>
        /// ViewModelの更新
        /// </summary>
        /// <param name="vm"></param>
        private void UpdateViewModel(OutputGeneralDataViewModel vm)
        {
            var saveData = Session[nameof(OutputGeneralDataViewModel)] as OutputGeneralDataViewModel;
            saveData.Details = vm.Details;
        }

        /// <summary>
        /// レイアウト情報ビュー取得処理
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public PartialViewResult GetLayoutInfo(long templateId)
        {
            // レイアウト情報取得してViewModelのプロパティ更新、部分ビューを返却
            var layout = Layout.GetLayout(Common.Profile.User.ShipperId, templateId);
            var vm = Session[nameof(OutputGeneralDataViewModel)] as OutputGeneralDataViewModel;
            vm.GeneralDataTemplateId = layout.TemplateId;
            vm.GeneralDataTemplateIdRelatingData = layout.TemplateId;
            vm.HeadingRow = (HeadingRow)layout.TitleClass;
            vm.EncodeType = (EncodeType)layout.EncodeType;
            vm.EncloseType = (EncloseType)layout.EncloseType;
            vm.FileType = (FileType)layout.FileType;

            return PartialView("~/Areas/DataEx/Views/GeneralData/_OutputGeneralDataHeader.cshtml", vm);

        }

        /// <summary>
        /// 条件設定のオブジェクト明細取得処理
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult GetOutputGeneralDataConditionObjectDetail(long templateId)
        {
            return GetDetailView(templateId, "~/Areas/Master/Views/EditLayout/_EditConditionDetail.cshtml");
        }

        /// <summary>
        /// 詳細用部分ビュー取得処理
        /// </summary>
        /// <param name="templateId"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        private PartialViewResult GetDetailView(long templateId,string viewName)
        {
            // 状態を更新
            UpdateObjectDetails(templateId);
            return PartialView(viewName, Session[nameof(OutputGeneralDataViewModel)]);
        }

        /// <summary>
        /// 各ViewModelのオブジェクト詳細一覧更新
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        private void UpdateObjectDetails(long templateId)
        {
            // オブジェクト明細一覧を取得する時は、新規の場合なので、
            // 条件設定の方の一覧もインスタンスを作成しておく
            var condition = Session[nameof(OutputGeneralDataViewModel)] as OutputGeneralDataViewModel;

            // オブジェクトIDが同じ場合は更新不要
            if (templateId == condition.GeneralDataTemplateId)
            {
                condition.GeneralDataTemplateId = templateId;
            }

            // オブジェクトIDが異なる場合は
            // オブジェクト一覧を、出力設定ViewModel、条件設定ViewModel共に更新する必要がある
            condition.Details = GetLayoutConditions(templateId);
        }


        /// <summary>
        ///  オブジェクト明細リスト取得
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        private static List<ObjectDetailDTO> GetLayoutConditions(long templateId)
        {
            var objectDetails = EditLayoutQuery.GetLayoutConditions(Common.Profile.User.ShipperId, templateId).ToList();
            return objectDetails;
        }

        /// <summary>
        /// ファイル出力
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ExportCsv(OutputGeneralDataViewModel vm)
        {
            // 出力用データ取得
            var (layoutName,encoding,fileType ,data) = OutputLayoutQuery.GetCsvData(vm.GeneralDataTemplateId, vm.Details);

            // 出力用データをレスポンス用のストリームに書き込む
            var stream = new MemoryStream();
            using (TextWriter streamWriter = new StreamWriter(stream, encoding))
            {
                foreach(var item in data)
                    streamWriter.WriteLine(item);
                streamWriter.Flush();
            }
               
            // ファイル名作成し、レスポンス
            var filename = $"{layoutName}{DateTime.Now.ToString("yyyyMMddhhmmss")}.{(fileType  == FileType.CSV ? "csv" : "tsv")}";
            return File(stream.ToArray(), MimeMapping.GetMimeMapping(filename), filename) ;
        }
    }
}