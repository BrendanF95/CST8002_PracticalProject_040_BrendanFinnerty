/// <summary>
/// Course: CST8002 - Programming Language Research
/// Professor: Tyler Delay
/// Due Date: October 12, 2025
/// Author: Brendan Finnerty
/// 
/// Model/Entity layer - Represents a single clam survey record
/// </summary>

using System;

namespace CST8002_PracticalProject.Model
{
    /// <summary>
    /// Represents a clam survey record from the Pacific Rim dataset
    /// with fields corresponding to CSV columns
    /// </summary>
    public class ClamRecord
    {
        private int id;
        private string siteIdentification;
        private string year;
        private string transect;
        private string quadrat;
        private string speciesCommonName;
        private string count;

        /// <summary>
        /// Default constructor for ClamRecord
        /// </summary>
        public ClamRecord()
        {
            id = 0;
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
        public ClamRecord(string site, string yr, string trans, string quad, string species, string cnt)
        {
            siteIdentification = site;
            year = yr;
            transect = trans;
            quadrat = quad;
            speciesCommonName = species;
            count = cnt;
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string SiteIdentification
        {
            get { return siteIdentification; }
            set { siteIdentification = value; }
        }

        public string Year
        {
            get { return year; }
            set { year = value; }
        }

        public string Transect
        {
            get { return transect; }
            set { transect = value; }
        }

        public string Quadrat
        {
            get { return quadrat; }
            set { quadrat = value; }
        }

        public string SpeciesCommonName
        {
            get { return speciesCommonName; }
            set { speciesCommonName = value; }
        }

        public string Count
        {
            get { return count; }
            set { count = value; }
        }

        /// <summary>
        /// Returns a CSV-formatted string of the record
        /// </summary>
        public string ToCsvString()
        {
            return $"{siteIdentification},{year},{transect},{quadrat},{speciesCommonName},{count}";
        }

        public override string ToString()
        {
            return $"Site: {siteIdentification}, Year: {year}, Transect: {transect}, " +
                   $"Quadrat: {quadrat}, Species: {speciesCommonName}, Count: {count}";
        }
    }
}