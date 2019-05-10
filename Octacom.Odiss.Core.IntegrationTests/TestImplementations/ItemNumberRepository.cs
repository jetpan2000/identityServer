using System;
using System.Collections.Generic;
using System.Linq;
using Octacom.DapperRepository;
using Octacom.Odiss.Core.DataLayer;

namespace Octacom.Odiss.Core.IntegrationTests.TestImplementations
{
    public class ItemNumberRepository : DbRepository<ItemNumber, Guid, MainDatabase>
    {
        public ItemNumberRepository() : base("dbo", "ItemNumber", "Id", null)
        {
        }

        protected override void SetNewKey(ItemNumber item)
        {
            // NOOP
        }

        public override IEnumerable<string> ColumnNames => base.ColumnNames.Except(new string[] { "Id" });
    }
}
