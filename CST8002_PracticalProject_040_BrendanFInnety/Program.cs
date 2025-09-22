/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: September 21, 2025
/// Author: Brendan Finnerty
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;

namespace CST8002_PracticalProject
{
    class Program
    {
        private const string CSV_FILE_PATH = @"C:\Users\crazy\source\repos\CST8002_PracticalProject\CST8002_PracticalProject_040_BrendanFInnety\pacific_rim_npr_coastalmarine_intertidal_bivalves_clams_1997-2017_data.csv";
        private static List<ClamRecord> clamRecords;

        static void Main(string[] args)
        {
            Console.WriteLine("CST8002 Practical Project - Brendan Finnerty");
            DisplayHeader();

            clamRecords = new List<ClamRecord>();
            ReadClamDataFromCSV();
            DisplayAllClamRecords();

            Console.ReadKey();
        }
        private static void DisplayHeader()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  CST8002 Practical Project 1 - Pacific Rim Clams Dataset     ║");
            Console.WriteLine("║  Author: Brendan Finnerty                                    ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        private static void ReadClamDataFromCSV()
        {
            try
            {
                if (!File.Exists(CSV_FILE_PATH))
                {
                    throw new FileNotFoundException($"The file '{CSV_FILE_PATH}' was not found.");
                }

                string[] lines = File.ReadAllLines(CSV_FILE_PATH);
                Console.WriteLine($"Successfully opened file: {CSV_FILE_PATH}");
                Console.WriteLine($"Total lines in file: {lines.Length}");

                int startLine = 2;
                int recordsToRead = 5;
                int endLine = Math.Min(startLine + recordsToRead, lines.Length);

                for (int i = startLine; i < endLine; i++)
                {
                    ParseAndStoreClamRecord(lines[i]);
                }

                Console.WriteLine($"Successfully parsed {clamRecords.Count} records.\n");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"ERROR: Unable to read file - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: An unexpected error occurred - {ex.Message}");
            }
        }

        private static void ParseAndStoreClamRecord(string line)
        {
            try
            {
                string[] fields = line.Split(',');

                if (fields.Length >= 6)
                {
                    ClamRecord record = new ClamRecord(
                        fields[0].Trim(),
                        fields[1].Trim(),
                        fields[2].Trim(),
                        fields[3].Trim(),
                        fields[4].Trim(),
                        fields[5].Trim()
                    );
                    clamRecords.Add(record);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing record: {ex.Message}");
            }

        }
        private static void DisplayAllClamRecords()
        {
            Console.WriteLine("\n=== DISPLAYING CLAM SURVEY RECORDS ===\n");

            for (int i = 0; i < clamRecords.Count; i++)
            {
                Console.WriteLine($"Record #{i + 1}:");
                clamRecords[i].DisplayRecord();
            }

            Console.WriteLine($"\nTotal Records: {clamRecords.Count}");
            Console.WriteLine($"\nProgram Author: Brendan Finnerty");
        }
    }

}