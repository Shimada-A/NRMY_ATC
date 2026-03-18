using Share.Extensions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Models;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutCondition
    {
        public static void UpdateList(List<LayoutCondition> list)
        {
            list.ForEach(x => x.SetBaseInfoUpdate());
            MvcDbContext.OracleConnection.BulkUpdate(list);
        }

        protected override string UpdateSql()
        {
            return @"
                UPDATE M_LAYOUT_CONDITIONS 
                SET
                    UPDATE_DATE = SYSTIMESTAMP
                    , UPDATE_USER_ID = :UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME = :UPDATE_PROGRAM_NAME
                    , UPDATE_COUNT = UPDATE_COUNT + 1
                    , CONDITION_CLASS = :CONDITION_CLASS
                    , CONDITION_VALUE_FROM = :CONDITION_VALUE_FROM
                    , CONDITION_VALUE_TO = :CONDITION_VALUE_TO
                    , SORT_ORDER = :SORT_ORDER
                    , SORT_DIRECTION = :SORT_DIRECTION 
                WHERE
                    TEMPLATE_ID = :TEMPLATE_ID 
                    and OBJECT_ID = :OBJECT_ID 
                    and COLUMN_ID = :COLUMN_ID 
                    and SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
            ";
        }
    }
}