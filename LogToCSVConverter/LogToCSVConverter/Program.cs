using System.Threading.Tasks;

namespace LogToCSVConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
             * $ logParser.exe --log-dir <Dir-Path> --log-level <info|warn|debug>  
                --log-level <info|warn|debug> --csv <Out-FilePath>   --help 
              
             */
            /* logParser.exe --log-dir D:\Office\OfficeAssignment\01_log_to_csv\InputFile --log-level error --log-level warn --csv D:\Office\OfficeAssignment\01_log_to_csv\InputFile\Output\log.csv 
*/
            if (args != null)
            {
                LogToCSVParser logToCSVParser = new LogToCSVParser();

                await logToCSVParser.LogParser(args);
            }
            else
            {
                StringUtility.ShowHelpMessageForInvalidInput();
            }


        }

    }
}
