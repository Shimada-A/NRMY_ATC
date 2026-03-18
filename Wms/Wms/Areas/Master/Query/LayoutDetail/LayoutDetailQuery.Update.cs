using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Models;
using Share.Extensions.Classes;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutDetail
    {
        public static void UpdateList(List<LayoutDetail> list)
        {
            list.ForEach(x => x.SetBaseInfoUpdate());
            MvcDbContext.OracleConnection.BulkExecute(UpdateListSql(),list);    
        }

        protected static string UpdateListSql()
        {
            return @"
                UPDATE M_LAYOUT_DETAILS 
                SET
                    UPDATE_DATE = SYSTIMESTAMP
                    , UPDATE_USER_ID = :UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME = :UPDATE_PROGRAM_NAME
                    , UPDATE_COUNT = UPDATE_COUNT + 1
                    , COLUMN_NO = :COLUMN_NO 
                    , OBJECT_ID = :OBJECT_ID
                    , COLUMN_ID = :COLUMN_ID
                    , DATA_TYPE = :DATA_TYPE
                    , TITLE_NAME = :TITLE_NAME
                    , DIGIT = :DIGIT
                    , PAD_CLASS = :PAD_CLASS
                    , PAD_DIRECTION = :PAD_DIRECTION
                    , PAD_CHAR = :PAD_CHAR
                    , START_POSITION = :START_POSITION
                    , END_POSITION = :END_POSITION
                    , UPDATE_CLASS = :UPDATE_CLASS
                    , FIXED_VALUE = :FIXED_VALUE 
                WHERE
                    TEMPLATE_ID = :TEMPLATE_ID 
                    and LINE_NO = :LINE_NO 
                    and SUB_NO = :SUB_NO 
                    and SHIPPER_ID = :SHIPPER_ID
                    AND M.CENTER_ID = :CENTER_ID
            ";
        }
    }
}