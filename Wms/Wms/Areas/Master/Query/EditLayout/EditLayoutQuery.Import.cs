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
        public static void ConfirmImport(EditImport import)
        {
            using (var tran = MvcDbContext.Current.Database.BeginTransaction())
            {
                // Layoutテーブル更新
                long templateId;
                if (import.IsNewLayout)
                {
                    templateId = Layout.GetMaxTemplateId(import.ShipperId) + 1;
                    import.TemplateId = templateId;
                    RegistLayout(import);
                }
                else
                {
                    templateId = import.TemplateId;
                    UpdateLayout(import);
                }

                LayoutDetail.PhysicalDelete(import.TemplateId);
                RegistLayoutDetail(import);

                tran.Commit();
            }
        }

        /// <summary>
        /// LayoutDetail登録
        /// </summary>
        /// <param name="import"></param>
        public static void RegistLayoutDetail(EditImport import)
        {
            import.TableInfos.ForEach(x => x.TemplateId = import.TemplateId);
            var list = CopyToLayoutDetails(import.TableInfos);
            LayoutDetail.InsertList(list);
        }

        private static LayoutDetail CopyToLayoutDetail(ObjectDetailDTO dto,byte subNo,int columnNo)
        {
            var data = new LayoutDetail();
            data.ShipperId = dto.ShipperId;
            data.CenterId = Common.Profile.User.CenterId;
            data.TemplateId = dto.TemplateId;
            data.ColumnNo = columnNo;
            data.SubNo = subNo;
            data.ObjectId = dto.ObjectId;
            data.ColumnId = dto.ColumnId;
            data.DataType = (byte)dto.DataType;
            data.TitleName = dto.ColumnName;
            data.UpdateClass = (byte)dto.UpdateClass;
            data.FixedValue = dto.FixedValue;

            return data;
        }

        public static List<LayoutDetail> CopyToLayoutDetails(List<ObjectDetailDTO> dataList)
        {
            var list = new List<LayoutDetail>();
            int i = 1;
            foreach (var item in dataList)
            {
                var data = CopyToLayoutDetail(item,1,item.ColumnNoFirst.GetValueOrDefault());
                data.LineNo = i;
                list.Add(data);
                data = CopyToLayoutDetail(item, 2, item.ColumnNoSecond.GetValueOrDefault());
                data.LineNo = i;
                list.Add(data);
                i++;
            }

            return list;
        }


    }
}