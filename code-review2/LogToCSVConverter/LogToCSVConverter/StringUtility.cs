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
        public static void ShowHelpMessage()
        {
            Console.WriteLine("Below are the list of command usage!! \n");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("$LogToCSVConverter.exe --log-dir <Dir-Path> \n--log-level <info|warn|debug> \n--log - level < info | warn | debug > \n--csv < Out - FilePath > ");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine(@"Ex: LogToCSVConverter.exe --log-dir D:\Office\OfficeAssignment\01_log_to_csv\InputFile --log-level error --log-level warn --csv D:\Office\OfficeAssignment\01_log_to_csv\InputFile\Output\log.csv ");
            Console.WriteLine("-------------------------------------------------------------------------------");
        }
        public static void ShowHelpMessageForInvalidInput()
        {
            Console.WriteLine("Please provide valid inputs. Type Command LogToCSVConverter.exe --help  for more info.");
        }


        public static List<InputParams> ReadArgs(string[] args)
        {
            List<InputParams> lstArgs = new List<InputParams>();
            if (args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    InputParams inputParams = new InputParams();

                    inputParams.Command = args[i];
                    inputParams.Data = args[++i];

                    lstArgs.Add(inputParams);
                }
            }
            return lstArgs;
        }

        public static StringBuilder AddLogData(string data, int MaxDataLength)
        {
            StringBuilder strOutput = new StringBuilder();
            if (MaxDataLength >= 21)// For Log Data
            {
                var logInfo = data.Substring(21, MaxDataLength - 21).Trim();
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

        public static StringBuilder AddTime(string data, int MaxDataLength, string inputFormatForTime)
        {
            StringBuilder strOutput = new StringBuilder();

            DateTime concatinateTimeHHMMSS = new DateTime();
            if (MaxDataLength >= 15)// Check length For Time
            {
                var timeDataFromFile = data.Substring(6, 8).Trim();

                if (DateTime.TryParse(timeDataFromFile, out concatinateTimeHHMMSS))
                {
                    string formattedTime = concatinateTimeHHMMSS.ToString(inputFormatForTime);
                    strOutput.Append(formattedTime + ",");
                }

            }

            return strOutput;
        }

        public static bool ValidationForParams(List<InputParams> inputParams)
        {
            List<string> lstLog = new List<string> { "info", "debug", "warn", "error", "trace" };
            int ret = 0;
            var sourceDirectory = inputParams.Where(x => x.Command.Trim().ToLower() == "--log-dir").Take(1).ToList();
            var outputFilePath = inputParams.Where(x => x.Command.Trim().ToLower() == "--csv").Take(1).ToList();
            var logLevel = inputParams.Where(x => x.Command.Trim().ToLower() == "--log-level").ToList();

            if (sourceDirectory != null && sourceDirectory.Any() && sourceDirectory.Count > 0 && outputFilePath != null && outputFilePath.Any() && logLevel != null)
            {
                if (!Directory.Exists(sourceDirectory[0].Data))
                {
                    Console.WriteLine("No such Directory Exists " + sourceDirectory[0].Data);
                    ret = 1;
                }
                if (!Directory.Exists(Path.GetDirectoryName(outputFilePath[0].Data)))
                {
                    Console.WriteLine("No such Directory Exists for destination file path " + outputFilePath[0].Data);
                    var destinationDirectory = StringUtility.AssemblyDirectory;  // AssemblyDirectorylocation show for .exe
                    foreach (var item in inputParams)
                    {
                        if (item.Command == "--csv")
                        {
                            item.Data = destinationDirectory + "\\log.csv";
                            Console.WriteLine("Giving default path for destination file " + item.Data);
                            //outputFilePath.Clear();
                            /// outputFilePath.Add(new InputParams { Command = "--csv", Data = item.Data.ToString() });
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
    }
}
