using LogToCSVConverter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LogToCSVConverter
{

    public static class StringUtility
    {
        #region Properties
        #endregion

        #region PrivateMethods
        #endregion

        #region PublicMethods
        
        /// <summary>
        /// Log Data VAlidation and append data to CSV file
        /// </summary>
        /// <param name="data">Read data from file as one string</param>
        /// <param name="MaxDataLength">Read Max data length from file</param>
        /// <returns></returns>
        #region LogDataValidationAndAppendDataForCSVFile

        public static StringBuilder AddLogData(string data, int MaxDataLength)
        {
            StringBuilder strOutput = new StringBuilder();
            if (MaxDataLength >= 21)// For Log Data
            {
                var logInfo = data[21..MaxDataLength].Trim();
                if (logInfo.Length > 1)
                {
                    if (logInfo.Contains(","))
                    {
                        strOutput.Append("\"" + logInfo.Remove(0, 1) + "\"");

                    }
                    else
                    {
                        strOutput.Append(logInfo.Remove(0, 1));
                    }
                }
            }
            return strOutput;
        }

        /// <summary>
        /// Log Data Validation and Append Data Without Time Or Log Level Reference for CSV file
        /// </summary>
        /// <param name="data">Read data from file as one string</param>
        /// <param name="allLines">All data List</param>
        /// <param name="appendContaxtNumber">Append Contaxt Number in All data List</param>
        /// <param name="appendLogLevel">Append log level in All data List</param>
        /// <param name="needToIncludeRow"></param>
        /// <returns>StringBuilder</returns>
        public static StringBuilder AppendDataWithoutTimeOrLogLevelReference(string data, List<string> allLines, string appendContaxtNumber, string appendLogLevel, bool needToIncludeRow)
        {
            StringBuilder strOutput = new StringBuilder();

            if (appendLogLevel.Length != 0 && needToIncludeRow)
            {
                strOutput.Append(appendLogLevel + ",");
                if (data.Contains(","))// If the data contains "," then Enclose the data with Double quotes 
                {
                    allLines.Add(appendContaxtNumber + "," + appendLogLevel.ToUpper() + ",,," + "\"" + data + "\"");

                }
                else// If data does not have comma "," so we can direactlt add it to list
                {
                    allLines.Add(appendContaxtNumber + "," + appendLogLevel.ToUpper() + ",,," + data);
                }

            }
            return strOutput;
        }

        /// <summary>
        /// adds time to CSV data file 
        /// </summary>
        /// <param name="data"> Read data from file as one string</param>
        /// <param name="MaxDataLength">Max Data length for input log file</param>
        /// <param name="inputFormatForTime">Input date formate for log file </param>
        /// <returns>it returns StringBuilder which contains formated time in "hh:mm tt"</returns>
        public static StringBuilder AddTimeToFile(string data, int MaxDataLength, string inputFormatForTime)
        {
            StringBuilder strOutput = new StringBuilder();

            if (MaxDataLength >= 15)// Check length For Time
            {
                var timeDataFromFile = data.Substring(6, 8).Trim();

                if (DateTime.TryParse(timeDataFromFile, out DateTime concatinateTimeHHMMSS))
                {
                    string formattedTime = concatinateTimeHHMMSS.ToString(inputFormatForTime);
                    strOutput.Append(formattedTime + ",");
                }

            }

            return strOutput;
        }

        #endregion

        /// <summary>
        /// Its reads the cammand line arguments and create command and data combination and append to a list or return List of arguments
        /// </summary>
        /// <param name="args">input parameter argument as string</param>
        /// <returns>List of arguments<InputParams></returns>
        #region ListOfInputParameters
        public static List<InputParams> ReadArgs(string[] args)
        {
            List<InputParams> lstArgs = new List<InputParams>();
            if (args.Length > 0) //Check lenght of argument
            {
                for (int i = 0; i < args.Length; i++)
                {
                    InputParams inputParams = new InputParams
                    {
                        Command = args[i],
                        Data = args[++i]
                    };

                    lstArgs.Add(inputParams);
                }
            }
            return lstArgs;
        }
        #endregion

        /// <summary>
        /// runs validations -  check valid path. check empty string
        /// </summary>
        /// <param name="inputParams">Its contains list of input parameters witch contains command and data which is pass in arguments</param>
        /// <returns>if condition satisfy then return true or false</returns>
        #region ValidationForParams

        public static bool ValidationForParams(List<InputParams> inputParams)
        {
            List<string> lstLog = new List<string> { "info", "debug", "warn", "error", "trace" };
            int ret = 0;
            var sourceDirectory = inputParams.Where(x => x.Command.Trim().ToLower() == "--log-dir").Take(1).ToList();
            var outputFilePath = inputParams.Where(x => x.Command.Trim().ToLower() == "--csv").Take(1).ToList();
            var logLevel = inputParams.Where(x => x.Command.Trim().ToLower() == "--log-level").ToList();

            if (sourceDirectory != null && sourceDirectory.Any() && sourceDirectory.Count > 0 && outputFilePath != null && outputFilePath.Any() && logLevel != null)
            {
                if (!Directory.Exists(sourceDirectory[0].Data)) // check empty string
                {
                    Console.WriteLine("No such Directory Exists " + sourceDirectory[0].Data);
                    ret = 1;
                }
                if (!Directory.Exists(Path.GetDirectoryName(outputFilePath[0].Data)))
                {
                    Console.WriteLine("No such Directory Exists for destination file path " + outputFilePath[0].Data);
                    var destinationDirectory = StringUtility.AssemblyDirectory;  // AssemblyDirectory location show for .exe
                    foreach (var item in inputParams)
                    {
                        if (item.Command == "--csv")
                        {
                            item.Data = destinationDirectory + "\\log.csv";
                            Console.WriteLine("Giving default path for destination file " + item.Data);
                            break;
                        }
                    }
                    ret = 0;
                }
                if (Path.GetExtension(outputFilePath[0].Data) != ".csv")
                {
                    Console.WriteLine("Invalid file extension for destination file " + outputFilePath[0].Data);
                    ret = 1;
                }
                if (logLevel.Count > 0)
                {
                    foreach (var log in logLevel)
                    {
                        if (!lstLog.Contains(log.Data))
                        {
                            Console.WriteLine("Invalid log level supplied " + log.Data);
                            ret = 1;
                        }
                    }
                }
            }
            else
            {
                ret = 1;
            }

            return ret == 0;  //If ret is == 0 means there is no validation error so the function will return true 
                              // and the validation will be successfully done

        }

        #endregion

        /// <summary>
        /// Assembly Directory location show for .exe file
        /// </summary>
        #region DefaultPathForCSVOutput
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);

            }
        }
        #endregion

        #endregion

    }
}
