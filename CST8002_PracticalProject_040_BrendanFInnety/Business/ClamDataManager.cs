/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: November 30 2025
/// Author: Brendan Finnerty
/// 
/// Business layer - Manages clam data operations and business logic
/// PROJECT 4: Added multi-column search/filter functionality
/// </summary>

using CST8002_PracticalProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CST8002_PracticalProject_040_BrendanFInnety
{
    /// <summary>
    /// Business logic layer for managing clam survey data with database operations
    /// </summary>
    public class ClamDataManager
    {
        private DataRepository repository;

        /// <summary>
        /// Constructor initializes the data manager with database repository
        /// </summary>
        public ClamDataManager()
        {
            repository = new DataRepository();
        }

        /// <summary>
        /// Gets the count of records from the database
        /// </summary>
        public int RecordCount
        {
            get
            {
                try
                {
                    return repository.GetRecordCount();
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Reloads data from CSV file into the database
        /// </summary>
        /// <returns>Number of records loaded</returns>
        public int ReloadData()
        {
            try
            {
                return repository.ReloadFromCSV();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all records from the database
        /// </summary>
        /// <returns>List of all clam records</returns>
        public List<ClamRecord> GetAllRecords()
        {
            try
            {
                return repository.LoadRecords();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a single record by index (1-based for user display)
        /// </summary>
        /// <param name="index">Record number (1-based)</param>
        /// <returns>ClamRecord at the specified index</returns>
        public ClamRecord GetRecord(int index)
        {
            try
            {
                List<ClamRecord> records = repository.LoadRecords();
                if (index >= 1 && index <= records.Count)
                {
                    return records[index - 1];
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a new record and stores it in the database
        /// </summary>
        /// <param name="record">The record to create</param>
        /// <returns>True if successful</returns>
        public bool CreateRecord(ClamRecord record)
        {
            try
            {
                if (record != null)
                {
                    return repository.InsertRecord(record);
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an existing record in the database
        /// </summary>
        /// <param name="index">Record number to update (1-based)</param>
        /// <param name="updatedRecord">New record data</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateRecord(int index, ClamRecord updatedRecord)
        {
            try
            {
                if (index >= 1 && updatedRecord != null)
                {
                    List<ClamRecord> records = repository.LoadRecords();
                    if (index <= records.Count)
                    {
                        int databaseId = records[index - 1].Id;
                        return repository.UpdateRecord(databaseId, updatedRecord);
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a record from the database
        /// </summary>
        /// <param name="index">Record number to delete (1-based)</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteRecord(int index)
        {
            try
            {
                if (index >= 1)
                {
                    List<ClamRecord> records = repository.LoadRecords();
                    if (index <= records.Count)
                    {
                        int databaseId = records[index - 1].Id;
                        return repository.DeleteRecord(databaseId);
                    }
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// PROJECT 4: Searches records based on multiple column criteria
        /// Performs case-insensitive partial matching on specified fields
        /// </summary>
        /// <param name="siteFilter">Filter for site identification (null/empty to skip)</param>
        /// <param name="yearFilter">Filter for year (null/empty to skip)</param>
        /// <param name="transectFilter">Filter for transect (null/empty to skip)</param>
        /// <param name="quadratFilter">Filter for quadrat (null/empty to skip)</param>
        /// <param name="speciesFilter">Filter for species common name (null/empty to skip)</param>
        /// <param name="countFilter">Filter for count (null/empty to skip)</param>
        /// <returns>List of records matching all specified criteria</returns>
        public List<ClamRecord> SearchRecords(
            string siteFilter = null,
            string yearFilter = null,
            string transectFilter = null,
            string quadratFilter = null,
            string speciesFilter = null,
            string countFilter = null)
        {
            try
            {
                List<ClamRecord> allRecords = repository.LoadRecords();
                List<ClamRecord> filteredRecords = allRecords;

                // Apply site filter if provided
                if (!string.IsNullOrWhiteSpace(siteFilter))
                {
                    filteredRecords = filteredRecords
                        .Where(r => r.SiteIdentification.IndexOf(siteFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                // Apply year filter if provided
                if (!string.IsNullOrWhiteSpace(yearFilter))
                {
                    filteredRecords = filteredRecords
                        .Where(r => r.Year.IndexOf(yearFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                // Apply transect filter if provided
                if (!string.IsNullOrWhiteSpace(transectFilter))
                {
                    filteredRecords = filteredRecords
                        .Where(r => r.Transect.IndexOf(transectFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                // Apply quadrat filter if provided
                if (!string.IsNullOrWhiteSpace(quadratFilter))
                {
                    filteredRecords = filteredRecords
                        .Where(r => r.Quadrat.IndexOf(quadratFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                // Apply species filter if provided
                if (!string.IsNullOrWhiteSpace(speciesFilter))
                {
                    filteredRecords = filteredRecords
                        .Where(r => r.SpeciesCommonName.IndexOf(speciesFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                // Apply count filter if provided
                if (!string.IsNullOrWhiteSpace(countFilter))
                {
                    filteredRecords = filteredRecords
                        .Where(r => r.Count.IndexOf(countFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();
                }

                return filteredRecords;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// PROJECT 4: Gets count of active filter criteria
        /// Helper method to determine how many filters are being applied
        /// </summary>
        /// <returns>Number of non-empty filter parameters</returns>
        public int GetActiveFilterCount(
            string siteFilter,
            string yearFilter,
            string transectFilter,
            string quadratFilter,
            string speciesFilter,
            string countFilter)
        {
            int count = 0;
            if (!string.IsNullOrWhiteSpace(siteFilter)) count++;
            if (!string.IsNullOrWhiteSpace(yearFilter)) count++;
            if (!string.IsNullOrWhiteSpace(transectFilter)) count++;
            if (!string.IsNullOrWhiteSpace(quadratFilter)) count++;
            if (!string.IsNullOrWhiteSpace(speciesFilter)) count++;
            if (!string.IsNullOrWhiteSpace(countFilter)) count++;
            return count;
        }
    }
}