using DuplicateNPL_Model;
using MaxvalEntity;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateNPL_LogManager
{
    public class LogManager
    {
        /// <summary>
        /// Logs the request info and errors
        /// Note: Connection string passed as param to avoid circular dependancy with common DLL
        /// </summary>
        /// <param name="connectionString">sql connection string</param>
        /// <param name="msg">Log Message</param>
        public static void Insert_PAIRSyncLog(string connectionString, string msg, Exception expThrown = null)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("usp_pairsync_console_log_insert", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@logMessage", msg);
                        sqlConnection.Open();
                        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCommand.ExecuteScalar();
                    }
                }
                if (expThrown != null && GetConfigValue<int>(ConfigurationNames.LogToCloudWatch) == 1)
                {
                    SaveExceptionToCloudWatch(expThrown, msg);
                }
            }
            catch (Exception ex)
            {
                if (expThrown != null && GetConfigValue<int>(ConfigurationNames.LogToCloudWatch) == 1)
                {
                    SaveExceptionToCloudWatch(expThrown, msg);
                }
            }
        }
        /// <summary>
        /// Save exception details to CloudWatch
        /// </summary>
        /// <param name="expThrown"></param>
        /// <param name="maxIDSlogID"></param>
        public static void SaveExceptionToCloudWatch(Exception expThrown, string errorMessage)
        {
            //Get the Error Log Details from database
            ErrorDetails errorDetail = null;
            if (expThrown != null && !string.IsNullOrEmpty(expThrown.Message) && File.Exists(GetConfigValue<string>(ConfigurationNames.ErrorFilePath)))
            {
                List<ErrorDetails> errorDetails = LoadJsonFile<ErrorDetails>(GetConfigValue<string>(ConfigurationNames.ErrorFilePath));
                errorDetail = errorDetails.Where(x => expThrown.Message.ToUpper().Contains(x.ErrorMessage.ToUpper())).FirstOrDefault();
            }
            LogEntity logEntity = new LogEntity()
            {
                LogGroup = GetConfigValue<string>(ConfigurationNames.CloudWatchLogGroup),
                LogLevel = errorDetail != null ? (Level)Enum.Parse(typeof(Level), errorDetail.Severity) : Level.Error,
                LogData = new LogData()
                {
                    Environment = GetConfigValue<string>(ConfigurationNames.Environment),
                    ClientName = GetConfigValue<string>(ConfigurationNames.ClientName),
                    Product = ConfigurationNames.ProductName,
                    Context = $"ErrorOccurredTime: {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt")}",
                    Details = new Details()
                    {
                        Message = expThrown != null ? expThrown.Message : errorMessage,
                        Errors = new List<Errors>()
                            {
                                new Errors()
                                {
                                    Code=errorDetail != null ? errorDetail.ErrorCode : $"MaxIDS.PS.000",
                                    Description=expThrown!=null && expThrown.InnerException !=null ? $"InnerException: {expThrown.InnerException}" : errorMessage
                                }
                            }
                    },
                    StackTrace = expThrown != null ? expThrown.StackTrace : string.Empty
                }
            };
            LogResponse status = new Maxval.Log(GetConfigValue<string>(ConfigurationNames.NLogConfigPath)).WriteLog(logEntity);
        }
        /// <summary>
        /// Get the configuration value by name and type
        /// </summary>
        /// <typeparam name="T">generic type</typeparam>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static T GetConfigValue<T>(string configName)
        {
            object value = ConfigurationManager.AppSettings[configName];
            return value != null ? (T)Convert.ChangeType(value, typeof(T)) : default(T);
        }
        /// <summary>
        /// Load json to passing list T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>List items for the passed generic type</returns>
        public static List<T> LoadJsonFile<T>(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<T>>(json);
        }
        /// <summary>
        /// Congfiguration appsettings name
        /// </summary>
        public struct ConfigurationNames
        {
            public const string Error = "Error";
            public const string ProductName = "PAIRSYNC";
            public const string CloudWatchLogGroup = "CloudWatchLogGroup";
            public const string NLogConfigPath = "NLogConfigPath";
            public const string ErrorFilePath = "ErrorFilePath";
            public const string Environment = "Environment";
            public const string ClientName = "ClientName";
            public const string LogToCloudWatch = "LogToCloudWatch";
        }
    }
}
