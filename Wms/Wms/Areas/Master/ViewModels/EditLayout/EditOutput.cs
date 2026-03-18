using Share.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wms.Areas.Master.Models;
using Wms.Common;

namespace Wms.Areas.Master.ViewModels.EditLayout
{
    public class EditOutput : EditLayoutBase
    {
        public EditOutput()
        {

        }

        public EditOutput(Layout layout,List<LayoutCondition> conditions,List<LayoutDetail> detail) : base(layout)
        {
            ObjectDetails = new List<ObjectDetailDTO>();
            foreach(var item in conditions)
            {
                ObjectDetails.Add(new ObjectDetailDTO(item));
            }
            SetFileDetails(detail);
        }

        public EditOutput(Layout layout, List<ObjectDetailDTO> conditions, List<LayoutDetail> detail) : base(layout)
        {
            ObjectDetails = conditions;

            SetFileDetails(detail);
            var diff = Math.Max(1, conditions.Count - detail.Count);
            for (int i = 0; i < diff; i++)
            {
                EditOutputFileDetails.Add(new EditOutputFileDetail { ShipperId = ShipperId, IsNewData = true,RowNo = detail.Count + i ,SubNo=1});
            }
        }
        
        /// <summary>
        /// LayoutDetailテーブルのデータをFileDetailsインスタンスにセット
        /// </summary>
        /// <param name="details"></param>
        private void SetFileDetails(List<LayoutDetail> details)
        {
            EditOutputFileDetails = new List<EditOutputFileDetail>();
            foreach (var item in details)
            {
                EditOutputFileDetails.Add(new EditOutputFileDetail(item) { IsNewData = false});
            }
        }

        /// <summary>
        /// オブジェクト明細
        /// </summary>
        public List<ObjectDetailDTO> ObjectDetails { get; set; }


        /// <summary>
        ///  出力ファイル設定
        /// </summary>
        public List<EditOutputFileDetail> EditOutputFileDetails{ get; set; }
    }
}