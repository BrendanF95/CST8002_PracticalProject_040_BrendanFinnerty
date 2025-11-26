/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: November 30 2025
/// Author: Brendan Finnerty
/// 
/// Presentation layer - Handles user interface and interactions
/// PROJECT 4: Added multi-column search/filter functionality
/// </summary>

using CST8002_PracticalProject.Model;
using System;
using System.Collections.Generic;

namespace CST8002_PracticalProject_040_BrendanFInnety
{
    /// <summary>
    /// Main program class - handles user interface and menu system with database operations
    /// </summary>
    class Program
    {
        private static ClamDataManager dataManager;
        private const string AUTHOR_NAME = "Brendan Finnerty";

        static void Main(string[] args)
        {
            dataManager = new ClamDataManager();

            DisplayHeader();
            InitialLoad();
            RunMainMenu();
        }

        /// <summary>
        /// Displays the program header with author name
        /// </summary>
        private static void DisplayHeader()
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║  CST8002 Practical Project 4 - Pacific Rim Clams Dataset     ║");
            Console.WriteLine("║  MULTI-COLUMN SEARCH/FILTER - SQLite Implementation          ║");
            Console.WriteLine($"║  Program by: {AUTHOR_NAME,-46}║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        /// <summary>
        /// Initial data load on startup - loads from database
        /// </summary>
        private static void InitialLoad()
        {
            try
            {
                Console.WriteLine("Connecting to SQLite database...");
                int count = dataManager.RecordCount;

                if (count == 0)
                {
                    Console.WriteLine("Database is empty. Loading from CSV file...");
                    count = dataManager.ReloadData();
                }

                Console.WriteLine($"Successfully loaded {count} records from database.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                Console.WriteLine("Starting with empty database.\n");
            }
        }

        /// <summary>
        /// Main menu loop
        /// </summary>
        private static void RunMainMenu()
        {
            bool running = true;

            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ReloadDataMenu();
                        break;
                    case "2":
                        DisplayRecordsMenu();
                        break;
                    case "3":
                        CreateRecordMenu();
                        break;
                    case "4":
                        EditRecordMenu();
                        break;
                    case "5":
                        DeleteRecordMenu();
                        break;
                    case "6":
                        SearchFilterMenu();
                        break;
                    case "7":
                        running = false;
                        Console.WriteLine("\nThank you for using the Clam Data Manager!");
                        Console.WriteLine($"Program by: {AUTHOR_NAME}");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Please try again.\n");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the main menu options
        /// </summary>
        private static void DisplayMenu()
        {
            Console.WriteLine($"\n=== MAIN MENU === (Program by: {AUTHOR_NAME})");
            Console.WriteLine($"Current Records in Database: {dataManager.RecordCount}");
            Console.WriteLine("\n1. Reload data from CSV file (replaces database contents)");
            Console.WriteLine("2. Display records from database");
            Console.WriteLine("3. Create new record in database");
            Console.WriteLine("4. Edit existing record in database");
            Console.WriteLine("5. Delete record from database");
            Console.WriteLine("6. Search/Filter records (NEW - Project 4)");
            Console.WriteLine("7. Exit");
            Console.Write("\nEnter your choice: ");
        }

        /// <summary>
        /// Handles reloading data from CSV into database
        /// </summary>
        private static void ReloadDataMenu()
        {
            try
            {
                Console.WriteLine("\nReloading data from CSV file into database...");
                Console.WriteLine("This will replace all existing database records.");
                Console.Write("Are you sure? (yes/no): ");
                string confirm = Console.ReadLine();

                if (confirm.ToLower() == "yes")
                {
                    int count = dataManager.ReloadData();
                    Console.WriteLine($"Successfully reloaded {count} records into database.");
                    Console.WriteLine($"Program by: {AUTHOR_NAME}");
                }
                else
                {
                    Console.WriteLine("Reload cancelled.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles displaying records (single or multiple)
        /// </summary>
        private static void DisplayRecordsMenu()
        {
            Console.WriteLine("\n=== DISPLAY RECORDS FROM DATABASE ===");
            Console.WriteLine("1. Display single record");
            Console.WriteLine("2. Display all records");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                DisplaySingleRecord();
            }
            else if (choice == "2")
            {
                DisplayAllRecords();
            }
        }

        /// <summary>
        /// Displays a single record by index
        /// </summary>
        private static void DisplaySingleRecord()
        {
            Console.Write($"\nEnter record number (1-{dataManager.RecordCount}): ");
            if (int.TryParse(Console.ReadLine(), out int recordNum))
            {
                ClamRecord record = dataManager.GetRecord(recordNum);
                if (record != null)
                {
                    Console.WriteLine($"\n=== Record #{recordNum} (from Database) ===");
                    Console.WriteLine(record.ToString());
                    Console.WriteLine($"\nProgram by: {AUTHOR_NAME}");
                }
                else
                {
                    Console.WriteLine("Invalid record number.");
                }
            }
        }

        /// <summary>
        /// Displays all records from database with author name every 10 records
        /// </summary>
        private static void DisplayAllRecords()
        {
            try
            {
                List<ClamRecord> records = dataManager.GetAllRecords();
                Console.WriteLine($"\n=== DISPLAYING ALL RECORDS FROM DATABASE ({records.Count} total) ===\n");

                for (int i = 0; i < records.Count; i++)
                {
                    Console.WriteLine($"Record #{i + 1}: {records[i].ToString()}");

                    if ((i + 1) % 10 == 0)
                    {
                        Console.WriteLine($"--- Program by: {AUTHOR_NAME} ---");
                    }
                }

                Console.WriteLine($"\nTotal: {records.Count} records");
                Console.WriteLine($"Program by: {AUTHOR_NAME}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles creating a new record in the database
        /// </summary>
        private static void CreateRecordMenu()
        {
            Console.WriteLine("\n=== CREATE NEW RECORD IN DATABASE ===");
            Console.Write("Site Identification: ");
            string site = Console.ReadLine();
            Console.Write("Year: ");
            string year = Console.ReadLine();
            Console.Write("Transect: ");
            string transect = Console.ReadLine();
            Console.Write("Quadrat: ");
            string quadrat = Console.ReadLine();
            Console.Write("Species Common Name: ");
            string species = Console.ReadLine();
            Console.Write("Count: ");
            string count = Console.ReadLine();

            try
            {
                ClamRecord newRecord = new ClamRecord(site, year, transect, quadrat, species, count);
                bool success = dataManager.CreateRecord(newRecord);

                if (success)
                {
                    Console.WriteLine("\nRecord created successfully in database!");
                    Console.WriteLine($"Program by: {AUTHOR_NAME}");
                }
                else
                {
                    Console.WriteLine("\nFailed to create record.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles editing an existing record in the database
        /// </summary>
        private static void EditRecordMenu()
        {
            Console.Write($"\nEnter record number to edit (1-{dataManager.RecordCount}): ");
            if (int.TryParse(Console.ReadLine(), out int recordNum))
            {
                try
                {
                    ClamRecord existing = dataManager.GetRecord(recordNum);
                    if (existing != null)
                    {
                        Console.WriteLine($"\nCurrent record: {existing.ToString()}");
                        Console.WriteLine("\nEnter new values (press Enter to keep current value):");

                        Console.Write($"Site [{existing.SiteIdentification}]: ");
                        string site = Console.ReadLine();
                        Console.Write($"Year [{existing.Year}]: ");
                        string year = Console.ReadLine();
                        Console.Write($"Transect [{existing.Transect}]: ");
                        string transect = Console.ReadLine();
                        Console.Write($"Quadrat [{existing.Quadrat}]: ");
                        string quadrat = Console.ReadLine();
                        Console.Write($"Species [{existing.SpeciesCommonName}]: ");
                        string species = Console.ReadLine();
                        Console.Write($"Count [{existing.Count}]: ");
                        string count = Console.ReadLine();

                        ClamRecord updated = new ClamRecord(
                            string.IsNullOrWhiteSpace(site) ? existing.SiteIdentification : site,
                            string.IsNullOrWhiteSpace(year) ? existing.Year : year,
                            string.IsNullOrWhiteSpace(transect) ? existing.Transect : transect,
                            string.IsNullOrWhiteSpace(quadrat) ? existing.Quadrat : quadrat,
                            string.IsNullOrWhiteSpace(species) ? existing.SpeciesCommonName : species,
                            string.IsNullOrWhiteSpace(count) ? existing.Count : count
                        );

                        if (dataManager.UpdateRecord(recordNum, updated))
                        {
                            Console.WriteLine("\nRecord updated successfully in database!");
                            Console.WriteLine($"Program by: {AUTHOR_NAME}");
                        }
                        else
                        {
                            Console.WriteLine("\nFailed to update record.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid record number.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Handles deleting a record from the database
        /// </summary>
        private static void DeleteRecordMenu()
        {
            Console.Write($"\nEnter record number to delete (1-{dataManager.RecordCount}): ");
            if (int.TryParse(Console.ReadLine(), out int recordNum))
            {
                try
                {
                    ClamRecord record = dataManager.GetRecord(recordNum);
                    if (record != null)
                    {
                        Console.WriteLine($"\nRecord to delete: {record.ToString()}");
                        Console.Write("Are you sure? (yes/no): ");
                        string confirm = Console.ReadLine();

                        if (confirm.ToLower() == "yes")
                        {
                            if (dataManager.DeleteRecord(recordNum))
                            {
                                Console.WriteLine("Record deleted successfully from database!");
                                Console.WriteLine($"Program by: {AUTHOR_NAME}");
                            }
                            else
                            {
                                Console.WriteLine("Failed to delete record.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Deletion cancelled.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid record number.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// PROJECT 4: Handles multi-column search/filter functionality
        /// Allows users to search across multiple fields simultaneously
        /// </summary>
        private static void SearchFilterMenu()
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         MULTI-COLUMN SEARCH/FILTER (Project 4)               ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝");
            Console.WriteLine("\nEnter search criteria for any field (press Enter to skip):");
            Console.WriteLine("Note: Search is case-insensitive and matches partial text\n");

            Console.Write("Site Identification: ");
            string siteFilter = Console.ReadLine()?.Trim();

            Console.Write("Year: ");
            string yearFilter = Console.ReadLine()?.Trim();

            Console.Write("Transect: ");
            string transectFilter = Console.ReadLine()?.Trim();

            Console.Write("Quadrat: ");
            string quadratFilter = Console.ReadLine()?.Trim();

            Console.Write("Species Common Name: ");
            string speciesFilter = Console.ReadLine()?.Trim();

            Console.Write("Count: ");
            string countFilter = Console.ReadLine()?.Trim();

            try
            {
                // Count active filters
                int activeFilters = dataManager.GetActiveFilterCount(
                    siteFilter, yearFilter, transectFilter,
                    quadratFilter, speciesFilter, countFilter);

                if (activeFilters == 0)
                {
                    Console.WriteLine("\nNo search criteria entered. Returning to main menu.");
                    return;
                }

                // Display active filters
                Console.WriteLine($"\n=== ACTIVE FILTERS ({activeFilters}) ===");
                if (!string.IsNullOrWhiteSpace(siteFilter))
                    Console.WriteLine($"  - Site contains: '{siteFilter}'");
                if (!string.IsNullOrWhiteSpace(yearFilter))
                    Console.WriteLine($"  - Year contains: '{yearFilter}'");
                if (!string.IsNullOrWhiteSpace(transectFilter))
                    Console.WriteLine($"  - Transect contains: '{transectFilter}'");
                if (!string.IsNullOrWhiteSpace(quadratFilter))
                    Console.WriteLine($"  - Quadrat contains: '{quadratFilter}'");
                if (!string.IsNullOrWhiteSpace(speciesFilter))
                    Console.WriteLine($"  - Species contains: '{speciesFilter}'");
                if (!string.IsNullOrWhiteSpace(countFilter))
                    Console.WriteLine($"  - Count contains: '{countFilter}'");

                // Perform search
                Console.WriteLine("\nSearching database...");
                List<ClamRecord> results = dataManager.SearchRecords(
                    siteFilter, yearFilter, transectFilter,
                    quadratFilter, speciesFilter, countFilter);

                // Display results
                DisplaySearchResults(results, activeFilters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nERROR during search: {ex.Message}");
            }
        }

        /// <summary>
        /// PROJECT 4: Displays search results with formatting
        /// Helper method to present filtered records to the user
        /// </summary>
        /// <param name="results">List of matching records</param>
        /// <param name="filterCount">Number of filters applied</param>
        private static void DisplaySearchResults(List<ClamRecord> results, int filterCount)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║  SEARCH RESULTS: {results.Count} matching records found");
            Console.WriteLine($"║  Filters applied: {filterCount}");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════╝\n");

            if (results.Count == 0)
            {
                Console.WriteLine("No records match your search criteria.");
                Console.WriteLine($"Program by: {AUTHOR_NAME}");
                return;
            }

            // Display all matching records
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"Result #{i + 1}: {results[i].ToString()}");

                // Add author name every 10 results
                if ((i + 1) % 10 == 0)
                {
                    Console.WriteLine($"--- Program by: {AUTHOR_NAME} ---");
                }
            }

            Console.WriteLine($"\nTotal matching records: {results.Count}");
            Console.WriteLine($"Program by: {AUTHOR_NAME}");

            // Offer to export or further refine search
            Console.WriteLine("\nPress Enter to return to main menu...");
            Console.ReadLine();
        }
    }
}