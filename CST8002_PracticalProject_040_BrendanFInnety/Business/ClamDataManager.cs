/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: October 12, 2025
/// Author: Brendan Finnerty
/// 
/// Business layer - Manages clam data operations and business logic
/// </summary>

using CST8002_PracticalProject.Model;
using System;
using System.Collections.Generic;

namespace CST8002_PracticalProject_040_BrendanFInnety
{
    /// <summary>
    /// Business logic layer for managing clam survey data
    /// </summary>
    public class ClamDataManager
    {
        private List<ClamRecord> clamRecords;
        private DataRepository repository;

        /// <summary>
        /// Constructor initializes the data manager
        /// </summary>
        public ClamDataManager()
        {
            clamRecords = new List<ClamRecord>();
            repository = new DataRepository();
        }

        /// <summary>
        /// Gets the count of records in memory
        /// </summary>
        public int RecordCount
        {
            get { return clamRecords.Count; }
        }

        /// <summary>
        /// Loads/reloads data from the CSV file
        /// </summary>
        /// <returns>Number of records loaded</returns>
        public int ReloadData()
        {
            try
            {
                clamRecords = repository.LoadRecords();
                return clamRecords.Count;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Saves current data to a new CSV file with GUID-based name
        /// </summary>
        /// <returns>The filename that was created</returns>
        public string PersistData()
        {
            try
            {
                return repository.SaveRecords(clamRecords);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a single record by index
        /// </summary>
        /// <param name="index">Zero-based index</param>
        /// <returns>ClamRecord at the specified index</returns>
        public ClamRecord GetRecord(int index)
        {
            if (index >= 0 && index < clamRecords.Count)
            {
                return clamRecords[index];
            }
            return null;
        }

        /// <summary>
        /// Gets all records
        /// </summary>
        /// <returns>List of all clam records</returns>
        public List<ClamRecord> GetAllRecords()
        {
            return new List<ClamRecord>(clamRecords);
        }

        /// <summary>
        /// Adds a new record to the collection
        /// </summary>
        /// <param name="record">The record to add</param>
        public void CreateRecord(ClamRecord record)
        {
            if (record != null)
            {
                clamRecords.Add(record);
            }
        }

        /// <summary>
        /// Updates an existing record
        /// </summary>
        /// <param name="index">Index of record to update</param>
        /// <param name="updatedRecord">New record data</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateRecord(int index, ClamRecord updatedRecord)
        {
            if (index >= 0 && index < clamRecords.Count && updatedRecord != null)
            {
                clamRecords[index] = updatedRecord;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes a record from the collection
        /// </summary>
        /// <param name="index">Index of record to delete</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteRecord(int index)
        {
            if (index >= 0 && index < clamRecords.Count)
            {
                clamRecords.RemoveAt(index);
                return true;
            }
            return false;
        }
    }
}