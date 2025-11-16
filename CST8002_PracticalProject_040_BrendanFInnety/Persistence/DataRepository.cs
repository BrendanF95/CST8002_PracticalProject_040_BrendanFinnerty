/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: November 2025
/// Author: Brendan Finnerty
/// 
/// Persistence layer - Handles all Database operations using SQLite
/// PROJECT 3: Converted from File I/O to Database Connectivity
/// </summary>

using CST8002_PracticalProject.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.IO;

namespace CST8002_PracticalProject_040_BrendanFInnety
{
    /// <summary>
    /// Repository class for managing clam records in SQLite database
    /// </summary>
    public class DataRepository
    {
        private const string DB_FILE_PATH = @"clam_survey.db";
        private const string CSV_FILE_PATH = @"C:\Users\crazy\source\repos\CST8002_PracticalProject\CST8002_PracticalProject_040_BrendanFInnety\pacific_rim_npr_coastalmarine_intertidal_bivalves_clams_1997-2017_data.csv";
        private string connectionString;

        /// <summary>
        /// Constructor initializes database connection string and creates database if needed
        /// </summary>
        public DataRepository()
        {
            connectionString = $"Data Source={DB_FILE_PATH};";
            InitializeDatabase();
        }

        /// <summary>
        /// Creates the database and table structure if they don't exist
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS ClamRecords (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            SiteIdentification TEXT NOT NULL,
                            Year TEXT NOT NULL,
                            Transect TEXT NOT NULL,
                            Quadrat TEXT NOT NULL,
                            SpeciesCommonName TEXT NOT NULL,
                            Count TEXT NOT NULL
                        )";

                    using (SqliteCommand command = new SqliteCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error initializing database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Loads records from the database
        /// If database is empty, loads from CSV file first
        /// </summary>
        /// <returns>List of ClamRecord objects from database</returns>
        public List<ClamRecord> LoadRecords()
        {
            List<ClamRecord> records = new List<ClamRecord>();

            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Check if database is empty
                    string countQuery = "SELECT COUNT(*) FROM ClamRecords";
                    using (SqliteCommand countCmd = new SqliteCommand(countQuery, connection))
                    {
                        long count = (long)countCmd.ExecuteScalar();

                        // If database is empty, populate it from CSV
                        if (count == 0)
                        {
                            Console.WriteLine("Database is empty. Loading from CSV file...");
                            PopulateDatabaseFromCSV();
                        }
                    }

                    // Now read all records from database
                    string selectQuery = "SELECT Id, SiteIdentification, Year, Transect, Quadrat, SpeciesCommonName, Count FROM ClamRecords";

                    using (SqliteCommand command = new SqliteCommand(selectQuery, connection))
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ClamRecord record = new ClamRecord(
                                reader["SiteIdentification"].ToString(),
                                reader["Year"].ToString(),
                                reader["Transect"].ToString(),
                                reader["Quadrat"].ToString(),
                                reader["SpeciesCommonName"].ToString(),
                                reader["Count"].ToString()
                            );
                            record.Id = Convert.ToInt32(reader["Id"]);
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading records from database: {ex.Message}", ex);
            }

            return records;
        }

        /// <summary>
        /// Populates the database from the original CSV file (first 100 records)
        /// </summary>
        private void PopulateDatabaseFromCSV()
        {
            try
            {
                if (!File.Exists(CSV_FILE_PATH))
                {
                    throw new FileNotFoundException($"CSV file not found: {CSV_FILE_PATH}");
                }

                string[] lines = File.ReadAllLines(CSV_FILE_PATH);
                int maxRecords = 100;
                int recordsAdded = 0;

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Skip header row (line 0) and start from line 1
                    for (int i = 1; i < lines.Length && recordsAdded < maxRecords; i++)
                    {
                        string[] fields = lines[i].Split(',');

                        if (fields.Length >= 6)
                        {
                            string insertQuery = @"
                                INSERT INTO ClamRecords (SiteIdentification, Year, Transect, Quadrat, SpeciesCommonName, Count)
                                VALUES (@site, @year, @transect, @quadrat, @species, @count)";

                            using (SqliteCommand command = new SqliteCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@site", fields[0].Trim());
                                command.Parameters.AddWithValue("@year", fields[1].Trim());
                                command.Parameters.AddWithValue("@transect", fields[2].Trim());
                                command.Parameters.AddWithValue("@quadrat", fields[3].Trim());
                                command.Parameters.AddWithValue("@species", fields[4].Trim());
                                command.Parameters.AddWithValue("@count", fields[5].Trim());

                                command.ExecuteNonQuery();
                                recordsAdded++;
                            }
                        }
                    }
                }

                Console.WriteLine($"Loaded {recordsAdded} records from CSV into database.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error populating database from CSV: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Inserts a new record into the database
        /// </summary>
        /// <param name="record">The ClamRecord to insert</param>
        /// <returns>True if successful</returns>
        public bool InsertRecord(ClamRecord record)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = @"
                        INSERT INTO ClamRecords (SiteIdentification, Year, Transect, Quadrat, SpeciesCommonName, Count)
                        VALUES (@site, @year, @transect, @quadrat, @species, @count)";

                    using (SqliteCommand command = new SqliteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@site", record.SiteIdentification);
                        command.Parameters.AddWithValue("@year", record.Year);
                        command.Parameters.AddWithValue("@transect", record.Transect);
                        command.Parameters.AddWithValue("@quadrat", record.Quadrat);
                        command.Parameters.AddWithValue("@species", record.SpeciesCommonName);
                        command.Parameters.AddWithValue("@count", record.Count);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inserting record: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing record in the database by ID
        /// </summary>
        /// <param name="id">Record ID (1-based index)</param>
        /// <param name="record">Updated ClamRecord data</param>
        /// <returns>True if successful</returns>
        public bool UpdateRecord(int id, ClamRecord record)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = @"
                        UPDATE ClamRecords 
                        SET SiteIdentification = @site,
                            Year = @year,
                            Transect = @transect,
                            Quadrat = @quadrat,
                            SpeciesCommonName = @species,
                            Count = @count
                        WHERE Id = @id";

                    using (SqliteCommand command = new SqliteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@site", record.SiteIdentification);
                        command.Parameters.AddWithValue("@year", record.Year);
                        command.Parameters.AddWithValue("@transect", record.Transect);
                        command.Parameters.AddWithValue("@quadrat", record.Quadrat);
                        command.Parameters.AddWithValue("@species", record.SpeciesCommonName);
                        command.Parameters.AddWithValue("@count", record.Count);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating record: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a record from the database by ID
        /// </summary>
        /// <param name="id">Record ID to delete (1-based index)</param>
        /// <returns>True if successful</returns>
        public bool DeleteRecord(int id)
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM ClamRecords WHERE Id = @id";

                    using (SqliteCommand command = new SqliteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting record: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reloads data from CSV file, clearing existing database records first
        /// </summary>
        /// <returns>Number of records loaded</returns>
        public int ReloadFromCSV()
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    // Clear existing records
                    string clearQuery = "DELETE FROM ClamRecords";
                    using (SqliteCommand command = new SqliteCommand(clearQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Reset auto-increment counter
                    string resetQuery = "DELETE FROM sqlite_sequence WHERE name='ClamRecords'";
                    using (SqliteCommand command = new SqliteCommand(resetQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // Populate from CSV
                PopulateDatabaseFromCSV();

                // Return count
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM ClamRecords";
                    using (SqliteCommand command = new SqliteCommand(countQuery, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error reloading from CSV: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets the total count of records in the database
        /// </summary>
        /// <returns>Number of records</returns>
        public int GetRecordCount()
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM ClamRecords";
                    using (SqliteCommand command = new SqliteCommand(countQuery, connection))
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting record count: {ex.Message}", ex);
            }
        }
    }
}