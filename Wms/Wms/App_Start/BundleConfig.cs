namespace Wms
{
    using System;
    using System.Configuration;
    using System.Web.Optimization;

    public static class BundleConfig
    {
        private class MyBundleTransform : IBundleTransform
        {
            private readonly string _extension;

            private readonly string _version;

            public MyBundleTransform(string extension, string version)
            {
                _extension = extension;
                _version = version;
            }

            public void Process(BundleContext context, BundleResponse response)
            {
                foreach (var file in response.Files)
                {
                    if (file.IncludedVirtualPath.EndsWith(_extension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        file.IncludedVirtualPath = string.Format("{0}?ver={1}", file.IncludedVirtualPath, _version);
                    }
                }
            }
        }

        // バンドルの詳細については、https://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            if (bundles == null)
            {
                return;
            }

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/BootstrapJs").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(CreateCssBundle(
                "~/bundles/css",
                "~/Content/Calendar.css",
                "~/Content/site.css",
                "~/Content/Glimpse.css",
                "~/Content/bootstrap.min.css",
                "~/Content/css/jquery-ui.min.css",
                "~/Content/css/materialdesignicons.min.css",
                "~/Content/css/NumberRange.css",
                "~/Content/css/materialdesignicons.min.css",
                "~/Content/css/select2.min.css",
                "~/Content/main.css"));

            bundles.Add(new ScriptBundle("~/bundles/SiteJs").Include(
                      "~/Scripts/My/app.js",
                      "~/Scripts/My/jquery-ui.min.js",
                      "~/Scripts/My/Calendar.js",
                      "~/Scripts/My/Calendar.ja.js",
                      "~/Scripts/My/CalendarSetting.js",
                      "~/Scripts/My/DecimalChkValidation.js",
                      "~/Scripts/My/ValidateDate.js",
                      "~/Scripts/My/IsDateValidation.js",
                      "~/Scripts/My/IsIntegerValidation.js",
                      "~/Scripts/My/IsTimeValidation.js",
                      "~/Scripts/My/NumberRange.min.js",
                      "~/Scripts/My/NumberRangeSetting.js",
                      "~/Scripts/My/BrandSearchModal.js",
                      "~/Scripts/My/ColorSearchModal.js",
                      "~/Scripts/My/SizeSearchModal.js",
                      "~/Scripts/My/VendorSearchModal.js",
                      "~/Scripts/My/DeliareaGroupSearchModal.js",
                      "~/Scripts/My/ProgerssManager.js",
                      "~/Scripts/My/SyukkasakiTransporterSearchModal.js",
                      "~/Scripts/My/TransporterSearchModal.js",
                      "~/Scripts/jquery.signalR-2.4.1.min.js",
                      "~/Scripts/CreateFromLib/cwebclient.min.js"));

            // ファイルアップロードに必要なJS,CSS
            bundles.Add(CreateCssBundle(
                "~/bundles/FileUploadCss",
                "~/Scripts/dropzone/dropzone.min.css",
                "~/Content/css/FileUploadDialog.css"));

            bundles.Add(new ScriptBundle("~/bundles/FileUpload").Include(
                "~/Scripts/dropzone/dropzone.min.js",
                "~/Scripts/My/FileUploadModal.js"));

            // jQueryAddon
            bundles.Add(new ScriptBundle("~/bundles/jQueryAddon").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/jquery.unobtrusive-ajax*",
                "~/Scripts/select2.min.js"));

            // foolproof
            bundles.Add(new ScriptBundle("~/bundles/foolproof").Include(
                       "~/Scripts/My/mvcfoolproof.unobtrusive*",
                       "~/Scripts/My/MvcFoolproofJQueryValidation*"));

            bundles.Add(new ScriptBundle("~/bundles/mvcfoolproof").Include(
                       "~/Scripts/mvcfoolproof.unobtrusive.min.js",
                       "~/Scripts/MvcFoolproofJQueryValidation.min.js"));
        }

        private static Bundle CreateCssBundle(string virtualPath, params string[] virtualPathList)
        {
            const string KeyVersionCss = "VersionCss";
            const string Extension = ".css";
            var version = ConfigurationManager.AppSettings.Get(KeyVersionCss);
            var ret = new StyleBundle(virtualPath).Include(virtualPathList);

            ret.Transforms.Add(new MyBundleTransform(Extension, version));

            return ret;
        }
    }
}