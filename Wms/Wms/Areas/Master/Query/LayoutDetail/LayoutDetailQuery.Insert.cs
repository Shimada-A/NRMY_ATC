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
        public static void InsertList(List<LayoutDetail> list)
        {
            list.ForEach(x => x.SetBaseInfoInsert());
            MvcDbContext.OracleConnection.BulkInsert(list);
        }

        protected override string InsertSql()
        {
            return @"
                INSERT 
                INTO M_LAYOUT_DETAILS( 
                    MAKE_USER_ID
                    , MAKE_PROGRAM_NAME
                    , UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME
                    , SHIPPER_ID
                    , CENTER_ID
                    , TEMPLATE_ID
                    , LINE_NO
                    , COLUMN_NO
                    , SUB_NO
                    , OBJECT_ID
                    , COLUMN_ID
                    , DATA_TYPE
                    , TITLE_NAME
                    , DIGIT
                    , PAD_CLASS
                    , PAD_DIRECTION
                    , PAD_CHAR
                    , START_POSITION
                    , END_POSITION
                    , UPDATE_CLASS
                    , FIXED_VALUE
                ) 
                VALUES ( 
                    :MAKE_USER_ID
                    , :MAKE_PROGRAM_NAME
                    , :UPDATE_USER_ID
                    , :UPDATE_PROGRAM_NAME
                    , :SHIPPER_ID
                    , :CENTER_ID
                    , :TEMPLATE_ID
                    , :LINE_NO
                    , :COLUMN_NO
                    , :SUB_NO
                    , :OBJECT_ID
                    , :COLUMN_ID
                    , :DATA_TYPE
                    , :TITLE_NAME
                    , :DIGIT
                    , :PAD_CLASS
                    , :PAD_DIRECTION
                    , :PAD_CHAR
                    , :START_POSITION
                    , :END_POSITION
                    , :UPDATE_CLASS
                    , :FIXED_VALUE
                )
                ";
        }
    }
}