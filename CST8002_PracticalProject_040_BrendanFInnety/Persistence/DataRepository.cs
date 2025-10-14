/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: October 12, 2025
/// Author: Brendan Finnerty
/// 
/// Persistence layer - Handles all File I/O operations
/// </summary>

using CST8002_PracticalProject.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace CST8002_PracticalProject_040_BrendanFInnety
{
    /// <summary>
    /// Repository class for reading and writing clam records to/from CSV files
    /// </summary>
    public class DataRepository
    {
        private const string CSV_FILE_PATH = @"pacific_rim_npr_coastalmarine_intertidal_bivalves_clams_1997-2017_data.csv";

        /// <summary>
        /// Reads up to 100 clam records from the CSV file
        /// </summary>
        /// <returns>List of ClamRecord objects</returns>
        /// <exception cref="FileNotFoundException">Thrown when CSV file is not found</exception>
        public List<ClamRecord> LoadRecords()
        {
            List<ClamRecord> records = new List<ClamRecord>();

            try
            {
                if (!File.Exists(CSV_FILE_PATH))
                {
                    throw new FileNotFoundException($"The file '{CSV_FILE_PATH}' was not found.");
                }

                string[] lines = File.ReadAllLines(CSV_FILE_PATH);

                // Skip header row (line 0) and start from line 1
                int startLine = 1;
                int maxRecords = 100;
                int endLine = Math.Min(startLine + maxRecords, lines.Length);

                for (int i = startLine; i < endLine; i++)
                {
                    ClamRecord record = ParseCsvLine(lines[i]);
                    if (record != null)
                    {
                        records.Add(record);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (IOException ex)
            {
                throw new IOException($"Unable to read file: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error loading records: {ex.Message}", ex);
            }

            return records;
        }

        /// <summary>
        /// Parses a single CSV line into a ClamRecord object
        /// </summary>
        /// <param name="line">CSV line to parse</param>
        /// <returns>ClamRecord object or null if parsing fails</returns>
        private ClamRecord ParseCsvLine(string line)
        {
            try
            {
                string[] fields = line.Split(',');

                if (fields.Length >= 6)
                {
                    return new ClamRecord(
                        fields[0].Trim(),
                        fields[1].Trim(),
                        fields[2].Trim(),
                        fields[3].Trim(),
                        fields[4].Trim(),
                        fields[5].Trim()
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing line: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// Saves clam records to a CSV file with a GUID-based filename
        /// </summary>
        /// <param name="records">List of records to save</param>
        /// <returns>The filename that was created</returns>
        public string SaveRecords(List<ClamRecord> records)
        {
            try
            {
                // Generate GUID-based filename
                string guid = Guid.NewGuid().ToString();
                string filename = $"clam_data_{guid}.csv";

                using (StreamWriter writer = new StreamWriter(filename))
                {
                    // Write header
                    writer.WriteLine("SiteIdentification,Year,Transect,Quadrat,SpeciesCommonName,Count");

                    // Write each record
                    foreach (ClamRecord record in records)
                    {
                        writer.WriteLine(record.ToCsvString());
                    }
                }

                return filename;
            }
            catch (IOException ex)
            {
                throw new IOException($"Error saving file: {ex.Message}", ex);
            }
        }
    }
}