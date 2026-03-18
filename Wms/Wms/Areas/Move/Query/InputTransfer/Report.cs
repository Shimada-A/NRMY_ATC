namespace Wms.Areas.Move.Query.InputTransfer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Dapper;
    using Wms.Areas.Move.ViewModels.InputTransfer;
    using Wms.Common;
    using Wms.Models;
    using Wms.Query;

    public class Report : BaseQuery
    {
        /// <summary>
        /// 第1画面で受入登録したデータを対象にラベルを発行する
        /// </summary>
        /// <param name="seq">ワークID</param>
        /// <param name="centerId">センターID</param>
        /// <returns></returns>
        public IEnumerable<PrintCaseLabelCsv> PrintCaseLabelJanListing(long seq, string centerId)
        {
            var sql = $@"
                WITH
                    WORK_DATA AS (
                        SELECT
                                *
                        FROM
                                WW_ARR_TRANS_INPUT01
                        WHERE
                                SEQ = :SEQ
                            AND CENTER_ID = :CENTER_ID
                            AND SHIPPER_ID = :SHIPPER_ID
                            AND SELECTED_FLAG = 1
                            AND TRANSFER_CLASS <> 1     --1:WMS倉庫間移動は発行しない
                    )
                SELECT
                        TTR.CENTER_ID || '　' || MAX(MC.CENTER_NAME1) AS CENTER
                    ,   MAX(TTR.BOX_NO) AS LABEL_NUMBER_BARCODE
                FROM
                        T_TRANSFER_RESULTS TTR
                INNER JOIN
                        WORK_DATA WK
                ON
                        WK.SLIP_NO = TTR.SLIP_NO
                    AND WK.CENTER_ID = TTR.CENTER_ID
                    AND WK.SHIPPER_ID = TTR.SHIPPER_ID
                INNER JOIN
                        M_CENTERS MC
                ON
                        MC.CENTER_ID = TTR.CENTER_ID
                    AND MC.SHIPPER_ID = TTR.SHIPPER_ID
                GROUP BY
                        TTR.SLIP_NO
                    ,   TTR.CENTER_ID
                    ,   TTR.SHIPPER_ID
                ORDER BY
                        TTR.SLIP_NO
                    ,   TTR.CENTER_ID
                    ,   TTR.SHIPPER_ID
            ";

            return MvcDbContext.Current.Database.Connection.Query<PrintCaseLabelCsv>(sql,
                new
                {
                    SEQ = seq,
                    CENTER_ID = centerId,
                    SHIPPER_ID = Profile.User.ShipperId
                });
        }

        /// <summary>
        /// 第2画面で実績入力したデータを対象にラベルを発行する
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<PrintCaseLabelCsv> PrintCaseLabelJanListing2(List<string> boxNos, string centerId)
        {
            var centerName = MvcDbContext.Current.Warehouses.Where(x => x.CenterId == centerId && x.ShipperId == Profile.User.ShipperId).Select(x => x.CenterName1).Single();

            List<PrintCaseLabelCsv> pcl = new List<PrintCaseLabelCsv>();

            foreach (var boxNo in boxNos)
            {
                pcl.Add(new PrintCaseLabelCsv()
                {
                    Center = centerId + "　" + centerName,
                    LabelNumberBarcode = boxNo
                });
            }

            return pcl;
        }
    }
}