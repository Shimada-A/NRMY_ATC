namespace Wms.ViewModels
{
    public class VmError
    {
        public string StackTrace { get; set; }

        public string EventID { get; set; }

        public string TimeWritten { get; set; }

        public string Category { get; set; }

        public string Message { get; set; }

        public string ReplacementStrings { get; set; }

        public string Source { get; set; }

        public string Title { get; set; } = "Server Infomation.";

        public string Information { get; set; } = Share.Common.Resources.MessagesResource.TextReprocessing;

        public string DisplayDiv { get; set; } = "none";
    }
}