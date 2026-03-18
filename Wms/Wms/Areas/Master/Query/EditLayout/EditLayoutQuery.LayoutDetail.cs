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

        private static LayoutDetail CopyToLayoutDetail(EditOutputFileDetail dto)
        {
            var data = new LayoutDetail();
            data.ShipperId = dto.ShipperId;
            data.CenterId = Common.Profile.User.CenterId;
            data.TemplateId = dto.TemplateId;
            data.ColumnNo = (int)dto.ColumnNo;
            data.SubNo = (byte)dto.SubNo;
            data.ObjectId = dto.ObjectId;
            data.ColumnId = dto.ColumnId;
            data.DataType = (byte)dto.DataType;
            data.TitleName = dto.ColumnName;
            data.Digit = dto.Digit;
            // 固定値関連はファイル形式=固定値の時にセット
            data.PadClass = dto.PadClass;
            data.PadDirection = (byte)dto.PadDirection;
            data.PadChar = dto.PadValue;

            return data;    
        }

        public static List<LayoutDetail> CopyToLayoutDetails(List<EditOutputFileDetail> dataList)
        {
            var list = new List<LayoutDetail>();
            int i = 1;
            foreach(var item in dataList.Where(x => x.ColumnNo.HasValue))
            {
                var data = CopyToLayoutDetail(item);
                data.LineNo = i;
                list.Add(data);
                i++;
            }

            return list;
        }

        public static void PhysicalDeleteLayoutDetail(EditOutput edit)
        {
            LayoutDetail.PhysicalDelete(edit.TemplateId);
        }

        public static void RegistLayoutDetail(EditOutput edit)
        {
            var list = CopyToLayoutDetails(edit.EditOutputFileDetails);
            LayoutDetail.InsertList(list);
        }

        public static void UpdateLayoutDetail(EditOutput edit)
        {
            var list = CopyToLayoutDetails(edit.EditOutputFileDetails.Where(x => !x.IsNewData).ToList());
            LayoutDetail.UpdateList(list);
        }
    }
}