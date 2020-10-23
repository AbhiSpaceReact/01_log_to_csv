using LogToCSVConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogToCSVConverter
{
    public class LogToCSVParser
    {
        public async Task LogParser(string[] args)
        {
            if ((args.Length == 1 && args[0].Trim().ToLower() == "--help"))
            {
                StringUtility.ShowHelpMessage();
            }
            else if (args.Length > 0 && args.Length % 2 == 0) ///
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

                        List<string> lstFiles = FileUtility.GetFilesList(SourceDirectory);
                        Console.WriteLine("Exploring the log files");

                        foreach (var file in lstFiles)
                        {
                            Console.WriteLine("Processing Log File " + file);
                            await FileProcessor.ProcessLogFiles(file, logLevel, outputFilePath);
                        }
                    }
                    else
                    {
                        StringUtility.ShowHelpMessageForInvalidInput();

                    }
                }
                else
                {
                    StringUtility.ShowHelpMessageForInvalidInput();

                }
            }
            else
            {
                StringUtility.ShowHelpMessageForInvalidInput();
            }



        }
    }
}
