using LogToCSVConverter.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LogToCSVConverter
{
    public static class FileUtility
    {
        #region Properties
        #endregion

        #region PrivateMethods
        #endregion

        #region PublicMethods

        /// <summary>
        /// Its reads the cammand line arguments and create command and data combination and append to a list or return List of arguments
        /// </summary>
        /// <param name="args">input parameter argument as string</param>
        /// <returns>List of arguments<InputParams></returns>
        #region ReadCommandLineArgs
        public static List<InputParams> ReadCommandLineArgs(string[] args)
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
        /// writes the CSV data strings to the file
        /// </summary>
        /// <param name="DestinationCSVFilePath">output CSV file path </param>
        /// <param name="AllCSVLinesExtractedFromLogFile">All CSV data line extraced from log file</param>
        /// <returns></returns>
        #region WriteCSVDataToFile
        public static void WriteCSVDataToFile(string DestinationCSVFilePath, List<string> AllCSVLinesExtractedFromLogFile)
        {
            if (AllCSVLinesExtractedFromLogFile.Count > 0)
            {
                try
                {
                    // This text is added only once to the file.
                    if (!File.Exists(DestinationCSVFilePath))
                    {
                        // Create a file to write to.
                        string[] header = new[] { "No , Level , Date , Time , Text" };
                        File.WriteAllLinesAsync(DestinationCSVFilePath, header, Encoding.UTF8);
                        File.AppendAllLinesAsync(DestinationCSVFilePath, AllCSVLinesExtractedFromLogFile, Encoding.UTF8);
                    }
                    else
                    {
                        string newLine = Environment.NewLine;
                        File.AppendAllTextAsync(DestinationCSVFilePath, newLine, Encoding.UTF8);
                        File.AppendAllLinesAsync(DestinationCSVFilePath, AllCSVLinesExtractedFromLogFile, Encoding.UTF8);
                    }
                    Log.Information("CSV file created/Updated Successfully-" + DestinationCSVFilePath);
                }
                catch (Exception Ex)
                {
                    Log.Error(Ex.ToString());
                }

            }
        }
        #endregion

        /// <summary>
        /// Get the list of log file 
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <returns>List of Files</returns>
        #region GetLogFileList
        public static List<string> GetLogFileList(string sourceDirectory)
        {
            List<string> lstFiles = new List<string>();
            if (Directory.Exists(sourceDirectory))
            {
                lstFiles = Directory.GetFiles(sourceDirectory, "***.log***").ToList();
                if (lstFiles.Count==0)
                {
                    Log.Error("No Log File Exists at "+ sourceDirectory);
                }
            }
            else
            {
                Log.Error("No such Directory Exists");
            }
            return lstFiles;
        }
        #endregion

        /// <summary>
        /// Read Log of Log Files
        /// </summary>
        /// <param name="SourceDirectoryPathForLogFiles">Source Directory Path For Log Files</param>
        /// <returns>List of Files</returns>
        #region GetLogOfLogFiles
        public static List<string> GetListOfLogFiles(string SourceDirectoryPathForLogFiles)
        {
            Log.Information("Get log files");

           List<string> lstFiles = GetLogFileList(SourceDirectoryPathForLogFiles);

            return lstFiles;
        }
        #endregion

        #endregion
    }
}
