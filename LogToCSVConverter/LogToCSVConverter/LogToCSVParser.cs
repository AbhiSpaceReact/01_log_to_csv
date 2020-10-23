using LogToCSVConverter.Model;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace LogToCSVConverter
{
    public class LogToCSVParser
    {
        #region Properties
        List<string> _lstLogFiles;
        List<InputParams> _lstLogLevel;
        readonly string _outputFilePathForCSVFile;
        #endregion

        #region constructor
        /// <summary>
        /// Log to CSV Parser that takes listLogfiles,logLevel,CSVFilePath
        /// </summary>
        /// <param name="lstLogFiles">List of Log Files</param>
        /// <param name="lstLogLevel">List of Log Level</param>
        /// <param name="outputFilePathForCSVFile">OutPut File Path for CSV file</param>
        public LogToCSVParser(List<string> lstLogFiles, List<InputParams> lstLogLevel, string outputFilePathForCSVFile)
        {
            _lstLogFiles = lstLogFiles;
            _lstLogLevel = lstLogLevel;
            _outputFilePathForCSVFile = outputFilePathForCSVFile;
             Log.Information("Creating Constroctor for Log to CSV Parser");

        }
        #endregion

        #region PrivateMethods
        #endregion

        #region PublicMethods
       


        ///// <summary>
        /////  Parse log file to csv As per log level 
        ///// </summary>
        #region ParseLogFilesToCsvAsPerGivenLogLevel
        public void ParseLogFilesToCsvAsPerGivenLogLevel()
        {

            Log.Information("Exploring the log files");
            foreach (var file in _lstLogFiles )
            {
                Log.Information("Processing Log File " + file);
                FileProcessor.AllDataProcessFromLogFile(file, _lstLogLevel, _outputFilePathForCSVFile);
            }
        }

        #endregion
        #endregion
    }
}

