using Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Common;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class EditImport : EditLayoutBase
    {
        public EditImport() { }

        public EditImport(Layout layout,List<ObjectDetailDTO> details)
            : base(layout)
        {
            TableInfos = details;
        }

        public List<EditImportFileInfo> FileInfos { get; set; }

        public List<ObjectDetailDTO> TableInfos { get; set; }

        public List<EditImportConstraint> Constraints { get; set; }

#pragma warning disable CA1819 // プロパティは配列を返すことはできません
        public HttpPostedFileBase[]  Files { get; set; }
#pragma warning restore CA1819 // プロパティは配列を返すことはできません
    }
}