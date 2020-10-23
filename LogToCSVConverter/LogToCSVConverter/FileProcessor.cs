using LogToCSVConverter.Model;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace LogToCSVConverter
{
    public static class FileProcessor
    {
        
        #region Properties
        #endregion

        #region PrivateMethods
        #region AppendContextNumberToCSVRowIfAvailable
        private static void AppendContextNumberToCSVRowIfAvailable(StringBuilder strOutput, string appendContaxtNumber)
        {
            if (appendContaxtNumber.Trim() != "")// If any Incoming contaxt number is there, then add it to data row.
            {
                strOutput.Append(appendContaxtNumber + ",");
            }
        }

        #endregion
        #endregion

        #region PublicMethods
        /// <summary>
        /// All Data Process From the Log file 
        /// </summary>
        /// <param name="fullInputFilePath">Full Input data File path</param> 
        /// <param name="logLevel"></param>
        /// <param name="fullOutPutFilePath">Full Output data File Path </param>
        /// <returns>its not return</returns>
        #region AllDataProcessFromLogFile
        public static void AllDataProcessFromLogFile(string fullInputFilePath, List<InputParams> logLevel, string fullOutPutFilePath)
        {
            if (File.Exists(fullInputFilePath))
            {

                StringBuilder strOutput;
                var MaxDataLength = 0;
                int ContaxtNumber = 0;

                string year = DateTime.Now.Year.ToString();

                string inputDateFormat = "MM/dd/yyyy";
                string outputDateFormat = "dd MMM yyyy";
                string concatinateMMDDYYYY = "";
                string inputFormatForTime = "hh:mm tt";

                List<string> outputListOfCSVData = new List<string>();

                string appendContaxtNumber = "";
                string appendLogLevelForTheDatWhichAreInSecondRowInLogFile = "";
                bool needToIncludeRow = false;
                int dataLengthToTrimForDate = 5;
                int dataLengthToTrimForLogLevel = 20;
                int startingIndexToGetLogLevelInfoFromLogLine = 15;
                int maxLengthOfLogLevelField = 5;

                var Lines = File.ReadAllLines(fullInputFilePath); //Read all line from of the file 


                foreach (var Line in Lines)
                {
                    Log.Information("Lines Not found ", Lines);
                    strOutput = new StringBuilder();

                    MaxDataLength = Line.Length;
                    var rowTrimLength = Line.Trim().Length;

                    if (rowTrimLength > 0)
                    {
                        //Check if the data on Line is Number or not
                        // If Yes 

                        if (int.TryParse(Line, out ContaxtNumber))// if the row is contaxt number row, then get the number & move to next record. 
                        {
                            Log.Information("Context No Found " + ContaxtNumber);
                            appendContaxtNumber = Line.Trim();
                            continue;
                        }
                        else
                        {
                            AppendContextNumberToCSVRowIfAvailable(strOutput, appendContaxtNumber);

                            if (MaxDataLength >= dataLengthToTrimForDate)// check Max Data length For Date
                            {
                                Log.Information("Invalid date " + dataLengthToTrimForDate);
                                concatinateMMDDYYYY = Line.Substring(0, dataLengthToTrimForDate).Trim() + "/" + year;

                                if (DateTime.TryParseExact(concatinateMMDDYYYY, inputDateFormat, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out DateTime logDate))
                                {
                                    if (MaxDataLength >= dataLengthToTrimForLogLevel)//check Max Data length For Log Level
                                    {
                                        var logLevelFromFileLine = Line.Substring(startingIndexToGetLogLevelInfoFromLogLine, maxLengthOfLogLevelField).Trim().ToLower();
                                        if (logLevel.Where(x => x.Data.Equals(logLevelFromFileLine)).Any())
                                        {

                                            needToIncludeRow = true;
                                            
                                            appendLogLevelForTheDatWhichAreInSecondRowInLogFile = logLevelFromFileLine.ToUpper();

                                            // Add Log Level Info
                                            strOutput.Append(logLevelFromFileLine.ToUpper() + ",");

                                            //Add Date Info
                                            strOutput.Append(logDate.ToString(outputDateFormat) + ",");

                                            //Add Time Info
                                            strOutput.Append(StringUtility.AddTimeFieldToCSVDataLine(Line, MaxDataLength, inputFormatForTime));

                                            //Add Log Data
                                            strOutput.Append(StringUtility.AddLogDataToCSVDataLine(Line, MaxDataLength).ToString());

                                            outputListOfCSVData.Add(strOutput.ToString());
                                            continue;
                                        }
                                        else
                                        {
                                            needToIncludeRow = false;
                                            continue;
                                        }

                                    }
                                }
                                else// if we are not able to convert data to Date, Then We will simply add the whole row in List & will jump for next record 
                                {
                                    strOutput.Append(StringUtility.AppendDataWithoutTimeOrLogLevelReference(Line, outputListOfCSVData, appendContaxtNumber, appendLogLevelForTheDatWhichAreInSecondRowInLogFile, needToIncludeRow));
                                }
                            }
                            else// if we are not able to convert data to Date, Then We will simply add the whole row in List & will jump for next record 
                            {
                                strOutput.Append(StringUtility.AppendDataWithoutTimeOrLogLevelReference(Line, outputListOfCSVData, appendContaxtNumber, appendLogLevelForTheDatWhichAreInSecondRowInLogFile, needToIncludeRow));
                            }
                        }
                    }
                }
                
                FileUtility.WriteCSVDataToFile(fullOutPutFilePath, outputListOfCSVData);
            }
            else
            {
                Log.Error("File Not Found " + fullInputFilePath);
            }
        }

    }
        #endregion

        #endregion

}


