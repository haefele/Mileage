using System;
using System.Linq;
using Mileage.Server.Infrastructure.Raven.Analyzers;
using Mileage.Shared.Entities.Drivers;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Mileage.Server.Infrastructure.Raven.Indexes
{
    public class DriversForSearch : AbstractIndexCreationTask<Driver, DriversForSearch.Result>
    {
        public class Result
        {
            public string SearchText { get; set; }
            public string SearchTextForSuggestions { get; set; }
            public string DriversLicenses { get; set; }
            public bool PersonCarriageLicense { get; set; }
            public DateTimeOffset DateOfEntry { get; set; }
        }

        public DriversForSearch()
        {
            this.Map = drivers => 
                from driver in drivers
                select new 
                {
                    DateOfEntry = driver.DateOfEntry,
                    DriversLicenses = driver.DriversLicenses.Select(f => f.Class).ToArray(),
                    PersonCarriageLicense = driver.PersonCarriageLicense,
                    SearchText = new[]
                    {
                        driver.FirstName,
                        driver.LastName,
                        driver.MobileNumber,
                        driver.PhoneNumber,
                        driver.EmailAddress,
                        driver.Comment,
                        driver.Address.City,
                        driver.Address.PostalCode,
                        driver.Address.Street
                    },
                    SearchTextForSuggestions = new[]
                    {
                        driver.FirstName,
                        driver.LastName,
                        driver.MobileNumber,
                        driver.PhoneNumber,
                        driver.EmailAddress,
                        driver.Comment,
                        driver.Address.City,
                        driver.Address.PostalCode,
                        driver.Address.Street
                    }
                };

            this.Index(f => f.SearchText, FieldIndexing.Analyzed);
            this.Index(f => f.DriversLicenses, FieldIndexing.Analyzed);

            this.Analyze(f => f.SearchText, typeof(NGramAnalyzer).AssemblyQualifiedName);

            this.Suggestion(f => f.SearchTextForSuggestions);
        }

        public override string IndexName
        {
            get { return "Drivers/ForSearch"; }
        }
    }
}