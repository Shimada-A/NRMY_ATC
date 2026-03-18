using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Models;

namespace Wms.Areas.Master.Models
{
    public partial class LayoutDetail
    {

        public static List<LayoutDetail> GetLayoutDetails(string shipperId , long templateId)
        {
            var data = MvcDbContext.Current.LayoutDetail.Where(
                x => x.ShipperId == shipperId
                && x.CenterId == Profile.User.CenterId
                && x.TemplateId == templateId
                && x.DeleteFlag == 0).OrderBy(x => x.LineNo).ToList();
            return data;
        }

        public static List<ObjectDetailDTO> GetImportDataOfLayoutDetails(long templateId)
        {
            var param = CreateParamWith(new { TEMPLATE_ID = templateId });
            return MvcDbContext.OracleConnection.Query<ObjectDetailDTO>(GetImportDataOfLayoutDetailsSql(),param).ToList();
        }

        private static string GetImportDataOfLayoutDetailsSql()
        {
            return @"
                SELECT
                    0 IS_NEW
                    , M.COLUMN_ID
                    , M.SHIPPER_ID
                    , M.CENTER_ID
                    , M.TEMPLATE_ID
                    , M.OBJECT_ID
                    , M.DATA_TYPE
                    , O.COLUMN_NAME
                    , O.DIGIT_INT
                    , M.IMPORT_CLASS
                    , M.FIXED_VALUE
                    , CASE M.COLUMN_NO WHEN 0 THEN NULL ELSE M.COLUMN_NO END COLUMN_NO_FIRST
                    , CASE M2.COLUMN_NO  WHEN 0 THEN NULL ELSE M2.COLUMN_NO END  COLUMN_NO_SECOND
                    , O.PRIMARY_FLAG
                    , O.REQUIRED_FLAG
                    , M.UPDATE_CLASS 
                FROM
                    M_LAYOUT_DETAILS M
                    INNER JOIN M_LAYOUT_DETAILS M2
                        ON M2.SHIPPER_ID = M.SHIPPER_ID
                        AND M2.CENTER_ID = M.CENTER_ID
                        AND M2.TEMPLATE_ID = M.TEMPLATE_ID
                        AND M2.LINE_NO = M.LINE_NO
                        AND M2.SUB_NO = 2
                    LEFT JOIN M_OBJECT_DETAILS O 
                        ON O.SHIPPER_ID = M.SHIPPER_ID 
                        AND O.CENTER_ID = M.CENTER_ID
                        AND O.OBJECT_ID = M.OBJECT_ID 
                        AND O.COLUMN_ID = M.COLUMN_ID 
                WHERE
                    M.SHIPPER_ID = :SHIPPER_ID 
                    AND M.CENTER_ID = :CENTER_ID
                    AND M.TEMPLATE_ID = :TEMPLATE_ID
                    AND M.SUB_NO = 1
                ORDER BY
                    M.LINE_NO
                    , M.SUB_NO


            ";
        }

        protected override DynamicParameters CreateParam()
        {
            var param = base.CreateParam();
            param.AddDynamicParams(new
            {
                SHIPPER_ID = ShipperId,
                CENTER_ID = CenterId,
                TEMPLATE_ID = TemplateId,
                LINE_NO = LineNo,
                COLUMN_NO = ColumnNo,
                SUB_NO = SubNo,
                OBJECT_ID = ObjectId,
                COLUMN_ID = ColumnId,
                DATA_TYPE = DataType,
                TITLE_NAME = TitleName,
                DIGIT = Digit,
                PAD_CLASS = PadClass,
                PAD_DIRECTION = PadDirection,
                PAD_CHAR =PadChar,
                START_POSITION = StartPosition,
                END_POSITION = EndPosition,
                UPDATE_CLASS = UpdateClass,
                FIXED_VALUE =  FixedValue,
            });
            return param;
        }

    }
}