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
                    Console.WriteLine("CSV file created/Updated Successfully-" + DestinationCSVFilePath);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.ToString());
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
            }
            else
            {
                Console.WriteLine("No such Directory Exists");
            }
            return lstFiles;
        }
        #endregion

        #endregion
    }
}
