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
        protected override string UpdateSql()
        {
            return @"
                UPDATE M_LAYOUTS 
                SET
                    UPDATE_DATE = SYSTIMESTAMP
                    , UPDATE_USER_ID = :UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME = :UPDATE_PROGRAM_NAME
                    , UPDATE_COUNT = UPDATE_COUNT + 1
                    , TEMPLATE_NAME = :TEMPLATE_NAME
                    , OBJECT_TYPE = :OBJECT_TYPE
                    , IO_CLASS = :IO_CLASS
                    , FILE_TYPE = :FILE_TYPE
                    , ENCODE_TYPE = :ENCODE_TYPE
                    , ENCLOSE_TYPE = :ENCLOSE_TYPE
                    , TITLE_CLASS = :TITLE_CLASS
                    , OBJECT_ID = :OBJECT_ID 
                WHERE
                    TEMPLATE_ID = :TEMPLATE_ID 
                    and SHIPPER_ID = :SHIPPER_ID
                    AND CENTER_ID = :CENTER_ID
            ";
        }

    }
}