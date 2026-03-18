using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Models;

namespace Wms.Areas.Master.Query.EditLayout
{
    public partial class EditLayoutQuery
    {
        public static (EditOutput output, EditCondition condition) GetData(string shipperId, long templateId)
        {
            var layout = Layout.GetLayout(shipperId, templateId);
            var layoutDetails = LayoutDetail.GetLayoutDetails(shipperId, templateId);
            var layoutConditions = GetLayoutConditions(shipperId, templateId);
            var output = new EditOutput(layout, layoutConditions, layoutDetails);
            
            var condition = new EditCondition(layout, layoutConditions);

            return (output, condition);
        }

        /// <summary>
        /// 取込の登録済みデータをテンプレートIDで取得する
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static EditImport GetImportData(string shipperId, long templateId)
        {
            // レイアウト本体を取得
            var layout = Layout.GetLayout(shipperId, templateId);

            // 詳細データを取得
            var layoutDetails = LayoutDetail.GetImportDataOfLayoutDetails(templateId);
            var import = new EditImport(layout, layoutDetails);


            return import;
        }

        public static void Confirm(EditLayoutBase layout, EditOutput editOutput, EditCondition editCondition)
        {
            using (var tran = MvcDbContext.Current.Database.BeginTransaction())
            {
                // Layoutテーブル更新
                long templateId;
                if (layout.IsNewLayout)
                {
                    templateId = Layout.GetMaxTemplateId(layout.ShipperId) + 1;
                    layout.TemplateId = templateId;
                    RegistLayout(layout);
                }
                else
                {
                    templateId = layout.TemplateId;
                    UpdateLayout(layout);
                }

                // LayoutDetailテーブル更新
                editOutput.EditOutputFileDetails
                    .ForEach(x =>
                    {
                        x.TemplateId = templateId;
                        if (layout.FileType == Common.FileType.Fixed)
                        {
                            x.PadClass = 1;
                        }
                        else
                        {
                            x.PadClass = 0;
                        }
                    });

                PhysicalDeleteLayoutDetail(editOutput);
                RegistLayoutDetail(editOutput);

                // LayoutConditionテーブル更新
                editCondition.Details.ForEach(x => x.TemplateId = templateId);
                if (layout.IsNewLayout)
                    RegistLayoutCondition(editCondition);
                else
                    UpdateLayoutCondition(editCondition);

                tran.Commit();
            }
        }

        /// <summary>
        /// データ削除
        /// </summary>
        /// <param name="templateId"></param>
        public static void Delete(long templateId)
        {
            using (var tran = MvcDbContext.Current.Database.BeginTransaction())
            {
                Layout.Delete(templateId);
                LayoutDetail.Delete(templateId);
                LayoutCondition.Delete(templateId);
                tran.Commit();
            }
        }
    }
}