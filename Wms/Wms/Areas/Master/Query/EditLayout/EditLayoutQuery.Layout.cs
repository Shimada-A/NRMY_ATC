using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.ViewModels.EditLayout;

namespace Wms.Areas.Master.Query.EditLayout
{
    public partial class EditLayoutQuery
    {
        private static Layout CopyToLayout(EditLayoutBase data)
        {
            var layout = new Layout();
            layout.ShipperId = data.ShipperId;
            layout.CenterId = Common.Profile.User.CenterId;
            layout.TemplateId = data.TemplateId;
            layout.TemplateName = data.LayoutName;
            layout.ObjectType = data.ObjectType;
            layout.IoClass = (int)data.IoClass;
            layout.FileType = (int)data.FileType.GetValueOrDefault();
            layout.EncloseType = (int)data.EncloseType;
            layout.EncodeType = (int)data.EncodeType.GetValueOrDefault();
            layout.TitleClass = (int)data.HeadingRow;
            layout.ObjectId = data.ObjectId;

            return layout;
        }

        public static void RegistLayout(EditLayoutBase data)
        {
            var layout = CopyToLayout(data);
            layout.Insert();
        }

        public static void UpdateLayout(EditLayoutBase data)
        {
            var layout = CopyToLayout(data);
            layout.Update();
        }
    }
}