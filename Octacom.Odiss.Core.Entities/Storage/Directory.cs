namespace Octacom.Odiss.Core.Entities.Storage
{
    public class Directory
    {
        public string Id { get; set; }
        public string LocationId { get; set; }
        public Location Location { get; set; }
        public string Name { get; set; }
    }
}
