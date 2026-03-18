namespace Wms.Areas.Styleguide.Controllers
{
    using System.Web.Mvc;
    using Wms.Areas.Styleguide.ViewModels.FileUpload;

    /// <summary>
    /// ファイルアップロードコントローラー
    /// </summary>
    public class FileUploadController : Controller
    {
        // GET: Styleguide/FileUpload
        public ActionResult Index()
        {
            // 実画面を想定してViewModelを設定しています。
            var viewModel = new EditViewModel()
            {
                ItemId = "TEST-ITEM-ID",
            };

            return this.View(viewModel);
        }

        /// <summary>
        /// CSVアップロード
        /// </summary>
        /// <param name="viewModel">フォームデータ</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadCsv(EditViewModel viewModel)
        {
            if (!this.Request.IsAjaxRequest()) return new EmptyResult();

            for (int i = 0; i < this.Request.Files.Count; i++)
            {
                var file = this.Request.Files[i];

                // if (IsValidFile(file))
                // {
                //    // 何か処理
                // }
            }

            return this.Json(viewModel);
        }

        /// <summary>
        /// 画像アップロード
        /// </summary>
        /// <param name="viewModel">フォームデータ</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadImage(EditViewModel viewModel)
        {
            if (!this.Request.IsAjaxRequest()) return new EmptyResult();

            // S3アップロード
            for (int i = 0; i < this.Request.Files.Count; i++)
            {
                // var file = Request.Files[i];

                // if (IsValidFile(file))
                // {
                //    var shipper = new Shipper().GetShipperById(Common.Profile.User.ShipperId);
                //    string s3Key = GetS3Key(shipper, viewModel.ItemId);
                //    S3FileManager.Upload(AppConfig.BucketImageName, s3Key, file.InputStream, "image/jpeg");
                // }
            }

            return this.Json(viewModel);
        }
    }
}