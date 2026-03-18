using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Models;

namespace Wms.Areas.Master.Models
{
    public partial class Layout
    {
        protected override string InsertSql()
        {
            return @"
                INSERT 
                INTO M_LAYOUTS( 
                    MAKE_USER_ID
                    , MAKE_PROGRAM_NAME
                    , UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME
                    , SHIPPER_ID
                    , CENTER_ID
                    , TEMPLATE_ID
                    , TEMPLATE_NAME
                    , OBJECT_TYPE
                    , IO_CLASS
                    , FILE_TYPE
                    , ENCODE_TYPE
                    , ENCLOSE_TYPE
                    , TITLE_CLASS
                    , OBJECT_ID
                ) 
                VALUES ( 
                    :MAKE_USER_ID
                    , :MAKE_PROGRAM_NAME
                    , :UPDATE_USER_ID
                    , :UPDATE_PROGRAM_NAME
                    , :SHIPPER_ID
                    , :CENTER_ID
                    , :TEMPLATE_ID
                    , :TEMPLATE_NAME
                    , :OBJECT_TYPE
                    , :IO_CLASS
                    , :FILE_TYPE
                    , :ENCODE_TYPE
                    , :ENCLOSE_TYPE
                    , :TITLE_CLASS
                    , :OBJECT_ID
                )
            ";
        }
    }
}