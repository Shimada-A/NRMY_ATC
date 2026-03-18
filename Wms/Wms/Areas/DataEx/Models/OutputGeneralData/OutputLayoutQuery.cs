using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Areas.Master.Query.EditLayout;
using Wms.Areas.Master.ViewModels.EditLayout;
using Wms.Common;
using Wms.Models;

namespace Wms.Areas.DataEx.Models.OutputGeneralData
{
    public class OutputLayoutQuery
    {
        public static (string layoutName,Encoding encoding,FileType fileType,List<string> data) GetCsvData(long? templateId, List<ObjectDetailDTO> details)
        {
            var shipperId = Common.Profile.User.ShipperId;

            // 各種データ取得（M_Layout,M_LayoutDetails,M_LayoutConditions)
            var layout = Layout.GetLayout(shipperId, templateId.Value);
            var conditions = EditLayoutQuery.CopyToLayoutConditions(details);
            var layoutDetails = LayoutDetail.GetLayoutDetails(shipperId, templateId.Value);

            // Sql 発行
            var sql = OutputLayoutSqlGenerator.CreateSql(layout, layoutDetails, conditions);
            var command = MvcDbContext.OracleConnection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("SHIPPER_ID", shipperId);
            command.Parameters.Add("CENTER_ID", Common.Profile.User.CenterId);
            var data = command.ExecuteReader();

            // Csv/Tsvデータ作成
            var separator = layout.FileType == (int)FileType.CSV ? "," : "\t";
            var ret = new List<string>();
            
            // 見出し行ありの場合、見出し行を挿入
            if (layout.TitleClass == (byte)HeadingRow.Available)
            {
                var head = new List<string>();

                // ヘッダー
                foreach (var detail in layoutDetails)
                {
                    if (layout.EncloseType == (byte)EncloseType.Available)
                        head.Add($"\"{detail.TitleName}\"");
                    else
                        head.Add(detail.TitleName);
                }
                ret.Add(string.Join(separator, head));
            }

            // 本体
            while (data.Read())
            {
                var list = new List<string>();
                for(int i = 1;i< data.FieldCount; i++)
                {
                    if (layout.EncloseType == (byte)EncloseType.Available)
                        list.Add($"\"{Convert.ToString(data.GetValue(i))}\"");
                    else
                        list.Add(Convert.ToString(data.GetValue(i)));
                }
                ret.Add(string.Join(separator, list));
            }

            return ( layout.TemplateName,
                layout.EncodeType == (int)EncodeType.ShiftJis ? Encoding.GetEncoding("shift-jis") : Encoding.UTF8,
                (FileType)layout.FileType,
                ret);
        }
    }
}