using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogToCSVConverter
{
    public static class FileUtility
    {
        public static async Task WriteDataToFile(string fullOutFilePath, List<string> allLines)
        {
            await Task.Run(
                () =>
                {
                    if (allLines.Count > 0)
                    {
                        try
                        {
                            // This text is added only once to the file.
                            if (!File.Exists(fullOutFilePath))
                            {
                                // Create a file to write to.
                                string[] header = new[] { "No , Level , Date , Time , Text" };
                                File.WriteAllLinesAsync(fullOutFilePath, header, Encoding.UTF8);
                                File.AppendAllLinesAsync(fullOutFilePath, allLines, Encoding.UTF8);
                            }
                            else
                            {
                                string newLine = Environment.NewLine;
                                File.AppendAllTextAsync(fullOutFilePath, newLine, Encoding.UTF8);
                                File.AppendAllLinesAsync(fullOutFilePath, allLines, Encoding.UTF8);
                            }
                            Console.WriteLine("CSV file created/Updated Successfully-" + fullOutFilePath);
                        }
                        catch (Exception Ex)
                        {
                            Console.WriteLine(Ex.ToString());
                        }

                    }
                }
                );

        }

        public static List<string> GetFilesList(string sourceDirectory)
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

    }
}
