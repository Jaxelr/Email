using System;

namespace Email.Models
{
    public class FailedResponse
    {
        public FailedResponse(Exception ex)
        {
            Message = ex.Message;
#if DEBUG
            StackTrace = ex.StackTrace;
            Source = ex.Source;
            TargetMethod = ex.TargetSite.Name;
#endif
        }

        public string Message { get; set; }
#if DEBUG
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string TargetMethod { get; set; }
#endif
    }
}
