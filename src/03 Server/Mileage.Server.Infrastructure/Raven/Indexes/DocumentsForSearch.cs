using System.Linq;
using Mileage.Shared.Entities.Drivers;
using Mileage.Shared.Entities.Search;
using Mileage.Shared.Entities.Users;
using Mileage.Shared.Entities.Vehicles;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace Mileage.Server.Infrastructure.Raven.Indexes
{
    public class DocumentsForSearch : AbstractMultiMapIndexCreationTask<DocumentsForSearch.Result>
    {
        public class Result
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public SearchableItem Item { get; set; }
            public string[] SearchText { get; set; }
        }

        public DocumentsForSearch()
        {
            this.AddMap<Driver>(drivers => 
                from driver in drivers
                select new Result
                {
                    Id = driver.Id,
                    DisplayName = driver.FirstName + " " + driver.LastName,
                    Item = SearchableItem.Driver,
                    SearchText = driver.Tags
                        .Concat(driver.DriversLicenses.Select(f => f.Class))
                        .Concat(new []
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
                            })
                        .ToArray()
                });

            this.AddMap<User>(users => 
                from user in users
                select new Result
                {
                    Id = user.Id,
                    DisplayName = user.EmailAddress,
                    Item = SearchableItem.User,
                    SearchText = user.Tags
                        .Concat(new []
                            {
                                user.EmailAddress,
                            })
                        .ToArray()
                });

            this.AddMap<Vehicle>(vehicles => 
                from vehicle in vehicles
                select new Result
                {
                    Id = vehicle.Id,
                    DisplayName = vehicle.Make + " " + vehicle.Model,
                    Item = SearchableItem.Vehicle,
                    SearchText = vehicle.Tags
                        .Concat(new []
                            {
                                vehicle.Model,
                                vehicle.Make
                            })
                        .ToArray()
                });

            this.Index(f => f.SearchText, FieldIndexing.Analyzed);
            this.Suggestion(f => f.SearchText);
            this.TermVector(f => f.SearchText, FieldTermVector.WithPositionsAndOffsets);

            this.StoreAllFields(FieldStorage.Yes);
        }

        public override string IndexName
        {
            get { return "Documents/ForSearch"; }
        }
    }
}