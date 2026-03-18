using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Share.Extensions.Classes;
using Wms.Common;
using Wms.Models;
using Dapper;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Areas.DataEx.ViewModels.GeneralData;

namespace Wms.Areas.Master.Models
{
    public partial class Layout
    {
        public static long GetMaxTemplateId(string shipperId)
        {
            var data = MvcDbContext.Current.Layout
                .Where(x => x.ShipperId == shipperId
                                && x.CenterId == Profile.User.CenterId);
            if (!data.Any() )
                return 0;
            else
                return data.Max(x => x.TemplateId);
        }

        public static Layout GetLayout(string shipperId,IoClass ioClass,string templateName)
        {
            return MvcDbContext.Current.Layout.FirstOrDefault(
                x => x.ShipperId == shipperId 
                && x.IoClass == (int)ioClass 
                && x.TemplateName.Contains(templateName)
                && x.DeleteFlag == false
                );
        }

        public static List<EditLayoutIndexResultRow> GetLayouts(string shipperId, IoClass ioClass, string templateName)
        {
            var param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                SHIPPER_ID = shipperId,
                CENTER_ID = Profile.User.CenterId,
                IO_CLASS = ioClass,
                TEMPLATE_NAME = templateName
            });
            return MvcDbContext.OracleConnection.Query<EditLayoutIndexResultRow>(GetLayoutsSql(), param).ToList();
        }

        private static string GetLayoutsSql()
        {
            return @"
                --*DataTitle 'FLAGSHIP_DEV.M_LAYOUTS'
                --*CaptionFromComment
                SELECT
                    M.MAKE_USER_ID                                -- MAKE_USER_ID
                    , U.USER_NAME AS MAKE_USER_NAME
                    , M.UPDATE_USER_ID                            -- UPDATE_USER_ID
                    , U2.USER_NAME AS LAST_UPDATE_USER_NAME
                    , M.SHIPPER_ID                                -- SHIPPER_ID
                    , M.CENTER_ID
                    , M.TEMPLATE_ID                               -- TEMPLATE_ID
                    , M.TEMPLATE_NAME                             -- TEMPLATE_NAME
                    , M.IO_CLASS                                  -- IO_CLASS
                FROM
                    M_LAYOUTS M
                    LEFT JOIN M_USERS U
                    ON M.SHIPPER_ID = U.SHIPPER_ID
                    AND M.MAKE_USER_ID = U.USER_ID
                    LEFT JOIN M_USERS U2
                    ON M.SHIPPER_ID = U2.SHIPPER_ID
                    AND M.MAKE_USER_ID = U2.USER_ID
                WHERE
                    M.SHIPPER_ID = :SHIPPER_ID 
                    AND M.CENTER_ID = :CENTER_ID
                    and (:IO_CLASS = 0 OR M.IO_CLASS = :IO_CLASS) 
                    and (:TEMPLATE_NAME IS NULL OR M.TEMPLATE_NAME LIKE '%' || :TEMPLATE_NAME || '%') 
                    and M.DELETE_FLAG = 0 
                ORDER BY
                    M.TEMPLATE_ID
                    , M.SHIPPER_ID

            ";           
        }

        /// <summary>
        /// キーでのデータ取得
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static Layout GetLayout(string shipperId, long templateId)
        {
            return MvcDbContext.Current.Layout.FirstOrDefault(
                x => x.ShipperId == shipperId
                && x.CenterId == Profile.User.CenterId
                && x.TemplateId == templateId 
                && x.DeleteFlag ==false);
        }


        /// <summary>
        /// キーでのデータ取得
        /// </summary>
        /// <param name="shipperId"></param>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static GeneralDataViewModel GetLayoutWithObject(string shipperId, long templateId)
        {
            var param = new DynamicParameters();
            param.AddDynamicParams(new
            {
                SHIPPER_ID = shipperId,
                CENTER_ID = Profile.User.CenterId,
                TEMPLATE_ID = templateId
            });
            return MvcDbContext.OracleConnection.QueryFirstOrDefault<GeneralDataViewModel>(GetLayoutWithObjectSql(), param);
        }

        private static string GetLayoutWithObjectSql()
        {
            return @"
                SELECT
                    M.MAKE_USER_ID                              -- MAKE_USER_ID
                    , M.UPDATE_USER_ID                          -- UPDATE_USER_ID
                    , M.SHIPPER_ID                              -- SHIPPER_ID
                    , M.CENTER_ID
                    , M.TEMPLATE_ID                             -- TEMPLATE_ID
                    , M.TEMPLATE_NAME                           -- TEMPLATE_NAME
                    , M.IO_CLASS                                -- IO_CLASS
                    , M.TITLE_CLASS HEADING_ROW
                    , O.OBJECT_NAME
                FROM
                    M_LAYOUTS M 
                    LEFT JOIN M_OBJECTS O 
                        ON M.SHIPPER_ID = O.SHIPPER_ID 
                        AND M.CENTER_ID = O.CENTER_ID
                        AND M.OBJECT_ID = O.OBJECT_ID
                WHERE
                    M.SHIPPER_ID = :SHIPPER_ID 
                    AND M.CENTER_ID = :CENTER_ID
                    and M.TEMPLATE_ID = :TEMPLATE_ID 
                    and M.DELETE_FLAG = 0 
                ORDER BY
                    M.TEMPLATE_ID
                    , M.SHIPPER_ID
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
                TEMPLATE_NAME = TemplateName,
                OBJECT_TYPE = ObjectType,
                IO_CLASS = IoClass,
                FILE_TYPE = FileType,
                ENCODE_TYPE = EncodeType,
                ENCLOSE_TYPE = EncloseType,
                TITLE_CLASS = TitleClass,
                OBJECT_ID = ObjectId
            });
            return param;
        }


    }
}