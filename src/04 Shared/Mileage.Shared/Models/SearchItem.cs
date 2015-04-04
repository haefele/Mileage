using Mileage.Shared.Entities.Search;

namespace Mileage.Shared.Models
{
    public class SearchItem
    {
        public SearchItem(string id, string displayName, SearchableItem item)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.Item = item;
        }

        public SearchItem()
        {
            
        }

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public SearchableItem Item { get; set; }
    }
}