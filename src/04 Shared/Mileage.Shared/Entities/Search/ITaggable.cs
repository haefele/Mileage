using System.Collections.Generic;

namespace Mileage.Shared.Entities.Search
{
    public interface ITaggable
    {
        List<string> Tags { get; set; }
    }
}