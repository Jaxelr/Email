using System.Collections.Generic;

namespace EmailService.Entities
{
    public class AppSettings
    {
        public CacheConfig Cache { get; set; }
        public string SmtpServer { get; set; }
        public RouteDefinition RouteDefinition { get; set; }
        public ICollection<string> Addresses { get; set; }
    }
}
