using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Log general messages.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);


        /// <summary>
        /// Log warning messages.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(string message);


        /// <summary>
        /// Log debug messages.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);


        /// <summary>
        /// Log error messages.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);


        /// <summary>
        /// Logs error messages with exception
        /// </summary>
        /// <param name="ex">The execption.</param>
        /// <param name="message">The message.</param>
        void Error(Exception ex, string message = "");


        /// <summary>
        /// Log fatal error messages.
        /// </summary>
        /// <param name="message">The message.</param>
        void Fatal(string message);


        /// <summary>
        /// Logs fatal messages with exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The message.</param>
        void Fatal(Exception ex, string message = "");
    }

    public class NLogger : ILogger
    {
        /// <summary>
        /// NLog instance
        /// </summary>
        private NLog.Logger _logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="NLogger"/> class.
        /// </summary>
        public NLogger()
        {
            this._logger = LogManager.GetCurrentClassLogger();
        }


        /// <summary>
        /// Log general messages.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            this._logger.Info(message);
        }


        /// <summary>
        /// Log warning messages.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            this._logger.Warn(message);
        }


        /// <summary>
        /// Log debug messages.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Debug(string message)
        {
            this._logger.Debug(message);
        }


        /// <summary>
        /// Log error messages.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            this._logger.Error(message);
        }


        /// <summary>
        /// Logs error messages with exception
        /// </summary>
        /// <param name="ex">The execption.</param>
        /// <param name="message">The message.</param>
        public void Error(Exception ex, string message = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                this._logger.Error(message, ex);
            }
            else if (!string.IsNullOrWhiteSpace(ex.Message))
            {
                this._logger.Error(ex.Message, ex);
            }
            else
            {
                this._logger.Error(ex);
            }
        }


        /// <summary>
        /// Log fatal error messages.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Fatal(string message)
        {
            this._logger.Fatal(message);
        }


        /// <summary>
        /// Logs fatal messages with exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <param name="message">The message.</param>
        public void Fatal(Exception ex, string message = "")
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                this._logger.Fatal(message, ex);
            } else if (!string.IsNullOrWhiteSpace(ex.Message)) {
                this._logger.Fatal(ex.Message, ex);
            } else {
                this._logger.Fatal(ex);
            }
        }

    }
}