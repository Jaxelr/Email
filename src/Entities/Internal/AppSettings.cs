namespace EmailService.Entities
{
    public class AppSettings
    {
        public CacheConfig Cache { get; set; }
        public string SmtpServer { get; set; }
    }
}
