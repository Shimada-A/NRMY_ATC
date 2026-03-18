namespace Wms.Areas.Master.ViewModels.LocTransporter
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class SearchItem : Models.LocTransporter
    {
        public bool IsCheck { get; set; }

        public string CenterId { get; set; }

        public string CenterName { get; set; }

        public string ShipToStoreName { get; set; }

        public string AreaId { get; set; }

        public string Area { get; set; }

        public string PrefId { get; set; }

        public string PrefName { get; set; }

        public string TransporterName { get; set; }

        public string TransporterNameSun { get; set; }

        public string TransporterNameMon { get; set; }

        public string TransporterNameTue { get; set; }

        public string TransporterNameWed { get; set; }

        public string TransporterNameThu { get; set; }

        public string TransporterNameFri { get; set; }

        public string TransporterNameSat { get; set; }

        public string TransporterNameHol { get; set; }

        public DateTime NewStartDate { get; set; }

        public string ErrMsg { get; set; }

        public string Seq { get; set; }

        public string No { get; set; }

        public string RowId { get; set; }

        public string DispClientCd { get; set; } 

        public string ConsignorName { get; set; }

        public string DispControlId { get; set; }

        public string NaniwaConsignorName { get; set; }

        public string DispConsignorId { get; set; }

        public string WsConsignorName { get; set; }

    }
}