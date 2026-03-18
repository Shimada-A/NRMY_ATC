namespace Wms.Areas.Master.ViewModels.LocTransporter
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Report
    {
        public string CenterId { get; set; }

        public string ShipToStoreId { get; set; }
        
        public string ShipToStoreClass { get; set; }

        public string StartDate { get; set; }

        public string TransporterId { get; set; }

        public string LeadTimes { get; set; }

        //public string TransporterIdMon { get; set; }

        //public string LeadTimesMon { get; set; }

        //public string TransporterIdTue { get; set; }

        //public string LeadTimesTue { get; set; }

        //public string TransporterIdWed { get; set; }

        //public string LeadTimesWed { get; set; }

        //public string TransporterIdThu { get; set; }

        //public string LeadTimesThu { get; set; }

        //public string TransporterIdFri { get; set; }

        //public string LeadTimesFri { get; set; }

        //public string TransporterIdSat { get; set; }

        //public string LeadTimesSat { get; set; }

        //public string TransporterIdSun { get; set; }

        //public string LeadTimesSun { get; set; }

        //public string TransporterIdHol { get; set; }

        //public string LeadTimesHol { get; set; }

        public string ClientCd { get; set;}

        public string ControlId { get; set; }

        public string ConsignorId { get; set; }

    }
}