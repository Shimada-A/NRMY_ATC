using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Models;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutCondition
    {
        protected override DynamicParameters CreateParam()
        {
            var param = base.CreateParam();
            param.AddDynamicParams(new
            {
                SHIPPER_ID = ShipperId,
                CENTER_ID = CenterId,
                TEMPLATE_ID = TemplateId,
                OBJECT_ID = ObjectId,
                COLUMN_ID =ColumnId,
                CONDITION_CLASS = ConditionClass,
                CONDITION_VALUE_FROM = ConditionValueFrom,
                CONDITION_VALUE_TO = ConditionValueTo,
                SORT_ORDER = SortOrder,
                SORT_DIRECTION =  SortDirection
                });
            return param;
        }


    }
}