using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Helpers
{
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Gets the application setting vale
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>string</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static string GetAppSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            throw new ArgumentException(string.Format("{0} appSetting not found.", key));
        }
    }
}