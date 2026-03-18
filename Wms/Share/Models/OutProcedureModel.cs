using Share.Common;

namespace Share.Models
{
    public class OutProcedureModel
    {
        public ProcedureStatus status { get; set; } = ProcedureStatus.Success;
        public string message { get; set; }
    }
}
