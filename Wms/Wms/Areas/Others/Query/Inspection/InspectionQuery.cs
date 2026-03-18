namespace Wms.Areas.Others.Query.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Dapper;
    using PagedList;
    using Share.Extensions.Classes;
    using Share.Helpers;
    using Wms.Areas.Others.Models;
    using Wms.Areas.Others.ViewModels.Inspection;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;

    public class InspectionQuery
    {
        public bool InsertInspection(InspectionSearchConditions condition)
        {
            return true;
        }


        public IPagedList<InspectionResultRow> InspectionGetData(InspectionSearchConditions condition)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder query = new StringBuilder();
            query.Append(@"
                SELECT
                       *
                  FROM L_INSPECTION_LOG
            ");
            parameters.Add(":SHIPPER_ID", Profile.User.ShipperId);
            return null;
        }
    }
}