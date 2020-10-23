using LogToCSVConverter.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LogToCSVConverter
{
    static class Program
    {

        static void Main(string[] args)
        {
            SetupStaticLogger();  //Serilog Startup function
           
            try
            {
           
            if (args != null)
            {

                //TODO:ReadTheArgument
                //Validate The Argument
                var listOfInputArguments = ProcessInputArguments(args);

                if (listOfInputArguments != null)
                {
                    GetOutputCSVFileLocationAndLogLevels(listOfInputArguments, out string outputFilePathForCSVFile, out List<InputParams> logLevel, out string sourceDirectoryPathForLogFiles);

                    //Read The List Of Files From The Input Directory
                    var lstLogFileList = FileUtility.GetListOfLogFiles(sourceDirectoryPathForLogFiles);

                    if (lstLogFileList.Count > 0)
                        {
                            ProcessLogFilesConvertIntoCSV(outputFilePathForCSVFile, logLevel, lstLogFileList);

                        }

                    }

            }
            else
            {
                ShowHelpMessageForInvalidInput();
            }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occurred.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }


        #region PrivateMethods

        /// <summary>
        /// RUN
        /// </summary>
        /// <param name="outputFilePathForCSVFile"></param>
        /// <param name="logLevel"></param>
        /// <param name="lstLogFileList"></param>
        private static void ProcessLogFilesConvertIntoCSV(string outputFilePathForCSVFile, List<InputParams> logLevel, List<string> lstLogFileList)
        {

            //PassTheListOfFilesInBelowConstroctor
            LogToCSVParser logToCSVParser = new LogToCSVParser(lstLogFileList, logLevel, outputFilePathForCSVFile);
            logToCSVParser.ParseLogFilesToCsvAsPerGivenLogLevel();
            OpenOutputCSVFile(outputFilePathForCSVFile);
        }
        
        /// <summary>
        /// For Serilog startup function which add json file path 
        /// </summary>
        private static void SetupStaticLogger()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(@"D:\Office\OfficeAssignment\01_log_to_csv\LogToCSVConverter\LogToCSVConverter\appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

        }

        /// <summary>
            /// Open Output file for .csv file
            /// </summary>
            /// <param name="outputFilePathForCSVFile"></param>
        private static void OpenOutputCSVFile(string outputFilePathForCSVFile)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                //FileName = Path.GetDirectoryName(outputFilePathForCSVFile),//For Folder 
                FileName = outputFilePathForCSVFile,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        /// <summary>
        /// Get Output CSV File Location And Log Levels
        /// </summary>
        /// <param name="listOfInputArguments">List of input args</param>
        /// <param name="outputFilePathForCSVFile">output file path fpr csv file</param>
        /// <param name="logLevel">list of log levels</param>
        /// <param name="sourceDirectoryPathForLogFiles">source directory file for log files</param>
        private static void GetOutputCSVFileLocationAndLogLevels(List<InputParams> listOfInputArguments, out string outputFilePathForCSVFile, out List<InputParams> logLevel, out string sourceDirectoryPathForLogFiles)
        {
            sourceDirectoryPathForLogFiles = listOfInputArguments.Where(x => x.Command == "--log-dir").Take(1).ToList()[0].Data;
            outputFilePathForCSVFile = listOfInputArguments.Where(x => x.Command.Trim().ToLower() == "--csv").Take(1).ToList()[0].Data;
            logLevel = listOfInputArguments.Where(x => x.Command.Trim().ToLower() == "--log-level").ToList();
        }
       
        #endregion

        #region PublicMethods
        /// <summary>
        /// Show message for User
        /// </summary>
        #region ShowHelpMessageForInvalidInput 
        public static void ShowHelpMessageForInvalidInput()
        {

            Log.Information(Messages.InvalidInputParameter);
        }

        /// <summary>
        /// Show help message for User
        /// </summary>
        #region ShowHelpMessage
        public static void ShowHelpMessage()
        {
            Log.Information(Messages.ShowHelpMessages);
        }
        #endregion
        #endregion


        /// <summary>
        /// Prosess for input Arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns>List of input params</returns>
        #region ProcessInputArguments
        public static List<InputParams> ProcessInputArguments(string[] args)
        {
            List<InputParams> inputParams = new List<InputParams>();
            if ((args.Length == 1 && args[0].Trim().ToLower() == "--help"))
            {
                ShowHelpMessage();
                inputParams = null;
            }
            else if (args.Length > 0 && args.Length % 2 == 0) /// reminder == 0
            {
                inputParams = FileUtility.ReadCommandLineArgs(args);

                if (inputParams.Count > 0)
                {
                    var inputParamsAreValid = StringUtility.ValidationForParams(inputParams);

                    if (inputParamsAreValid)
                    {
                        return inputParams;
                    }
                    else
                    {
                        ShowHelpMessageForInvalidInput();
                        inputParams = null;
                    }
                }
                else
                {
                    ShowHelpMessageForInvalidInput();
                    inputParams = null;

                }
            }
            else
            {
                ShowHelpMessageForInvalidInput();
                inputParams = null;
            }
            return inputParams;
        }

        #endregion
        #endregion

    }

}




