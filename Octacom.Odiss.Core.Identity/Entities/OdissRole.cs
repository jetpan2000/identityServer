using Microsoft.AspNet.Identity;

namespace Octacom.Odiss.Core.Identity.Entities
{
    public class OdissRole : IRole<byte>
    {
        public byte Id { get; set; }
        public string Name { get; set; }
    }
}
