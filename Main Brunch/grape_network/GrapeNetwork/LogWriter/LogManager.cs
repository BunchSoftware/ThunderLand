using NLog;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GrapNetwork.LogWriter
{
    public class LogManager
    {
        private object sync = new object();
        private string pathToLog = "";
        private string typeFile = ".log";
        private string nameLog;
        private CultureInfo culture = new CultureInfo("en-US", false);
        private bool append = true;
        private AccessLevel accessLevel = AccessLevel.User;

        public LogManager(string pathToLog, string nameLog)
        {
            this.pathToLog = pathToLog;
            this.nameLog = nameLog;
        }

        /// <summary>
        /// Создает сообщение в журнале на уровне Info.
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            CultureInfo.CurrentCulture = culture;

            try
            {
                CheckExistsFile();
                string fileName = Path.Combine(pathToLog, $"{nameLog}{typeFile}");
                string text = $"\n{GetInfoAboutRecord()} [INFO]: {message}";
                CheckAppendTextToFile(fileName, text);
            }
            catch { }
        }

        /// <summary>
        /// Создает сообщение в журнале на уровне Debug.
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            CultureInfo.CurrentCulture = culture;

            try
            {
                CheckExistsFile();
                string fileName = Path.Combine(pathToLog, $"{nameLog}{typeFile}");
                string text = $"\n{GetInfoAboutRecord()} [DEBUG]: {message}";
                CheckAppendTextToFile(fileName, text);
            }
            catch { }
        }
        /// <summary>
        /// Создает сообщение в журнале на уровне Warning.
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            CultureInfo.CurrentCulture = culture;

            try
            {
                CheckExistsFile();
                string fileName = Path.Combine(pathToLog, $"{nameLog}{typeFile}");
                string text = $"\n{GetInfoAboutRecord()} [WARNING]: {message}";
                CheckAppendTextToFile(fileName, text);
            }
            catch { }   
        }
        /// <summary>
        /// Создает сообщение об ошибке в журнале на уровне Error.
        /// </summary>
        /// <param name="ex"></param>
        public void Error(Exception exception)
        {
            CultureInfo.CurrentCulture = culture;

            try
            {
                CheckExistsFile();
                string fileName = Path.Combine(pathToLog, $"{nameLog}{typeFile}");
                string text = $"\n{GetInfoAboutRecord()} [ERROR]: {exception.TargetSite.DeclaringType} {exception.TargetSite.Name} {exception.Message}";
                CheckAppendTextToFile(fileName, text);
            }
            catch { }
        }
        /// <summary>
        /// Создает сообщение об ошибке в журнале на уровне Fatal.
        /// </summary>
        /// <param name="exception"></param>
        public void Fatal(Exception exception)
        {
            CultureInfo.CurrentCulture = culture;

            try
            {
                CheckExistsFile();
                string fileName = Path.Combine(pathToLog, $"{nameLog}{typeFile}");
                string text = $"\n{GetInfoAboutRecord()} [FATAL]: {exception.TargetSite.DeclaringType} {exception.TargetSite.Name} {exception.Message}";
                CheckAppendTextToFile(fileName, text);
            }
            catch { }
        }

        /// <summary>
        /// Создает сообщение в журнале на необходимом уровне.
        /// </summary>
        /// <param name="message"></param>
        public void Log(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Info:
                    Info(message);
                    break;
                case LogLevel.Debug:
                    Debug(message);
                    break;
                case LogLevel.Warning:
                    Warning(message);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Создает сообщение об ошибке в журнале на необходимом уровне.
        /// </summary>
        /// <param name="message"></param>
        public void Log(LogLevel logLevel, Exception exception)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    Error(exception);
                    break;
                case LogLevel.Fatal:
                    Fatal(exception);
                    break;
                default:
                    break;
            }
        }

        private void CheckExistsFile()
        {
            if (!Directory.Exists(pathToLog))
                Directory.CreateDirectory(pathToLog);
        }
        private void CheckAppendTextToFile(string fileName, string text)
        {
            lock (sync)
            {
                if (append)
                    File.AppendAllText(fileName, text, Encoding.UTF8);
                else
                {
                    append = true;
                    File.WriteAllText(fileName, text, Encoding.UTF8);
                }    
            }
        }

        private string GetInfoAboutRecord()
        {
            return $"{DateTime.Now.ToString("MMM dd.MM.yyyy HH:mm:ss K")} {accessLevel.ToString()}:";
        }
    }
}
