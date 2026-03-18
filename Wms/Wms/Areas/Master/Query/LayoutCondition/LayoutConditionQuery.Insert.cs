using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Models;
using Share.Extensions.Classes;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutCondition
    {
        public static void InsertList(List<LayoutCondition> list) 
        {
            list.ForEach(x => x.SetBaseInfoInsert());
            MvcDbContext.OracleConnection.BulkInsert(list);
        }

        protected override string InsertSql()
        {
            return @"
                INSERT 
                INTO M_LAYOUT_CONDITIONS( 
                    MAKE_USER_ID
                    , MAKE_PROGRAM_NAME
                    , UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME
                    , SHIPPER_ID
                    , CENTER_ID
                    , TEMPLATE_ID
                    , OBJECT_ID
                    , COLUMN_ID
                    , CONDITION_CLASS
                    , CONDITION_VALUE_FROM
                    , CONDITION_VALUE_TO
                    , SORT_ORDER
                    , SORT_DIRECTION
                ) 
                VALUES ( 
                    :MAKE_USER_ID
                    , :MAKE_PROGRAM_NAME
                    , :UPDATE_USER_ID
                    , :UPDATE_PROGRAM_NAME
                    , :SHIPPER_ID
                    , :CENTER_ID
                    , :TEMPLATE_ID
                    , :OBJECT_ID
                    , :COLUMN_ID
                    , :CONDITION_CLASS
                    , :CONDITION_VALUE_FROM
                    , :CONDITION_VALUE_TO
                    , :SORT_ORDER
                    , :SORT_DIRECTION
                )
            ";
        }
    }
}