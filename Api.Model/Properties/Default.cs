using System.Configuration;

namespace Api.Model.Properties
{
    public static class Default
    {
        public const string Namespace = "http://schemas.tsa.com/email";
        public const string ServiceName = "Email";

        public static string GetAppSettingValue(AppSettingsKeys appSettingKey)
        {
            return ConfigurationManager.AppSettings[appSettingKey.ToString()];
        }

        public enum AppSettingsKeys
        {
            EmailHost
        }
    }
}