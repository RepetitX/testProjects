using System;
using System.Configuration;

namespace Common.Core
{
    public static class AppConfigHelper
    {
        public static T GetSetting<T>(string key, T defaultValue = default(T))
        {
            try
            {
                var setting = ConfigurationManager.AppSettings[key];

                if (!string.IsNullOrWhiteSpace(setting))
                    return (T)Convert.ChangeType(setting, typeof(T));
            }
            catch
            {
            }

            return defaultValue;
        }
    }
}