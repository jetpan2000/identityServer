using Octacom.DapperRepository;
using Octacom.Odiss.Core.Contracts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octacom.Odiss.Core.DataLayer.Application
{
    public class DatabaseRepository : DbRepository<Entities.Application.Database, Guid, MainDatabase>, IDatabaseRepository
    {
        public DatabaseRepository() : base("dbo", "Databases", "ID", null)
        {
        }
    }
}
