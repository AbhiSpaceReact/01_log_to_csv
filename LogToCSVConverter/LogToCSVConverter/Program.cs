using System;
using System.Threading.Tasks;

namespace LogToCSVConverter
{
    static class Program
    {

        static void Main(string[] args)
        {
            /*
              $ logParser.exe --log-dir <Dir-Path> --log-level <info|warn|debug>  
                --log-level <info|warn|debug> --csv <Out-FilePath>   --help 
            */
            /* logParser.exe --log-dir D:\Office\OfficeAssignment\01_log_to_csv\InputFile --log-level error --log-level warn --csv D:\Office\OfficeAssignment\01_log_to_csv\InputFile\Output\log.csv 
            */
            if (args != null)
            {
                LogToCSVParser logToCSVParser = new LogToCSVParser();

                logToCSVParser.LogParser(args);
            }
            else
            {
                ShowHelpMessageForInvalidInput();
                //StringUtility.ShowHelpMessageForInvalidInput();
            }
        }

        /// <summary>
        /// Show message for User
        /// </summary>
        #region ShowHelpMessageForInvalidInput 
        public static void ShowHelpMessageForInvalidInput()
        {
            Console.WriteLine("Please provide valid inputs. Type Command LogToCSVConverter.exe --help  for more info.");
        }
        #endregion

        #region ShowMessages
        public static void ShowHelpMessage()
        {

            Console.WriteLine("Below are the list of command usage!! \n");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("$LogToCSVConverter.exe --log-dir <Dir-Path> \n--log-level <info|warn|debug> \n--log - level < info | warn | debug > \n--csv < Out - FilePath > ");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine(@"Ex: LogToCSVConverter.exe --log-dir D:\Office\OfficeAssignment\01_log_to_csv\InputFile --log-level error --log-level warn --csv D:\Office\OfficeAssignment\01_log_to_csv\InputFile\Output\log.csv ");
            Console.WriteLine("-------------------------------------------------------------------------------");
        }
       
        #endregion

    }

}



