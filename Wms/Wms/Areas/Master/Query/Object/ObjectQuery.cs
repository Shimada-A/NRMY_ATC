using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Common;
using Wms.Models;

namespace Wms.Areas.Master.Models
{
    public partial class Objects
    {
        public IEnumerable<Object> GetData(string shipperId)
        {
            return MvcDbContext.Current.Objects.Where(
                x => x.ShipperId == shipperId
                && x.CenterId == Profile.User.CenterId
                && x.DeleteFlag == false);
        }
    }
}