using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Resources;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class Index : EditLayoutBase
    {

        public List<EditLayoutIndexResultRow> Results { get; set; } 
    }
}