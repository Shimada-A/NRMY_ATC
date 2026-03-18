namespace Wms.Areas.Arrival.Query.PrintCaseLabel
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Arrival.ViewModels.PrintCaseLabel;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="condition"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PrintCaseLabelCsv> PrintCaseLabelJanListing(PrintCaseLabelConditions condition)
        {
            List<PrintCaseLabelCsv> pcl = new List<PrintCaseLabelCsv>();

            DynamicParameters parameters = new DynamicParameters();

            StringBuilder query = new StringBuilder(@"
                    SELECT CENTER_ID || '　' || CENTER_NAME1 CENTER
                          ,TO_CHAR(SYSDATE,'YY/MM/DD') PRINT_DATE
                          ,SF_GET_ARRIVE_CASE_NO(SHIPPER_ID, CENTER_ID) LABEL_NUMBER_BARCODE
                      FROM M_CENTERS
                     WHERE SHIPPER_ID = :SHIPPER_ID
                       AND CENTER_ID = :CENTER_ID ");
            parameters.Add(":SHIPPER_ID", Common.Profile.User.ShipperId);
            parameters.Add(":CENTER_ID", Common.Profile.User.CenterId);

            for (var i = 0; i < condition.NumberofSheets; i++)
            {
                var data = MvcDbContext.Current.Database.Connection.Query<PrintCaseLabelSql>(query.ToString(), parameters).Single();

                pcl.Add(new PrintCaseLabelCsv()
                {
                    Center = data.Center,
                    PrintDate = data.PrintDate,
                    LabelNumberBarcode = data.LabelNumberBarcode,
                    DispIssueFlag = 0
                });
            }

            // Fill data to memory
            return pcl;
        }

        /// <summary>
        /// 配分Csvに出力するデータを取得する。
        /// </summary>
        /// <param name="search"></param>
        /// <returns>表形式で表示するため、ケース閾値設定マスタのデータを作る。</returns>
        public IEnumerable<PrintCaseLabelCsv> PrintCaseLabelListing(PrintCaseLabelConditions condition)
        {
            var param = new DynamicParameters();
            param.Add("IN_SHIPPER_ID", Profile.User.ShipperId, DbType.String, ParameterDirection.Input);
            param.Add("IN_CENTER_ID", Profile.User.CenterId, DbType.String, ParameterDirection.Input);
            param.Add("IN_USER_ID", Profile.User.UserId, DbType.String, ParameterDirection.Input);
            param.Add("IN_BOX_NO", condition.ReleaseBoxNo, DbType.String, ParameterDirection.Input);
            param.Add("OUT_STATUS", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("OUT_MESSAGE", dbType: DbType.String, direction: ParameterDirection.Output, size: 4000);
            param.Add("OUT_CENTER_ID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            param.Add("OUT_CENTER_NAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
            param.Add("OUT_CUSTOMER_ID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            param.Add("OUT_CUSTOMER_NAME", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
            param.Add("OUT_ITEM_ID", dbType: DbType.String, direction: ParameterDirection.Output, size: 20);
            param.Add("OUT_ITEM_COLOR", dbType: DbType.String, direction: ParameterDirection.Output, size: 5);
            param.Add("OUT_ITEM_SIZE", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
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

            List<PrintCaseLabelCsv> pc = new List<PrintCaseLabelCsv>();
            pc.Add(new PrintCaseLabelCsv()
            {
                Center = param.Get<string>("OUT_CENTER_ID") + '　' + param.Get<string>("OUT_CENTER_NAME"),
                Customer = param.Get<string>("OUT_CUSTOMER_ID") + '　' + param.Get<string>("OUT_CUSTOMER_NAME"),
                ItemCd = param.Get<string>("OUT_ITEM_ID"),
                Color = param.Get<string>("OUT_ITEM_COLOR"),
                Size = param.Get<string>("OUT_ITEM_SIZE"),
                ItemName = param.Get<string>("OUT_ITEM_NAME"),
                Jan1 = param.Get<string>("OUT_JAN").Substring(0, 7),
                Jan2 = param.Get<string>("OUT_JAN").Substring(7),
                PrintDate = (!string.IsNullOrEmpty(param.Get<string>("OUT_PRINT_DATE")))? System.DateTime.Parse(param.Get<string>("OUT_PRINT_DATE")).ToString("yyyy/MM/dd") : null,
                LabelNumberBarcode = param.Get<string>("OUT_BOX_NO"),
                DispIssueFlag = 1
            });

            return pc;
        }

    }
}