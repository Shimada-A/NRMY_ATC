namespace Wms.Areas.Master.ViewModels.DeliareaGroupSearchModal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dapper;

    public partial class DeliareaGroupViewModel
    {
        public List<DeliareaGroupViewModel> Listing()
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT MG.CENTER_ID
                      ,MD.DELIAREA_GROUP_ID
                      ,MG.GEN_NAME AS DELIAREA_GROUP_NAME
                      ,MG.GEN_CD
                      ,LISTAGG(TO_CHAR(MD.PREF_ID),',') WITHIN GROUP(ORDER BY MD.PREF_ID) AS PREF_ID
                      ,LISTAGG(TO_CHAR(MP.PREF_NAME),'、') WITHIN GROUP(ORDER BY MP.PREF_ID) AS PREF_NAME
                  FROM M_GENERALS MG
                  LEFT JOIN M_DELIAREA_GROUP MD ON MD.SHIPPER_ID = MG.SHIPPER_ID
                   AND MD.CENTER_ID = MG.CENTER_ID
                   AND MD.DELIAREA_GROUP_ID = MG.GEN_CD
                  LEFT JOIN M_PREFS MP ON MP.SHIPPER_ID = MD.SHIPPER_ID
                   AND MP.PREF_ID = MD.PREF_ID
                 WHERE MG.SHIPPER_ID = :SHIPPER_ID
                   AND MG.CENTER_ID  = :CENTER_ID
                   AND MG.REGISTER_DIVI_CD = '1'
                   AND MG.GEN_DIV_CD = 'DELI_AREA_SELECT_DIALOG_AREA_G'
                 GROUP BY MG.CENTER_ID
                         ,MD.DELIAREA_GROUP_ID
                         ,MG.GEN_NAME
                         ,MG.GEN_CD
                 ORDER BY TO_NUMBER(MG.GEN_CD)
            ");
            parameters.AddDynamicParams(new { SHIPPER_ID = Common.Profile.User.ShipperId, CENTER_ID = Common.Profile.User.CenterId });

            return
                Wms.Models.MvcDbContext.Current.Database.Connection.Query<DeliareaGroupViewModel>(
                    sql.ToString(), param: parameters).ToList();
        }
    }
}