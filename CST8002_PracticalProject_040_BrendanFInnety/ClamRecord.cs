/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: September 21, 2025
/// Author: Brendan Finnerty
/// 
/// This file contains the ClamRecord class that represents a single record
/// from the Pacific Rim Intertidal Bivalves (Clams) dataset.
/// </summary>

using System;

namespace CST8002_PracticalProject
{
    /// <summary>
    /// Represents a clam survey record from the Pacific Rim dataset
    /// with fields corresponding to CSV columns
    /// </summary>
    public class ClamRecord
    {
        // Private fields matching the CSV column names
        private string siteIdentification;
        private string year;
        private string transect;
        private string quadrat;
        private string speciesCommonName;
        private string count;

        /// <summary>
        /// Default constructor for ClamRecord
        /// Initializes a new instance with default values
        /// </summary>
        public ClamRecord()
        {
            siteIdentification = string.Empty;
            year = string.Empty;
            transect = string.Empty;
            quadrat = string.Empty;
            speciesCommonName = string.Empty;
            count = string.Empty;
        }

        /// <summary>
        /// Parameterized constructor for ClamRecord
        /// </summary>
        /// <param name="site">Site identification</param>
        /// <param name="yr">Year of survey</param>
        /// <param name="trans">Transect identifier</param>
        /// <param name="quad">Quadrat identifier</param>
        /// <param name="species">Species common name</param>
        /// <param name="cnt">Count of specimens</param>
        public ClamRecord(string site, string yr, string trans, string quad, string species, string cnt)
        {
            siteIdentification = site;
            year = yr;
            transect = trans;
            quadrat = quad;
            speciesCommonName = species;
            count = cnt;
        }

        /// <summary>
        /// Gets or sets the Site identification value
        /// </summary>
        public string SiteIdentification
        {
            get { return siteIdentification; }
            set { siteIdentification = value; }
        }

        /// <summary>
        /// Gets or sets the Year value
        /// </summary>
        public string Year
        {
            get { return year; }
            set { year = value; }
        }

        /// <summary>
        /// Gets or sets the Transect value
        /// </summary>
        public string Transect
        {
            get { return transect; }
            set { transect = value; }
        }

        /// <summary>
        /// Gets or sets the Quadrat value
        /// </summary>
        public string Quadrat
        {
            get { return quadrat; }
            set { quadrat = value; }
        }

        /// <summary>
        /// Gets or sets the Species Common Name
        /// </summary>
        public string SpeciesCommonName
        {
            get { return speciesCommonName; }
            set { speciesCommonName = value; }
        }

        /// <summary>
        /// Gets or sets the Count value
        /// </summary>
        public string Count
        {
            get { return count; }
            set { count = value; }
        }

        /// <summary>
        /// Returns a string representation of the clam record
        /// </summary>
        /// <returns>Formatted string with all record data</returns>
        public override string ToString()
        {
            return $"Site: {siteIdentification}, Year: {year}, Transect: {transect}, " +
                   $"Quadrat: {quadrat}, Species: {speciesCommonName}, Count: {count}";
        }

        /// <summary>
        /// Displays the clam record data in a formatted manner
        /// </summary>
        public void DisplayRecord()
        {
            Console.WriteLine($"  Site Identification: {siteIdentification}");
            Console.WriteLine($"  Year: {year}");
            Console.WriteLine($"  Transect: {transect}");
            Console.WriteLine($"  Quadrat: {quadrat}");
            Console.WriteLine($"  Species Common Name: {speciesCommonName}");
            Console.WriteLine($"  Count: {count}");
            Console.WriteLine(new string('-', 50));
        }
    }
}