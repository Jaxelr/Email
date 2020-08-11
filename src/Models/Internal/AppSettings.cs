using System.Collections.Generic;

namespace EmailService.Models
{
    public class AppSettings
    {
        public string SmtpServer { get; set; }
        public string ApiKey { get; set; }
        public RouteDefinition RouteDefinition { get; set; }
        public ICollection<string> ServerUrls { get; set; }
    }
}
