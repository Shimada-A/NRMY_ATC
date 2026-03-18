using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Models;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutDetail
    {
        public static void Delete(long templateId)
        {
            var param = CreateParamWith(new
            {
                TEMPLATE_ID = templateId
            });
            MvcDbContext.OracleConnection.Execute(DeleteSql(), param);
        }

        public static void PhysicalDelete(long templateId)
        {
            var param = CreateParamWith(new
            {
                TEMPLATE_ID = templateId
            });
            MvcDbContext.OracleConnection.Execute(PhysicalDeleteSql(), param);
        }

        private static string DeleteSql()
        {
            return @"
                UPDATE M_LAYOUT_DETAILS 
                SET
                    UPDATE_DATE = SYSTIMESTAMP
                    , UPDATE_USER_ID = :UPDATE_USER_ID
                    , UPDATE_PROGRAM_NAME = :UPDATE_PROGRAM_NAME
                    , UPDATE_COUNT = UPDATE_COUNT + 1 
                    , DELETE_FLAG = 1 
                WHERE
                    SHIPPER_ID = :SHIPPER_ID 
                    AND CENTER_ID = :CENTER_ID
                    AND TEMPLATE_ID = :TEMPLATE_ID
            ";
        }

        private static string PhysicalDeleteSql()
        {
            return @"
                DELETE FROM M_LAYOUT_DETAILS 
                WHERE
                    SHIPPER_ID = :SHIPPER_ID 
                    AND CENTER_ID = :CENTER_ID
                    AND TEMPLATE_ID = :TEMPLATE_ID
            ";
        }
    }
}