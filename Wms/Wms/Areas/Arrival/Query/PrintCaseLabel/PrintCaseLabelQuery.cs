namespace Wms.Areas.Arrival.Query.PrintCaseLabel
{
    using Dapper;
    using Share.Common;
    using System.Data;
    using Wms.Areas.Arrival.ViewModels.PrintCaseLabel;
    using Wms.Common;
    using Wms.Models;

    public class PrintCaseLabelQuery
    {
        /// <summary>
        /// 実績更新
        /// </summary>
        public string BoxNoCheck(PrintCaseLabelConditions SearchConditions)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", Profile.User.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", SearchConditions.ReleaseBoxNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);
            param.Add("OUT_CENTER_ID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            param.Add("OUT_CENTER_NAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
            param.Add("OUT_CUSTOMER_ID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            param.Add("OUT_CUSTOMER_NAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
            param.Add("OUT_ITEM_ID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            param.Add("OUT_ITEM_COLOR", dbType: DbType.String, direction: ParameterDirection.Output, size: 5);
            param.Add("OUT_ITEM_SIZE", dbType: DbType.String, direction: ParameterDirection.Output, size: 5);
            param.Add("OUT_ITEM_NAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
            param.Add("OUT_JAN", dbType: DbType.String, direction: ParameterDirection.Output, size: 13);
            param.Add("OUT_PRINT_DATE", dbType: DbType.String, direction: ParameterDirection.Output, size: 30);
            param.Add("OUT_BOX_NO", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            param.Add("OUT_QTY", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var db = MvcDbContext.Current.Database;

            db.Connection.Execute(
                "PK_W_ARR_PrintCaseLabel.CheckBoxNo",
                param,
                commandType: CommandType.StoredProcedure);

            if (param.Get<int>("OUT_STATUS") == (int)ProcedureStatus.Error)
            {
                return param.Get<string>("OUT_MESSAGE");
            }
            else
            {
                return null;
            }
            //return null;
        }
    }
}