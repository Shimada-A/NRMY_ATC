using Dapper;
using Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Models;

namespace Wms.Areas.Master.Query.EditLayout
{
    public partial class EditLayoutQuery
    {

        public static List<ObjectDetailDTO> GetLayoutConditions(string shipperId, long templateId)
        {
            var param = new DynamicParameters(new
            {
                SHIPPER_ID = shipperId,
                CENTER_ID = Common.Profile.User.CenterId,
                TEMPLATE_ID = templateId
            });

            var data =  MvcDbContext.OracleConnection
                .Query<ObjectDetailDTO>(GetLayoutConditionsSql(), param).ToList();

            data.ForEach(x => x.DataTypeName = EnumHelperEx.GetDisplayValue(x.DataType));
            return data;
        }

        private static string GetLayoutConditionsSql()
        {
            return @"
                --*DataTitle 'FLAGSHIP_DEV.M_LAYOUT_CONDITIONS'
                --*CaptionFromComment
                SELECT
                    0 As IS_NEW_DATA
                    , M.SHIPPER_ID                                -- SHIPPER_ID
                    , M.CENTER_ID                                -- CENTER_ID
                    , M.TEMPLATE_ID                             -- TEMPLATE_ID
                    , M.OBJECT_ID                               -- OBJECT_ID
                    , M.COLUMN_ID                               -- COLUMN_ID
                    , O.COLUMN_NO
                    , O.COLUMN_NAME
                    , O.DATA_TYPE
                    , O.DIGIT_INT
                    , O.DIGIT_DEC
                    , M.CONDITION_CLASS                         -- CONDITION_CLASS
                    , M.CONDITION_VALUE_FROM                    -- CONDITION_VALUE_FROM
                    , M.CONDITION_VALUE_TO                      -- CONDITION_VALUE_TO
                    , M.SORT_ORDER                              -- SORT_ORDER
                    , M.SORT_DIRECTION                          -- SORT_DIRECTION
                FROM
                    M_LAYOUT_CONDITIONS M 
                    LEFT JOIN M_OBJECT_DETAILS O 
                        ON M.SHIPPER_ID = O.SHIPPER_ID 
                        AND M.CENTER_ID = O.CENTER_ID
                        AND M.OBJECT_ID = O.OBJECT_ID 
                        AND M.COLUMN_ID = O.COLUMN_ID 
                WHERE
                    M.TEMPLATE_ID = :TEMPLATE_ID 
                    and M.SHIPPER_ID = :SHIPPER_ID 
                    AND M.CENTER_ID = :CENTER_ID
                ORDER BY
                    O.COLUMN_NO
                    , M.TEMPLATE_ID
                    , M.OBJECT_ID
                    , M.COLUMN_ID
                    , M.SHIPPER_ID
            ";
        }

        private static LayoutCondition CopyToLayoutCondition(ObjectDetailDTO dto)
        {
            var data = new LayoutCondition();
            data.ShipperId = dto.ShipperId;
            data.CenterId = Common.Profile.User.CenterId;
            data.TemplateId = dto.TemplateId;
            data.ObjectId = dto.ObjectId;
            data.ColumnId = dto.ColumnId;
            data.ConditionClass = (byte)dto.ConditionClass;
            data.ConditionValueFrom = dto.ConditionValueFrom;
            data.ConditionValueTo = dto.ConditionValueTo;
            data.SortOrder = dto.SortOrder;
            data.SortDirection = (byte)dto.SortDirection;
            return data;
        }

        public static List<LayoutCondition> CopyToLayoutConditions(List<ObjectDetailDTO> dataList)
        {
            var list = new List<LayoutCondition>();
            foreach(var item in dataList)
            {
                list.Add(CopyToLayoutCondition(item));
            }

            return list;
        }

        public static void RegistLayoutCondition(EditCondition edit)
        {
            var list = CopyToLayoutConditions(edit.Details);
            LayoutCondition.InsertList(list);
        }

        public static void UpdateLayoutCondition(EditCondition edit)
        {
            var list = CopyToLayoutConditions(edit.Details);
            LayoutCondition.UpdateList(list);
        }

    }
}