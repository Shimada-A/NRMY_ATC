using Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Models;

namespace Wms.Areas.Master.Models
{
    public partial class ObjectDetail
    {
        public static IEnumerable<ObjectDetailDTO> GetData(string shipperId, string objectId)
        {
            return MvcDbContext.Current.ObjectDetail.Where(
                x => x.ShipperId == shipperId
                && x.CenterId == Profile.User.CenterId
                && x.ObjectId == objectId
                && x.DeleteFlag == false).OrderBy(x => x.ColumnNo)
                .Select(x=> new ObjectDetailDTO() { 
                    ShipperId = x.ShipperId,
                    CenterId = x.CenterId,
                    ColumnNo = x.ColumnNo,
                    ColumnId = x.ColumnId,
                    ColumnName = x.ColumnName,
                    ObjectId = x.ObjectId,
                    DataType = x.DataType,
                    DigitInt = x.DigitInt,
                    PrimaryFlag = x.PrimaryFlag,
                    RequiredFlag = x.RequiredFlag
                });
            
        }
    }
}