namespace Mileage.Shared.Models
{
    public class TagWithCount
    {
        public TagWithCount(string tag, int count)
        {
            this.Tag = tag;
            this.Count = count;
        }

        public TagWithCount()
        {
            
        }

        public string Tag { get; set; }
        public int Count { get; set; }
    }
}