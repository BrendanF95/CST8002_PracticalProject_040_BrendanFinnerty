/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: November 2025
/// Author: Brendan Finnerty
/// 
/// Unit Test - Tests database connectivity functionality (Project 3)
/// Tests INSERT and SELECT operations to verify database integration
/// </summary>

using CST8002_PracticalProject.Model;
using CST8002_PracticalProject_040_BrendanFInnety;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CST8002_PracticalProject_Tests
{
    /// <summary>
    /// Unit tests for database connectivity operations in Project 3
    /// Tests the advanced feature: Database Connectivity (INSERT, SELECT)
    /// </summary>
    [TestClass]
    public class DatabaseConnectivityTests
    {
        private DataRepository repository;

        /// <summary>
        /// Initialize test repository before each test
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            repository = new DataRepository();
        }

        /// <summary>
        /// Test Case: Verify that InsertRecord adds a new record to the database
        /// and that the record can be retrieved with LoadRecords
        /// 
        /// This tests the core database connectivity functionality:
        /// - SQL INSERT operation
        /// - SQL SELECT operation  
        /// - Parameterized queries
        /// - Database connection handling
        /// </summary>
        [TestMethod]
        public void TestDatabaseInsertAndRetrieve()
        {
            // Arrange - Create a test record with unique data
            ClamRecord testRecord = new ClamRecord(
                "TEST_SITE_001",
                "2025",
                "TEST_TRANSECT",
                "TEST_QUADRAT",
                "Test Clam Species",
                "999"
            );

            // Act - Insert the record into the database
            bool insertSuccess = repository.InsertRecord(testRecord);

            // Load all records from database
            var allRecords = repository.LoadRecords();

            // Find our test record
            ClamRecord retrievedRecord = null;
            foreach (var record in allRecords)
            {
                if (record.SiteIdentification == "TEST_SITE_001" &&
                    record.Year == "2025" &&
                    record.SpeciesCommonName == "Test Clam Species")
                {
                    retrievedRecord = record;
                    break;
                }
            }

            // Assert - Verify the INSERT was successful
            Assert.IsTrue(insertSuccess, "InsertRecord should return true");

            // Assert - Verify we can retrieve the record via SELECT
            Assert.IsNotNull(retrievedRecord, "Test record should be retrievable from database");

            // Assert - Verify all fields were stored correctly
            Assert.AreEqual("TEST_SITE_001", retrievedRecord.SiteIdentification,
                "SiteIdentification should match");
            Assert.AreEqual("2025", retrievedRecord.Year,
                "Year should match");
            Assert.AreEqual("TEST_TRANSECT", retrievedRecord.Transect,
                "Transect should match");
            Assert.AreEqual("TEST_QUADRAT", retrievedRecord.Quadrat,
                "Quadrat should match");
            Assert.AreEqual("Test Clam Species", retrievedRecord.SpeciesCommonName,
                "SpeciesCommonName should match");
            Assert.AreEqual("999", retrievedRecord.Count,
                "Count should match");

            Console.WriteLine("✓ Database INSERT operation successful");
            Console.WriteLine("✓ Database SELECT operation successful");
            Console.WriteLine("✓ All fields stored and retrieved correctly");
            Console.WriteLine($"✓ Test passed - Database connectivity verified by: Brendan Finnerty");
        }

        /// <summary>
        /// Test Case: Verify that UpdateRecord modifies an existing database record
        /// This tests the SQL UPDATE operation
        /// </summary>
        [TestMethod]
        public void TestDatabaseUpdate()
        {
            // Arrange - Insert a test record first
            ClamRecord originalRecord = new ClamRecord(
                "UPDATE_TEST_SITE",
                "2024",
                "TRANS_001",
                "QUAD_001",
                "Original Species",
                "100"
            );
            repository.InsertRecord(originalRecord);

            // Find the record ID
            var records = repository.LoadRecords();
            int testRecordIndex = -1;
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].SiteIdentification == "UPDATE_TEST_SITE")
                {
                    testRecordIndex = i + 1; // 1-based index
                    break;
                }
            }

            // Act - Update the record
            ClamRecord updatedRecord = new ClamRecord(
                "UPDATE_TEST_SITE",
                "2025",
                "TRANS_002",
                "QUAD_002",
                "Updated Species",
                "200"
            );
            bool updateSuccess = repository.UpdateRecord(testRecordIndex, updatedRecord);

            // Retrieve the updated record
            var updatedRecords = repository.LoadRecords();
            ClamRecord retrievedRecord = updatedRecords[testRecordIndex - 1];

            // Assert
            Assert.IsTrue(updateSuccess, "UpdateRecord should return true");
            Assert.AreEqual("Updated Species", retrievedRecord.SpeciesCommonName,
                "Species name should be updated");
            Assert.AreEqual("2025", retrievedRecord.Year,
                "Year should be updated");
            Assert.AreEqual("200", retrievedRecord.Count,
                "Count should be updated");

            Console.WriteLine("✓ Database UPDATE operation successful");
            Console.WriteLine($"✓ Test passed by: Brendan Finnerty");
        }

        /// <summary>
        /// Test Case: Verify that DeleteRecord removes a record from the database
        /// This tests the SQL DELETE operation
        /// </summary>
        [TestMethod]
        public void TestDatabaseDelete()
        {
            // Arrange - Insert a test record to delete
            ClamRecord testRecord = new ClamRecord(
                "DELETE_TEST_SITE",
                "2024",
                "TRANS_DEL",
                "QUAD_DEL",
                "Delete Test Species",
                "50"
            );
            repository.InsertRecord(testRecord);

            // Find the record
            var recordsBefore = repository.LoadRecords();
            int recordCountBefore = recordsBefore.Count;
            int testRecordIndex = -1;

            for (int i = 0; i < recordsBefore.Count; i++)
            {
                if (recordsBefore[i].SiteIdentification == "DELETE_TEST_SITE")
                {
                    testRecordIndex = i + 1;
                    break;
                }
            }

            // Act - Delete the record
            bool deleteSuccess = repository.DeleteRecord(testRecordIndex);
            var recordsAfter = repository.LoadRecords();
            int recordCountAfter = recordsAfter.Count;

            // Assert
            Assert.IsTrue(deleteSuccess, "DeleteRecord should return true");
            Assert.AreEqual(recordCountBefore - 1, recordCountAfter,
                "Record count should decrease by 1");

            // Verify the record is actually gone
            bool recordStillExists = false;
            foreach (var record in recordsAfter)
            {
                if (record.SiteIdentification == "DELETE_TEST_SITE")
                {
                    recordStillExists = true;
                    break;
                }
            }
            Assert.IsFalse(recordStillExists, "Deleted record should not be retrievable");

            Console.WriteLine("✓ Database DELETE operation successful");
            Console.WriteLine($"✓ Test passed by: Brendan Finnerty");
        }
    }
}