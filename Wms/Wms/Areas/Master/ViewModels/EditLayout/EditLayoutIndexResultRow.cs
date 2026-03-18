using Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Common;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class EditLayoutIndexResultRow
    {
        public string  ShipperId { get; set; }

        public bool Checked { get; set; }

        public IoClass IoClass { get; set; }

        public string IoClassName { 
            get
            {
                return EnumHelperEx.GetDisplayValue(IoClass);
            }
        }

        public long TemplateId { get; set; }

        public string TemplateName { get; set; }
        public string MakeUserName { get; set; }

        public string LastUpdateUserName { get; set; }
    }
}