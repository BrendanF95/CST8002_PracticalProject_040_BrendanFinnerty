/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: November 2025
/// Author: Brendan Finnerty
/// 
/// Business layer - Manages clam data operations and business logic
/// PROJECT 3: Updated to work with database operations
/// </summary>

using CST8002_PracticalProject.Model;
using System;
using System.Collections.Generic;

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
                    // Get all records to find the actual database ID
                    List<ClamRecord> records = repository.LoadRecords();
                    if (index <= records.Count)
                    {
                        // Get the actual database ID from the record at this position
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
                    // Get all records to verify the index exists
                    List<ClamRecord> records = repository.LoadRecords();
                    if (index <= records.Count)
                    {
                        // Get the actual database ID from the record at this position
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
    }
}