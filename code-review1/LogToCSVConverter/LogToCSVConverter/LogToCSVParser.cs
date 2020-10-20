using LogToCSVConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogToCSVConverter
{
    public class LogToCSVParser
    {
        #region Properties
        #endregion

        #region PrivateMethods
        #endregion

        /// <summary>
        /// Its reads the cammand line arguments and Get log files 
        /// </summary>
        /// <param name="args">inpunt parameter arguments as string</param>
        /// <returns></returns>
        #region PublicMethods
        public void LogParser(string[] args)
        {
            if ((args.Length == 1 && args[0].Trim().ToLower() == "--help"))
            {
                Program.ShowHelpMessage();
            }
            else if (args.Length > 0 && args.Length % 2 == 0) /// reminder == 0
            {
                List<InputParams> inputParams = StringUtility.ReadArgs(args);

                if (inputParams.Count > 0)
                {
                    var inputParamsAreValid = StringUtility.ValidationForParams(inputParams);

                    if (inputParamsAreValid)
                    {
                        Console.WriteLine("Get log files");

                        string SourceDirectory = inputParams.Where(x => x.Command == "--log-dir").Take(1).ToList()[0].Data;
                        var outputFilePath = inputParams.Where(x => x.Command.Trim().ToLower() == "--csv").Take(1).ToList()[0].Data;
                        var logLevel = inputParams.Where(x => x.Command.Trim().ToLower() == "--log-level").ToList();

                        List<string> lstFiles = FileUtility.GetLogFileList(SourceDirectory);
                        Console.WriteLine("Exploring the log files");

                        foreach (var file in lstFiles)
                        {
                            Console.WriteLine("Processing Log File " + file);
                            FileProcessor.AllDataProcessFromLogFile(file, logLevel, outputFilePath);
                        }
                    }
                    else
                    {
                        Program.ShowHelpMessageForInvalidInput();


                    }
                }
                else
                {
                    Program.ShowHelpMessageForInvalidInput();

                }
            }
            else
            {
                Program.ShowHelpMessageForInvalidInput();
            }

        }

        #endregion
    }
}
