using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogToCSVConverter
{
    public static class Messages
    {

        #region PublicMethods
        #region InvalidInputParameter
        /// <summary>
        /// for user --help message
        /// </summary>
        public static string InvalidInputParameter = "Please provide valid inputs. Type Command LogToCSVConverter.exe --help  for more info.";

        #endregion

        #region ShowHelpMessages
        /// <summary>
        /// show message for user
        /// </summary>
        public static string ShowHelpMessages = 
            "Below are the list of command usage!! \n" +
            "-------------------------------------------------------------------------------" +
            "$LogToCSVConverter.exe --log-dir <Dir-Path> \n--log-level <info|warn|debug> \n--log - level < info | warn | debug > \n--csv < Out - FilePath > " +
            "-------------------------------------------------------------------------------" +
            @"Ex: LogToCSVConverter.exe --log-dir D:\Office\OfficeAssignment\01_log_to_csv\InputFile --log-level error --log-level warn --csv D:\Office\OfficeAssignment\01_log_to_csv\InputFile\Output\log.csv " +
            "-------------------------------------------------------------------------------";
        #endregion
        #endregion
    }
}
